using ECActivityAPI.Common;
using ECActivityAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ECActivityAPI.Controllers
{
    /// <summary>
    /// 个人菜品库
    /// </summary>
    public class MySelfDishKuController : ApiController
    {
        private CreditContext db = new CreditContext();
        private SqlHelper dataContext = new SqlHelper();
        private ImgHandle imgHandle = new ImgHandle();
        static private object olock = new object();

        #region 微信端

        #region 增加个人菜品库菜品(1个接口完成)--wx
        /// <summary>
        /// 增加个人菜品库菜品--wx
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddDish(dynamic requestData)
        {
            lock (olock)
            {
                try
                {
                    MySelfDishKu mySelfDishKu = JsonConvert.DeserializeObject<MySelfDishKu>(JsonConvert.SerializeObject(requestData.MySelfDishKu));

                    var middleRole = requestData.MySelfFRRole;
                    List<MySelfFRRole> addRole = new List<MySelfFRRole>();
                    for (int i = 0; i < middleRole.Count; i++)
                    {
                        addRole.Add(JsonConvert.DeserializeObject<MySelfFRRole>(JsonConvert.SerializeObject(middleRole[i])));
                    }

                    #region 判断个人菜品库能添加多少道菜品,确定菜品DishType编号

                    int mysele = addRole[0].MySelfFRId;
                    int memberId = addRole[0].MemberId;
                    var qRole = (from v in db.MySelfFR
                                 where v.MySelfFRId == mysele
                                 select v).FirstOrDefault();

                    var qMySelfDishKu = (from v in db.MySelfDishKu
                                         where v.CreateTime < qRole.EndTime && v.CreateTime > qRole.BeginTime && v.MemberId == memberId
                                         orderby v.DishType descending
                                         select v).FirstOrDefault();

                    if (qMySelfDishKu == null)
                        mySelfDishKu.DishType = 1;
                    else
                    {
                        int FRCount = qRole.FRCount;
                        int type = qMySelfDishKu.DishType;
                        if (type < FRCount)
                        {
                            type = type + 1;
                            mySelfDishKu.DishType = type;
                        }
                        else
                            return "本次活动上传菜品机会用完";
                    }

                    #endregion

                    #region 增加个人菜品库菜品

                    mySelfDishKu.CreateTime = DateTime.Now;
                    mySelfDishKu.UpateTime = DateTime.Now;
                    mySelfDishKu.ApplyTime = Convert.ToDateTime("1990-01-01");

                    db.MySelfDishKu.Add(mySelfDishKu);
                    db.SaveChanges();

                    #endregion

                    #region 增加调味信息

                    var middleDishMaterial = requestData.DishMaterial;
                    List<DishMaterial> addDishMaterial = JsonConvert.DeserializeObject<List<DishMaterial>>(JsonConvert.SerializeObject(middleDishMaterial));
                    //for (int i = 0; i < middleDishMaterial.Count; i++)
                    //{
                    //    addDishMaterial.Add(JsonConvert.DeserializeObject<DishMaterial>(JsonConvert.SerializeObject(middleDishMaterial[i])));
                    //}

                    for (int i = 0; i < addDishMaterial.Count; i++)
                    {
                        string insert = string.Format(@"INSERT INTO  [dbo].[DishMaterial] ([DishId]
           ,[Material]  ,[Unit],[MaterialType]  ,[CreateTime])  VALUES   ({0} ,'{1}' ,'{2}' ,{3} ,'{4}' )  ", mySelfDishKu.MySelfDishId, addDishMaterial[i].Material, addDishMaterial[i].Unit, addDishMaterial[i].MaterialType, addDishMaterial[i].CreateTime);
                        dataContext.ExecuteNonQuery(CommandType.Text, insert);
                    }

                    #endregion

                    #region 增加推荐调料

                    var middleMySelfFRRole = requestData.MySelfFRRole;

                    List<MySelfFRRole> addMySelfFRRole = JsonConvert.DeserializeObject<List<MySelfFRRole>>(JsonConvert.SerializeObject(middleMySelfFRRole));
                    //for (int i = 0; i < middleMySelfFRRole.Count; i++)
                    //{
                    //    addMySelfFRRole.Add(JsonConvert.DeserializeObject<MySelfFRRole>(JsonConvert.SerializeObject(middleMySelfFRRole[i])));
                    //}

                    for (int i = 0; i < addMySelfFRRole.Count; i++)
                    {
                        string insert = string.Format(@"INSERT INTO  [dbo].[MySelfFRRole] ([MemberId]
           ,[MySelfDishKuId] ,[MySelfFRId],[Unit],[CreateTime])  VALUES   ({0} ,'{1}' ,'{2}' ,{3} ,'{4}' )  ", addMySelfFRRole[i].MemberId, mySelfDishKu.MySelfDishId, addMySelfFRRole[i].MySelfFRId, addMySelfFRRole[i].Unit, addMySelfFRRole[i].CreateTime);
                        dataContext.ExecuteNonQuery(CommandType.Text, insert);
                    }

                    #endregion

                    #region 增加做菜步骤
                    var middleDishStep = requestData.DishStep;

                    List<DishStep> addDishStep = JsonConvert.DeserializeObject<List<DishStep>>(JsonConvert.SerializeObject(middleDishStep));
                    //for (int i = 0; i < middleDishStep.Count; i++)
                    //{
                    //    addDishStep.Add(JsonConvert.DeserializeObject<DishStep>(JsonConvert.SerializeObject(middleDishStep[i])));
                    //}
                    for (int i = 0; i < addDishStep.Count; i++)
                    {
                        string insert = string.Format("INSERT INTO  DishStep (DishId,StepId,StepName,CreateTime) VALUES  ({0} ,{1} ,'{2}' ,'{3}' ) ", mySelfDishKu.MySelfDishId, i + 1, addDishStep[i].StepName, addDishStep[i].CreateTime);
                        dataContext.ExecuteNonQuery(CommandType.Text, insert);
                    }

                    #endregion

                    return "OK";
                }
                catch (Exception)
                {
                    return "No";
                }
            }
        }
        #endregion

        #region 获取个人菜品库 “我的”--wx
        /// <summary>
        /// 获取个人菜品库 “我的”--wx
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetMy(dynamic requestData)
        {
            try
            {
                string openId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.OpenId));
                int memberId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.MemberId));
                int pageIndex = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.PageIndex));
                string sql = string.Format(@"select * from(
select DC.DishChefId as DishId, C.MemberId,DC.DishType,DC.ApplyTime ,DC.DishName,DC.DishPicUrl,DC.VisitCount ,2 as PeopleType ,case when  DCL.DishId is not null then 't' else 'f' end as IsCollect 
from DishChef as DC 
left join Chef as C on DC.ChefId=C.ChefId
left join 
(select * from DishCollectLog where OpenId='{0}')as DCL on DC.DishChefId=DCL.DishId
where C.MemberId={1} and DC.IsApply=1
union all
select MS.MySelfDishId as DishId,MS.MemberId,MS.DishType,MS.ApplyTime,MS.DishName,MS.DishPicUrl,MS.VisitCount,4 as PeopleType ,case when  DCL.DishId is not null then 't' else 'f' end as IsCollect from MySelfDishKu as MS
left join 
(select * from DishCollectLog where OpenId='{0}')as DCL on MS.MySelfDishId=DCL.DishId
where MS.MemberId={1} and MS.IsApply=1
) as my order by my.ApplyTime desc", openId, memberId);

                var q = dataContext.ExecuteDataTable(CommandType.Text, sql);

                int totalCount = q.Rows.Count;

                if (pageIndex > 0)
                    q = imgHandle.GetPagedTable(q, pageIndex, 10);

                return "{\"totalCount\":" + totalCount + ",\"Info\":" + JsonConvert.SerializeObject(q, DateTimeConvent.DateTimehh()) + "}";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 获取个人菜品库“收藏"--wx
        /// <summary>
        /// 获取个人菜品库“收藏”--wx
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetMyCollect(dynamic requestData)
        {
            try
            {
                string openId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.OpenId));
                int memberId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.MemberId));
                int pageIndex = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.PageIndex));
                string sql = string.Format(@"select * from 
(
select DCL.DishId,DCL.OpenId,DCL.DishType,DCL.CreateTime,DT.DishName,DT.DishPicUrl,DT.VisitCount,T.TutorName as PeopleName,T.TutorHotel as HotelName,1 as PeopleType from DishCollectLog as DCL
left join DishTutor as DT on DCL.DishId=DT.TutorDishId
left join Tutor as T on DT.TutorId=T.TutorId
where 1=1 and DCL.DishId between 200000 and 500000-1
 
 and DCL.OpenId='{0}'
 union all
---收藏厨师菜品
select DCL.DishId,DCL.OpenId,DCL.DishType,DCL.CreateTime,DC.DishName,DC.DishPicUrl,DC.VisitCount,RM.MemberName as PeopleName,RM.HotelName as HotelName, 2 as PeopleType from DishCollectLog as DCL
left join DishChef as DC on DCL.DishId=DC.DishChefId
left join Chef as C  on DC.ChefId=C.ChefId
left join RegistMember as RM on C.MemberId=RM.MemberId
where 1=1 and DCL.DishId between 500000 and 1000000
and DCL.OpenId='{0}'
 union all
---收藏星厨菜品

select DCL.DishId,DCL.OpenId,DCL.DishType,DCL.CreateTime,DCS.DishName,DCS.DishPicUrl,DCS.VisitCount,CS.ChefStarName as PeopleName,CS.HotelName as HotelName,  3 as PeopleType from DishCollectLog as DCL 
left join DishChefStar as DCS on DCL.DishId=DCS.DishChefStarId
left join ChefStar as CS on DCS.ChefStarId=CS.ChefStarId
where 1=1 and DCL.DishId between 80000 and 150000
and DCL.OpenId='{0}'
) as Collect
order by Collect.CreateTime desc", openId);

                var q = dataContext.ExecuteDataTable(CommandType.Text, sql);

                int totalCount = q.Rows.Count;

                if (pageIndex > 0)
                    q = imgHandle.GetPagedTable(q, pageIndex, 10);

                return "{\"totalCount\":" + totalCount + ",\"Info\":" + JsonConvert.SerializeObject(q, DateTimeConvent.DateTimehh()) + "}";
            }
            catch (Exception)
            {
                return "No";
            }
            //return null;
        }


        #endregion

        #region 获取个人菜品库推荐调料--wx
        /// <summary>
        ///  获取个人菜品库推荐调料--wx
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetMySelfFR()
        {
            try
            {
                string sql = "select MySelfFRId,FRName,FRPicUrl from MySelfFR where BeginTime<GETDATE() and EndTime>GETDATE()  order by CreateTime";
                var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
                return JsonConvert.SerializeObject(q, DateTimeConvent.DateTimehh());
            }
            catch (Exception)
            {
                return "No";
            }

        }
        #endregion 

        #region 增加个人菜品库浏览次数--以后菜品增加浏览做统一接口--wx
        /// <summary>
        ///  增加个人菜品库浏览次数--wx
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddDishVisitLog(dynamic requestData)
        {
            try
            {
                DishVisitLog m = JsonConvert.DeserializeObject<DishVisitLog>(JsonConvert.SerializeObject(requestData));
                var q = (from v in db.MySelfDishKu
                         where v.MySelfDishId == m.DishId
                         select v).FirstOrDefault();
                m.DishType = q.DishType;
                db.DishVisitLog.Add(m);
                db.SaveChanges();
                var sql = string.Format(@"update MySelfDishKu set VisitCount=VisitCount+1 where MySelfDishId={0}", m.DishId);
                dataContext.ExecuteNonQuery(CommandType.Text, sql);

                var sqlCount = string.Format(@"SELECT [MySelfDishId],[VisitCount] FROM [MySelfDishKu] where [MySelfDishId]={0}", m.DishId);
               var qVisitCount= dataContext.ExecuteDataTable(CommandType.Text, sqlCount);
               return JsonConvert.SerializeObject(qVisitCount);
            }
            catch (Exception)
            {
                return "No";
            }
        }

        #endregion

        #region 当前上传个人菜品库是否超过规定上传菜数--wx
        /// <summary>
        ///  当前上传个人菜品库是否超过规定上传菜数--wx
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string IsOverLoad(dynamic requestData)
        {
            try
            {
                int MySelfFRId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.MySelfFRId));

                int memberId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.MemberId));

                var qMySelfFR = (from v in db.MySelfFR
                                 where v.MySelfFRId == MySelfFRId
                                 select v).FirstOrDefault();

                var qMySelfDishKu = (from v in db.MySelfDishKu
                                     where v.MemberId == memberId && v.CreateTime >= qMySelfFR.BeginTime && v.CreateTime <= qMySelfFR.EndTime
                                     orderby v.DishType descending
                                     select v).FirstOrDefault();

                if (qMySelfDishKu == null)
                    return "{\"IsOverLoad\":\"t\",\"Usered\":\"0\"}";
                else
                {
                    if (qMySelfDishKu.DishType >= qMySelfFR.FRCount)
                        return "{\"IsOverLoad\":\"f\",\"Usered\":\""+qMySelfDishKu.DishType+"\"}";
                }

                return "{\"IsOverLoad\":\"t\",\"Usered\":\""+qMySelfDishKu.DishType+"\"}";
            }
            catch (Exception)
            {
                return "No";
            }

        }

        #endregion

        #region 获取个人菜品库留言信息--wx
        /// <summary>
        /// 获取个人菜品库留言信息--wx
        /// </summary>
        /// <param name="dishId">菜品Id</param>
        /// <param name="index">页数</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        [HttpGet]
        public string GetDishComment(int dishId, int index, int pageSize)
        {
            try
            {
                var q = (from v in db.DishComment
                         where v.DishId == dishId
                         select new
                         {
                             MemberName = v.MemberName,
                             HeadPicUrl = v.HeadPicUrl,
                             CreateTime = v.CreateTime,
                             Comment = v.Comment
                         });
                var count = q.Count();
                var lastq = q.OrderByDescending(v => v.CreateTime).Skip((index - 1) * pageSize).Take(pageSize);
                 return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(lastq,DateTimeConvent.DateTimehh()) + "}";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 获取个人菜品库菜品信息(编辑使用)--个人菜品库编辑页面--wx
        /// <summary>
        /// 获取个人菜品库菜品信息(编辑使用)--个人菜品库编辑页面---wx
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetMySelfDish(int mySelfDishId)
        {
            try
            {
                //int mySelfDishId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.MySelfDishId));

                var dishInfo = (from v in db.MySelfDishKu
                                where v.MySelfDishId == mySelfDishId
                                select new
                                {
                                    MySelfDishId = v.MySelfDishId,
                                    DishPicUrl = v.DishPicUrl,
                                    DishName = v.DishName,
                                    DishStory = v.DishStory,
                                    IsApply = v.IsApply,
                                    MemberId = v.MemberId,
                                    PhoneNum=v.PhoneNum
                                }).FirstOrDefault();

                var dishMaterial = (from v in db.DishMaterial
                                    where v.DishId == mySelfDishId
                                    select new
                                    {
                                        DishId = v.DishId,
                                        Material = v.Material,
                                        Unit = v.Unit,
                                        MaterialType = v.MaterialType
                                    });

                var role = string.Format(@"select A.MySelfDishKuId,B.FRName,A.Unit,B.MySelfFRId  from MySelfFRRole as A  
 left  join MySelfFR B on A.MySelfFRId=B.MySelfFRId
 where A.MySelfDishKuId={0}", mySelfDishId);

                var qrole = dataContext.ExecuteDataTable(CommandType.Text, role);

                var dishStep = (from v in db.DishStep
                                where v.DishId == mySelfDishId
                                select new
                                {
                                    DishId = v.DishId,
                                    StepId = v.StepId,
                                    StepName = v.StepName
                                });
                var datetimeNow = DateTime.Now;

                var sqlMyselfFR = string.Format(" select MySelfFRId,FRName,FRPicUrl from MySelfFR where EndTime>='{0}' and EndTime>='{0}'", datetimeNow);
                var qMySelftFR = dataContext.ExecuteDataTable(CommandType.Text, sqlMyselfFR);

                return "{\"dishInfo\":" + JsonConvert.SerializeObject(dishInfo) + ",\"dishMaterial\":" + JsonConvert.SerializeObject(dishMaterial) + ",\"qrole\":" + JsonConvert.SerializeObject(qrole) + ",\"dishStep\":" + JsonConvert.SerializeObject(dishStep) + ",\"MySelftFR\":" + JsonConvert.SerializeObject(qMySelftFR) + "}";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 修改个人菜品菜品（1个接口修改--个人菜品库编辑页面--wx
        /// <summary>
        /// 修改个人菜品菜品（1个接口修改）--个人菜品库编辑页面--wx
        /// </summary>
        /// <returns>不可修改，OK，No</returns>
        [HttpPost]
        public string UpdateMySelfDishWX(dynamic requestData)
        {
            try
            {
                #region 修改基本菜品信息

                MySelfDishKu mySelfDishKu = JsonConvert.DeserializeObject<MySelfDishKu>(JsonConvert.SerializeObject(requestData.MySelfDishKu));

                if (mySelfDishKu.IsApply != 2)
                    return "不可修改";

                var sqlMySelfDishKu = string.Format(@"Update  [dbo].[MySelfDishKu]  set [DishStory]='{0}', [DishPicUrl]='{1}'  ,  [DishName]='{2}', [UpateTime]='{3}'  ,[PhoneNum]='{5}' where   MySelfDishId={4}", mySelfDishKu.DishStory, mySelfDishKu.DishPicUrl, mySelfDishKu.DishName, DateTime.Now, mySelfDishKu.MySelfDishId,mySelfDishKu.PhoneNum);

                dataContext.ExecuteNonQuery(CommandType.Text, sqlMySelfDishKu);

                #endregion

                #region 修改菜品调料

                var middleDishMaterial = requestData.DishMaterial;
                List<DishMaterial> addDishMaterial = new List<DishMaterial>();
                for (int i = 0; i < middleDishMaterial.Count; i++)
                {
                    addDishMaterial.Add(JsonConvert.DeserializeObject<DishMaterial>(JsonConvert.SerializeObject(middleDishMaterial[i])));
                }
                //删除原来调料
                string deleteDishMaterial = string.Format(@"DELETE FROM [dbo].[DishMaterial] where DishId={0}", mySelfDishKu.MySelfDishId);
                dataContext.ExecuteNonQuery(CommandType.Text, deleteDishMaterial);

                for (int i = 0; i < addDishMaterial.Count; i++)
                {
                    string insert = string.Format(@"INSERT INTO  [dbo].[DishMaterial] ([DishId]
           ,[Material]  ,[Unit],[MaterialType]  ,[CreateTime])  VALUES   ({0} ,'{1}' ,'{2}' ,{3} ,'{4}' )  ", mySelfDishKu.MySelfDishId, addDishMaterial[i].Material, addDishMaterial[i].Unit, addDishMaterial[i].MaterialType, addDishMaterial[i].CreateTime);
                    dataContext.ExecuteNonQuery(CommandType.Text, insert);
                }


                #endregion

                #region 修改推荐调料

                var middleMySelfFRRole = requestData.MySelfFRRole;

                List<MySelfFRRole> addMySelfFRRole = new List<MySelfFRRole>();
                for (int i = 0; i < middleMySelfFRRole.Count; i++)
                {
                    addMySelfFRRole.Add(JsonConvert.DeserializeObject<MySelfFRRole>(JsonConvert.SerializeObject(middleMySelfFRRole[i])));
                }
                //删除原来调料
                string deleteMySelfFRRole = string.Format(@"DELETE FROM [dbo].[MySelfFRRole] where MySelfDishKuId={0}", mySelfDishKu.MySelfDishId);
                dataContext.ExecuteNonQuery(CommandType.Text, deleteMySelfFRRole);

                for (int i = 0; i < addMySelfFRRole.Count; i++)
                {
                    string insert = string.Format(@"INSERT INTO  [dbo].[MySelfFRRole] ([MemberId]
           ,[MySelfDishKuId] ,[MySelfFRId],[Unit],[CreateTime])  VALUES   ({0} ,'{1}' ,'{2}' ,{3} ,'{4}' )  ", addMySelfFRRole[i].MemberId, mySelfDishKu.MySelfDishId, addMySelfFRRole[i].MySelfFRId, addMySelfFRRole[i].Unit, addMySelfFRRole[i].CreateTime);
                    dataContext.ExecuteNonQuery(CommandType.Text, insert);
                }


                #endregion

                #region 修改菜品步骤

                var middleDishStep = requestData.DishStep;

                List<DishStep> addDishStep = new List<DishStep>();
                for (int i = 0; i < middleDishStep.Count; i++)
                {
                    addDishStep.Add(JsonConvert.DeserializeObject<DishStep>(JsonConvert.SerializeObject(middleDishStep[i])));
                }
                //删除
                string deleteDishStep = string.Format(@"DELETE FROM [dbo].[DishStep] where DishId={0}", mySelfDishKu.MySelfDishId);
                dataContext.ExecuteNonQuery(CommandType.Text, deleteDishStep);

                for (int i = 0; i < addDishStep.Count; i++)
                {
                    string insert = string.Format("INSERT INTO  DishStep (DishId,StepId,StepName,CreateTime) VALUES  ({0} ,{1} ,'{2}' ,'{3}' ) ", mySelfDishKu.MySelfDishId, i + 1, addDishStep[i].StepName, addDishStep[i].CreateTime);
                    dataContext.ExecuteNonQuery(CommandType.Text, insert);
                }
                #endregion

                return "OK";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #endregion

        #region 后台

        #region 增加个人菜品库推荐调料--web
        /// <summary>
        /// 增加个人菜品库推荐调料--web
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddMySelfFR(dynamic requestData)
        {
            try
            {
                string updateSingle = JsonConvert.SerializeObject(requestData);
                var q = JsonConvert.DeserializeObject<MySelfFR>(updateSingle);

                q.CreateTime = DateTime.Now;
                q.UpdateTime = DateTime.Now;

                db.MySelfFR.Add(q);
                db.SaveChanges();
                return "OK";
            }
            catch (Exception)
            {
                return "No";
            }

        }

        
        #endregion

        #region 后台修改个人菜品库推荐调料--web
        /// <summary>
        /// 后台修改个人菜品库推荐调料--web
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string UpdateMySelfFR(dynamic requestData)
        {
            try
            {
                string updateSingle = JsonConvert.SerializeObject(requestData);
                var q = JsonConvert.DeserializeObject<MySelfFR>(updateSingle);


                var sql = string.Format(@"UPDATE [dbo].[MySelfFR]  SET [ActivityName] ='{7}'  ,[FRName] ='{0}'  ,[FRPicUrl] = '{1}' ,[FRCount] ={2} ,[BeginTime] ='{3}'  ,[EndTime] ='{4}'  ,[UpdateTime] = '{5}'
 WHERE MySelfFRId={6}", q.FRName, q.FRPicUrl, q.FRCount, q.BeginTime, q.EndTime,DateTime.Now.ToString(),q.MySelfFRId,q.ActivityName);
                dataContext.ExecuteNonQuery(CommandType.Text, sql);
                return "OK";
            }
            catch (Exception)
            {
                return "No";
            }
            
        }
        
        #endregion

        #region 个人菜品库上传的菜品列表--反馈菜栏下--web
        /// <summary>
        /// 个人菜品库上传的菜品列表--反馈菜栏下--web
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetMySelfDishKu(dynamic requestData)
        {
            try
            {
                //审核状态，-1，表示全部，0、待审核   1、审核通过   2、审核不通过
                int isApply = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.IsApply));

                //上传时间
                string beginTime = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.BeginTime));
                string endTime = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.EndTime));

                //会员姓名，菜品名，产品名称
                string name = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.Name));

                //页数

                int pageIndex = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.PageIndex));

                string sql = string.Format(@"select MyDK.MemberId,RM.MemberName,MyDK.MySelfDishId,MyDK.DishName,FR.FRName,MyDK.CreateTime,MyDK.IsApply  ,MyDK.PhoneNum  from MySelfDishKu as MyDK 
	left join RegistMember as RM on MyDK.MemberId=RM.MemberId
	left join ( select B.MySelfDishKuId , FRName=(stuff( (select ' '+A.FRName from (select role.*,FR.FRName  from MySelfFRRole as role left join MySelfFR as FR on role.MySelfFRId=FR.MySelfFRId)as A  where B.MySelfDishKuId=A .MySelfDishKuId for xml path('')),1, 1, ''))
 from ( select role.*,FR.FRName  from MySelfFRRole as role left join MySelfFR as FR on role.MySelfFRId=FR.MySelfFRId) as B group by B.MySelfDishKuId) As FR on MyDK.MySelfDishId=FR.MySelfDishKuId  where 1=1");

                if (isApply > -1)
                    sql = sql + " and MyDK.IsApply= " + isApply;
                if (!string.IsNullOrEmpty(beginTime))
                    sql = sql + "  and MyDK.CreateTime>='" + beginTime + "'  ";
                if (!string.IsNullOrEmpty(endTime))
                    sql = sql + "  and MyDK.CreateTime<='" + endTime + "'  ";

                if (!string.IsNullOrEmpty(name))
                    sql = sql + "  and (RM.MemberName  like '%" + name + "%'  or  MyDK.DishName    like '%" + name + "%'  or    FR.FRName  like '%" + name + "%')  ";
                sql = sql + "    order by MyDK.CreateTime desc";


                var q = dataContext.ExecuteDataTable(CommandType.Text, sql);

                int totalCount = q.Rows.Count;

                if (pageIndex > 0)
                    q = imgHandle.GetPagedTable(q, pageIndex, 10);

                return "{\"totalCount\":" + totalCount + ",\"Info\":" + JsonConvert.SerializeObject(q, DateTimeConvent.DateTimehh()) + "}";
            }
            catch (Exception)
            {
                return "No";
            }


        }



        #endregion

        #region 获取个人菜品库菜品信息(编辑使用)--一道完整菜品--web，wx
        /// <summary>
        /// 获取个人菜品库菜品信息(编辑使用)--一道完整菜品--web，wx
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetMySelfDish(dynamic requestData)
        {
            try
            {
                int mySelfDishId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.MySelfDishId));

                var dishInfo = (from v in db.MySelfDishKu
                                where v.MySelfDishId == mySelfDishId
                                select new
                                         {
                                             MySelfDishId = v.MySelfDishId,
                                             DishPicUrl = v.DishPicUrl,
                                             DishName = v.DishName,
                                             DishStory = v.DishStory,
                                             IsApply = v.IsApply,
                                             MemberId=v.MemberId,
                                            PhoneNum= v.PhoneNum
                                         }).FirstOrDefault();

                var dishMaterial = (from v in db.DishMaterial
                                    where v.DishId == mySelfDishId
                                    select new
                                    {
                                        DishId = v.DishId,
                                        Material = v.Material,
                                        Unit = v.Unit,
                                        MaterialType = v.MaterialType
                                    });

                var role = string.Format(@"select A.MySelfDishKuId,B.FRName,A.Unit,B.MySelfFRId  from MySelfFRRole as A  
 left  join MySelfFR B on A.MySelfFRId=B.MySelfFRId
 where A.MySelfDishKuId={0}", mySelfDishId);

                var qrole = dataContext.ExecuteDataTable(CommandType.Text, role);

                var dishStep = (from v in db.DishStep
                                where v.DishId == mySelfDishId
                                select new
                                {
                                    DishId = v.DishId,
                                    StepId = v.StepId,
                                    StepName = v.StepName
                                });
                var datetimeNow=DateTime.Now;
                var sqlMyselfFR = string.Format(" select MySelfFRId,FRName,FRPicUrl from MySelfFR where EndTime>='{0}' and EndTime>='{0}'", datetimeNow);
                var qMySelftFR = dataContext.ExecuteDataTable(CommandType.Text, sqlMyselfFR);

                return "{\"dishInfo\":" + JsonConvert.SerializeObject(dishInfo) + ",\"dishMaterial\":" + JsonConvert.SerializeObject(dishMaterial) + ",\"qrole\":" + JsonConvert.SerializeObject(qrole) + ",\"dishStep\":" + JsonConvert.SerializeObject(dishStep) + ",\"MySelftFR\":" + JsonConvert.SerializeObject(qMySelftFR) + "}";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 个人菜品库审核--web
        /// <summary>
        /// 个人菜品库审核--web
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string MySelfDishIsApply(dynamic requestData)
        {
            try
            {
                int mySelfDishId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.DishId));
                int isApply = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.IsApply));
                var sql = string.Format(" Update  [dbo].[MySelfDishKu] set IsApply={0}, ApplyTime='{1}'  where MySelfDishId={2}", isApply, DateTime.Now, mySelfDishId);
                dataContext.ExecuteNonQuery(CommandType.Text, sql);
                return "OK";
            }
            catch (Exception)
            {
                return "";
            }
        }

        #endregion

        #region 获取个人菜品库推荐调料---web
        /// <summary>
        /// 获取个人菜品库推荐调料---web
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetMySelfFRWeb()
        {
            try
            {
                var qMySelfFR = (from v in db.MySelfFR
                                 select v);
                return JsonConvert.SerializeObject(qMySelfFR);
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 修改个人菜品菜品（1个接口修改）--web
        /// <summary>
        /// 修改个人菜品菜品（1个接口修改）--web
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string UpdateMySelfDish(dynamic requestData)
        {
            try
            {
                #region 修改基本菜品信息

                MySelfDishKu mySelfDishKu = JsonConvert.DeserializeObject<MySelfDishKu>(JsonConvert.SerializeObject(requestData.MySelfDishKu));



                var sqlMySelfDishKu = string.Format(@"Update  [dbo].[MySelfDishKu]  set [DishStory]='{0}', [DishPicUrl]='{1}'  ,  [DishName]='{2}', [UpateTime]='{3}', [IsApply]=0    where   MySelfDishId={4}", mySelfDishKu.DishStory, mySelfDishKu.DishPicUrl, mySelfDishKu.DishName, DateTime.Now, mySelfDishKu.MySelfDishId);

                dataContext.ExecuteNonQuery(CommandType.Text, sqlMySelfDishKu);

                #endregion

                #region 修改菜品调料

                var middleDishMaterial = requestData.DishMaterial;
                List<DishMaterial> addDishMaterial = new List<DishMaterial>();
                for (int i = 0; i < middleDishMaterial.Count; i++)
                {
                    addDishMaterial.Add(JsonConvert.DeserializeObject<DishMaterial>(JsonConvert.SerializeObject(middleDishMaterial[i])));
                }
                //删除原来调料
                string deleteDishMaterial = string.Format(@"DELETE FROM [dbo].[DishMaterial] where DishId={0}", mySelfDishKu.MySelfDishId);
                dataContext.ExecuteNonQuery(CommandType.Text, deleteDishMaterial);

                for (int i = 0; i < addDishMaterial.Count; i++)
                {
                    string insert = string.Format(@"INSERT INTO  [dbo].[DishMaterial] ([DishId]
           ,[Material]  ,[Unit],[MaterialType]  ,[CreateTime])  VALUES   ({0} ,'{1}' ,'{2}' ,{3} ,'{4}' )  ", mySelfDishKu.MySelfDishId, addDishMaterial[i].Material, addDishMaterial[i].Unit, addDishMaterial[i].MaterialType, addDishMaterial[i].CreateTime);
                    dataContext.ExecuteNonQuery(CommandType.Text, insert);
                }


                #endregion

                #region 修改推荐调料

                var middleMySelfFRRole = requestData.MySelfFRRole;

                List<MySelfFRRole> addMySelfFRRole = new List<MySelfFRRole>();
                for (int i = 0; i < middleMySelfFRRole.Count; i++)
                {
                    addMySelfFRRole.Add(JsonConvert.DeserializeObject<MySelfFRRole>(JsonConvert.SerializeObject(middleMySelfFRRole[i])));
                }
                //删除原来调料
                string deleteMySelfFRRole = string.Format(@"DELETE FROM [dbo].[MySelfFRRole] where MySelfDishKuId={0}", mySelfDishKu.MySelfDishId);
                dataContext.ExecuteNonQuery(CommandType.Text, deleteMySelfFRRole);

                for (int i = 0; i < addMySelfFRRole.Count; i++)
                {
                    string insert = string.Format(@"INSERT INTO  [dbo].[MySelfFRRole] ([MemberId]
           ,[MySelfDishKuId] ,[MySelfFRId],[Unit],[CreateTime])  VALUES   ({0} ,'{1}' ,'{2}' ,{3} ,'{4}' )  ", addMySelfFRRole[i].MemberId, mySelfDishKu.MySelfDishId, addMySelfFRRole[i].MySelfFRId, addMySelfFRRole[i].Unit, addMySelfFRRole[i].CreateTime);
                    dataContext.ExecuteNonQuery(CommandType.Text, insert);
                }


                #endregion

                #region 修改菜品步骤

                var middleDishStep = requestData.DishStep;

                List<DishStep> addDishStep = new List<DishStep>();
                for (int i = 0; i < middleDishStep.Count; i++)
                {
                    addDishStep.Add(JsonConvert.DeserializeObject<DishStep>(JsonConvert.SerializeObject(middleDishStep[i])));
                }
                //删除
                string deleteDishStep = string.Format(@"DELETE FROM [dbo].[DishStep] where DishId={0}", mySelfDishKu.MySelfDishId);
                dataContext.ExecuteNonQuery(CommandType.Text, deleteDishStep);

                for (int i = 0; i < addDishStep.Count; i++)
                {
                    string insert = string.Format("INSERT INTO  DishStep (DishId,StepId,StepName,CreateTime) VALUES  ({0} ,{1} ,'{2}' ,'{3}' ) ", mySelfDishKu.MySelfDishId, i + 1, addDishStep[i].StepName, addDishStep[i].CreateTime);
                    dataContext.ExecuteNonQuery(CommandType.Text, insert);
                }
                #endregion

                return "OK";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion


        #region 发送模板消息--个人菜品库审核模板消息--web
        /// <summary>
        /// 发送模板消息--个人菜品库审核模板消息--web
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string SentTemplate(dynamic requestData)
        {
            try
            {
                //需要MemberId获得OpenId，dishName,mySelfDishId，context ，matchName，isApply,url

                int memberId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.MemberId));

                int mySelfDishId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.MySelfDishId));

                string dishName = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.DishName));

                string context = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.Context));
                //暂定固定值---20180331
                string matchName = "征集黄豆酱热卖菜";

                int isApply = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.IsApply));

                string url = "";
                //审核通过
                if (isApply == 1)
                    url = "http://jifenweixin.shinho.net.cn/#/component/dishshow";
                //审核不合格
                if (isApply == 2)
                    url = "http://jifenweixin.shinho.net.cn/#/component/dishupload?mySelfDishId=" + mySelfDishId;

                var qOpenId = (from v in db.OpenIdAssociated
                               where v.UserId == memberId && v.UserType == 2
                               select v).FirstOrDefault();
                if (qOpenId == null)
                    return "No";
                var ret = imgHandle.SentTemplate(qOpenId.OpenId, dishName, context, matchName, isApply, url);

                return ret;
            }
            catch (Exception)
            {
                return "No";
            }
        }

        #endregion





        #endregion
    }
}

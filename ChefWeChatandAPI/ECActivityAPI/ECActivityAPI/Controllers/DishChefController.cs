using ECActivityAPI.Common;
using ECActivityAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ECActivityAPI.Controllers
{
    /// <summary>
    /// 厨师菜品
    /// </summary>
    public class DishChefController : ApiController
    {
        private CreditContext db = new CreditContext();
        private SqlHelper dataContext = new SqlHelper();
        private ImgHandle imgHandle = new ImgHandle();

        #region 增加厨师菜品基本（修改）信息--本地菜，创新菜--wx
        /// <summary>
        /// 增加厨师菜品基本（修改）信息--本地菜，创新菜--wx
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public string AddDishChef(dynamic requestData)
        {
            try
            {
                string query = JsonConvert.SerializeObject(requestData);
                DishChef model = JsonConvert.DeserializeObject<DishChef>(query);

                var q = (from v in db.DishChef
                         where v.ChefId == model.ChefId && v.DishType == model.DishType
                         select v).FirstOrDefault();
                if (q == null)
                {
                    model.CreateTime = DateTime.Now;
                    model.UpdateTime = DateTime.Now;
                    model.ApplyTime = Convert.ToDateTime("1900-01-01");
                    model.SelectedTime = Convert.ToDateTime("1900-01-01");
                    db.DishChef.Add(model);
                    db.SaveChanges();
                    return model.DishChefId.ToString();
                }
                else
                {
                    q.DishStory = model.DishStory;
                    q.DishPicUrl = model.DishPicUrl;
                    q.DishName = model.DishName;
                   
                    q.UpdateTime = DateTime.Now;
                    db.Entry<DishChef>(q).State = EntityState.Modified;
                    db.SaveChanges();
                    string update = "update DishChef set IsApply=0 where ChefId=" + q.ChefId;
                    dataContext.ExecuteNonQuery(CommandType.Text, update);
                    return q.DishChefId.ToString();
                }
            }
            catch (Exception)
            {
                return "No";
            }
           
        }
        
        #endregion

        #region 新增(修改)调料--本地菜，创新菜--wx
        /// <summary>
        /// 新增(修改)导师调料--本地菜，创新菜--wx
        /// </summary>
        /// <param name="requestData">调料数组</param>
        /// <returns>OK，No</returns>
        [HttpPost]
        public string AddDishMaterial(List<DishMaterial> requestData)
        {
            try
            {
                //首先删除已存在菜品Id的调料
                var sql = string.Format(@"Delete DishMaterial where DishId={0}", requestData[0].DishId);
                dataContext.ExecuteNonQuery(CommandType.Text, sql);

                //保存调料信息
//                foreach (DishMaterial dm in requestData)
//                {
//                    string insert = string.Format(@"INSERT INTO  [dbo].[DishMaterial] ([DishId]
//           ,[Material]  ,[Unit],[MaterialType]  ,[CreateTime])  VALUES   ({0} ,'{1}' ,'{2}' ,{3} ,'{4}' )  ", dm.DishId, dm.Material, dm.Unit, dm.MaterialType, dm.CreateTime);
//                    //db.DishMaterial.Add(dm);
//                    //db.SaveChanges();
//                    dataContext.ExecuteNonQuery(CommandType.Text, insert);
//                }

                for (int i = 0; i < requestData.Count; i++)
                {
                    DishMaterial dm = requestData[i];
                    if (!string.IsNullOrEmpty(dm.Material) && !string.IsNullOrEmpty(dm.Unit))
                    {
                        string insert = string.Format(@"INSERT INTO  [dbo].[DishMaterial] ([DishId]
           ,[Material]  ,[Unit],[MaterialType]  ,[CreateTime])  VALUES   ({0} ,'{1}' ,'{2}' ,{3} ,'{4}' )  ", dm.DishId, dm.Material, dm.Unit, dm.MaterialType, dm.CreateTime);
                        //db.DishMaterial.Add(dm);
                        //db.SaveChanges();
                        dataContext.ExecuteNonQuery(CommandType.Text, insert);
                    }
                   
                }
               
                var dishId = (from v in db.DishChef
                              where v.DishChefId == requestData[0].DishId
                              select v
                                ).FirstOrDefault();
                string update = "update DishChef set IsApply=0 where ChefId=" + dishId.ChefId;
                dataContext.ExecuteNonQuery(CommandType.Text, update);
                    return "OK";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion 

        #region 新增(修改)菜品步骤--本地菜，创新菜--wx
        /// <summary>
        /// 新增(修改)菜品步骤--本地菜，创新菜--wx
        /// </summary>
        /// <param name="requestData">步骤数组</param>
        /// <returns></returns>
        [HttpPost]
        public string AddDishStep(List<DishStep> requestData)
        {
            try
            {
                //先删除已有调料
                var sql = string.Format(@"Delete DishStep where DishId={0}", requestData[0].DishId);
                dataContext.ExecuteNonQuery(CommandType.Text, sql);

                //保存
                int t = 1;
                foreach (DishStep i in requestData)
                {
                    if (string.IsNullOrEmpty(i.StepName))
                        continue;
                    i.StepId = t;
                    i.OpenId = "0";
                    db.DishStep.Add(i);
                    t++;
                    db.SaveChanges();
                }
                var dishId = (from v in db.DishChef
                              where v.DishChefId == requestData[0].DishId
                              select v
                                 ).FirstOrDefault();
                string update = "update DishChef set IsApply=0 where ChefId=" + dishId.ChefId;
                dataContext.ExecuteNonQuery(CommandType.Text, update);
                return "OK";
            }
            catch (Exception)
            {
                return "No";
            }
        }


        #endregion

        #region 新增(修改)厨师推荐的调料--本地菜，创新菜，报名信息编辑--web，wx
        /// <summary>
        /// 新增(修改)调料推荐--本地菜，创新菜--wx
        /// </summary>
        /// <param name="requestData">使用调料数组</param>
        /// <returns></returns>
        public string AddFR(dynamic requestData)
        {
            try
            {
                string addMatchRegion = JsonConvert.SerializeObject(requestData);
                List<FlavourRecRole> model = JsonConvert.DeserializeObject<List<FlavourRecRole>>(addMatchRegion);
                var sql = string.Format(@"Delete FlavourRecRole where RoleId={0} and DishId={1}", model[0].RoleId, model[0].DishId);
                dataContext.ExecuteNonQuery(CommandType.Text, sql);

                foreach (FlavourRecRole m in model)
                {
                    m.CreateTime = DateTime.Now;
                    m.UpdateTime = DateTime.Now;
                    db.FlavourRecRole.Add(m);
                    db.SaveChanges();
                }

              
                string update = "update DishChef set IsApply=0 where ChefId=" + model[0].RoleId;
                return "OK";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 读取菜品信息（编辑菜品时）--本地菜，创新菜--wx
        /// <summary>
        /// 读取菜品信息（编辑菜品时）--本地菜，创新菜--wx
        /// </summary>
        /// <param name="memberId">厨师ID</param>
        /// <param name="dishType">菜品类别，1，创新，2，本地</param>
        /// <returns></returns>
        [HttpGet]
        public string GetDishChefByMemberId(int memberId, int dishType)
        {
            try
            {
                var qcaID = (from v in db.ChefActivity
                             where v.IsEnable == 1
                             select v
                               ).FirstOrDefault();
                if (qcaID == null)
                    return "活动没有进行";
                var qchef = (from v in db.Chef
                             where v.MemberId == memberId && v.ChefActivityId==qcaID.ChefActivityId
                             select new
                             {
                                 ChefId = v.ChefId,
                                 ChefActivityId = v.ChefActivityId
                             }).FirstOrDefault();
                if (qchef != null)
                {
                    //获取厨师菜品基本信息
                    var qdishChef = (from v in db.DishChef
                                     where v.ChefId == qchef.ChefId && v.DishType == dishType
                                     select new
                                     {
                                         DishChefId = v.DishChefId,
                                         ChefId = v.ChefId,
                                         DishStory = v.DishStory,
                                         DishPicUrl = v.DishPicUrl,
                                         DishName=v.DishName
                                     }).FirstOrDefault();
                    //菜品调料信息
                    var dishType1DishMaterial = (from v in db.DishMaterial
                                                 where v.DishId == qdishChef.DishChefId
                                                 select new
                                                 {
                                                     DishId = v.DishId,
                                                     Material = v.Material,
                                                     Unit = v.Unit,
                                                     MaterialType = v.MaterialType
                                                 });
                    //菜品步骤信息
                    var dishType1DishStep = (from v in db.DishStep
                                             where v.DishId == qdishChef.DishChefId
                                             select new
                                             {
                                                 DishId = v.DishId,
                                                 StepId = v.StepId,
                                                 StepName = v.StepName
                                             });


                    //厨师菜品推荐调料信息
                    var sqlSelect = string.Format(@"select FR.FlavourRecId,FR.FlavourName,FRR.Unit  from FlavourRec as FR join FlavourRecRole FRR on FR.FlavourRecId=FRR.FlavourRecId where FRR.RoleId={0} and FRR.DishId={1}", qdishChef.ChefId, qdishChef.DishChefId);

                    var select = dataContext.ExecuteDataTable(CommandType.Text, sqlSelect);



                    //返回全部调料信息
                    var sqlselectAll = string.Format(@"select FlavourRecId,ChefActivityId,FlavourName,FlavourPicUrl,FlavourType from FlavourRec where ChefActivityId={0} and FlavourType=2", qchef.ChefActivityId);
                    var selectAll = dataContext.ExecuteDataTable(CommandType.Text, sqlselectAll);



                    return "{\"dish\":{\"Basic\":" + JsonConvert.SerializeObject(qdishChef) + ",\"DishMaterial\":" + JsonConvert.SerializeObject(dishType1DishMaterial) + ",\"DishStep\":" + JsonConvert.SerializeObject(dishType1DishStep) + ",\"select\":" + JsonConvert.SerializeObject(select) + ",\"AllSelect\":" + JsonConvert.SerializeObject(selectAll) + "}}";

                }
                else { return "No"; }

            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 读取活动厨师调料--本地菜，创新菜--web,wx
        /// <summary>
        /// 读取活动的厨师调料--本地菜，创新菜--web,wx
        /// </summary>
        /// <param name="caId">活动Id</param>
        /// <returns></returns>
        [HttpGet]
        public string GetFRChef(int caId)
        {
            try
            {
                var q = (from v in db.FlavourRec
                         where v.ChefActivityId == caId && v.FlavourType == 2
                         select new
                         {
                             ChefActivityId = v.ChefActivityId,
                             FlavourRecId = v.FlavourRecId,
                             FlavourName = v.FlavourName,
                             FlavourPicUrl = v.FlavourPicUrl
                         });
                return JsonConvert.SerializeObject(q);
            }
            catch (Exception)
            {
                return "[]";
            }
        }

        #endregion


        #region 参赛菜品列表(报名菜品)--参赛菜品--wx
        /// <summary>
        ///  参赛菜品列表(报名菜品)--参赛菜品--wx
        /// </summary>
        /// <param name="chefActivityId">区域Id</param>
        /// <param name="matchRegionId">赛区Id</param>
        /// <param name="openId">OpenId</param>
        /// <param name="orderBy">按照哪个排序time,count</param>
        /// <param name="isDesc">t,降序，f,升序</param>
        /// <param name="pageindex">页数</param>
        /// <returns></returns>
        [HttpGet]
        public string GetDishListApply(int chefActivityId, int matchRegionId, string openId, string orderBy, string isDesc, int pageindex)
        {
            //获取当前活动ID
            var qcaID = (from v in db.ChefActivity
                         where v.IsEnable == 1
                         select v
                               ).FirstOrDefault();

            chefActivityId = qcaID.ChefActivityId;




            if (orderBy != "time" && orderBy != "count")
                orderBy = "time";

            try
            {
                //入选菜品
                var qnewSelectSql = (from c in db.Chef
                                     join DC in db.DishChef on c.ChefId equals DC.ChefId
                                     join RM in db.RegistMember on c.MemberId equals RM.MemberId
                                     where DC.IsApply == 1 && c.ChefActivityId == chefActivityId
                                     select new
                                     {
                                         DishId = DC.DishChefId,
                                         ChefId = c.ChefId,
                                         DishName = DC.DishName,
                                         DishPicUrl = DC.DishPicUrl,
                                         DishType = DC.DishType,
                                         VisitCount = DC.VisitCount,
                                         IsSelected=DC.IsSelected,
                                         MemberName = RM.MemberName,
                                         HotelName = RM.HotelName,
                                         MtchRegionId = c.MatchRegionId,
                                         ApplyTime = DC.ApplyTime
                                     });
                if (matchRegionId > 0)
                    qnewSelectSql.Where(v => v.MtchRegionId == matchRegionId);
                var count = qnewSelectSql.Count();
                if (count == 0)
                {
                    return "{ \"Count\":\"" + 0 + "\",\"data\":[]}"; 
                }
                if (isDesc != "f" && isDesc != "t")
                    isDesc = "f";
                if (orderBy == "time")
                {
                    if (isDesc == "f")
                        qnewSelectSql = qnewSelectSql.OrderByDescending(v => v.ApplyTime);
                    if (isDesc == "t")
                        qnewSelectSql = qnewSelectSql.OrderBy(v => v.ApplyTime);
                }
                if (orderBy == "count")
                {
                    if (isDesc == "f")
                        qnewSelectSql = qnewSelectSql.OrderByDescending(v => v.VisitCount);
                    if (isDesc == "t")
                        qnewSelectSql = qnewSelectSql.OrderBy(v => v.VisitCount);
                }
                qnewSelectSql = qnewSelectSql.Skip((pageindex - 1) * 6).Take(6);

                //标记哪些收藏菜品
                var dishCollectlog = (from v in db.DishCollectLog
                                      where v.OpenId == openId
                                      select v);
                var endsql = (from v in qnewSelectSql
                              join p in dishCollectlog on v.DishId equals p.DishId into collectlog
                              from p in collectlog.DefaultIfEmpty()
                              select new
                              {
                                  DishId = v.DishId,
                                  ChefId = v.ChefId,
                                  DishName = v.DishName,
                                  DishPicUrl = v.DishPicUrl,
                                  DishType = v.DishType,
                                  VisitCount = v.VisitCount,
                                  MemberName = v.MemberName,
                                  HotelName = v.HotelName,
                                  IsCollect = p.Id == null ? "f" : "t",
                                  IsSelected= v.IsSelected
                              }).Distinct();
                return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(endsql) + "}"; 
            }
            catch (Exception)
            {
                return "{ \"Count\":\"" + 0 + "\",\"data\":[]}"; 
            }
            
        }
        #endregion

        #region 读取活动的赛区--参赛菜品--wx
        /// <summary>
        /// 读取活动赛区--参赛菜品--wx
        /// </summary>
        /// <param name="CAId">活动Id</param>
        /// <returns></returns>
        [HttpGet]
        public string GetMatchRegionByCAId(int CAId)
        {
            try
            {
                //获取当前活动ID
                var qcaID = (from v in db.ChefActivity
                             where v.IsEnable == 1
                             select v
                                   ).FirstOrDefault();

                CAId = qcaID.ChefActivityId;
                var qMatchRegion = (from v in db.MatchRegion
                                    where v.ChefActivityId == CAId 
                                    select v).GroupBy(a => a.ProvinceName).ToList();
                string json = "[";
                for (int i = 0; i < qMatchRegion.Count; i++)
                {
                    var ddddd = qMatchRegion[i].Key;
                    var sql = string.Format(@"select MatchRegionId,AreaName,ChefActivityId from MatchRegion where ChefActivityId={0} and  ProvinceName='{1}'", CAId, qMatchRegion[i].Key);
                    var qAreaName = dataContext.ExecuteDataTable(CommandType.Text, sql);

                    if (qAreaName.Rows.Count > 0)
                    {
                        json += "{\"ProvinceName\":\"" + qMatchRegion[i].Key + "\"," + "\"AreaName\":" + JsonConvert.SerializeObject(qAreaName) + "}";
                        if (i != (qMatchRegion.Count - 1))
                            json += ",";
                    }
                }
                json += "]";

                return json;
            }
            catch (Exception)
            {
                return "[]";
            }
        }

        #endregion

        #region 参赛菜品列表(投票菜品)--参赛菜品--wx
        /// <summary>
        ///  参赛菜品列表(投票菜品)--参赛菜品--wx
        /// </summary>
        /// <param name="chefActivityId">区域Id</param>
        /// <param name="matchRegionId">赛区Id</param>
        /// <param name="openId">OpenId</param>
        /// <param name="isDesc">t,降序，f,升序</param>
        /// <param name="pageindex">页数</param>
        /// <returns></returns>
        [HttpGet]
        public string GetDishListApplySelected(int chefActivityId, int matchRegionId, string openId, string isDesc, int pageindex)
        {
          
            try
            {
                //获取当前活动ID
                var qcaID = (from v in db.ChefActivity
                             where v.IsEnable == 1
                             select v
                                   ).FirstOrDefault();

                chefActivityId = qcaID.ChefActivityId;
                //入选菜品
                var qnewSelectSql = (from c in db.Chef
                                     join DC in db.DishChef on c.ChefId equals DC.ChefId
                                     join RM in db.RegistMember on c.MemberId equals RM.MemberId
                                     where DC.IsApply == 1 &&DC.IsSelected==1 && c.ChefActivityId == chefActivityId
                                     select new
                                     {
                                         DishId = DC.DishChefId,
                                         ChefId = c.ChefId,
                                         DishName = DC.DishName,
                                         DishPicUrl = DC.DishPicUrl,
                                         DishType = DC.DishType,
                                         VisitCount = DC.VisitCount,
                                         MemberName = RM.MemberName,
                                         HotelName = RM.HotelName,
                                         SelectedCount = DC.SelectedCount,
                                         MtchRegionId = c.MatchRegionId,
                                         ApplyTime = DC.ApplyTime
                                     });
                if (matchRegionId > 0)
                    qnewSelectSql.Where(v => v.MtchRegionId == matchRegionId);
                var count = qnewSelectSql.Count();
                if (count == 0)
                {
                    return "{ \"Count\":\"" + 0 + "\",\"data\":[]}";
                }
                if (isDesc != "f" && isDesc != "t")
                    isDesc = "f";
                if (isDesc == "f")
                    qnewSelectSql = qnewSelectSql.OrderByDescending(v => v.SelectedCount);
                if (isDesc == "t")
                    qnewSelectSql = qnewSelectSql.OrderBy(v => v.SelectedCount);
                
                qnewSelectSql = qnewSelectSql.Skip((pageindex - 1) * 6).Take(6);

                //标记哪些收藏菜品
                var dishCollectlog = (from v in db.DishCollectLog
                                      where v.OpenId == openId
                                      select v);
                var endsql = (from v in qnewSelectSql
                              join p in dishCollectlog on v.DishId equals p.DishId into collectlog
                              from p in collectlog.DefaultIfEmpty()
                              select new
                              {
                                  DishId = v.DishId,
                                  ChefId = v.ChefId,
                                  DishName = v.DishName,
                                  DishPicUrl = v.DishPicUrl,
                                  DishType = v.DishType,
                                  VisitCount = v.VisitCount,
                                  SelectCount=v.SelectedCount,
                                  MemberName = v.MemberName,
                                  HotelName = v.HotelName,
                                  IsCollect = p.Id == null ? "f" : "t"
                              });
                return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(endsql) + "}";
            }
            catch (Exception)
            {
                return "{ \"Count\":\"" + 0 + "\",\"data\":[]}";
            }
        }
        #endregion

        #region 增加看过信息，获取选手菜品详情--选手菜品详情--wx
        /// <summary>
        /// 获取选手菜品详情--选手菜品详情--wx
        /// </summary>
        /// <param name="requestData">看过Json</param>
        /// <returns></returns>
        [HttpPost]
        public string GetDishChefDetailByDishId(dynamic requestData)
        {
            try
            {
                //首先增加看过统计
                var q = JsonConvert.SerializeObject(requestData);
                DishVisitLog m = JsonConvert.DeserializeObject<DishVisitLog>(q);
                db.DishVisitLog.Add(m);
                db.SaveChanges();
                var sql = string.Format(@"update DishChef set VisitCount=VisitCount+1 where DishChefId={0}", m.DishId);
                dataContext.ExecuteNonQuery(CommandType.Text, sql); 





                //菜品基本信息
                var dishType = (from v in db.DishChef
                                where v.DishChefId == m.DishId
                                select new {
                                    DishChefId=v.DishChefId,
                                    DishTyPe=v.DishType,
                                    ChefId=v.ChefId,
                                    DishPicUrl= v.DishPicUrl,
                                    DishName= v.DishName,
                                    PrasieCount=v.PrasieCount,
                                    VisitCount=v.VisitCount,
                                    DishStory=v.DishStory

                                }).FirstOrDefault();
                //菜品调料信息
                var dishType1DishMateria = (from v in db.DishMaterial
                                             where v.DishId == dishType.DishChefId
                                             select new
                                             {
                                                 DishId = v.DishId,
                                                 Material = v.Material,
                                                 Unit = v.Unit,
                                                 MaterialType = v.MaterialType
                                             });
                
                
                //菜品推荐选择的使用量
                var sqlSelectUnit = string.Format(@"select FR.FlavourRecId,FR.FlavourName,FRR.Unit  from FlavourRec as FR join FlavourRecRole FRR on FR.FlavourRecId=FRR.FlavourRecId where FRR.RoleId={0} and FRR.DishId={1}", dishType.ChefId, dishType.DishChefId);

                var selectUnit = dataContext.ExecuteDataTable(CommandType.Text, sqlSelectUnit);











                //菜品步骤信息
                var dishType1DishStep = (from v in db.DishStep
                                         where v.DishId == dishType.DishChefId
                                         select new
                                         {
                                             DishId = v.DishId,
                                             StepId = v.StepId,
                                             StepName = v.StepName
                                         });

                //菜品推荐调料信息
                var sqlSelect = string.Format(@"select FR.FlavourName,FR.FlavourPicUrl,FR.FlavourType,FR.FlavourRecId  from FlavourRec as FR join FlavourRecRole FRR on FR.FlavourRecId=FRR.FlavourRecId where FRR.RoleId={0} and FRR.DishId={1}", dishType.ChefId, dishType.DishChefId);

                var select = dataContext.ExecuteDataTable(CommandType.Text, sqlSelect);

                //是否点赞过
                var Parse = (from v in db.DishPrasieLog
                               where v.OpenId == m.OpenId && v.DishId == dishType.DishChefId
                               select v).FirstOrDefault();
                var IsPrasie = "f";
                if (Parse != null)
                    IsPrasie = "t";


                //菜品是否收藏
                var isDishChefColle = (from v in db.DishCollectLog
                                       where v.DishId == m.DishId && v.OpenId == m.OpenId
                                       select v).FirstOrDefault();
                var isCollect = "f";
                if (isDishChefColle != null)
                    isCollect = "t";




                return "{\"dishInfo\":" + JsonConvert.SerializeObject(dishType) + ",\"dishType1DishMateria\":" + JsonConvert.SerializeObject(dishType1DishMateria) + ",\"selectDishMateria\":" + JsonConvert.SerializeObject(selectUnit) + ",\"dishType1DishStep\":" + JsonConvert.SerializeObject(dishType1DishStep) + ",\"select\":" + JsonConvert.SerializeObject(select) + ",\"IsPrasie\":\"" + IsPrasie + "\",\"IsCollect\":\"" + isCollect + "\"}";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 增加选手菜品点赞--选手菜品详情,投票菜品详情--wx
        /// <summary>
        ///  增加选手菜品点赞--选手菜品详情,投票菜品详情--wx
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddDishPrasieLog(dynamic requestData)
        {
            try
            {
                // 先判断当前用户是否点赞过本菜品，如果点赞了，不在增加，如果没有增加
                //都返回当前菜品的ID号和点赞数

                var q = JsonConvert.SerializeObject(requestData);
                DishPrasieLog m = JsonConvert.DeserializeObject<DishPrasieLog>(q);

                var qDishPrasieLog = (from v in db.DishPrasieLog
                                      where v.DishId == m.DishId && v.OpenId == m.OpenId && v.DishType == m.DishType
                                      select new
                                      {
                                          DishId = v.DishId
                                      }).FirstOrDefault();

                if (qDishPrasieLog == null)
                {
                    db.DishPrasieLog.Add(m);
                    db.SaveChanges();
                    var sql = string.Format(@"update DishChef set PrasieCount=PrasieCount+1 where DishChefId={0}", m.DishId);
                    dataContext.ExecuteNonQuery(CommandType.Text, sql);
                    var qTutorDishId = (from v in db.DishChef
                                        where v.DishChefId == m.DishId
                                        select new
                                        {
                                            TutorDishId = v.DishChefId,
                                            PrasieCount = v.PrasieCount
                                        }).FirstOrDefault();
                    return JsonConvert.SerializeObject(qTutorDishId);
                }

                else
                {
                    return "No";
                }
            }
            catch (Exception)
            {
                return "No";
            }

        }

        #endregion

        #region 选手介绍--选手菜品详情--wx
        /// <summary>
        /// 选手介绍--选手菜品详情--wx
        /// </summary>
        /// <param name="dishId">菜品Id</param>
        /// <returns></returns>
        [HttpGet]
        public string GetChefInfo(int dishId)
        {

            var chefInfoNoLink = (from v in db.DishChef
                            where v.DishChefId == dishId
                            select new { 
                                DishId=v.DishChefId,
                                DishName=v.DishName,
                                ChefId=v.ChefId,
                                DishType=v.DishType
                            });
            var chefInfo = (from v in db.Chef
                            join p in chefInfoNoLink on v.ChefId equals p.ChefId
                            join q in db.RegistMember on v.MemberId equals q.MemberId
                            join b in db.MatchRegion on v.MatchRegionId equals b.MatchRegionId
                            select new { 
                                MemberName=q.MemberName,
                                HotelName=q.HotelName,
                                AreaName=b.AreaName,
                                HeadPicUrl=v.ChefPicUrl  
                            }).FirstOrDefault();
            var zhong = chefInfoNoLink.FirstOrDefault();

            var chefInfoLink = (from v in db.DishChef
                                where v.DishChefId != dishId && v.ChefId == zhong.ChefId
                                  select new
                                  {
                                      DishId = v.DishChefId,
                                      DishName = v.DishName,
                                      ChefId = v.ChefId,
                                      DishType = v.DishType
                                  }).FirstOrDefault();

            return "{\"ChefInfo\":" + JsonConvert.SerializeObject(chefInfo) + ",\"chefInfoNoLink\":" + JsonConvert.SerializeObject(zhong) + ",\"chefInfoLink\":" + JsonConvert.SerializeObject(chefInfoLink) + "}";
        }
        #endregion

        #region 增加选手菜品留言信息--选手菜品详情,投票菜品详情--wx
        /// <summary>
        /// 增加选手菜品留言信息--选手菜品详情,投票菜品详情--wx
        /// </summary>
        /// <param name="requsetData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddDishComment(dynamic requestData)
        {
            try
            {
                //增加留言
                string addDishComment = JsonConvert.SerializeObject(requestData);
                DishComment model = JsonConvert.DeserializeObject<DishComment>(addDishComment);
                db.DishComment.Add(model);
                db.SaveChanges();


                var q = (from v in db.DishComment
                         where v.DishId == model.DishId
                         select v);
                var count = q.Count();
                var lastq = q.OrderByDescending(v => v.CreateTime).Skip(0).Take(3);
                //查找HotelName
                var lastResult = (from v in lastq
                                  join p in db.OpenIdAssociated on v.OpenId equals p.OpenId
                                  join n in db.RegistMember on p.UserId equals n.MemberId
                                  select new
                                  {
                                      HeadPicUrl = v.HeadPicUrl,
                                      MemebName = v.MemberName,
                                      Comment = v.Comment,
                                      CreateTime = v.CreateTime,
                                      HotelName = n.HotelName
                                  });


                return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(lastResult, DateTimeConvent.DateTimehh()) + "}";
            }
            catch (Exception)
            {

                return "{\"Count\":0}";
            }


        }
        #endregion

        #region 选手菜品留言信息--选手菜品详情,投票菜品详情--wx,web
        /// <summary>
        /// 选手菜品留言信息--选手菜品详情,投票菜品详情--wx
        /// </summary>
        /// <param name="dishId">菜品ID</param>
        /// <returns></returns>
        [HttpGet]
        public string GetDishChefComment(int dishId, int index, int pageSize)
        {
            var q = (from v in db.DishComment
                     where v.DishId == dishId
                     select v);
            var count = q.Count();
            var lastq = q.OrderByDescending(v=>v.CreateTime).Skip((index - 1) * pageSize).Take(pageSize);
            //查找HotelName
            var lastResult = (from v in lastq 
                              join p in db.OpenIdAssociated on v.OpenId equals p.OpenId
                              join n in db.RegistMember on p.UserId equals n.MemberId
                              select new
                              {
                                  HeadPicUrl = v.HeadPicUrl,
                                  MemebName = v.MemberName,
                                  Comment = v.Comment,
                                  CreateTime = v.CreateTime,
                                  HotelName = n.HotelName
                              });
            return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(lastResult,DateTimeConvent.DateTimehh()) + "}";
        }
        #endregion

        #region  增加看过信息，获取投票菜品信息--投票菜品详情--wx
        /// <summary>
        /// 增加看过信息，获取投票菜品信息--投票菜品详情--wx
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetSelectedDishChef(dynamic requestData)
        {
            try
            {
                //首先增加看过统计
                var q = JsonConvert.SerializeObject(requestData);
                DishVisitLog m = JsonConvert.DeserializeObject<DishVisitLog>(q);
                db.DishVisitLog.Add(m);
                db.SaveChanges();
                var sql = string.Format(@"update DishChef set VisitCount=VisitCount+1 where DishChefId={0}", m.DishId);
                dataContext.ExecuteNonQuery(CommandType.Text, sql);





                //菜品基本信息
                var dishType = (from v in db.DishChef
                                where v.DishChefId == m.DishId
                                select new
                                {
                                    DishChefId = v.DishChefId,
                                    DishTyPe = v.DishType,
                                    ChefId = v.ChefId,
                                    DishPicUrl = v.DishPicUrl,
                                    DishName = v.DishName,
                                    PrasieCount = v.PrasieCount,
                                    VisitCount = v.VisitCount,
                                    SelectedCount= v.SelectedCount,
                                    DishStory = v.DishStory

                                }).FirstOrDefault();
                //菜品调料信息
                var dishType1DishMateria = (from v in db.DishMaterial
                                            where v.DishId == dishType.DishChefId
                                            select new
                                            {
                                                DishId = v.DishId,
                                                Material = v.Material,
                                                Unit = v.Unit,
                                                MaterialType = v.MaterialType
                                            });

                //菜品推荐选择的使用量
                var sqlSelectUnit = string.Format(@"select FR.FlavourRecId,FR.FlavourName,FRR.Unit  from FlavourRec as FR join FlavourRecRole FRR on FR.FlavourRecId=FRR.FlavourRecId where FRR.RoleId={0} and FRR.DishId={1}", dishType.ChefId, dishType.DishChefId);

                var selectUnit = dataContext.ExecuteDataTable(CommandType.Text, sqlSelectUnit);






                //菜品步骤信息
                var dishType1DishStep = (from v in db.DishStep
                                         where v.DishId == dishType.DishChefId
                                         select new
                                         {
                                             DishId = v.DishId,
                                             StepId = v.StepId,
                                             StepName = v.StepName
                                         });

                



                //菜品推荐调料信息
                var sqlSelect = string.Format(@"select FR.FlavourName,FR.FlavourPicUrl,FR.FlavourType,FR.FlavourRecId  from FlavourRec as FR join FlavourRecRole FRR on FR.FlavourRecId=FRR.FlavourRecId where FRR.RoleId={0} and FRR.DishId={1}", dishType.ChefId, dishType.DishChefId);

                var select = dataContext.ExecuteDataTable(CommandType.Text, sqlSelect);

                //是否点赞过
                var Parse = (from v in db.DishPrasieLog
                             where v.OpenId == m.OpenId && v.DishId == dishType.DishChefId
                             select v).FirstOrDefault();
                var IsPrasie = "f";
                if (Parse != null)
                    IsPrasie = "t";


                //是否投票
                var Selected = (from v in db.DishSelectedLog
                                where v.OpenId == m.OpenId && v.DishId == m.DishId
                                select v).FirstOrDefault();
                var IsSelected = "f";
                if (Selected != null)
                    IsSelected = "t";


                
                //菜品是否收藏
                var isDishChefColle = (from v in db.DishCollectLog
                                       where v.DishId == m.DishId && v.OpenId == m.OpenId
                                       select v).FirstOrDefault();
                var isCollect = "f";
                if (isDishChefColle != null)
                    isCollect = "t";


                //菜品所在赛区的截止报名时间
                var matchRegion = (from v in db.Chef
                                   join p in db.MatchRegion on v.MatchRegionId equals p.MatchRegionId
                                   where v.ChefId == dishType.ChefId
                                   select new { 
                                      ChefId= v.ChefId,
                                      MatchRegionId=v.MatchRegionId,
                                      EndTime=p.EndTime
                                   }).FirstOrDefault();
                string isEnd = "t";//未开始投票
                if (matchRegion != null)
                {
                    if (matchRegion.EndTime > DateTime.Now || matchRegion.EndTime == DateTime.Now)
                        isEnd = "f";
                    if (matchRegion.EndTime < DateTime.Now)
                        isEnd = "t";
                }



                return "{\"dishInfo\":" + JsonConvert.SerializeObject(dishType) + ",\"dishType1DishMateria\":" + JsonConvert.SerializeObject(dishType1DishMateria) + ",\"selectDishMateria\":" + JsonConvert.SerializeObject(selectUnit) + ",\"dishType1DishStep\":" + JsonConvert.SerializeObject(dishType1DishStep) + ",\"select\":" + JsonConvert.SerializeObject(select) + ",\"IsPrasie\":\"" + IsPrasie + "\",\"IsSelected\":\"" + IsSelected + "\",\"IsCollect\":\"" + isCollect + "\",\"IsEnd\":\"" + isEnd + "\"}";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 增加投票数量--投票菜品详情--wx
        /// <summary>
        /// 增加投票数量--投票菜品详情--wx
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddDishSelectedLog(dynamic requestData)
        {
            try
            {
                // 先判断当前用户是否点赞过本菜品，如果点赞了，不在增加，如果没有增加
                //都返回当前菜品的ID号和点赞数

                var q = JsonConvert.SerializeObject(requestData);
                DishSelectedLog m = JsonConvert.DeserializeObject<DishSelectedLog>(q);

                var qDishSelectedLog = (from v in db.DishSelectedLog
                                        where v.DishId == m.DishId && v.OpenId == m.OpenId && v.DishType == m.DishType
                                        select new
                                        {
                                            DishId = v.DishId
                                        }).FirstOrDefault();

                if (qDishSelectedLog == null)
                {
                    db.DishSelectedLog.Add(m);
                    db.SaveChanges();
                    var sql = string.Format(@"update DishChef set SelectedCount=SelectedCount+1 where DishChefId={0}", m.DishId);
                    dataContext.ExecuteNonQuery(CommandType.Text, sql);
                    var qTutorDishId = (from v in db.DishChef
                                        where v.DishChefId == m.DishId
                                        select new
                                        {
                                            TutorDishId = v.DishChefId,
                                            SelectedCount = v.SelectedCount
                                        }).FirstOrDefault();
                    return JsonConvert.SerializeObject(qTutorDishId);
                }
                else {
                    return "No";
                }
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 厨师菜品后台，菜品审核

        #region 获取报名信息列表--报名信息--web

        /// <summary>
        /// 获取报名信息列表--报名信息--web
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetDishChefList(dynamic requestData)
        {
            try
            {
                var chefActivityId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.ChefActivityId));
                var pageIndex = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.pageIndex));
                var sql =
                    string.Format(@"SELECT SP.ChefId AS ChefId,SP.MemberId AS MemberId,SP.CreateTime AS CreateTime,SP.MatchRegionId AS MatchRegionId,SP.IsApply AS IsApply,SP.ChefActivityId AS ChefActivityId,SP.DishChefOneId,SP.DishOne AS DishOne,SP.DishChefTwoId,SP.DishTow AS DishTow,SP.MemberName AS MemberName,SP.HotelName AS HotelName,dc.DishName AS DishNameSelected,dc.SelectedCount AS SelectedCount,mr.AreaName  AS AreaName FROM  
(SELECT    BS.* ,rm.MemberName,rm.HotelName  FROM  (SELECT T.ChefId,T.MemberId,T.CreateTime,T.MatchRegionId,TD.IsApply, T.ChefActivityId,TD.DishChefOneId,TD.DishOne,TD.DishChefTwoId,TD.DishTow 
from Chef as T LEFT  join(select case when tab1.ChefId is null then tab2.ChefId else tab1.ChefId end   ChefId,tab1.IsApply,tab1.DishChefOneId,tab1.菜品1 as DishOne,tab2.DishChefTwoId,tab2.菜品2 as DishTow from 
(select ChefId,IsApply,DishChefId as DishChefOneId,DishName as '菜品1' from DishChef where DishType = 1) tab1 
full join 
(select ChefId,IsApply,DishChefId as DishChefTwoId,DishName as '菜品2' from DishChef where DishType = 2) tab2 
on tab1.ChefId = tab2.ChefId) as TD on T.ChefId=TD.ChefId  where T.ChefActivityId={0}) AS BS  LEFT JOIN dbo.RegistMember AS rm   ON rm.MemberId=BS.MemberId)
AS SP LEFT JOIN (SELECT * from dbo.DishChef WHERE IsSelected=1 ) AS dc ON SP.ChefId=dc.ChefId  LEFT
JOIN dbo.MatchRegion AS mr ON SP.MatchRegionId=mr.MatchRegionId
WHERE 1=1", chefActivityId);
//                    string.Format(@"
//SELECT SP.ChefId AS ChefId,SP.MemberId AS MemberId,SP.CreateTime AS CreateTime,SP.MatchRegionId AS MatchRegionId,SP.IsApply AS IsApply,SP.ChefActivityId AS ChefActivityId,SP.DishOne AS DishOne,SP.DishTow AS DishTow,SP.MemberName AS MemberName,SP.HotelName AS HotelName,dc.DishName AS DishNameSelected,dc.SelectedCount AS SelectedCount,mr.AreaName  AS AreaName FROM  
//(SELECT    BS.* ,rm.MemberName,rm.HotelName  FROM  (SELECT T.ChefId,T.MemberId,T.CreateTime,T.MatchRegionId,TD.IsApply, T.ChefActivityId,TD.DishOne,TD.DishTow 
//from Chef as T  join(select case when tab1.ChefId is null then tab2.ChefId else tab1.ChefId end   ChefId,tab1.IsApply,tab1.菜品1 as DishOne,tab2.菜品2 as DishTow from 
//(select ChefId,IsApply,DishName as '菜品1' from DishChef where DishType = 1) tab1 
//full join 
//(select ChefId,IsApply,DishName as '菜品2' from DishChef where DishType = 2) tab2 
//on tab1.ChefId = tab2.ChefId) as TD on T.ChefId=TD.ChefId  where T.ChefActivityId={0}) AS BS  LEFT JOIN dbo.RegistMember AS rm   ON rm.MemberId=BS.MemberId)
//AS SP LEFT JOIN (SELECT * from dbo.DishChef WHERE IsSelected=1 ) AS dc ON SP.ChefId=dc.ChefId 
//JOIN dbo.MatchRegion AS mr ON SP.MatchRegionId=mr.MatchRegionId
//WHERE 1=1 ", chefActivityId);


                var IsApply = requestData.IsApply;
                if (IsApply != null )
                {
                    
                    var isApply = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(IsApply));

                    if (isApply>-1&& isApply<4)
                    {
                        sql += " and SP.IsApply=" + isApply.ToString(); 
                    }
                }

                var MatchRegionId = requestData.MatchRegionId;

                if (MatchRegionId != null )
                {
                    var matchRegionId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(MatchRegionId));

                    if (MatchRegionId>0)
                    {
                        sql += " and SP.MatchRegionId=" + matchRegionId.ToString(); 
                    }
                }


                var BeginTime = requestData.BeginTime;
                if (BeginTime != null)
                {
                    var beginTime = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(BeginTime));
                    if (!string.IsNullOrEmpty(beginTime))
                    {
                        sql += " and  SP.CreateTime>'" + beginTime + "'"; 
                    }
                }

                var EndTime = requestData.EndTime;
                if (EndTime != null)
                {
                    var endTime = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(EndTime));
                    if (!string.IsNullOrEmpty(endTime))
                    {
                        sql += " and  SP.CreateTime<'" + endTime + "'"; 
                    }
                }

                var Name = requestData.Name;
                if (Name != null)
                {
                    var name = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(Name));
                    if (!string.IsNullOrEmpty(name))
                    {
                        sql += " and (SP.DishOne like '%" + name + "%' or  SP.DishTow like '%" + name + "%' or SP.MemberName like '%" + name + "%')"; 
                    }
                }

                var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
                var count = q.Rows.Count;

                if (pageIndex <= 0)
                    pageIndex = 1;
                var lastq = imgHandle.GetPagedTable(q, pageIndex,10);

                return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(lastq,DateTimeConvent.DateTimedd()) + "}";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        
        #endregion

        #region 获取赛区--报名信息--web
        /// <summary>
        /// 根据活动ID获取赛区--报名信息--web
        /// </summary>
        /// <param name="caId">活动ID</param>
        /// <returns>赛区信息</returns>
        [HttpGet]
        public string GetAllByChefActivityId(int caId)
        {
            try
            {
                var q = (from v in db.MatchRegion
                         where v.ChefActivityId == caId
                         select new
                         {
                             MatchRegionId = v.MatchRegionId,
                             AreaName = v.AreaName,
                         });
                
                return JsonConvert.SerializeObject(q);
            }
            catch (Exception)
            {
                return "No";
            }
        }




        #endregion

        #region 获取设置投票菜品--报名信息--web
        /// <summary>
        /// 获取设置投票菜品--报名信息--web
        /// </summary>
        /// <param name="chefId">厨师Id</param>
        /// <returns></returns>
        [HttpGet]
        public string GetSetSelectedDishByChefId(int chefId)
        {
            try
            {
                var qDishChef = (from v in db.DishChef
                                 where v.ChefId == chefId
                                 select new
                                 {
                                     DishChefId = v.DishChefId,
                                     DishName = v.DishName,
                                     IsSelected=v.IsSelected,
                                     DishType = v.DishType
                                 });

                return JsonConvert.SerializeObject(qDishChef);
            }
            catch (Exception)
            {
                return "No";
            }
        }


        #endregion

        #region 设置为投票菜品--报名信息--web
        /// <summary>
        /// 设置为投票菜品--报名信息--web
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string SetSelectedDihsChef(dynamic requestData)
        {
            try
            {
                var DishChefId = requestData.DishChefId;

                var dishChefId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(DishChefId));

                var ChefId = requestData.ChefId;

                var chefId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(ChefId));

                var SelectedPerson = requestData.SelectedPerson;
                var selectedPerson = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(SelectedPerson));


                var sqlSelected = string.Format(@"update DishChef set IsSelected=1 ,SelectedTime='{1}',SelectedPerson='{2}' where DishChefId={0}", dishChefId, DateTime.Now.ToString(), SelectedPerson);

                dataContext.ExecuteNonQuery(CommandType.Text, sqlSelected);

                var sqlNoSelected = string.Format(@"update DishChef set IsSelected=2 where DishChefId!={0} and ChefId={1}", dishChefId, chefId);

                dataContext.ExecuteNonQuery(CommandType.Text, sqlNoSelected);
                return "Ok";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        
        #endregion

        #region 获取厨师的基本信息(包括OpenId)--报名信息审核--web
        /// <summary>
        /// 获取厨师的基本信息(包括OpenId)--报名信息审核--web
        /// </summary>
        /// <param name="chefId">厨师Id</param>
        /// <returns></returns>
        [HttpGet]
        public string GetChefByChefId(int chefId)
        {
            try
            {
                var sql = string.Format(@"select c.ChefId,r.MemberName,c.Birthday,m.AreaName,c.ApplyType,c.ReferrerName,c.ChefActivityId,c.ChefPicUrl,c.ChefHotelPicUrl,c.MatchRegionId,p.OpenId ,r.HotelName as ChefHotelName from Chef as c LEFT JOIN OpenIdAssociated as p ON
c.MemberId=p.UserId
LEFT JOIN MatchRegion as m on c.MatchRegionId=m.MatchRegionId
LEFT JOIN RegistMember as r on c.MemberId=r.MemberId
where c.ChefId={0}", chefId);
                var db = dataContext.ExecuteDataTable(CommandType.Text, sql);
                return JsonConvert.SerializeObject(db, DateTimeConvent.DateTimedd());
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 获取菜品信息--报名信息审核--web
        /// <summary>
        ///  获取菜品信息--报名信息审核--web
        /// </summary>
        /// <param name="chefId">厨师Id</param>
        /// <param name="dishType">菜品类别</param>
        /// <returns></returns>
        [HttpGet]
        public string GetAuditDishChef(int chefId, int dishType)
        {
            try
            {
                //获取厨师菜品基本信息
                var qdishChef = (from v in db.DishChef
                                 where v.ChefId == chefId && v.DishType == dishType
                                 select new
                                 {
                                     DishChefId = v.DishChefId,

                                     ChefId = v.ChefId,
                                     DishStory = v.DishStory,
                                     DishPicUrl = v.DishPicUrl,
                                     DishName = v.DishName
                                 }).FirstOrDefault();
                //菜品调料信息
                var dishType1DishMaterial = (from v in db.DishMaterial
                                             where v.DishId == qdishChef.DishChefId
                                             select new
                                             {
                                                 DishId = v.DishId,
                                                 Material = v.Material,
                                                 Unit = v.Unit,
                                                 MaterialType = v.MaterialType
                                             });
                //菜品步骤信息
                var dishType1DishStep = (from v in db.DishStep
                                         where v.DishId == qdishChef.DishChefId
                                         select new
                                         {
                                             DishId = v.DishId,
                                             StepId = v.StepId,
                                             StepName = v.StepName
                                         });



                //厨师菜品推荐调料信息
                var sqlSelect = string.Format(@"select FR.FlavourRecId,FR.FlavourName,FRR.Unit  from FlavourRec as FR join FlavourRecRole FRR on FR.FlavourRecId=FRR.FlavourRecId where FRR.RoleId={0} and FRR.DishId={1}", qdishChef.ChefId, qdishChef.DishChefId);

                var select = dataContext.ExecuteDataTable(CommandType.Text, sqlSelect);


                //全部厨师推荐的调料
                var chefInfo = (from v in db.Chef
                                where v.ChefId == chefId
                                select v
                                  ).FirstOrDefault();

                var flavourRecInfo = (from v in db.FlavourRec
                                      where v.ChefActivityId == chefInfo.ChefActivityId && v.FlavourType == 2
                                      select new
                                      {
                                          ChefActivityId = v.ChefActivityId,
                                          FlavourRecId = v.FlavourRecId,
                                          FlavourName = v.FlavourName,
                                          FlavourPicUrl = v.FlavourPicUrl
                                      });

                return "{\"dish\":{\"Basic\":" + JsonConvert.SerializeObject(qdishChef) + ",\"DishMaterial\":" + JsonConvert.SerializeObject(dishType1DishMaterial) + ",\"DishStep\":" + JsonConvert.SerializeObject(dishType1DishStep) + "},\"FlavourRec\":" + JsonConvert.SerializeObject(flavourRecInfo) + ",\"select\":" + JsonConvert.SerializeObject(select) + "}";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion


        #region 设置审核通过--报名信息审核--web
        /// <summary>
        /// 设置审核通过--报名信息审核--web
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string SetApplyDihsChef(dynamic requestData)
        {

            try
            {
                int chefId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.ChefId));
                string ApplyPerson = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.ApplyPerson));
                var openId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.OpenId));
                var isPass = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.IsApply));

                var context = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.Context));


                //var chefId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.ChefId));
                var matchName = "";


                string sql = string.Format(@" update DishChef set IsApply={0},ApplyTime='{2}',ApplyPerson='{3}' where ChefId={1} ", isPass, chefId, DateTime.Now.ToString(), ApplyPerson);

                dataContext.ExecuteNonQuery(CommandType.Text, sql);

                #region 发送模板消息
               // 根据厨师ID查询活动信息

                var ChefActivity = (from v in db.Chef
                                    join p in db.ChefActivity on v.ChefActivityId equals p.ChefActivityId
                                    select p).FirstOrDefault();

                matchName = ChefActivity.ChefActivityName;
                imgHandle.SentTemplate(openId, isPass, matchName, context); 
                #endregion

                return "OK";
            }
            catch (Exception)
            {
                return "No";
            }
            
        }

        #endregion

        #region 厨师信息编辑--报名信息编辑--web
        /// <summary>
        /// 厨师信息编辑--报名信息编辑--web
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string UpdateChef(dynamic requestData)
        {
            try
            {
                string query = JsonConvert.SerializeObject(requestData);
                Chef model = JsonConvert.DeserializeObject<Chef>(query);
                if (model.Birthday == Convert.ToDateTime("0001/1/1 0:00:00"))
                {
                    model.Birthday = Convert.ToDateTime("1900-1-1");
                }
                var q = (from v in db.Chef
                         where v.ChefId==model.ChefId
                         select v
                       ).FirstOrDefault();

                model.ChefId = 0;
                //q.MemberId = model.MemberId;
                //q.ChefActivityId = model.ChefActivityId;
                q.MatchRegionId = model.MatchRegionId;
                q.ApplyType = model.ApplyType;
                q.ReferrerName = model.ReferrerName;
                q.Birthday = model.Birthday;
                q.ChefPicUrl = model.ChefPicUrl;
                //q.ChefHotelName = model.ChefHotelName;
                q.ChefHotelPicUrl = model.ChefHotelPicUrl;
                db.Entry<Chef>(q).State = EntityState.Modified;
                db.SaveChanges();
                return "OK";
              
            }
            catch (Exception)
            {
                return "No";
            }
        }
        
        #endregion

        #region 菜品信息编辑--报名信息编辑--web
        /// <summary>
        /// 菜品信息编辑--报名信息编辑--web
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string UpdateDishChef(dynamic requestData)
        {
            try
            {
                #region 修改(新增)菜品基本信息
                string dishChef = JsonConvert.SerializeObject(requestData.dishChef);
                DishChef model = JsonConvert.DeserializeObject<DishChef>(dishChef);

                var q = (from v in db.DishChef
                         where v.DishChefId == model.DishChefId
                         select v).FirstOrDefault();
                int dishChefId = 0;

                if (q == null)
                {
                    model.CreateTime = DateTime.Now;
                    model.UpdateTime = DateTime.Now;
                    model.ApplyTime = Convert.ToDateTime("1900-01-01");
                    model.SelectedTime = Convert.ToDateTime("1900-01-01");
                    db.DishChef.Add(model);
                    db.SaveChanges();
                    //return model.DishChefId.ToString();
                    dishChefId = model.DishChefId;
                }

                else
                {
                    q.DishStory = model.DishStory;
                    q.DishPicUrl = model.DishPicUrl;
                    q.DishName = model.DishName;
                    q.UpdateTime = DateTime.Now;
                    db.Entry<DishChef>(q).State = EntityState.Modified;
                    db.SaveChanges();
                    dishChefId = q.DishChefId;
                }
                #endregion


                #region 修改菜品调料

                var middledishMaterial = requestData.dishMaterial;
                //首先删除已存在菜品Id的调料
                List<DishMaterial> qdishMaterial = new List<DishMaterial>();

                for (int i = 0; i < middledishMaterial.Count; i++)
                {
                    qdishMaterial.Add(JsonConvert.DeserializeObject<DishMaterial>(JsonConvert.SerializeObject(middledishMaterial[i])));
                }

                var sql = string.Format(@"Delete DishMaterial where DishId={0}", dishChefId);
                dataContext.ExecuteNonQuery(CommandType.Text, sql);

                //保存调料信息
                foreach (DishMaterial dm in qdishMaterial)
                {
                    dm.DishId = dishChefId;
                    db.DishMaterial.Add(dm);
                    //db.SaveChanges();
                }
                

                #endregion

                #region 修改菜品步骤

                var middleDishStep = requestData.dishStep;
                //先删除已有调料


                List<DishStep> qDishStep = new List<DishStep>();

                for (int i = 0; i < middleDishStep.Count; i++)
                {
                    qDishStep.Add(JsonConvert.DeserializeObject<DishStep>(JsonConvert.SerializeObject(middleDishStep[i])));
                }
                string sqlDishStep = string.Format(@"Delete DishStep where DishId={0}", dishChefId);
                dataContext.ExecuteNonQuery(CommandType.Text, sqlDishStep);
                //保存
                int t = 1;
                foreach (DishStep i in qDishStep)
                {
                    i.DishId = dishChefId;
                    i.StepId = t;
                    db.DishStep.Add(i);
                    t++;
                    //db.SaveChanges();
                }
                

                #endregion

                #region 修改指定调料信息

               var chefId=model.ChefId;

               int dishId =dishChefId;


               var flavourRecRole = requestData.FlavourRecRole;
               //首先删除已存在菜品Id的调料
               List<FlavourRecRole> qflavourRecRole = new List<FlavourRecRole>();

               for (int i = 0; i < flavourRecRole.Count; i++)
               {
                   qflavourRecRole.Add(JsonConvert.DeserializeObject<FlavourRecRole>(JsonConvert.SerializeObject(flavourRecRole[i])));
               }

               var sql1 = string.Format(@"Delete FlavourRecRole where DishId={0} and RoleId={1}", dishId, chefId);
               dataContext.ExecuteNonQuery(CommandType.Text, sql1);

               //保存调料信息
               foreach (FlavourRecRole dm1 in qflavourRecRole)
               {
                   dm1.RoleId = chefId;
                   dm1.DishId = dishId;
                   dm1.UpdateTime = DateTime.Now;
                   dm1.CreateTime = DateTime.Now;
                   db.FlavourRecRole.Add(dm1);
                   
               }

                #endregion

               db.SaveChanges();
                return "OK";
            }
            catch (Exception)
            {
                return "No";
            }
        }

        #endregion

        #region 发送审核结果模板消息--报名信息审核--web
        ///// <summary>
        ///// 发送审核结果模板消息--报名信息审核--web
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public string SentTemplate(dynamic requestData)
        //{
            
        //    return null;
        //}

        #endregion

        #endregion
    }
}

using ECActivityAPI.Common;
using ECActivityAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
    /// 厨师
    /// </summary>
    public class ChefController : ApiController
    {
        private CreditContext db = new CreditContext();
        private SqlHelper dataContext = new SqlHelper();

        #region 读取活动设置为开放的赛区--活动报基本信息--wx
        /// <summary>
        /// 读取活动设置为开放的赛区--活动报名页--wx
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetMatchRegion()
        {
            try
            {
                var qChefActivity = (from v in db.ChefActivity
                                     where v.IsEnable == 1
                                     select new
                                     {
                                         ChefActivityId = v.ChefActivityId
                                     }).FirstOrDefault();
                var qMatchRegion = (from v in db.MatchRegion
                                    where v.ChefActivityId == qChefActivity.ChefActivityId && v.EndTime > DateTime.Now
                                    select v).GroupBy(a => a.ProvinceName).ToList();
                string json = "[";
                for (int i = 0; i < qMatchRegion.Count; i++)
                {
                    var ddddd = qMatchRegion[i].Key;
                    var sql = string.Format(@"select MatchRegionId,AreaName,ChefActivityId from MatchRegion where ChefActivityId={0} and  ProvinceName='{1}' and  EndTime>'{2}'", qChefActivity.ChefActivityId, qMatchRegion[i].Key, DateTime.Now);
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

        #region 新增（修改）厨师基本信息--活动报基本信息-wx
        /// <summary>
        /// 新增（修改）厨师基本信息--活动报名页-wx
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddChef(Chef requestData)
        {
            try
            {
                //string query = JsonConvert.SerializeObject(requestData);
                //Chef model = JsonConvert.DeserializeObject<Chef>(query);
                Chef model = requestData;
                if (model.Birthday == Convert.ToDateTime("0001/1/1 0:00:00") )
                {
                    model.Birthday = Convert.ToDateTime("1900-1-1");
                }
                var q = (from v in db.Chef
                         where v.MemberId == model.MemberId && v.ChefActivityId==model.ChefActivityId
                         select v
                       ).FirstOrDefault();
               
                if (q == null)
                {
                    db.Chef.Add(model);
                    db.SaveChanges();
                    return model.ChefId.ToString();
                }
                else 
                {
                    q.MemberId = model.MemberId;
                    q.ChefActivityId = model.ChefActivityId;
                    q.MatchRegionId = model.MatchRegionId;
                    q.ApplyType = model.ApplyType;
                    q.ReferrerName = model.ReferrerName;
                    q.Birthday = model.Birthday;
                    q.ChefPicUrl = model.ChefPicUrl;
                    //q.ChefHotelName = model.ChefHotelName;
                    q.ChefHotelPicUrl = model.ChefHotelPicUrl;
                    db.Entry<Chef>(q).State = EntityState.Modified;
                    db.SaveChanges();

                    string update = "update DishChef set IsApply=0 where ChefId=" + q.ChefId;
                    dataContext.ExecuteNonQuery(CommandType.Text, update);

                    return q.ChefId.ToString();
                }
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 获取厨师基本信息--活动报基本信息--wx
        /// <summary>
        /// 获取厨师基本信息--活动报基本信息--wx
        /// </summary>
        /// <param name="mbId">会员（游客）Id</param>
        /// <param name="caId">活动ID</param>
        /// <returns></returns>
        [HttpGet]
        public string GetChefByMBIdCAId(int mbId,int caId)
        {
            var q = (from v in db.Chef
                     join p in db.MatchRegion on v.MatchRegionId equals p.MatchRegionId
                     where v.MemberId == mbId && v.ChefActivityId == caId
                     select new
                     {
                         ChefId = v.ChefId ,
                         MemberId = v.MemberId,
                         ChefActivityId = v.ChefActivityId,
                         MatchRegionId = v.MatchRegionId,
                         AreaName=p.AreaName,
                         ApplyType = v.ApplyType,
                         ReferrerName = v.ReferrerName,
                         Birthday = v.Birthday ,
                         ChefPicUrl = v.ChefPicUrl,
                         //ChefHotelName = v.ChefHotelName,
                         ChefHotelPicUrl = v.ChefHotelPicUrl
                     });
            return JsonConvert.SerializeObject(q,DateTimeConvent.DateTimedd());
            
        }
        #endregion

        #region 检测是否有为填写完的报名信息--EC活动首页--wx
        /// <summary>
        ///  检测是否有为填写完的报名信息--活动首页--wx
        /// </summary>
        /// <param name="mbId">会员Id</param>
        /// <param name="caId">活动Id</param>
        /// <returns></returns>
        [HttpGet]
        public string IsChefNull(int mbId, int caId)
        {
            try
            {
                var q = (from v in db.Chef
                         where v.MemberId == mbId && v.ChefActivityId == caId
                         select v).ToList();

                var isChefNull = "f";
                if (string.IsNullOrEmpty(q[0].ApplyType) || q[0].Birthday == Convert.ToDateTime("1900-01-01 00:00:00.000") || string.IsNullOrEmpty(q[0].ChefPicUrl)  || string.IsNullOrEmpty(q[0].ChefHotelPicUrl))
                {
                    return "{\"IsChefNull\":\"" + isChefNull + "\"}";
                }
                if (q[0].ApplyType == "自己报名" && string.IsNullOrEmpty(q[0].ReferrerName))
                {
                    return "{\"IsChefNull\":\"" + isChefNull + "\"}";
                }
                isChefNull = "t";
                return "{\"IsChefNull\":\"" + isChefNull + "\"}";
            }
            catch (Exception)
            {
                return "{\"IsChefNull\":\"No\"}";
            }

        }
        #endregion

        #region 后台手动增加活动信息

        #region 首先查询到会员基本信息，根据姓名，或者会员号（MemberId）--新增厨师--web
        /// <summary>
        /// 首先查询到会员基本信息，根据姓名，或者会员号（MemberId）--新增厨师--web
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetMemberInfo(dynamic requestData)
        {
            try
            {
                string Name = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.Name));
                int memberId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.MemberId));
                var sql = string.Format("SELECT  MemberId ,MemberName from [RegistMember]  where 1=1 ");
                bool bName = Name != null && !string.IsNullOrEmpty(Name);

                if (bName)
                    sql = sql + " and  MemberName like '%" + Name + "%'";
                if (memberId != 0)
                    sql = sql + " and MemberId=" + memberId;
                if (!bName && memberId == 0)
                    sql = sql + " and MemberId=0";
                var table = dataContext.ExecuteDataTable(CommandType.Text, sql);
                var count=   JsonConvert.SerializeObject(table).Count();
                if (count == 2)
                    return "No";
                return JsonConvert.SerializeObject(table);
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 增加厨师基本信息--新增厨师--web
        /// <summary>
        /// 增加厨师基本信息--新增厨师--web
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddChefInfo(dynamic requestData)
        {
            try
            {
                string query = JsonConvert.SerializeObject(requestData);
                Chef model = JsonConvert.DeserializeObject<Chef>(query);
                //Chef model = requestData;
                if (model.Birthday == Convert.ToDateTime("0001/1/1 0:00:00"))
                {
                    model.Birthday = Convert.ToDateTime("1900-1-1");
                }
                var q = (from v in db.Chef
                         where v.MemberId == model.MemberId && v.ChefActivityId == model.ChefActivityId
                         select v
                       ).FirstOrDefault();

                if (q == null)
                {
                    db.Chef.Add(model);
                    db.SaveChanges();
                    return model.ChefId.ToString();
                }
                else
                    return "厨师已存在";

                #region 不需要修改
                //else
                //{
                //    q.MemberId = model.MemberId;
                //    q.ChefActivityId = model.ChefActivityId;
                //    q.MatchRegionId = model.MatchRegionId;
                //    q.ApplyType = model.ApplyType;
                //    q.ReferrerName = model.ReferrerName;
                //    q.Birthday = model.Birthday;
                //    q.ChefPicUrl = model.ChefPicUrl;
                //    //q.ChefHotelName = model.ChefHotelName;
                //    q.ChefHotelPicUrl = model.ChefHotelPicUrl;
                //    db.Entry<Chef>(q).State = EntityState.Modified;
                //    db.SaveChanges();

                //    string update = "update DishChef set IsApply=0 where ChefId=" + q.ChefId;
                //    dataContext.ExecuteNonQuery(CommandType.Text, update);

                //    return q.ChefId.ToString();
                //} 
                #endregion
            }
            catch (Exception)
            {
                return "No";
            }
           
        }
        #endregion

        #region 增加厨师菜品基本信息--新增厨师--web
        /// <summary>
        /// 增加厨师菜品基本信息--新增厨师--web
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddDishChefInfo(dynamic requestData)
        {

            try
            {

                string dishChef = JsonConvert.SerializeObject(requestData.dishChef);
                DishChef model = JsonConvert.DeserializeObject<DishChef>(dishChef);

                var qDishChef = (from v in db.DishChef
                                 where v.ChefId == model.ChefId && v.DishType == model.DishType
                                 select v).FirstOrDefault();

                //是空才允许增加
                if (qDishChef == null)
                {
                    #region 增加菜品基本信息
                    model.UpdateTime = DateTime.Now;
                    model.ApplyTime = Convert.ToDateTime("1900-01-01");
                    model.SelectedTime = Convert.ToDateTime("1900-01-01");
                    db.DishChef.Add(model);
                    db.SaveChanges();
                    #endregion

                    try
                    {
                        var dishChefId = model.DishChefId;

                        #region 增加菜品调料

                        var middledishMaterial = requestData.dishMaterial;

                        List<DishMaterial> qdishMaterial = new List<DishMaterial>();

                        for (int i = 0; i < middledishMaterial.Count; i++)
                        {
                            qdishMaterial.Add(JsonConvert.DeserializeObject<DishMaterial>(JsonConvert.SerializeObject(middledishMaterial[i])));
                        }

                        //保存调料信息
                        foreach (DishMaterial dm in qdishMaterial)
                        {
                            dm.DishId = dishChefId;
                            db.DishMaterial.Add(dm);
                        }
                        //db.SaveChanges();

                        #endregion

                        #region 增加菜品步骤

                        var middleDishStep = requestData.dishStep;
                        List<DishStep> qDishStep = new List<DishStep>();

                        for (int i = 0; i < middleDishStep.Count; i++)
                        {
                            qDishStep.Add(JsonConvert.DeserializeObject<DishStep>(JsonConvert.SerializeObject(middleDishStep[i])));
                        }

                        //保存
                        int t = 1;
                        foreach (DishStep ds in qDishStep)
                        {
                            ds.DishId = dishChefId;
                            ds.StepId = t;
                            db.DishStep.Add(ds);
                            t++;
                        }



                        #endregion

                        #region 增加指定调料信息

                        var chefId = model.ChefId;
                        var dishId = model.DishChefId;

                        var flavourRecRole = requestData.flavourRecRole;

                        List<FlavourRecRole> qflavourRecRole = new List<FlavourRecRole>();

                        for (int i = 0; i < flavourRecRole.Count; i++)
                        {
                            qflavourRecRole.Add(JsonConvert.DeserializeObject<FlavourRecRole>(JsonConvert.SerializeObject(flavourRecRole[i])));
                        }
                        //保存调料信息
                        foreach (FlavourRecRole fr in qflavourRecRole)
                        {
                            fr.RoleId = chefId;
                            fr.DishId = dishId;
                            fr.UpdateTime = DateTime.Now;
                            fr.CreateTime = DateTime.Now;
                            db.FlavourRecRole.Add(fr);
                        }
                        #endregion

                        db.SaveChanges();
                    }
                    catch (Exception)
                    {
                        //return "菜品Id已生成";
                        return "OK";
                    }

                    return "OK";
                }

                return "已存在";
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

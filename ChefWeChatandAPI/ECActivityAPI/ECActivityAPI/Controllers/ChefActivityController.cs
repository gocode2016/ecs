using ECActivityAPI.Common;
using ECActivityAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
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
    /// 厨师活动表
    /// </summary>
    public class ChefActivityController : ApiController
    {
        private CreditContext db = new CreditContext();
        private SqlHelper dataContext = new SqlHelper();

        #region 增加厨师活动表--活动列表配置信息--web
        /// <summary>
        /// 增加厨师活动表--活动列表配置信息--web
        /// </summary>
        /// <param name="requestData">厨师活动表格式</param>
        /// <returns>OK,成功；No,失败</returns>
        [HttpPost]
        public string AddChefActivity(dynamic requestData)
        {
            try
            {
                
                string query = JsonConvert.SerializeObject(requestData);
                ChefActivity model = JsonConvert.DeserializeObject<ChefActivity>(query);
                #region 每次新增确保只有一个是进行状态
                if (model.IsEnable == 1)
                {
                    var qchefActivity = (from v in db.ChefActivity
                                         where v.IsEnable == 1
                                         select v).FirstOrDefault();
                    if (qchefActivity != null)
                    {
                        if (qchefActivity.IsEnable == 1)
                        {
                            qchefActivity.IsEnable = 0;
                            db.SaveChanges();
                        }
                    }
                } 
                #endregion
                model.CreateTime = DateTime.Now;
                model.UpdateTime = DateTime.Now;
                model.IsDel = 1;
                db.ChefActivity.Add(model);
                db.SaveChanges();
                return "OK";
            }
            catch (Exception  e)
            {
                return e.Message;
            }
        }
        #endregion

        #region 获取厨师活动--活动列表--web
        /// <summary>
        /// 获取厨师活动--活动列表--web
        /// </summary>
        /// <returns>厨师活动基本信息</returns>
        [HttpGet]
        public string GetChefActivityAll()
        {
            string sql = string.Format("select * from ChefActivity");

            var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
            return JsonConvert.SerializeObject(q);
        }
        #endregion

        #region 获取单个厨师活动--活动列表配置信息--web
        /// <summary>
        /// 获取单个厨师活动--活动列表配置信息--web
        /// </summary>
        /// <returns>厨师活动基本信息</returns>
        [HttpGet]
        public string GetCAByCAId(int ca)
        {
            string sql = string.Format("select ChefActivityId,ChefActivityName,IsEnable from ChefActivity where ChefActivityId={0} ", ca);
            var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
            return JsonConvert.SerializeObject(q);
        }
        #endregion

        #region 删除活动操作--活动列表配置信息--web
        /// <summary>
        /// 删除活动操作--活动列表配置信息--web
        /// </summary>
        /// <param name="ChefActivityId">活动Id</param>
        /// <param name="UpdatePerson">修改人</param>
        /// <returns>OK，成功，No，失败</returns>
        [HttpPost]
        public string UpdateIsDel(int ChefActivityId,string UpdatePerson)
        {
            //先查询
            var chefActivity=(from v in db.ChefActivity
                              where v.ChefActivityId==ChefActivityId
                              select v).Single();
            chefActivity.UpdatePerson = UpdatePerson;
            chefActivity.UpdateTime = DateTime.Now;
            chefActivity.IsDel = 0;
            var count=db.SaveChanges();
            if (count > 0)
                return "OK";
            else
                return "No";
        } 
        #endregion

        #region 修改活动--活动列表配置信息--web
        /// <summary>
        /// 修改活动,每次只启用一个活动，修改这个，停用另外的--活动列表配置信息--web
        /// </summary>
        /// <param name="requestDate">修改数据</param>
        /// <returns>OK，成功，No ,失败</returns>
        [HttpPost]
        public string UpdateIsEnable(dynamic requestDate)
        {
            try
            {
                var qchefActivity = (from v in db.ChefActivity
                                     where v.IsEnable == 1
                                     select v).FirstOrDefault();
                if (qchefActivity != null)
                {
                    if (qchefActivity.IsEnable == 1)
                    {
                        if (qchefActivity != null)
                        {
                            qchefActivity.IsEnable = 0;
                            db.Entry<ChefActivity>(qchefActivity).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }
                ChefActivity chefActivity = new ChefActivity();
                chefActivity.ChefActivityId = requestDate.ChefActivityId;
                chefActivity.ChefActivityName = requestDate.ChefActivityName;
                chefActivity.IsEnable = requestDate.IsEnable;
                chefActivity.UpdatePerson = requestDate.UpdatePerson;
                chefActivity.UpdateTime = DateTime.Now;
                //db.Entry<ChefActivity>(chefActivity).State = EntityState.Modified;
                //db.Entry<ChefActivity>(chefActivity).Property("CreatePerson").IsModified = false;
                //db.Entry<ChefActivity>(chefActivity).Property("CreateTime").IsModified = false;
                //db.SaveChanges();
                var sql = string.Format(@"update ChefActivity set ChefActivityName='{0}',IsEnable={1},UpdatePerson='{2}',UpdateTime='{3}' where ChefActivityId={4}", chefActivity.ChefActivityName, chefActivity.IsEnable, chefActivity.UpdatePerson, chefActivity.UpdateTime, chefActivity.ChefActivityId);
                dataContext.ExecuteNonQuery(CommandType.Text,sql);
                return "OK";
            }
            catch (Exception e)
            {
                return "No";
            }
        }
        #endregion

        #region 查询活动及本活动的赛区--活动列表配置信息--web
        /// <summary>
        /// 查询活动及本活动的赛区--活动列表配置信息--web
        /// </summary>
        /// <param name="ca">活动ID</param>
        /// <returns></returns>
        [HttpGet]
        public string GetCAMRByCA(int caId)
        {
            var caByca = (from v in db.ChefActivity
                          where v.ChefActivityId == caId
                          select new {
                              ChefActivityId=v.ChefActivityId,
                              ChefActivityName=v.ChefActivityName,
                              UpdatePerson=v.UpdatePerson,
                              IsEnable=v.IsEnable
                          });
            var mr = (from v in db.MatchRegion
                      where v.ChefActivityId == caId
                      select new { 
                          
                          ProvinceName=v.ProvinceName,
                          AreaName=v.AreaName,
                          ChefActivityId=v.ChefActivityId,

                          MatchRegionId=v.MatchRegionId,


                          BeginTime=v.BeginTime,
                          EndTime=v.EndTime,
                          CreatePerson=v.CreatePerson,
                          UpdatePerson=v.UpdatePerson
                      });
            var iso = new IsoDateTimeConverter();
            iso.DateTimeFormat = "yyyy-MM-dd HH:mm";

            return "{\"ChefActivity\":" + JsonConvert.SerializeObject(caByca, iso) + ",\"MatchRegion\":" + JsonConvert.SerializeObject(mr,iso) + "}";
        }
        #endregion



    }
}

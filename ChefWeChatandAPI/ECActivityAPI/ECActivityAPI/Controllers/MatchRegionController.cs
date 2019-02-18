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
    public class MatchRegionController : ApiController
    {
        private CreditContext db = new CreditContext();
        private SqlHelper dataContext = new SqlHelper();

        #region 增加活动赛区--活动列表--web
        /// <summary>
        /// 增加活动赛区--活动列表--web
        /// </summary>
        /// <param name="requestData"> 厨师活动赛区格式是数组</param>
        /// <returns>OK,成功；No,失败</returns>
        [HttpPost]
        public string AddMatchRegion(dynamic requestData)
        {
            try
            {
                string addMatchRegion = JsonConvert.SerializeObject(requestData);
                List<MatchRegion> model = JsonConvert.DeserializeObject<List<MatchRegion>>(addMatchRegion);

                ////var allDel = string.Format(@"DELETE MatchRegion where ChefActivityId={0}", model[0].ChefActivityId);
                //dataContext.ExecuteNonQuery(CommandType.Text, allDel);
                foreach (MatchRegion m in model)
                {
                    var q = (from v in db.MatchRegion
                             where v.MatchRegionId == m.MatchRegionId
                             select v).FirstOrDefault();
                    if (q!=null)
                    {
                        m.CityName = m.AreaName;
                       // m.CreateTime = DateTime.Now;
                        m.UpdateTime = DateTime.Now;
                        q.AreaName = m.AreaName;
                        q.BeginTime = m.BeginTime;
                        q.CityName = m.AreaName;
                        q.EndTime = m.EndTime;
                        q.ProvinceName = m.ProvinceName;
                        q.UpdatePerson = m.UpdatePerson;
                        q.UpdateTime = DateTime.Now;

                        m.MatchRegionId = 0;
                        db.Entry<MatchRegion>(q).State = EntityState.Modified;
                    }
                    else
                    {
                        m.CityName = m.AreaName;
                        m.CreateTime = DateTime.Now;
                        m.UpdateTime = DateTime.Now;
                        //db.MatchRegion.Add(m);
                        //db.SaveChanges();
                        var sql = string.Format("INSERT INTO [dbo].[MatchRegion]([ProvinceName],[CityName],[AreaName],[ChefActivityId],[BeginTime],[EndTime],[CreatePerson],[UpdatePerson],[CreateTime],[UpdateTime],[Remark])VALUES('{0}','{1}','{2}',{3},'{4}','{5}','{6}','{7}','{8}','{9}','{10}')",m.ProvinceName,m.CityName,m.AreaName,m.ChefActivityId,m.BeginTime,m.EndTime,m.CreatePerson,m.UpdatePerson,m.CreateTime,m.UpdateTime,m.Remark);

                        dataContext.ExecuteNonQuery(CommandType.Text, sql);
                    }
                }
                db.SaveChanges();
                return "OK";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 根据活动ID获取赛区--活动列表--web
        /// <summary>
        /// 根据活动ID获取赛区--活动列表--web
        /// </summary>
        /// <param name="requestDate">活动ID</param>
        /// <returns>赛区信息</returns>
        [HttpGet]
        public string GetAllByChefActivityId(int chefActivityId)
        {
            try
            {
                var q = (from v in db.MatchRegion
                         where v.ChefActivityId == chefActivityId
                         select new
                         {
                             MatchRegionId = v.MatchRegionId,
                             AreaName = v.AreaName,
                             ProvinceName = v.ProvinceName,
                             BeginTime = v.BeginTime,
                             EndTime = v.EndTime
                         });
                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd hh:mm";
                return JsonConvert.SerializeObject(q,iso);
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion
    }
}

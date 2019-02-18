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
    public class DishCollecLogController : ApiController
    {
        private CreditContext db = new CreditContext();
        private SqlHelper dataContext = new SqlHelper();

        #region 增加收藏菜品信息--菜品详情页--wx
        /// <summary>
        /// 增加收藏菜品信息--菜品详情页--wx
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddDishCollectLog(dynamic requestData)
        {
            try
            {
                var q = JsonConvert.SerializeObject(requestData);
                DishCollectLog m = JsonConvert.DeserializeObject<DishCollectLog>(q);
                var d = (from v in db.DishCollectLog
                         where v.OpenId == m.OpenId && v.DishId==m.DishId
                         select v).FirstOrDefault();
                if (d == null)
                {
                    db.DishCollectLog.Add(m);
                    db.SaveChanges();
                }
                return "OK";
            }
            catch (Exception)
            {
                return "No";
            }
          
        }
        #endregion

        #region 取消收藏菜品信息--菜品详情页--wx
        /// <summary>
        /// 取消收藏菜品信息--菜品详情页--wx
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string DelDishCollectLog(dynamic requestData)
        {
            try
            {
                var q = JsonConvert.SerializeObject(requestData);
                DishCollectLog m = JsonConvert.DeserializeObject<DishCollectLog>(q);
               
                var sql = string.Format("DELETE from DishCollectLog where OpenId='{0}' and DishId={1} and DishType={2}",m.OpenId,m.DishId,m.DishType);
                dataContext.ExecuteNonQuery(CommandType.Text, sql);
                return "OK";
            }
            catch (Exception)
            {
                return "No";
            }
          
        }
        #endregion

        #region 菜品收藏状态--菜品详情页--wx
        /// <summary>
        /// 菜品收藏状态--菜品详情页--wx
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetDishCollectState(dynamic requestData)
        {
            try
            {
                DishCollectLog m = JsonConvert.DeserializeObject<DishCollectLog>(JsonConvert.SerializeObject(requestData));
                var q = (from v in db.DishCollectLog
                         where v.OpenId == m.OpenId && v.DishId == m.DishId
                         select v).FirstOrDefault();
                var isCollect = "f";
                if (q != null)
                    isCollect = "t";
                return "{\"IsCollect\":\"" + isCollect + "\"}";
            }
            catch (Exception)
            {
                return "{\"IsCollect\":\"f\"}";
            }
        }

        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CreditUserAPI.Common;
using CreditUserAPI.Models;
using Newtonsoft.Json;
using System.Data;

namespace CreditUserAPI.Controllers
{
    /// <summary>
    /// 队员大区API
    /// </summary>
    public class RegionController : ApiController
    {
        private CreditContext db = new CreditContext();
        private SqlHelper dataContext = new SqlHelper();

        #region 新增大区
        /// <summary>
        /// 新增大区
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpGet]
        public string AddRegion(string requestData)
        {
            Region model = JsonConvert.DeserializeObject<Region>(requestData);
            model.CreateDate = DateTime.Now;
            db.Region.Add(model);
            db.SaveChanges();
            return model.RegionName;
        } 
        #endregion

        #region 获取所有大区
        /// <summary>
        /// 获取所有大区
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetRegionList()
        {
            string sql = string.Format("select RegionId, RegionName from Region");
            var q = dataContext.ExecuteDataTable(CommandType.Text, sql);

            return JsonConvert.SerializeObject(q); ;
        } 
        #endregion

    }
}

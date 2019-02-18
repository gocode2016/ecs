using IntegralMallAPI.Common;
using IntegralMallAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IntegralMallAPI.Controllers
{
    /// <summary>
    /// 类目部分API
    /// </summary>
    public class CategoryController : ApiController
    {
        private IntegralMallContext db = new IntegralMallContext();
        private SqlHelper dataContext = new SqlHelper();

        /// <summary>
        /// 获得所有类目
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetCategory()
        {
            var sql = string.Format("Select * from Category");
            var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
            return JsonConvert.SerializeObject(q);
        }
    }
}

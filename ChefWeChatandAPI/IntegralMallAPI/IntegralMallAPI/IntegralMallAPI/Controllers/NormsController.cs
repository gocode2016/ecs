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
    public class NormsController : ApiController
    {
        private IntegralMallContext db = new IntegralMallContext();
        private SqlHelper dataContext = new SqlHelper();

        /// <summary>
        /// 新增商品规格
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public int AddNorms(dynamic requestData)
        {
            string query = JsonConvert.SerializeObject(requestData);
            Norms norms = JsonConvert.DeserializeObject<Norms>(query);
            db.Norms.Add(norms);

            return db.SaveChanges();
        }

        /// <summary>
        /// 更改商品规格
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public int UpdateNorms(dynamic requestData)
        {
            string query = JsonConvert.SerializeObject(requestData);
            Norms norms = JsonConvert.DeserializeObject<Norms>(query);
            EntityState statebefore = db.Entry(norms).State;
            db.Entry(norms).State = EntityState.Modified;

            return db.SaveChanges();
        }

        /// <summary>
        /// 微信用
        /// 获取商品规格信息
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetProductNorms(int productId = 0)
        {
            if (productId > 0)
            {
                string sql = string.Format("select n.* from Norms as n where ProductId = {0} And IsEnable = 0 And n.NormsId in (select NormsId from SKU)", productId);
                var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
                return JsonConvert.SerializeObject(q);
            }
            else
            {
                return "Request Parameter Error";            
            }
        }


        /// <summary>
        /// 后台用
        /// 获取商品规格信息
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetSysProductNorms(int productId = 0)
        {
            if (productId > 0)
            {
                string sql = string.Format("select * from Norms where ProductId = {0}", productId);
                var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
                return JsonConvert.SerializeObject(q);
            }
            else
            {
                return "Request Parameter Error";
            }
        }

    }
}

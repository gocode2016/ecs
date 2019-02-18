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
    public class SaleActivityController : ApiController
    {
        private IntegralMallContext db = new IntegralMallContext();
        private SqlHelper dataContext = new SqlHelper();

        /// <summary>
        /// 新增SKU折扣活动
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddSaleActivity(dynamic requestData)
        {
            try
            {
                string query = JsonConvert.SerializeObject(requestData);
                SaleActivity model = JsonConvert.DeserializeObject<SaleActivity>(query);
                //db.SaleActivity.Add(model);

                string sql = string.Format("Insert into SaleActivity Values({0}, '{1}','{2}', {3},{4})", model.SkuId, model.StartTime, model.EndTime, model.SalePrice, model.Limit);
                int q = dataContext.ExecuteNonQuery(CommandType.Text,sql);
                return q.ToString();

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        /// <summary>
        /// 根据sku获取活动信息
        /// </summary>
        /// <param name="skuId"></param>
        /// <returns></returns>
        [HttpPost]
        public string FindSkuSale(int skuId = 0)
        {
            if (skuId <= 0)
            {
                return "Request Error";
            }

            SaleActivity salemodel = (from o in db.SaleActivity
                                     where o.SkuId == skuId
                                     select o).FirstOrDefault();

            string result = JsonConvert.SerializeObject(salemodel);
            return result;
        }

        /// <summary>
        /// 修改SKU折扣活动
        /// </summary>k
        /// <param name="requestData"></param>
        /// <returns></returns>
        public int UpdateSkuSale(dynamic requestData)
        {
            string query = JsonConvert.SerializeObject(requestData);
            SaleActivity product = JsonConvert.DeserializeObject<SaleActivity>(query);
            EntityState statebefore = db.Entry(product).State;
            db.Entry(product).State = EntityState.Modified;

            return db.SaveChanges();
        }

    }
}

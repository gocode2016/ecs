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
    public class SKUController : ApiController
    {
        private IntegralMallContext db = new IntegralMallContext();
        private SqlHelper dataContext = new SqlHelper();

        /// <summary>
        /// 新增SKU
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public int AddSKU(dynamic requestData)
        {
            string query = JsonConvert.SerializeObject(requestData);
            SKU sku = JsonConvert.DeserializeObject<SKU>(query);
            db.SKU.Add(sku);
            db.SaveChanges();

            if (sku.IsShow == 1)
            {
                string normsSql = string.Format("Update Norms Set IsEnable = {0} From Norms as n inner join SKU as s on n.NormsId = s.NormsId and s.SkuId = {1}", 0, sku.SkuId);
                dataContext.ExecuteNonQuery(CommandType.Text, normsSql);
            }

            return sku.SkuId;
        }

        /// <summary>
        /// 修改SKU
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public int UpdateSKU(dynamic requestData)
        {
            string query = JsonConvert.SerializeObject(requestData);
            SKU sku = JsonConvert.DeserializeObject<SKU>(query);
            EntityState statebefore = db.Entry(sku).State;
            db.Entry(sku).State = EntityState.Modified;
            int count = db.SaveChanges();

            //当修改上下架状态时 修改规格
            if (sku.IsShow == 1)
            {
                string normsSql = string.Format("Update Norms Set IsEnable = {0} From Norms as n inner join SKU as s on n.NormsId = s.NormsId and s.SkuId = {1}", 0, sku.SkuId);
                dataContext.ExecuteNonQuery(CommandType.Text, normsSql);
            }
            else if (sku.IsShow == 0)
            {
                string normsSql = string.Format("Update Norms Set IsEnable = {0} From Norms as n inner join SKU as s on n.NormsId = s.NormsId and s.SkuId = {1}", 1, sku.SkuId);
                dataContext.ExecuteNonQuery(CommandType.Text, normsSql);
            }

            return count;
        }

        /// <summary>
        /// SKU列表
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetSKUList(dynamic requestData)
        {
            try
            {
                string productName = requestData.ProductName;
                int productId = requestData.ProductId;
                string skuCode = requestData.SkuCode;

                int beginPage = (requestData.IndexPage - 1) * requestData.PageSize + 1;
                int endPage = requestData.IndexPage * requestData.PageSize;

                var sql = SqlParamHelper.Builder
                    .Append("Select s.*, ROW_NUMBER() over(order by s.SkuId desc) as num from SKU  as s")
                    .Append("inner join Product as p on p.ProductId = s.ProductId");

                if (!string.IsNullOrEmpty(productName))
                {
                    sql.Where(" p.ProductName like '%" + productName + "%'");
                }
                else if (productId > 0)
                {
                    sql.Where(" p.ProductId = " + productId + "");
                }
                else if (!string.IsNullOrEmpty(skuCode))
                {
                    sql.Where(" s.SkuCode like '%" + skuCode + "%'");
                }
                sql.Where(" s.IsEnable = 0");
                var pageSql = SqlParamHelper.Builder
                    .Append("select * from (" + sql.SQL + ") as sku")
                    .Append("where sku.num >= " + beginPage + " and sku.num <= " + endPage + "");

                var q = dataContext.ExecuteDataTable(CommandType.Text, pageSql.SQL, sql.Arguments);

                //计算分页
                var countSql = string.Format("select Count(*) from(" + sql.SQL + ")as skuCount");
                var count = dataContext.ExecuteScalar(CommandType.Text, countSql);

                return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(q) + "}";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 根据规格获取SKU
        /// </summary>
        /// <param name="normsId"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetSkuByNorms(int normsId = 0)
        {
            if (normsId > 0)
            {
                var sql = string.Format("Select * from SKU as s where s.NormsId = {0} and s.IsEnable = 0", normsId);
                var q = dataContext.ExecuteDataTable(CommandType.Text, sql);

                var actSql = string.Format("select sa.* from SaleActivity as sa left join SKU as s on sa.SkuId = s.SkuId where GETDATE() Between sa.StartTime and sa.EndTime and s.NormsId = {0}", normsId);
                var sale = dataContext.ExecuteDataTable(CommandType.Text, actSql);

                return "{ \"SkuData\":" + JsonConvert.SerializeObject(q) + ",\"Sale\":" + JsonConvert.SerializeObject(sale) + "}";
            }
            else
            {
                return "Request Parameter Error";
            }
        }

        /// <summary>
        /// 批量上下架
        /// </summary>
        /// <param name="excType">操作项</param>
        /// <param name="skuList">SkuId数组</param>
        /// <returns></returns>
        [HttpPost]
        public string IsShowSKU(int excType, int[] skuList)
        {
            string skuSql = string.Empty;
            string normsSql = string.Empty;

            if (skuList.Length <= 0)
            {
                return "Request Parameter Error";
            }

            try
            {
                if (excType == 1)
                {
                    for (int i = 0; i < skuList.Length; i++)
                    {
                        skuSql = string.Format("Update SKU Set IsShow = 0 Where SkuId = {0}", skuList[i]);
                        dataContext.ExecuteNonQuery(CommandType.Text, skuSql);
                        normsSql = string.Format("Update Norms Set IsEnable = {0} From Norms as n inner join SKU as s on n.NormsId = s.NormsId and s.SkuId = {1}", excType, skuList[i]);
                        dataContext.ExecuteNonQuery(CommandType.Text, normsSql);

                    }
                }
                else
                {
                    for (int i = 0; i < skuList.Length; i++)
                    {
                        skuSql = string.Format("Update SKU Set IsShow = 1 Where SkuId = {0}", skuList[i]);
                        dataContext.ExecuteNonQuery(CommandType.Text, skuSql);
                        normsSql = string.Format("Update Norms Set IsEnable = {0} From Norms as n inner join SKU as s on n.NormsId = s.NormsId and s.SkuId = {1}", excType, skuList[i]);
                        dataContext.ExecuteNonQuery(CommandType.Text, normsSql);

                    }
                }
                
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "Excute Success";
        }

    }
}

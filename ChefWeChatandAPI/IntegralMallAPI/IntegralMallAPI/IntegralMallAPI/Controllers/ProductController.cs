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
    public class ProductController : ApiController
    {
        private IntegralMallContext db = new IntegralMallContext();
        private SqlHelper dataContext = new SqlHelper();

        /// <summary>
        /// 新增商品
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns>返回执行行数</returns>
        [HttpPost]
        public string AddProduct(dynamic requestData)
        {
            try
            {
                string query = JsonConvert.SerializeObject(requestData);
                Product product = JsonConvert.DeserializeObject<Product>(query);
                db.Product.Add(product);
                db.SaveChanges();
                return product.ProductId.ToString();

            }
            catch (Exception ex )
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetProductList(dynamic requestData)
        {
            int productType = requestData.productType;
            int categoryId = requestData.categoryId;
            string productName = requestData.productName;

            int beginPage = (requestData.IndexPage - 1) * requestData.PageSize + 1;
            int endPage = requestData.IndexPage * requestData.PageSize;

            var sql = SqlParamHelper.Builder
                .Append("select p.*, (Select COUNT(SkuId) from SKU where ProductId = p.ProductId)as SkuCount, ROW_NUMBER() over(order by p.ProductId desc) as num from Product as p");

            if (productType > 0)
            {
                sql.Where(" p.ProductType = "+ productType +"");
            }
            else if (categoryId > 0)
            {
                sql.Where(" p.CategoryId = " + categoryId + "");
            }
            else if (!string.IsNullOrEmpty(productName))
            {
                sql.Where(" p.ProductName like '%"+ productName +"%'");
            }
            sql.Where(" p.IsEnable = 0");

            var pageSql = SqlParamHelper.Builder
                .Append("select * from (" + sql.SQL + ") as product")
                .Append("where product.num >= " + beginPage + " and product.num <= " + endPage + "");

            var q = dataContext.ExecuteDataTable(CommandType.Text, pageSql.SQL, sql.Arguments);

            //计算分页
            var countSql = string.Format("select Count(*) from(" + sql.SQL + ")as productCount");
            var count = dataContext.ExecuteScalar(CommandType.Text, countSql);

            return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(q) + "}";
        }


        /// <summary>
        /// 获取商品列表
        /// 微信端使用
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetWeChatProductList(dynamic requestData)
        {
            try
            {
                int productType = requestData.productType;
                int categoryId = requestData.categoryId;
                string productName = requestData.productName;
                int priceOD = requestData.priceOD;
                int saleOD = requestData.saleOD;

                int beginPage = (requestData.IndexPage - 1) * requestData.PageSize + 1;
                int endPage = requestData.IndexPage * requestData.PageSize;

                var sql = SqlParamHelper.Builder
                    .Append("select p.*, (Select COUNT(SkuId) from SKU where ProductId = p.ProductId and IsShow = 1) as SkuCount,(select Count(*) from OrderDetaile as o where o.ProductId = p.ProductId) as Salecount, ROW_NUMBER() over(order by p.ProductId desc) as num from Product as p");
                sql.Where("p.ProductId in (Select ProductId from Norms)");

                if (productType > 0)
                {
                    sql.Where(" p.ProductType = " + productType + "");
                }
                if (categoryId > 0)
                {
                    sql.Where(" p.CategoryId = " + categoryId + "");
                }
                if (!string.IsNullOrEmpty(productName))
                {
                    sql.Where(" p.ProductName like '%" + productName + "%'");
                }
                
                sql.Where(" p.IsEnable = 0");

                var pageSql = SqlParamHelper.Builder
                    .Append("select * from (" + sql.SQL + ") as product")
                    .Append("where product.num >= " + beginPage + " and product.num <= " + endPage + "");
                pageSql.Where(" product.SkuCount > 0");
                if (priceOD == 1)
                {
                    pageSql.OrderBy(" product.ProductPrice desc");
                }
                else if (priceOD == 2)
                {
                    pageSql.OrderBy(" product.ProductPrice asc");
                }
                else if (saleOD == 1)
                {
                    pageSql.OrderBy(" product.Salecount desc");
                }
                else if (saleOD == 2)
                {
                    pageSql.OrderBy(" product.Salecount asc");
                }
                
                var q = dataContext.ExecuteDataTable(CommandType.Text, pageSql.SQL, sql.Arguments);

                //计算分页
                var countSql = string.Format("select Count(*) from(" + sql.SQL + ")as productCount");
                var count = dataContext.ExecuteScalar(CommandType.Text, countSql);
                return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(q) + "}";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            
        }

        /// <summary>
        /// 修改商品
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public int UpdateProduct(dynamic requestData)
        {
            string query = JsonConvert.SerializeObject(requestData);
            Product product = JsonConvert.DeserializeObject<Product>(query);
            EntityState statebefore = db.Entry(product).State;
            db.Entry(product).State = EntityState.Modified;

            return db.SaveChanges();
        }

        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet]
        public string RemoveProduct(int productId = 0)
        {
            if (productId > 0)
            {
                string sql = string.Format("Update Product Set IsEnable = 1 Where ProductId = {0}; Update SKU Set IsEnable = 1 Where ProductId = {0};", productId);
                dataContext.ExecuteNonQuery(CommandType.Text, sql);
                return "Excute Success";
            }
            return "Excute Error";
        }

        /// <summary>
        /// 获取商品详细信息
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public string GetProductById(int productId = 0)
        {
            if (productId > 0)
            {
                string sql = string.Format("Select * from Product where ProductId = {0} and IsEnable = 0", productId);
                var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
                return JsonConvert.SerializeObject(q);
            }
            else
            {
                return "Request Parameter Error";
            }
        }


        //[HttpGet]
        //public string UpdateAwsImg()
        //{
        //    List<Product> q = (from o in db.Product
        //                       select o).ToList();
        //    Product pro = new Product();

        //    foreach (var item in q)
        //    {
        //        item.ProductContent = item.ProductContent.Replace("shkapi4qas.shinho.net.cn/cts/cycommon", "kongapi.shinho.net.cn/ecs/common");

        //        EntityState statebefore = db.Entry(item).State;
        //        db.Entry(item).State = EntityState.Modified;
        //        int count = db.SaveChanges();
        //    }

        //    return "success";
        //}

    }
}

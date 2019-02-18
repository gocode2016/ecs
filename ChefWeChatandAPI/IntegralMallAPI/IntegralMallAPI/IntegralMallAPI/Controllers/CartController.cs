using IntegralMallAPI.Common;
using IntegralMallAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IntegralMallAPI.Controllers
{
    public class CartController : ApiController
    {
        private IntegralMallContext db = new IntegralMallContext();
        private SqlHelper dataContext = new SqlHelper();

        /// <summary>
        /// 新增购物车
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddCart(dynamic requestData)
        {
            try
            {
                string query = JsonConvert.SerializeObject(requestData);
                Cart cart = JsonConvert.DeserializeObject<Cart>(query);

                string sql = string.Format("Select Count(*) from Cart where SkuId = {0} and MemberId = {1}", cart.SkuId, cart.MemberId);
                var result = dataContext.ExecuteScalar(CommandType.Text, sql);

                if ((int)result > 0)
                {
                    string saveSql = string.Format("Update Cart Set Count = Count + {0} Where SkuId = {1} and MemberId = {2}", cart.Count, cart.SkuId, cart.MemberId);
                    dataContext.ExecuteNonQuery(CommandType.Text, saveSql);
                }
                else
                {
                    db.Cart.Add(cart);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "Excute Success";
        }

        /// <summary>
        /// 修改购物车
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public int UpdateCart(dynamic requestData)
        {
            int count = requestData.Count;
            int cartId = requestData.CartId;

            var sql = string.Format("update Cart Set Count = {0} Where CartId = {1}", count, cartId);
            int result = dataContext.ExecuteNonQuery(CommandType.Text, sql);

            return result;
        }

        /// <summary>
        /// 删除购物车
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        [HttpPost]
        public string DeleteCart(int[] cartIdList)
        {
            try
            {
                string sql = string.Empty;
                for (int i = 0; i < cartIdList.Length; i++)
                {
                    sql = string.Format("delete Cart where CartId = {0}", cartIdList[i]);
                    dataContext.ExecuteNonQuery(CommandType.Text, sql);
                }
                return "Excute Success";
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }

        /// <summary>
        /// 获取用户购物车列表
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetCartList(int memberId = 0)
        {
            if (memberId > 0)
            {
                //var sql = string.Format("Select c.*, s.IsShow from Cart as c inner join SKU as s on c.SkuId = s.SkuId  Where c.MemberId = {0}", memberId);

                var sql = SqlParamHelper.Builder
                    .Append("Select c.*, s.IsShow, ")
                    .Append("case when GETDATE() Between sa.StartTime and sa.EndTime then sa.SalePrice else -1 end as SalePrice, ")
                    .Append("sa.Limit from Cart as c inner join SKU as s on c.SkuId = s.SkuId  ")
                    .Append("left join SaleActivity as sa on sa.SkuId = s.SkuId Where c.MemberId = " + memberId + "");

                var q = dataContext.ExecuteDataTable(CommandType.Text, sql.SQL);
                return JsonConvert.SerializeObject(q);
            }
            else
            {
                return "Request Parameter Error";
            }
        }
    }
}

using IntegralCardAPI.Common;
using IntegralCardAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IntegralCardAPI.Controllers
{
    /// <summary>
    /// 积分商品
    /// </summary>
    public class IntegralGoodsController : ApiController
    {
        private IntegralCardContext db = new IntegralCardContext();
        private SqlHelper dataContext = new SqlHelper();

        /// <summary>
        /// 新增积分商品
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddIntegralGoods(dynamic requestData)
        {
            string query = JsonConvert.SerializeObject(requestData);
            IntegralGoods goods = JsonConvert.DeserializeObject<IntegralGoods>(query);

            string sql = string.Format("Select COUNT(*) from IntegralGoods where GoodsNo = {0}", goods.GoodsNo);
            var count = dataContext.ExecuteScalar(CommandType.Text, sql);
            if ((int)count > 0)
            {
                return "false";
            }

            db.IntegralGoods.Add(goods);
            db.SaveChanges();

            return goods.GoodsId.ToString();
        }

        /// <summary>
        /// 修改积分商品
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public int UpdateIntegralGoods(dynamic requestData)
        {
            string query = JsonConvert.SerializeObject(requestData);
            IntegralGoods goods = JsonConvert.DeserializeObject<IntegralGoods>(query);

            EntityState statebefore = db.Entry(goods).State;
            db.Entry(goods).State = EntityState.Modified;

            return db.SaveChanges();
        }

        /// <summary>
        /// 删除积分商品
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        [HttpGet]
        public int RemoveIngegralGoods(int goodsId = 0)
        {
            if (goodsId > 0)
            {
                string sql = string.Format("Update IntegralGoods Set GoodsState = 100 Where GoodsId = {0}", goodsId);
                var result = dataContext.ExecuteNonQuery(CommandType.Text, sql);
                return result;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取积分商品详情
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public string GetIntegralInfo(int goodsId = 0)
        {
            if (goodsId > 0)
            {
                string sql = string.Format("Select * from IntegralGoods Where GoodsId = {0}", goodsId);
                var result = dataContext.ExecuteDataTable(CommandType.Text, sql);
                return JsonConvert.SerializeObject(result); ;
            }
            else
            {
                return "Request Parameter Error";
            }
        }

        /// <summary>
        /// 积分商品列表
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetIntegralGoodsList(dynamic requestData)
        {
            string goodsName = requestData.GoodsName;
            int state = requestData.State;

            int beginPage = (requestData.IndexPage - 1) * requestData.PageSize + 1;
            int endPage = requestData.IndexPage * requestData.PageSize;

            var sql = SqlParamHelper.Builder
                .Append("select g.*,  ROW_NUMBER() over(order by g.GoodsId desc) as num from IntegralGoods as g");

            if (!string.IsNullOrEmpty(goodsName))
            {
                sql.Where(" g.GoodsName like '%" + goodsName + "%'");
            }
            else if (state > 0)
            {
                sql.Where(" g.GoodsState = " + state + "");
            }

            var pageSql = SqlParamHelper.Builder
               .Append("select * from (" + sql.SQL + ") as goods")
               .Append("where goods.num >= " + beginPage + " and goods.num <= " + endPage + "");

            var q = dataContext.ExecuteDataTable(CommandType.Text, pageSql.SQL, sql.Arguments);

            //计算分页
            var countSql = string.Format("select Count(*) from(" + sql.SQL + ")as goodsCount");
            var count = dataContext.ExecuteScalar(CommandType.Text, countSql);

            return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(q) + "}";
        }

        /// <summary>
        /// 积分后台获取积分商品下拉框
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetIntegralGoodsSelectList()
        {
            string sql = string.Format("Select GoodsId as value, GoodsName as label from IntegralGoods Where GoodsState <> 100");
            var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
            return JsonConvert.SerializeObject(q); ;
        }


    }
}

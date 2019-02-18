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
    /// 积分规则
    /// </summary>
    public class IntegralRuleController : ApiController
    {
        private IntegralCardContext db = new IntegralCardContext();
        private SqlHelper dataContext = new SqlHelper();
        

        /// <summary>
        /// 新增积分规则
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddIntegralRule(dynamic requestData)
        {
            try
            {
                string query = JsonConvert.SerializeObject(requestData);
                IntegralRule rule = JsonConvert.DeserializeObject<IntegralRule>(query);

                db.IntegralRule.Add(rule);
                db.SaveChanges();

                if (rule.RuleType == 2)
                {
                    var chancelist = requestData.ChanceList;
                    List<IntegralRuleChance> list = JsonConvert.DeserializeObject<List<IntegralRuleChance>>(chancelist.ToString());
                    string sql = string.Empty;

                    foreach (var item in list)
                    {
                        sql = string.Format("Insert into IntegralRuleChance Values({0},{1},{2},{3});", rule.RuleId, item.Integral, item.PutNum, item.Chance);
                        dataContext.ExecuteNonQuery(CommandType.Text, sql);
                    }
                }
                return rule.RuleId.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            
        }

        /// <summary>
        /// 修改积分规则
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public int UpdateIntegralRule(dynamic requestData)
        {
            string query = JsonConvert.SerializeObject(requestData);
            IntegralRule rule = JsonConvert.DeserializeObject<IntegralRule>(query);

            EntityState statebefore = db.Entry(rule).State;
            db.Entry(rule).State = EntityState.Modified;
            int q = db.SaveChanges();

            if (rule.RuleType == 2)
            {
                var chancelist = requestData.ChanceList;
                List<IntegralRuleChance> list = JsonConvert.DeserializeObject<List<IntegralRuleChance>>(chancelist.ToString());
                string sql = string.Empty;

                foreach (var item in list)
                {
                    sql = string.Format("UPDATE [IntegralRuleChance] SET [RuleId] = {0},[Integral] = {1},[PutNum] = {2},[Chance] = {3} WHERE RuleChanceId = {4};", item.RuleId, item.Integral, item.PutNum, item.Chance, item.RuleChanceId);
                    dataContext.ExecuteNonQuery(CommandType.Text, sql);
                }
            }

            return q;
        }

        /// <summary>
        /// 删除积分规则
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        [HttpGet]
        public int RemoveIntegralRule(int ruleId = 0)
        {
            if (ruleId > 0)
            {
                string sql = string.Format("Update IntegralRule Set RuleState = 100 Where RuleId = {0}", ruleId);
                var result = dataContext.ExecuteNonQuery(CommandType.Text, sql);
                return result;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取概率规则详情
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetRuleInfo(int ruleId = 0)
        {
            if (ruleId > 0)
            {
                string sql = string.Format("Select * from IntegralRuleChance Where RuleId = {0}", ruleId);
                var result = dataContext.ExecuteDataTable(CommandType.Text, sql);
                return JsonConvert.SerializeObject(result);
            }
            else
            {
                return "Request Parameter Error";
            }
        }

        /// <summary>
        /// 获取积分规则列表
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetRuleList(dynamic requestData)
        {
            string ruleName = requestData.RuleName;
            string goodsName = requestData.GoodsName;
            int ruleState = requestData.RuleState;
            string startTime = requestData.StartTime;
            string endTime = requestData.EndTime;

            int beginPage = (requestData.IndexPage - 1) * requestData.PageSize + 1;
            int endPage = requestData.IndexPage * requestData.PageSize;

            var sql = SqlParamHelper.Builder
                .Append("Select ir.*, ig.GoodsName, ig.GoodsNo, ig.GoodsInterNo, ig.GoodsImageUrl, ROW_NUMBER() over(order by ir.RuleId desc) as num from IntegralRule as ir")
                .Append("inner join IntegralGoods as ig on ir.GoodsId = ig.GoodsId");

            if (!string.IsNullOrEmpty(ruleName))
            {
                sql.Where(" ir.RuleName like '%" + ruleName + "%'");
            }
            else if (!string.IsNullOrEmpty(goodsName))
            {
                sql.Where(" ig.GoodsName like '%" + goodsName + "%'");
            }
            else if (ruleState > 0)
            {
                sql.Where(" ir.RuleState = " + ruleState + "");
            }
            else if (!string.IsNullOrEmpty(startTime))
            {
                sql.Where(" ir.CreateDate Between '"+ startTime +"' and '"+ endTime +"'");
            }
            sql.Where(" ir.RuleState <> 100");
            var pageSql = SqlParamHelper.Builder
              .Append("select * from (" + sql.SQL + ") as rules")
              .Append("where rules.num >= " + beginPage + " and rules.num <= " + endPage + "");

            var q = dataContext.ExecuteDataTable(CommandType.Text, pageSql.SQL, sql.Arguments);

            //计算分页
            var countSql = string.Format("select Count(*) from(" + sql.SQL + ")as rulesCount");
            var count = dataContext.ExecuteScalar(CommandType.Text, countSql);

            return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(q) + "}";
        }

        [HttpGet]
        public string GetChance()
        {
            Random rd = new Random();
            string q = "";
            for (int i = 0; i < 100; i++)
            {
                q += " | " + rd.Next(1,101);
            }

            return q;
        }

        /// <summary>
        /// 获取扫码概率积分
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetScanChanceLog()
        {
            string sql = string.Format("Select Top 10 o.HeadImgUrl, o.Nickname, mid.MemberId, mid.IntegralNum, mid.CreatDate from MemberIntegralDetail as mid left join OpenIdAssociated as o on mid.MemberId = o.UserId and o.UserType = 2 where mid.IntegralSource like '%活动%' order by IntegralId desc");

            var q = dataContext.ExecuteDataTable(CommandType.Text, sql);

            return JsonConvert.SerializeObject(q); ;
        }

    }
}

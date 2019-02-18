using CommonAPI.Common;
using CommonAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CommonAPI.Controllers
{
    public class LuckDrawController : ApiController
    {
        private CreditContext db = new CreditContext();
        private SqlHelper dataContext = new SqlHelper();
        
        /// <summary>
        /// 新增抽奖日志
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> AddDrawLog(dynamic requestData)
        {
            try
            {
                string query = JsonConvert.SerializeObject(requestData);
                LuckDrawLog model = JsonConvert.DeserializeObject<LuckDrawLog>(query);
                model.DrawTime = DateTime.Now;

                db.LuckDrawLog.Add(model);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "Exc Success";
        }

        /// <summary>
        /// 判断抽奖次数
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CheckDraw(dynamic requestData)
        {
            int memberId = requestData.MemberId;

            if (memberId <= 0)
            {
                return "-99";
            }

            try
            {
                string sql = string.Format("select Count(*) from LuckDrawLog where MemberId = {0} and DateDiff(dd,DrawTime,getdate())=0", memberId);
                var count = dataContext.ExecuteScalar(CommandType.Text, sql);
                if ((int)count >= 2)
                {
                    return "-1";
                }
                else if ((int)count == 0)
                {
                    return "0";
                }

                string sql2 = string.Format("select Count(*) from LuckDrawLog where MemberId = {0} and IsShare = 1 and DateDiff(dd,DrawTime,getdate()) = 0", memberId);
                var q = dataContext.ExecuteScalar(CommandType.Text, sql2);

                if ((int)q < 1)
                {
                    return "-1";
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        /// <summary>
        /// 页面分享
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ShareDraw(dynamic requestData)
        {
            int memberId = requestData.MemberId;

            string sql = string.Format("Update LuckDrawLog Set IsShare = 1 where MemberId = {0} and DateDiff(dd,DrawTime,getdate())=0", memberId);
            dataContext.ExecuteNonQuery(CommandType.Text, sql);

            return "Exc Success";
        }

        /// <summary>
        /// 查询中奖列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> GetDrawList()
        {
            string sql = string.Format("Select * from LuckDrawLog  where DrawName <> '谢谢参与'  order by DrawTime desc");
            var q = dataContext.ExecuteDataTable(CommandType.Text,sql);

            return JsonConvert.SerializeObject(q); ;
        }

        /// <summary>
        /// 米其林抽奖
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> MemberDrawing(int memberId)
        {
            if (memberId <= 0)
            {
                return "Exc Error";
            }

            Random rd = new Random();
            int a = rd.Next(0, 100);

            string sql = string.Empty;

            if (memberId == 0)
            {
                sql = string.Format("select Stock from SKU where SkuId = 1036");
                var count = dataContext.ExecuteScalar(CommandType.Text, sql);
                if ((int)count > 0)
                {
                    return "{\"ProductId\":1081, \"SkuId\": 1108, \"Text\":\"iPhone X\" }";
                }
            }
            //二等奖 禧平方礼盒
            if (a <= 1)
            {
                sql = string.Format("select Stock from SKU where SkuId = 1036");
                var count = dataContext.ExecuteScalar(CommandType.Text, sql);
                if ((int)count > 0)
                {
                    return "{\"ProductId\":1072, \"SkuId\": 1036, \"Text\":\"禧平方调味品礼盒\" }";
                }
            }
            //三等奖 厨师服
            if (a <= 5)
            {
                sql = string.Format("select Stock from SKU where SkuId = 1109");
                var count = dataContext.ExecuteScalar(CommandType.Text, sql);
                if ((int)count > 0)
                {
                    return "{\"ProductId\":1082, \"SkuId\": 1109, \"Text\":\"白色厨师服\" }";
                }
            }
            //q等奖 电子秤
            if (a <= 11)
            {
                sql = string.Format("select Stock from SKU where SkuId = 1013");
                var count = dataContext.ExecuteScalar(CommandType.Text, sql);
                if ((int)count > 0)
                {
                    return "{\"ProductId\":1012, \"SkuId\": 1013, \"Text\":\"电子秤\" }";
                }
            }
            //五等奖 杂志
            if (a <= 23)
            {
                sql = string.Format("select Stock from SKU where SkuId = 1089");
                var count = dataContext.ExecuteScalar(CommandType.Text, sql);
                if ((int)count > 0)
                {
                    return "{\"ProductId\":1072, \"SkuId\": 1089, \"Text\":\"《千滋百味》夏季刊本\" }";
                }
            }
            //六等奖 积分
            if (a <= 43)
            {
                sql = string.Format("update RegistMember Set LeaveIntegral = LeaveIntegral + 20, TotalIntegral = TotalIntegral + 80 where MemberId = {0}", memberId);
                dataContext.ExecuteNonQuery(CommandType.Text, sql);

                string logsql = string.Format("insert into MemberIntegralDetail values({0},20,'{1}',1,'欣标准赢壕礼','{1}',' 欣标准赢壕礼','','');", memberId, DateTime.Now);
                dataContext.ExecuteNonQuery(CommandType.Text, logsql);

                return "{\"ProductId\":0, \"SkuId\": 0, \"Text\":\"20积分\" }";
            }

            return "{\"ProductId\":0, \"SkuId\": 0, \"Text\":\"谢谢参与\" }";
        }

    }
}

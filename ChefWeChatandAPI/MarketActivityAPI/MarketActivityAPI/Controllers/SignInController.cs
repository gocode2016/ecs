using MarketActivityAPI.Common;
using MarketActivityAPI.Models;
using MarketActivityAPI.WeiXin;
using MarketActivityAPI.WeiXin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MarketActivityAPI.Controllers
{
    public class SignInController : ApiController
    {
        private MarketActivityContext db = new MarketActivityContext();
        private SqlHelper dataContext = new SqlHelper();

        /// <summary>
        /// 签到活动-签到操作
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string SignIn(dynamic requestData)
        {
            try
            {
                int activityId = requestData.ActivityId;
                int memberId = requestData.UserId;
                string openId = requestData.OpenId;
                string signDate = requestData.SignDate;

                //return openId + "|" + signDate + "|" + memberId + "|" + activityId;

                //查询出活动
                SignActivity activity = (from o in db.SignActivity
                                         where o.ActivityId == activityId
                                         select o).FirstOrDefault();

                if (activity == null)
                {
                    return "{ \"Count\":\"-1\",\"text\":\"\"}"; ;
                }

                if (string.IsNullOrEmpty(signDate))
                {
                    if (activity.ActState == -1 || activity.StartTime > DateTime.Now || DateTime.Now > activity.EndTime)
                    {
                        return "{ \"Count\":\"-1\",\"text\":\"\"}";
                    }
                }

                //验证是否签过到
                string actName = (from o in db.SignInLog
                                  where o.UserId == memberId && o.ActivityId == activityId
                                  select o.ActivityName).FirstOrDefault();

                if (!string.IsNullOrEmpty(actName))
                {
                    return "{ \"Count\":\"2\",\"text\":\"" + activity.ActName + "\"}";
                }

                //新增签到
                SignInLog model = new SignInLog();
                model.ActivityId = activityId;
                model.ActivityName = activity.ActName;
                if (string.IsNullOrEmpty(signDate))
                {
                    model.SignDate = DateTime.Now;
                }
                else
                {
                    model.SignDate = Convert.ToDateTime(signDate);
                }
                model.OpenId = openId;
                model.UserId = memberId;

                db.SignInLog.Add(model);
                int a = db.SaveChanges();

                if (a > 0)
                {
                    //操作活动积
                    string integralSql = string.Format("update RegistMember Set LeaveIntegral = LeaveIntegral + {0} where MemberId = {1}", activity.Integral, memberId);
                    int nums = dataContext.ExecuteNonQuery(CommandType.Text, integralSql);

                    string logsql = string.Format("insert into MemberIntegralDetail values({0},{1},'{2}',2,'线下签到','{2}','{3}','','');", memberId, activity.Integral, model.SignDate, activity.ActName);
                    dataContext.ExecuteNonQuery(CommandType.Text, logsql);

                    return "{ \"Count\":\"1\",\"text\":\"" + activity.ActName + "\"}"; 
                }
                else
                {
                    return "{ \"Count\":\"2\",\"text\":\"" + activity.ActName + "\"}";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 签到活动-查看签到列表
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetSignInLog(dynamic requestData)
        {
            int? memberId = requestData.MemberId;
            string memberName = requestData.MemberName;
            int activityId = requestData.ActivityId;
            int beginPage = (requestData.IndexPage - 1) * requestData.PageSize + 1;
            int endPage = requestData.IndexPage * requestData.PageSize;

            var sql = SqlParamHelper.Builder
                    .Append("Select s.*, r.MemberName, ROW_NUMBER() over(order by s.ActivityId desc) as num from SignInLog  as s")
                    .Append("left join RegistMember as r on s.UserId = r.MemberId");

            if (!string.IsNullOrEmpty(memberName))
            {
                sql.Where(" r.MemberName like '%" + memberName + "%'");
            }
            if (memberId != null && memberId > 0)
            {
                sql.Where(" s.UserId = "+ memberId +"");
            }
            sql.Where(" s.ActivityId = "+ activityId +"");

            var pageSql = SqlParamHelper.Builder
                   .Append("select * from (" + sql.SQL + ") as SignIn")
                   .Append("where SignIn.num >= " + beginPage + " and SignIn.num <= " + endPage + "");

            var q = dataContext.ExecuteDataTable(CommandType.Text, pageSql.SQL, sql.Arguments);

            //计算分页
            var countSql = string.Format("select Count(*) from(" + sql.SQL + ")as SignCount");
            var count = dataContext.ExecuteScalar(CommandType.Text, countSql);

            return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(q) + "}";
        }
        
        /// <summary>
        /// 删除签到活动数据
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public int RemoveSignLog(dynamic requestData)
        {
            int signId = requestData.SignId;

            if (signId > 0)
            {
                string sql = string.Format("Delete SignInLog Where SignId = {0}", signId);
                return dataContext.ExecuteNonQuery(CommandType.Text, sql);
            }
            return 0;
        }

        /// <summary>
        /// 生成活动二维码
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        [HttpPost]
        public string CreateCode_Simple(int activityId = 0)
        {
            if (activityId<=0)
            {
                return "-1";
            }

            string appid = System.Configuration.ConfigurationManager.AppSettings["WeiXinAppid"];
            string secret = System.Configuration.ConfigurationManager.AppSettings["WeiXinSecret"];

            string postData = "{\"action_name\": \"QR_LIMIT_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": " + activityId + "}}}";
            string ret = PostUrl.PostJson("https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + BasicApi.GetAccessToken(), postData);
            var result = JsonHelper.JsonToObject<QRcode>(ret);
            return result.ticket;
        } 

    }
}

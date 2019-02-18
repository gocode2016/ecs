using CreditUserAPI.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CreditUserAPI.Controllers
{
    /// <summary>
    /// 会员积分部分API
    /// </summary>
    public class MemberIntegralDetailController : ApiController
    {
        private CreditContext db = new CreditContext();
        private SqlHelper dataContext = new SqlHelper();


        #region 获取厨师积分收支详情
        /// <summary>
        /// 获取厨师积分收支详情
        /// </summary>
        /// <param name="memberId">会员ID</param>
        /// <returns></returns>
        [HttpGet]
        public string GetMemberIntegralList(int memberId = 0)
        {
            if (memberId > 0)
            {
                string sql = string.Format("Select * from MemberIntegralDetail Where MemberId = {0} Order By IntegralId desc", memberId);
                var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
                return JsonConvert.SerializeObject(q);
            }
            else
            {
                return "Request Parameter Error";
            }
        } 
        #endregion

        #region 后台获取厨师积分收支详情
        /// <summary>
        /// 后台获取厨师积分收支详情
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string FindMemberIntegralList(dynamic requestData)
        {
            int memberId = requestData.MemberId;
            string startDate = requestData.StartDate;
            string EndDate = requestData.EndDate;
            int state = requestData.State;

            int beginPage = (requestData.IndexPage - 1) * requestData.PageSize + 1;
            int endPage = requestData.IndexPage * requestData.PageSize;

            if (memberId > 0)
            {
                try
                {
                    var sql = SqlParamHelper.Builder
                                .Append("Select mi.*, ROW_NUMBER() over(order by mi.IntegralId desc) as num from MemberIntegralDetail as mi ");

                    if (state == 1)
                    {
                        sql.Where(" mi.IntegralType = 1");
                    }
                    else if (state == 2)
                    {
                        sql.Where(" mi.IntegralType = 2");
                    }
                    if (!string.IsNullOrEmpty(startDate))
                    {
                        sql.Where(" mi.CreatDate Between '" + startDate + "' and '" + EndDate + "'");
                    }
                    sql.Where(" mi.MemberId = " + memberId + "");
                    //sql.OrderBy(" mi.IntegralId DESC");

                    var pageSql = SqlParamHelper.Builder
                        .Append("select * from (" + sql.SQL + ") as memberIntegral")
                        .Append("where memberIntegral.num >= " + beginPage + " and memberIntegral.num <= " + endPage + "");

                    var q = dataContext.ExecuteDataTable(CommandType.Text, pageSql.SQL, sql.Arguments);

                    //计算分页
                    var countSql = string.Format("select Count(*) from(" + sql.SQL + ")as interalCount");
                    var Count = dataContext.ExecuteScalar(CommandType.Text, countSql);

                    //return pageSql.SQL;
                    return "{ \"Count\":\"" + Count + "\",\"data\":" + JsonConvert.SerializeObject(q) + "}";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            else
            {
                return "Request Parameter Error";
            }
        } 
        #endregion

    }
}

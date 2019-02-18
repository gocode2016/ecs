using CommonAPI.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CommonAPI.Controllers
{
    /// <summary>
    /// 元宵节活动
    /// </summary>
    public class SubjectController : ApiController
    {
        private CreditContext db = new CreditContext();
        private SqlHelper dataContext = new SqlHelper();

        /// <summary>
        /// 元宵节-获取题目
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetSubject() 
        {
            var sql = SqlParamHelper.Builder
                .Append("select * from (Select top 2 * from SweetSubject where SubLevel = 1 order by NEWID() asc) as a union")
                .Append("select * from (Select top 2 * from SweetSubject where SubLevel = 2 order by NEWID() asc) as b union")
                .Append("select * from (Select top 1 * from SweetSubject where SubLevel = 3 order by NEWID() asc) as b");

            var data = dataContext.ExecuteDataTable(CommandType.Text, sql.SQL);

            return JsonConvert.SerializeObject(data);
        }

        /// <summary>
        /// 元宵节日志及验证
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public int SetSubLog(dynamic requestData)
        {
            int memberId = requestData.MemberId;
            if (memberId <= 0)
            {
                return -1;
            }
            string memberSql = string.Format("Select Count(*) from SubjectLog where MemberId = {0}", memberId);

            var count = dataContext.ExecuteScalar(CommandType.Text, memberSql);
            if ((int)count == 0)
            {
                var sql = string.Format("insert into SubjectLog Values({0},'{1}')", memberId, DateTime.Now);
                dataContext.ExecuteNonQuery(CommandType.Text, sql);
                return 1;
            }
            else
            {
                return -1;
            }
        }

    }
}

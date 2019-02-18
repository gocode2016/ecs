using MarketActivityAPI.Common;
using MarketActivityAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MarketActivityAPI.Controllers
{
    public class SignActivityController : ApiController
    {
        private MarketActivityContext db = new MarketActivityContext();
        private SqlHelper dataContext = new SqlHelper();


        /// <summary>
        /// 根据活动ID获取活动内容
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        [HttpPost]
        public string FindSignActivity(int activityId = 0)
        {
            if (activityId <= 0)
            {
                return "-1";
            }

            SignActivity q = (from o in db.SignActivity
                              where o.ActivityId == activityId
                              select o).FirstOrDefault();

            return JsonConvert.SerializeObject(q); ;
        }

        /// <summary>
        /// 新增线下签到活动
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public int AddActivity(dynamic requestData)
        {
            string query = JsonConvert.SerializeObject(requestData);
            SignActivity model = JsonConvert.DeserializeObject<SignActivity>(query);
            model.RegistDate = DateTime.Now;
            db.SignActivity.Add(model);

            return db.SaveChanges();
        }

        /// <summary>
        /// 获取线下签到列表
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetActivityList(dynamic requestData)
        {
            int actState = requestData.ActState;
            string actName = requestData.ActName;
            string date = requestData.ActDate;

            int beginPage = (requestData.IndexPage - 1) * requestData.PageSize + 1;
            int endPage = requestData.IndexPage * requestData.PageSize;

            var sql = SqlParamHelper.Builder
                    .Append("Select s.*, ROW_NUMBER() over(order by s.ActivityId desc) as num from SignActivity  as s");

            if (actState != 0)
            {
                sql.Where(" s.ActState = "+ actState +"");
            }
            if (!string.IsNullOrEmpty(actName))
            {
                sql.Where(" s.ActName like '%"+ actName +"%'");
            }
            if (!string.IsNullOrEmpty(date))
            {
                sql.Where(" s.StartTime < '"+ date +"' and s.EndTime > '"+ date +"'");
            }
            var pageSql = SqlParamHelper.Builder
                    .Append("select * from (" + sql.SQL + ") as activity")
                    .Append("where activity.num >= " + beginPage + " and activity.num <= " + endPage + "");

            var q = dataContext.ExecuteDataTable(CommandType.Text, pageSql.SQL, sql.Arguments);

            //计算分页
            var countSql = string.Format("select Count(*) from(" + sql.SQL + ")as actCount");
            var count = dataContext.ExecuteScalar(CommandType.Text, countSql);

            return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(q) + "}";
        }

        /// <summary>
        /// 更改活动
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public int UpdateActivity(dynamic requestData)
        {
            string query = JsonConvert.SerializeObject(requestData);
            SignActivity activity = JsonConvert.DeserializeObject<SignActivity>(query);
            EntityState statebefore = db.Entry(activity).State;
            db.Entry(activity).State = EntityState.Modified;

            return db.SaveChanges();
        }

    }
}

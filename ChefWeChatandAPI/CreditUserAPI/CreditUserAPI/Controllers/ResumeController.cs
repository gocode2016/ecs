using CreditUserAPI.Common;
using CreditUserAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CreditUserAPI.Controllers
{
    public class ResumeController : ApiController
    {
        private CreditContext db = new CreditContext();
        private SqlHelper dataContext = new SqlHelper();

        #region 会员新增从业经历
        /// <summary>
        /// 会员新增从业经历
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public int SaveMemberResume(dynamic requestData)
        {
            string query = JsonConvert.SerializeObject(requestData);
            MemberResume model = JsonConvert.DeserializeObject<MemberResume>(query);

            db.MemberResume.Add(model);
            db.SaveChanges();

            return model.ResumeId;
        } 
        #endregion

        #region 会员更改从业经历
        /// <summary>
        /// 会员更改从业经历
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public int UpdateMemberResume(dynamic requestData)
        {
            string query = JsonConvert.SerializeObject(requestData);
            MemberResume model = JsonConvert.DeserializeObject<MemberResume>(query);

            EntityState statebefore = db.Entry(model).State;
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();

            return model.ResumeId;
        } 
        #endregion

        #region 查询会员从业经历
        /// <summary>
        /// 查询会员从业经历
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetMemberResumeList(dynamic requestData)
        {

            var sql = string.Format("Select * from MemberResume Where MemberId = {0}", requestData.MemberId);
            var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
            return JsonConvert.SerializeObject(q);
        } 
        #endregion

        #region 会员从业经历详细信息
        /// <summary>
        /// 会员从业经历详细信息
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetMemberResume(dynamic requestData)
        {
            var sql = string.Format("Select * from MemberResume Where ResumeId = {0}", requestData.ResumeId);
            var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
            return JsonConvert.SerializeObject(q);
        } 
        #endregion

    }
}


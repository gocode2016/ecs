using CreditUserAPI.Common;
using CreditUserAPI.Models;
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
    /// 注册码部分API
    /// </summary>
    public class RegistCodeController : ApiController
    {
        private CreditContext db = new CreditContext();
        private SqlHelper dataContext = new SqlHelper();

        #region 队员生成注册码（一次十条）
        /// <summary>
        /// 队员生成注册码（一次十条）
        /// </summary>
        /// <param name="salemanId">队员ID</param>
        /// <returns>返回是否新增成功</returns>
        [HttpGet]
        public string AddRegistCode(string salemanId)
        {
            try
            {
                int Id = Convert.ToInt32(salemanId);
                if (Id <= 0)
                {
                    return "Request Parameter Error";
                }

                for (int i = 0; i < 10; i++)
                {
                    RegistCode code = new RegistCode();
                    code.SalemanId = Id;
                    code.MemberId = 0;
                    code.RegistCodeState = 1;
                    code.GenerDate = DateTime.Now;
                    //code.UseDate = DateTime.MinValue;

                    db.RegistCode.Add(code);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return ("程序出错，信息为:" + ex.InnerException);
            }

            return "Succss";
        } 
        #endregion

        #region 获取队员注册码列表
        /// <summary>
        /// 获取队员注册码列表
        /// </summary>
        /// <param name="salemanId">队员Id</param>
        /// <returns></returns>
        [HttpPost]
        public string RegistCodeList(dynamic requestData)
        {
            int salemanId = requestData.SalemanId;
            int codeState = requestData.CodeState;

            if (salemanId > 0)
            {
                if (codeState == 1)
                {
                    string sql = string.Format("select * from RegistCode where SalemanId  = {0} and RegistCodeState = 1", salemanId);
                    var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
                    return JsonConvert.SerializeObject(q);
                }
                else
                {
                    string sql = string.Format("select rc.*, rm.MemberName, rm.HotelName from RegistCode as rc inner join RegistMember as rm on rm.MemberId = rc.MemberId where SalemanId  = {0} and RegistCodeState = 2", salemanId);
                    var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
                    return JsonConvert.SerializeObject(q);
                }
                
            }
            else
            {
                return "Request Parameter Error";
            }
        } 
        #endregion

        #region 判断注册码是否合法
        /// <summary>
        /// 判断注册码是否合法
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        public string CheckRegistCode(int code = 0)
        {
            if (code > 0)
            {
                string sql = string.Format("select * from RegistCode where RegistCodeId = {0} and RegistCodeState = 1", code);
                var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
                return q.Rows.Count.ToString();
            }
            else
            {
                return "Request Parameter Error";
            }
        } 
        #endregion

        /// <summary>
        /// 会员使用注册码
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string BindRegistCode(dynamic requestData)
        {
            string code = requestData.RegistCode;
            string memberId = requestData.MemberId;

            //判定为会员根据注册码查询出队员ID 
            var salmanSql = string.Format("Select SalemanId from RegistCode Where RegistCodeId = {0}", code);
            var salemanId = dataContext.ExecuteScalar(CommandType.Text, salmanSql);

            var sql = string.Format("Select RegistCodeState from RegistCode Where RegistCodeId = {0} and MemberId <> -1", requestData.RegistCode);
            var q = dataContext.ExecuteScalar(CommandType.Text, sql);
            if (q == null || Convert.ToInt32(q) != 1)
            {
                return "-1";
            }

            try
            {
                if (Convert.ToInt32(salemanId) > 0)
                {
                    string useCodeSql = string.Format("Update RegistCode Set MemberId = {0}, RegistCodeState = 2, UseDate = '{2}' where RegistCodeId = {1}; Insert into MemberRef Values({0}, {3}, '{2}'); Update RegistMember Set RecommendId = {3} where MemberId = {0};", memberId, code, DateTime.Now, salemanId);
                    dataContext.ExecuteNonQuery(CommandType.Text, useCodeSql);
                }
                else
                {
                    return "-1";
                }
            }
            catch (Exception ex)
            {
                return "-1";
            }

            return "Exc Success";
        }

        /// <summary>
        /// 队员绑定会员
        /// </summary>
        /// <param name="requesData"></param>
        /// <returns></returns>
        [HttpPost]
        public string BindMember(dynamic requestData)
        {
            string memberId = requestData.MemberId;
            string salemanId = requestData.SalemanId;

            if (string.IsNullOrEmpty(memberId) || string.IsNullOrEmpty(salemanId))
            {
                return "Exc error";
            }

            try
            {
                RegistCode model = new RegistCode();
                model.SalemanId = Convert.ToInt32(salemanId);
                model.MemberId = Convert.ToInt32(memberId);
                model.RegistCodeState = 2;
                model.GenerDate = DateTime.Now;
                model.UseDate = DateTime.Now;

                db.RegistCode.Add(model);
                db.SaveChanges();

                string useCodeSql = string.Format("Insert into MemberRef Values({0}, {2}, '{1}'); Update RegistMember Set RecommendId = {2} where MemberId = {0};", memberId, DateTime.Now, salemanId);
                dataContext.ExecuteNonQuery(CommandType.Text, useCodeSql);

            }
            catch (Exception ex)
            {
                return ex.Message;
            }


            return "Exc Success";
        }

        /// <summary>
        /// 获取绑定信息
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetMemberRegistCode(dynamic requestData)
        {
            string memberId = requestData.MemberId;

            if (string.IsNullOrEmpty(memberId))
            {
                return "Exc Success";
            }

            string sql = string.Format("select r.RecommendId, s.Name, s.Telephone, r.RegistDate from [MemberRef] as r left join RegistSaleMan as s on r.RecommendId = s.SalemanId where r.MemberId = {0}",memberId);

            var q = dataContext.ExecuteDataTable(CommandType.Text, sql);

            return JsonConvert.SerializeObject(q);
        }

    }
}

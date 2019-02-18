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
    /// OPENID信息API
    /// </summary>
    public class OpenIdAssociatedController : ApiController
    {
        private CreditContext db = new CreditContext();
        private SqlHelper dataContext = new SqlHelper();

        #region OpenId用户关联表-新增
        /// <summary>
        /// OpenId用户关联表-新增
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddOpenIdAssociate(string requestData)
        {
            OpenIdAssociated model = JsonConvert.DeserializeObject<OpenIdAssociated>(requestData);
            db.OpenIdAssociated.Add(model);
            db.SaveChanges();

            return model.UserId.ToString();
        } 
        #endregion

        #region 根据OpenId获取用户
        /// <summary>
        /// 根据OpenId获取用户
        /// </summary>
        /// <parm name="openId"></param>
        /// <returns></returns>
        [HttpGet]
        public string FindUserByOpenId(string openId = "")
        {
            if (!string.IsNullOrEmpty(openId))
            {
                var sql = string.Format("Select * from OpenIdAssociated Where OpenId = '{0}'", openId);
                var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
                if (q == null)
                {
                    return "-1";
                }
                return JsonConvert.SerializeObject(q);
            }
            else
            {
                return "Request Parameter Error.";
            }
        } 
        #endregion
    }
}

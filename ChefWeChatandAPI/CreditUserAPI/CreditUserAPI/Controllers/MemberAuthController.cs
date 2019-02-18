using CreditUserAPI.Common;
using CreditUserAPI.Models;
using CreditUserAPI.WeiXin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace CreditUserAPI.Controllers
{
    /// <summary>
    /// 会员花式认证
    /// </summary>
    public class MemberAuthController : ApiController
    {
        private CreditContext db = new CreditContext();
        private SqlHelper dataContext = new SqlHelper();

        /// <summary>
        /// 会员实名认证
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string RealAuthAdd(dynamic requestData)
        {
            try
            {
                string query = JsonConvert.SerializeObject(requestData);
                MemberRealAuth model = JsonConvert.DeserializeObject<MemberRealAuth>(query);
                model.AuthTime = DateTime.Now;

                db.MemberRealAuth.Add(model);
                db.SaveChanges();
                //return JsonConvert.SerializeObject(model);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "Exc Success";
        }

        /// <summary>
        /// 修改会员实名认证
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string RealAuthUpdate(dynamic requestData)
        {
            string query = JsonConvert.SerializeObject(requestData);
            MemberRealAuth model = JsonConvert.DeserializeObject<MemberRealAuth>(query);
            model.AuthTime = DateTime.Now;
            EntityState statebefore = db.Entry(model).State;
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();

            return "Exc Success";
        }

        /// <summary>
        /// 查询实名认证信息
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetMemberAuth(int memberId)
        {
            if (memberId <= 0)
            {
                return "request error";
            }

            MemberRealAuth auth = (from o in db.MemberRealAuth
                                   where o.MemberId == memberId
                                   select o).FirstOrDefault();

            return JsonConvert.SerializeObject(auth);
        }

        /// <summary>
        /// 认证反馈信息接口
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AuthSuccess(dynamic requestData)
        {
            int memberId = requestData.MemberId;
            string remark = requestData.Remark;
            int authState = requestData.AuthState;

            if (memberId <= 0 || string.IsNullOrEmpty(remark))
            {
                return "request error";
            }

            try
            {
                int recommendid = 0;
                if (authState == 1)
                {
                    recommendid = 99;
                }
                string sql = string.Format("Update MemberRealAuth Set AuthState = {0}, Remark = '{1}' Where MemberId = {2}; Update RegistMember Set RecommendId = {3} Where MemberId = {2}", authState, remark, memberId,recommendid);
                dataContext.ExecuteNonQuery(CommandType.Text, sql);

                string openId = (from m in db.Member
                              join o in db.OpenIdAssociated on m.MemberId equals o.UserId
                              where o.UserType == 2 && o.UserId == memberId
                              select o.OpenId).SingleOrDefault();

                if (string.IsNullOrEmpty(openId))
                {
                    return "Msg error";
                }
                string msg = string.Empty;
                if (authState == 1)
                {
                    string json = "{\"touser\":\"" + openId + "\"," +
                                    "\"template_id\":\"b0-aVm0y5Urmbup-cnCykHlYWHiuaGYy9DabB-ePetU\"," +
                                    "\"url\":\"http://jifenweixin.shinho.net.cn/#/component/personal\"," +
                                    "\"data\":{" +
                                            "\"first\": {" +
                                                "\"value\":\"通过——审核结果通知\"," +
                                                "\"color\":\"#173177\"" +
                                            "}," +
                                            "\"keyword1\":{" +
                                                "\"value\":\"平台会员认证\"," +
                                                "\"color\":\"#173177\"" +
                                            "}," +
                                         "\"keyword2\":{" +
                                                 "\"value\":\"" + DateTime.Now + "\"," +
                                                "\"color\":\"#173177\"" +
                                            "}," +
                                            "\"remark\":{" +
                                                "\"value\":\"" + remark + "\"," +
                                                "\"color\":\"#173177\"" +
                                            "}" +
                                    "}" +
                                "}";

                    SendMuBanMsg(json);
                }
                else
                {
                    string json = "{\"touser\":\"" + openId + "\"," +
                                    "\"template_id\":\"b0-aVm0y5Urmbup-cnCykHlYWHiuaGYy9DabB-ePetU\"," +
                                    "\"url\":\"http://jifenweixin.shinho.net.cn/#/component/personal\"," +
                                    "\"data\":{" +
                                            "\"first\": {" +
                                                "\"value\":\"不通过——审核结果通知\"," +
                                                "\"color\":\"#173177\"" +
                                            "}," +
                                            "\"keyword1\":{" +
                                                "\"value\":\"平台会员认证\"," +
                                                "\"color\":\"#173177\"" +
                                            "}," +
                                         "\"keyword2\":{" +
                                                 "\"value\":\"" + DateTime.Now + "\"," +
                                                "\"color\":\"#173177\"" +
                                            "}," +
                                            "\"remark\":{" +
                                                "\"value\":\"" + remark + ",点击详情去修改\"," +
                                                "\"color\":\"#173177\"" +
                                            "}" +
                                    "}" +
                                "}";

                    SendMuBanMsg(json);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "Exc Success";
        }

        public string SendMuBanMsg(string json)
        {
            string q = PostJson("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + BasicApi.GetAccessToken(), json);
            return q;
        }


        /// <summary>
        /// post提交json
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        private string PostJson(string url, string json)
        {
            string ret = string.Empty;
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                byte[] data = System.Text.Encoding.UTF8.GetBytes(json);
                request.Method = "POST";
                request.ContentType = "application/json"; //设置内容类型
                request.ContentLength = data.Length;
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);
                    reqStream.Close();
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.IO.Stream stream = response.GetResponseStream();
                System.IO.StreamReader streamReader = new System.IO.StreamReader(stream, Encoding.UTF8);
                ret = streamReader.ReadToEnd();

                streamReader.Close();
                stream.Close();
                response.Close();

                return ret;
            }
            catch (Exception ex)
            {
                //LogHelper.WriteLog(string.Format("{0}\n{1}\n{2}\n", ex.Message, ex.Source, ex.StackTrace));
                return ret;
            }

        }

    }
}

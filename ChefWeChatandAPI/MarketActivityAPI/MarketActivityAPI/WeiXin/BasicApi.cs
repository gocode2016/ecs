using MarketActivityAPI.Common;
using MarketActivityAPI.WeiXin.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;

namespace MarketActivityAPI.WeiXin
{
    public class BasicApi
    {
        static object _lock = new object();
        static string token = ConfigurationManager.AppSettings["WeiXinToken"];
        static string encodingAESKey = ConfigurationManager.AppSettings["WeiXinEncodingAESKey"];
        static string appid = ConfigurationManager.AppSettings["WeiXinAppid"];
        static string secret = ConfigurationManager.AppSettings["WeiXinSecret"];
        static string domain = ConfigurationManager.AppSettings["WeChat"];

        private const long EpochOffsetTicks = 621355968000000000;
        private const long MillisecondsPerTicks = 10000;
        private const long SecondsPerTicks = 10000000;

        public static string GetToken()
        {
            return token;
        }

        public static string GetEncodingAESKey()
        {
            return encodingAESKey;
        }

        /// <summary>
        /// 微信回调入口
        /// </summary>
        /// <param name="Request"></param>
        /// <param name="Response"></param>       
        public static void ReceviceMain(HttpRequestBase Request, HttpResponseBase Response)
        {
            try
            {
                if (Request.HttpMethod.ToLower() != "post")
                {
                    Response.End();
                }
                else
                {
                    Stream s = System.Web.HttpContext.Current.Request.InputStream;
                    byte[] b = new byte[s.Length];
                    s.Read(b, 0, (int)s.Length);
                    var postStr = Encoding.UTF8.GetString(b);
                    string sMsg = "";
                    //LogHelper.WriteLog("post结果：" + postStr);
                    var checkgrp = new ResultCryptography()
                    {
                        encrypttype = "raw",
                        timestamp = "",
                        nonce = "",
                        sToken = token,
                        sEncodingAESKey = encodingAESKey,
                        sAppID = appid
                    };
                    if (Request.QueryString["encrypt_type"] != null && Request.QueryString["encrypt_type"].ToLower() == "aes")
                    {
                        //解密
                        string sReqMsgSig = Request.QueryString["msg_signature"].ToString();
                        string sReqTimeStamp = Request.QueryString["timestamp"].ToString();
                        string sReqNonce = Request.QueryString["nonce"].ToString();

                        checkgrp.encrypttype = "aes";
                        checkgrp.timestamp = sReqTimeStamp;
                        checkgrp.nonce = sReqNonce;

                        WXBizMsgCrypt wxcpt = new WXBizMsgCrypt(token, encodingAESKey, appid);

                        int ret = 0;
                        ret = wxcpt.DecryptMsg(sReqMsgSig, sReqTimeStamp, sReqNonce, postStr, ref sMsg);

                        if (ret != 0)
                        {
                            LogHelper.WriteLog("ERR: Decrypt fail:" + ErrorManage.GetErrorInfo(ret));
                            return;
                        }
                    }
                    else
                    {
                        sMsg = postStr;
                    }
                    var msgModel = XmlHelpler.GetMsg(sMsg);

                    ReceiveMsg.SwitchCase(Response, msgModel, checkgrp);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("ReceviceMain-" + ex.Message + "," + ex.Source + "," + ex.StackTrace);
            }
        }

        /// <summary>
        /// 验证微信签名
        /// </summary>
        public static bool CheckSignature(string token, string signature, string timestamp, string nonce)
        {
            string[] ArrTmp = { token, timestamp, nonce };

            Array.Sort(ArrTmp);
            string tmpStr = string.Join("", ArrTmp);

            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();

            LogHelper.WriteLog(tmpStr +"|"+ signature);

            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string SHA(string content)
        {
            SHA1 algorithm = SHA1.Create();
            byte[] data = algorithm.ComputeHash(Encoding.UTF8.GetBytes(content));
            string sh1 = "";
            for (int i = 0; i < data.Length; i++)
            {
                sh1 += data[i].ToString("x2").ToUpperInvariant();
            }
            return sh1;
        } 


        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <returns></returns>
        public static string GetAccessToken()
        {
            string key = "AccessToken";

            AccessToken token = null;
            try
            {
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appid, secret));
                req.Method = "GET";
                using (WebResponse wr = req.GetResponse())
                {
                    HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();
                    StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                    string content = reader.ReadToEnd();

                    if (!string.IsNullOrEmpty(content))
                     {
                        token = JsonHelper.JsonToObject<AccessToken>(content);
                        if (token != null)
                        {
                            //LogHelper.WriteLog("Token插入缓存,Token有效期为:" + (token.expires_in - 1800).ToString());
                            CacheExtend.CacheInsert(key, token, DateTime.Now.AddSeconds(token.expires_in - 1800));
                        }
                        else
                        {
                            LogHelper.WriteLog(ErrorManage.GetErrorInfo(token.errcode ?? -1));
                        }
                    }
                    else
                    {
                        LogHelper.WriteLog("Token 返回的值为NULL");
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(string.Format("{0}\n{1}\n{2}\n", ex.Message, ex.Source, ex.StackTrace));
            }
            return (token == null ? "" : token.access_token);
            

        }

        /// <summary>
        /// 获取jsapi_ticket
        /// </summary>
        /// <returns></returns>
        public static string GetJsapiTicket()
        {
            string accessToken = GetAccessToken();
            string key = "jsapi_ticket";
            if (CacheExtend.HasCache(key))
            {
                return CacheExtend.GetCache<JsapiTicket>(key).ticket;
            }

            lock (_lock)
            {
                if (CacheExtend.HasCache(key))
                {
                    return CacheExtend.GetCache<JsapiTicket>(key).ticket;
                }
                JsapiTicket ticket = null;
                try
                {
                    HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", accessToken));
                    req.Method = "GET";
                    using (WebResponse wr = req.GetResponse())
                    {
                        HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();
                        StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                        string content = reader.ReadToEnd();

                        if (!string.IsNullOrEmpty(content))
                        {
                            ticket = JsonHelper.JsonToObject<JsapiTicket>(content);
                            if (ticket != null)
                            {
                                //LogHelper.WriteLog("jsapi_ticket插入缓存,jsapi_ticket有效期为:" + (ticket.expires_in - 1800).ToString());
                                CacheExtend.CacheInsert(key, ticket, DateTime.Now.AddSeconds(ticket.expires_in - 1800));
                            }
                            else
                            {
                                LogHelper.WriteLog(ErrorManage.GetErrorInfo(ticket.errcode ?? -1));
                            }
                        }
                        else
                        {
                            LogHelper.WriteLog("jsapi_ticket 返回的值为NULL");
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(string.Format("{0}\n{1}\n{2}\n", ex.Message, ex.Source, ex.StackTrace));
                }
                return (ticket == null ? "" : ticket.ticket);
            }
        }

        /// <summary>
        /// 获取jsapi_ticket签名
        /// </summary>
        public static string GetJsapiTicketSignature(string noncestr, string timestamp, string url)
        {
            string jsapi_ticket = GetJsapiTicket();
            string string1 = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}",jsapi_ticket,noncestr,timestamp,url);
            return FormsAuthentication.HashPasswordForStoringInConfigFile(string1, "SHA1");
        }


        #region 网页授权

        /// <summary>
        /// 网页授权
        /// </summary>
        /// <returns></returns>
        public static string Authorize(string ret)
        {
            string redirect_uri = System.Web.HttpUtility.UrlEncode(domain + "/WeChat/Authorize?ret=" + ret);
            return string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state=1#wechat_redirect", appid, redirect_uri);
        }

        /// <summary>
        /// 网页授权根据code获取access_token
        /// </summary>
        /// <param name="code"></param>
        public static OAuthAccessToken GetOAuthAccessToken(string code)
        {
            string content= RequestCreate(string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", appid, secret, code));
            if (!string.IsNullOrEmpty(content))
            {
                return JsonHelper.JsonToObject<OAuthAccessToken>(content);
            }
            return null;
        }

        /// <summary>
        /// 根据access_token和openid获取用户信息
        /// </summary>
        /// <param name="access_token"></param> string access_token, string openid
        /// <param name="openid"></param>
        public static UserInfo GetUserInfo(string code)
        {
            OAuthAccessToken token = GetOAuthAccessToken(code);
            string content = RequestCreate(string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN", token.access_token, token.openid));
            if (!string.IsNullOrEmpty(content))
            {
                return JsonHelper.JsonToObject<UserInfo>(content);
            }
            return null;
        }

        #endregion

        /// <summary>
        /// get提交获取数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string RequestCreate(string url)
        {
            string content = string.Empty;
            try
            {
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
                req.Method = "GET";
                using (WebResponse wr = req.GetResponse())
                {
                    HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();
                    StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                    content = reader.ReadToEnd();
                    if (string.IsNullOrEmpty(content))
                    {
                        LogHelper.WriteLog("返回的值为NULL;Url:" + url);
                    }
                    else
                    {
                        LogHelper.WriteLog("请求的url为：" + url + "。现在的时间是" + DateTime.Now.ToString());
                    }   
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(string.Format("{0}\n{1}\n{2}\n", ex.Message, ex.Source, ex.StackTrace));
            }
            return content;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long LocalTimeToEpochTimeSeconds(DateTime time)
        {
            return (time.ToUniversalTime().Ticks - EpochOffsetTicks) / SecondsPerTicks;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long LocalTimeToEpochTimeMilliseconds(DateTime time)
        {
            return (time.ToUniversalTime().Ticks - EpochOffsetTicks) / MillisecondsPerTicks;
        }

    }
}

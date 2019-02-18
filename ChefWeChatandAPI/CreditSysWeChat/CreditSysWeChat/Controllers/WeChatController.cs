using CreditSysWeChat.Common;
using CreditSysWeChat.WeiXin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
namespace CreditSysWeChat.Controllers
{
    [AllowAnonymous]
    public class WeChatController : Controller
    {
        /// <summary>
        /// 消息推送接收函数
        /// </summary>
        [HttpPost]
        public void Invoke()
        {
            BasicApi.ReceviceMain(Request, Response);
        }

        /// <summary>
        /// 校验是否为微信接入
        /// </summary>
        /// <param name="signature">微信加密签名</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <param name="echostr">随机字符串</param>
        [HttpGet]
        public void Invoke(string signature, string timestamp, string nonce, string echostr)
        {
            string token = ConfigurationManager.AppSettings["WeiXinToken"];
            LogHelper.WriteLog("post结果：" + echostr);
            if (BasicApi.CheckSignature(token, signature, timestamp, nonce))
            {
                LogHelper.WriteLog("post结果：" + echostr);
                if (!string.IsNullOrEmpty(echostr))
                {
                    Response.Write(echostr);
                    Response.End();
                }
            }
        }


        /// <summary>
        /// 获取微信用户信息接口
        /// </summary>
        /// <returns>用户信息</returns>
        [HttpGet]
        public string GetAccessToken()
        {
            var a = BasicApi.GetAccessToken();
            return a;
        }

        /// <summary>
        /// 获取微信授权地址
        /// </summary>
        /// <param name="ret">回调的url</param>
        /// <returns></returns>
        [HttpGet]
        public string GetAuthorizeUrl(string ret)
        {
            return BasicApi.Authorize(ret);
        }

        /// <summary>
        /// 获取微信用户信息接口
        /// </summary>
        /// <returns>用户信息</returns>
        [HttpGet]
        public string GetUserInfo()
        {
            var user = BasicApi.GetUserInfo("client_credential");
            return JsonConvert.SerializeObject(user);
        }
        [HttpGet]
        public void Menu()
        {
            SendMsg.CreateMenu();
        }

        [HttpGet]
        public string CreateSubscribeQRcode(int salemanId = 0)
        {
            return SendMsg.CreateSubscribeQRcode(salemanId);
        }

        [HttpGet]
        public string CreateLimitSubscribeQRcode(int scene_id = 0)
        {
            if (scene_id == 0)
            {
                return "sceneid不能为0";
            }
            return SendMsg.CreateLimitSubscribeQRcode(scene_id);
        }
        [HttpGet]
        public string CreateLimitSubscribeQRcodeByscene_str(string scene_str = "")
        {
            if (!string.IsNullOrEmpty(scene_str))
            {
                return SendMsg.CreateLimitSubscribeQRcode(scene_str);
            }
            else
            { 
                return "sceneid不能为0";
            }
           
        }
        /// <summary>
        /// 获取时间戳以及随机字符
        /// </summary>
        /// <returns></returns> 
        [HttpGet]
        public string GetTimestampAndNonceStr()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string timestamp = Convert.ToInt64(ts.TotalSeconds).ToString();
            string nonceStr = Common.ValidateCode.CreateRandomCode(10);
            Common.LogHelper.WriteLog("随机字符串:" + nonceStr);
            return timestamp + "|" + nonceStr;
        }

        [HttpGet]
        public string SaveWeChatImage(string serverId)
        {
            LogHelper.WriteLog("SaveWeChatImage-RequestPara:" + serverId);
            var url = "http://api.weixin.qq.com/cgi-bin/media/get?access_token=" + BasicApi.GetAccessToken() + "&media_id=" + serverId;

            try
            {
                var filename = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".png";
                string uploadPath = System.Web.HttpContext.Current.Server.MapPath("/Upload/WeChatImage");

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                FileStream file = new FileStream(uploadPath + "/" + filename, FileMode.Create, FileAccess.Write);//创建写入文件
                file.Close();

                var path = uploadPath + "/" + filename;

                var result = SaveImage(url, path);

                S3Unit.UploadFile("WeChatImage", path);
                if (result == "success")
                {
                    return "/WeChatImage/" + filename;
                }
                else
                {
                    return "Excute Error";
                }

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public string SaveImage(string url, string path)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

            req.ServicePoint.Expect100Continue = false;
            req.Method = "GET";
            req.KeepAlive = true;

            req.ContentType = "image/png";
            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();

            System.IO.Stream stream = null;

            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                Image.FromStream(stream).Save(path);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.ToString());
                return "Exc Error";
            }
            finally
            {
                // 释放资源
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
            }
            return "success";
        }

        /// <summary>
        /// 获取jsapi_ticket签名
        /// </summary>
        /// <param name="noncestr">随机字符串</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="url">url</param>
        /// <returns></returns>
        [HttpGet]
        public string GetJsapiTicketSignature(string noncestr, string timestamp, string url)
        {
            LogHelper.WriteLog("传递过来的参数：" + noncestr + "|" + timestamp + "|" + url);
            string result = BasicApi.GetJsapiTicketSignature(noncestr, timestamp, url);
            LogHelper.WriteLog("返回的签名："+ result);
            return result;
        }

        /// <summary>
        /// 网页授权的回调函数
        /// </summary>
        /// <param name="ret">传入的为跳转链接</param>
        public void Authorize(string ret)
        {
            string code = Request.QueryString["code"];
            //LogHelper.WriteLog("wechat:" + code + "|" + ret);
            if (string.IsNullOrEmpty(code))
            {
                Response.Write("未获取到code");
            }
            else
            {
                //调用微信API获取当前用户信息
                var user = BasicApi.GetUserInfo(code);
                if (user != null)
                {
                    LogHelper.WriteLog("wechat openid:" + user.openid + "NickName:" + user.nickname);
                    HttpCookie cookie = new HttpCookie("WeiXinUser");

                    cookie.Values["UserId"] = "0";
                    cookie.Values["UserType"] = "0";
                    cookie.Values["Openid"] = user.openid;
                    cookie.Values["Nickname"] = Server.UrlEncode(user.nickname);
                    cookie.Values["Sex"] = user.sex;
                    cookie.Values["Province"] = Server.UrlEncode(user.province);
                    cookie.Values["City"] = Server.UrlEncode(user.city);
                    cookie.Values["Country"] = Server.UrlEncode(user.country);
                    cookie.Values["Headimgurl"] = user.headimgurl;

                    //把用户的微信信息（openid）存入cookie
                    Response.AppendCookie(cookie);
                    Response.Redirect(ret);
                }
            }
        }

    }
}
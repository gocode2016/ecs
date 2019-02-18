using CreditSysWeChat.Common;
using CreditSysWeChat.WeiXin.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace CreditSysWeChat.WeiXin
{
    public class SendMsg
    {
        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="Response"></param>
        /// <param name="msg"></param>
        public static void CreateMenu()
        
        {
            StringBuilder sb = new StringBuilder();
            string url = ConfigurationManager.AppSettings["WeChat"];
            string json = "{\"button\":[" +
                 "{\"name\":\"我的\",\"sub_button\":[" +
                //"{\"type\":\"view\",\"name\":\"我的账户\",\"url\":\"" + url + "component/personal\"}," +
                       "{\"type\":\"view\",\"name\":\"注册&账户\",\"url\":\"" + url + "?action=personal\"}," +
                       "{\"type\": \"scancode_waitmsg\",\"name\": \"扫一扫\",\"key\": \"rselfmenu_0_1\",\"sub_button\": [ ]}," +
                       "{\"type\":\"view\",\"name\":\"商城入口\",\"url\":\"" + url + "?action=shop\"}," +
                       "{\"type\":\"view\",\"name\":\"积分抽奖\",\"url\":\"" + url + "?action=integraldraw\"}," +
                       "{\"type\":\"view\",\"name\":\"会员须知\",\"url\":\"" + url + "?action=membernotice\"}" +
                 "]}," +
               "{\"type\":\"view\",\"name\":\"" + "🔥欣鲜菜谱库" + "\",\"url\":" + "\"http://jifenweixin.shinho.net.cn/?action=dishstore\"" +
                 "}," +
                "{\"name\":\"码上抢红包\",\"sub_button\":[" +
                       "{\"type\":\"view\",\"name\":\"活动入口\",\"url\":\"" + url + "?action=scanhome\"}," +
                       "{\"type\":\"view\",\"name\":\"我的零钱包\",\"url\":\"" + url + "?action=scanrecord\"}," +
                        "{\"type\":\"view\",\"name\":\"兑奖规则\",\"url\":\"" + url + "?action=scanrule\"}" +
                 "]}" +
          "]}";

            PostJson("https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + BasicApi.GetAccessToken(), json);
        }
        /// <summary>
        /// 1注册成功给1发模板消息
        /// </summary>
        /// <param name="Response"></param>
        /// <param name="msg"></param>
        public static void SendMuBanMsg(string json)
        {
            PostJson("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + BasicApi.GetAccessToken(), json);
        }

        /// <summary>
        /// 创建队员关注二维码
        /// </summary>
        public static string CreateSubscribeQRcode(int salemanId)
        {
            string json = "{\"expire_seconds\": 2592000, \"action_name\": \"QR_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": " + salemanId.ToString() + "}}}";
            string ret = PostJson("https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + BasicApi.GetAccessToken(), json);
            var q = JsonHelper.JsonToObject<QRcode>(ret);
            return q.ticket;
        }

        /// <summary>
        /// 创建永久整数类型的关注二维码
        /// </summary>
        public static string CreateLimitSubscribeQRcode(int sceneid)
        {
            string json = "{\"action_name\": \"QR_LIMIT_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": " + sceneid.ToString() + "}}}";
            string ret = PostJson("https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + BasicApi.GetAccessToken(), json);
            var q = JsonHelper.JsonToObject<QRcode>(ret);
            return q.ticket;
        }

        /// <summary>
        /// 创建永久字符串形式的二维码参数
        /// </summary>
        public static string CreateLimitSubscribeQRcode(string scene_str)
        {
            string json = "{\"action_name\": \"QR_LIMIT_STR_SCENE\", \"action_info\": {\"scene\": {\"scene_str\": \"" + scene_str + "\"}}}";
            string ret = PostJson("https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + BasicApi.GetAccessToken(), json);
            var q = JsonHelper.JsonToObject<QRcode>(ret);
            return q.ticket;
        }

        /// <summary>
        /// post提交json
        /// </summary> 
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        private static string PostJson(string url, string json)
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
                LogHelper.WriteLog(string.Format("{0}\n{1}\n{2}\n", ex.Message, ex.Source, ex.StackTrace));
                return ret;
            }

        }
    }
}

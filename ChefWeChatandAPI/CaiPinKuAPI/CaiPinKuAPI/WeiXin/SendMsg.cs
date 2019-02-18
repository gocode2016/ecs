using CaiPinKuAPI.Common;
using CaiPinKuAPI.WeiXin.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace CaiPinKuAPI.WeiXin
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
            string url = ConfigurationManager.AppSettings["WeiXinDomain"];
            string json = "{\"button\":[" +
                  "{\"name\":\"我的\",\"sub_button\":[" +
                        "{\"type\":\"view\",\"name\":\"我的账户\",\"url\":\"" + url + "component/personal\"}," +
                        "{\"type\":\"view\",\"name\":\"我要注册\",\"url\":\"" + url + "component/register\"}," +
                        "{\"type\":\"view\",\"name\":\"会员须知\",\"url\":\"" + url + "component/membernotice\"}," +
                        "{\"type\": \"scancode_waitmsg\",\"name\": \"扫一扫\",\"key\": \"rselfmenu_0_1\",\"sub_button\": [ ]}]" +
                  "}," +
                  "{\"name\":\"家味\",\"sub_button\":[" +
                        "{\"type\":\"view\",\"name\":\"积分红包\",\"url\":\"http://mp.weixin.qq.com/s/5H6_rnk86ai2fOs-ChI2Gg\"}," +
                        "{\"type\":\"view\",\"name\":\"免费抽奖\",\"url\":\"" + url + "component/hometastedraw\"}," +
                        "{\"type\":\"view\",\"name\":\"年夜饭菜谱\",\"url\":\"" + url + "component/menu\"}]" +
                  "}," +
                  "{\"name\":\"积分活动\",\"sub_button\":[" +
                        "{\"type\":\"view\",\"name\":\"每日签到\",\"url\":\"" + url + "component/sign\"}," +
                        "{\"type\":\"view\",\"name\":\"积分兑换\",\"url\":\"" + url + "component/shop\"}," +
                        "{\"type\":\"view\",\"name\":\"积分抽奖\",\"url\":\"" + url + "component/integraldraw\"}," +
                        "{\"type\":\"view\",\"name\":\"扫码规则\",\"url\":\"" + url + "component/activityrule\"}]" +
                  "}" +
           "]}";
           // string json = "{\"button\":[" +
           //       "{\"name\":\"我的\",\"sub_button\":[" +
           //             "{\"type\":\"view\",\"name\":\"我的账户\",\"url\":\"http://testjifenweixin.shinho.net.cn/#/component/personal\"}," +
           //             "{\"type\":\"view\",\"name\":\"我要注册\",\"url\":\"http://testjifenweixin.shinho.net.cn/#/component/register\"}," +
           //             "{\"type\":\"view\",\"name\":\"会员须知\",\"url\":\"http://testjifenweixin.shinho.net.cn/#/component/membernotice\"}," +
           //             "{\"type\": \"scancode_waitmsg\",\"name\": \"扫一扫\",\"key\": \"rselfmenu_0_1\",\"sub_button\": [ ]}]" +
           //       "}," +
           //       "{\"name\":\"家味\",\"sub_button\":[" +
           //     "{\"type\":\"view\",\"name\":\"每日资讯\",\"url\":\"http://m.linkshop.com/news/show.aspx?id=394095&from=singlemessage\"}," +
           //             "{\"type\":\"view\",\"name\":\"年夜饭菜谱\",\"url\":\"" + url + "component/menu\"}]" +
           //       "}," +
           //       "{\"name\":\"积分活动\",\"sub_button\":[" +
           //             "{\"type\":\"view\",\"name\":\"积分红包\",\"url\":\"http://mp.weixin.qq.com/s/5H6_rnk86ai2fOs-ChI2Gg\"}," +
           //             "{\"type\":\"view\",\"name\":\"每日签到\",\"url\":\"http://testjifenweixin.shinho.net.cn/#/component/sign\"}," +
           //             "{\"type\":\"view\",\"name\":\"积分兑换\",\"url\":\"" + url + "component/shop\"}," +
           //             "{\"type\":\"view\",\"name\":\"积分抽奖\",\"url\":\"" + url + "component/integraldraw\"}," +
           //             "{\"type\":\"view\",\"name\":\"扫码规则\",\"url\":\"" + url + "component/activityrule\"}]" +
           //       "}" +
           //"]}";
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
            string json = "{\"expire_seconds\": 1800, \"action_name\": \"QR_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": " + salemanId.ToString() + "}}}";
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CommonAPI.Controllers
{
    /// <summary>
    /// 短信部分
    /// </summary>
    public class SMSController : ApiController
    {

        /// <summary>
        /// 用户注册发送短信验证码
        /// </summary>
        /// <param name="requestData">Telephone：手机号</param>
        /// <returns>返回验证码</returns>
        [HttpPost]
        public string SendRegistSMS(dynamic requestData)
        {
            Random rd = new Random();
            string auth_code = rd.Next(100000, 999999).ToString();
            string content = string.Format("尊敬的用户，您好，注册欣和味达美餐饮服务会员的验证码是{0}，有效时间30分钟，请及时使用", auth_code);

            string flag = string.Empty;
            string url = "http://shsms.shinho.net.cn/api/sms_publicer?phone=" + requestData.Telephone + "&message=" + content + "&sysname=味达美餐饮积分系统";
            flag = GetHttp(url);
            if (flag.Split(',')[0] == "0")
            {
                flag = auth_code;
            }
            else
            {
                flag = flag.Split(',')[0];
            }

            return flag;
        }

        
        public static string GetHttp(string url)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Timeout = 20000;

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            string responseContent = streamReader.ReadToEnd();

            httpWebResponse.Close();
            streamReader.Close();

            return responseContent;
        }
    }
}

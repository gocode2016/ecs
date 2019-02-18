using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Configuration;
using System.Text;


namespace WxScanAPI.Common
{
    public class WeiXinHelper
    {
        /// <summary>
        /// 进行企业付款
        /// </summary>
        /// <param name="requestdata"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string SendCorpPayWithCert(string requestdata, string url)
        {
            string certPath = ConfigurationManager.AppSettings["certPath"].ToString();
            string certPassword = ConfigurationManager.AppSettings["certPassword"].ToString();

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            LogHelper.WriteMsg(System.AppDomain.CurrentDomain.BaseDirectory + certPath);
            X509Certificate2 cer = new X509Certificate2(System.AppDomain.CurrentDomain.BaseDirectory + certPath, certPassword, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            HttpWebRequest webrequest = (HttpWebRequest)HttpWebRequest.Create(url); 
            webrequest.ClientCertificates.Add(cer);
            webrequest.Method = "post";
            webrequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            //webrequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36";
            byte[] data = Encoding.UTF8.GetBytes(requestdata);
            webrequest.ContentLength = data.Length;
            using (Stream stream = webrequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            HttpWebResponse webreponse = (HttpWebResponse)webrequest.GetResponse();
            Stream streamReturn = webreponse.GetResponseStream();
            StreamReader sr = new StreamReader(streamReturn); //创建一个stream读取流  
            string r = sr.ReadToEnd();   //从头读到尾，放到字符串html  
            return r;
        }

        /// <summary>
        /// 发送现金红包
        /// </summary>
        /// <param name="requestdata"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string SendRedPackWithCert(string requestdata, string url)
        {
            string certPath = ConfigurationManager.AppSettings["certPath"].ToString();
            string certPassword = ConfigurationManager.AppSettings["certPassword"].ToString();

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            X509Certificate cer = new X509Certificate(certPath, certPassword);
            HttpWebRequest webrequest = (HttpWebRequest)HttpWebRequest.Create(url);
            webrequest.ClientCertificates.Add(cer);
            webrequest.Method = "post";
            webrequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            byte[] data = Encoding.UTF8.GetBytes(requestdata);
            webrequest.ContentLength = data.Length;
            using (Stream stream = webrequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            HttpWebResponse webreponse = (HttpWebResponse)webrequest.GetResponse();
            Stream streamReturn = webreponse.GetResponseStream();
            StreamReader sr = new StreamReader(streamReturn); //创建一个stream读取流  
            string r = sr.ReadToEnd();   //从头读到尾，放到字符串html  
            return r;
        }

        /*CheckValidationResult的定义*/ 
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受     
        }
    }
}
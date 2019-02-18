using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace CaiPinKuAPI.Common
{
    public static class ImageHandle
    {
        static object _lock = new object();
        /// <summary>
        /// 域名
        /// </summary>
        public static string DNS = System.Configuration.ConfigurationManager.AppSettings["ImageDomain"];
        //"http://shkapi4qas.shinho.net.cn/cts/ecapi/";
        //"http://jifenweixin.shinho.net.cn:8083/";
        /// <summary>
        /// appid
        /// </summary>
        public static string appid =
            //测试
            //"wxa9067ffebe95ca17";
            //正式
            "wx9657251bde1a71c4";

        /// <summary>
        /// secret
        /// </summary>
        public static string secret =
            //测试
            //"b4f6329d708c9b51f89d48f61a45d215";
            //正式
            "cb9dcd4b486ff56b34502e3c6d7bd64c";


        #region base64转为图片
        /// <summary>
        /// base64转为图片
        /// </summary>
        /// <param name="baseString">base64格式</param>
        /// <param name="fileType">照片格式</param>
        /// <param name="filePath">保存文件路径</param>
        /// <returns></returns>
        public static string Base64StringToImage(string baseString, string fileType, string filePath)
        {
            try
            {
                byte[] arr = Convert.FromBase64String(baseString);
                MemoryStream ms = new MemoryStream(arr);
                Bitmap bmp = new Bitmap(ms);

                var filename = DateTime.Now.ToString("yyMMddhhmmssff") + "." + fileType;
                string uploadPath = System.Web.HttpContext.Current.Server.MapPath(filePath);

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                FileStream fs1 = new FileStream(uploadPath + "/" + filename, FileMode.Create, FileAccess.Write);//创建写入文件 
                fs1.Close();
                bmp.Save(uploadPath + "/" + filename);
                ms.Close();
                return DNS + filePath + "/" + filename;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region 取出<img>中Src的信息
        /// <summary>   
        /// 取得HTML中所有图片的 URL。   
        /// </summary>   
        /// <param name="sHtmlText">HTML代码</param>   
        /// <returns>图片的URL列表</returns>   
        public static string[] GetHtmlImageUrlList(string sHtmlText)
        {
            // 定义正则表达式用来匹配 img 标签   
            //Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[''']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n'''<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

            // 搜索匹配的字符串   
            MatchCollection matches = regImg.Matches(sHtmlText);
            int i = 0;
            string[] sUrlList = new string[matches.Count];

            // 取得匹配项列表   
            foreach (Match match in matches)
                sUrlList[i++] = match.Groups["imgUrl"].Value;
            return sUrlList;
        }
        #endregion
    }
}
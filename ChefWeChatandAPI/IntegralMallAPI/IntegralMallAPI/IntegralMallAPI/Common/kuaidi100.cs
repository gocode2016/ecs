using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace IntegralMallAPI.Common
{
    public class kuaidi100
    {
        public static Dictionary<string, string> Getcompany()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("zhongtong", "中通");
            dic.Add("shentong", "申通");
            dic.Add("yuantong", "圆通");
            dic.Add("ems", "EMS");
            dic.Add("yunda", "韵达");
            return dic;
        }

        /// <summary>
        /// 获取快递信息
        /// </summary>
        /// <returns></returns>
        public static string GetInfo(string code, string postId)
        {
            var val = GetHtml(code, postId);
            //return HtmlSubstring(val);
            return val;
        }

        /// <summary>
        /// 获取页面源代码
        /// </summary>
        private static string GetHtml(string code, string postId)
        {
            //string url = "http://wap.kuaidi100.com/wap_result.jsp?rand=" + DateTime.Now.ToString("ddmmss") + "&id=" + code + "&fromWeb=null&&postid=" + postId;
            string url = "https://m.kuaidi100.com/query?type=" + code + "&postid=" + postId;
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader reader = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 20000;
                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK && response.ContentLength < 1024 * 1024)
                {
                    reader = new StreamReader(response.GetResponseStream());
                    string html = reader.ReadToEnd();
                    return html;
                }
            }
            catch { }
            finally
            {
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                if (reader != null)
                    reader.Close();
                if (request != null)
                    request = null;
            }
            return string.Empty;
        }

        /// <summary>
        /// 正则截取字符串
        /// </summary>
        /// <returns></returns>
        private static string HtmlSubstring(string val)
        {
            string ret = "暂无快递信息";
            try
            {
                var math = Regex.Match(val, "<p><strong>查询结果如下所示：</strong></p>([.\\s\\S]*?)</form>", RegexOptions.IgnoreCase);
                if (math.Groups.Count == 2)
                {
                    ret = math.Groups[1].Value;
                }
            }
            catch (ArgumentException ex)
            {
                return "发生错误：" + ex.Message;
            }
            return ret;
        }
    }
}
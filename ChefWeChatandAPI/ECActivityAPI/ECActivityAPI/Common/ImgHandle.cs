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

namespace ECActivityAPI.Common
{
    public class ImgHandle
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
        public string Base64StringToImage(string baseString, string fileType,string filePath)
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
        public  string[] GetHtmlImageUrlList(string sHtmlText)
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

        #region Table分页
        public DataTable GetPagedTable(DataTable dt, int PageIndex, int PageSize)//PageIndex表示第几页，PageSize表示每页的记录数
        {
            if (PageIndex == 0)
                return dt;//0页代表每页数据，直接返回

            DataTable newdt = dt.Copy();
            newdt.Clear();//copy dt的框架

            int rowbegin = (PageIndex - 1) * PageSize;
            int rowend = PageIndex * PageSize;

            if (rowbegin >= dt.Rows.Count)
                return newdt;//源数据记录数小于等于要显示的记录，直接返回dt

            if (rowend > dt.Rows.Count)
                rowend = dt.Rows.Count;
            for (int i = rowbegin; i <= rowend - 1; i++)
            {
                DataRow newdr = newdt.NewRow();
                DataRow dr = dt.Rows[i];
                foreach (DataColumn column in dt.Columns)
                {
                    newdr[column.ColumnName] = dr[column.ColumnName];
                }
                newdt.Rows.Add(newdr);
            }
            return newdt;
        }
        #endregion

        

        #region 审核发送模板消息
        /// <summary>
        /// 审核发送模板消息
        /// </summary>
        /// <returns></returns>
        public string SentTemplate(string openId, int isPass, string matchName, string context)
        {
            var audit = "";
            var url = "";
            var isUpdate = "";
            if (isPass == 1)
                audit = "通过报名审核";
            if (isPass == 2)
            {
                audit = "没有通过报名审核:" + context;
                url = "\"url\":\"" + "http://testjifenweixin.shinho.net.cn/#/component/enterstep" + "\",";
                isUpdate = "，单击修改";
            }
            var json = "{\"touser\":\"" + openId + "\"," +
                     "\"template_id\":\"2_8pRWBnNSV-fJoLkhWyHN4Fkh3qnOtzuKB7WPv17GY\"," +
                //"\"url\":\"" + url+ "\"," +
                     url +
                     "\"data\":{" +
                             "\"first\": {" +
                                 "\"value\":\"您参加的" + matchName + "活动审核结果\"," +
                                 "\"color\":\"#ff0000\"" +
                             "}," +
                             "\"keyword1\":{" +
                                 "\"value\":\"" + matchName + "\"," +
                                 "\"color\":\"#173177\"" +
                             "}," +
                            "\"keyword2\":{" +
                                 "\"value\":\"" + audit + "\"," +
                                 "\"color\":\"#ff0000\"" +
                             "}," +
                             "\"keyword3\":{" +
                                 "\"value\":\"" + DateTime.Now + "\"," +
                                 "\"color\":\"#173177\"" +
                             "}," +
                             "\"remark\":{" +
                                 "\"value\":\"感谢您的参加" + isUpdate + "\"," +
                                 "\"color\":\"#173177\"" +
                             "}" +
                     "}" +
                 "}";
          
            PostJson("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" +this.GetAccessToken(), json);

            return "";
        } 
        #endregion

        #region 审核发送模板消息
        /// <summary>
        /// 审核发送模板消息
        /// </summary>
        /// <returns></returns>
        public string SentTemplate(string openId, string matchName, int isPass, string context)
        {
            var audit = "";
            var url = "";
            var isUpdate = "";
            if (isPass == 1)
                audit = "通过报名审核";
            if (isPass == 2)
            {
                audit = "没有通过报名审核:" + context;
                url = "\"url\":\"" + "http://testjifenweixin.shinho.net.cn/#/component/enterstep" + "\",";
                isUpdate = "，单击修改";
            }
            var json = "{\"touser\":\"" + openId + "\"," +
                     "\"template_id\":\"2_8pRWBnNSV-fJoLkhWyHN4Fkh3qnOtzuKB7WPv17GY\"," +
                //"\"url\":\"" + url+ "\"," +
                     url +
                     "\"data\":{" +
                             "\"first\": {" +
                                 "\"value\":\"您参加的" + matchName + "活动审核结果\"," +
                                 "\"color\":\"#ff0000\"" +
                             "}," +
                             "\"keyword1\":{" +
                                 "\"value\":\"" + matchName + "\"," +
                                 "\"color\":\"#173177\"" +
                             "}," +
                            "\"keyword2\":{" +
                                 "\"value\":\"" + audit + "\"," +
                                 "\"color\":\"#ff0000\"" +
                             "}," +
                             "\"keyword3\":{" +
                                 "\"value\":\"" + DateTime.Now + "\"," +
                                 "\"color\":\"#173177\"" +
                             "}," +
                             "\"remark\":{" +
                                 "\"value\":\"感谢您的参加" + isUpdate + "\"," +
                                 "\"color\":\"#173177\"" +
                             "}" +
                     "}" +
                 "}";

            PostJson("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + this.GetAccessToken(), json);

            return "";
        }
        #endregion

        #region 审核发送模板消息
        /// <summary>
        /// 审核发送模板消息
        /// </summary>
        ///  <param name="openId">OpenId</param>
        /// <param name="dishName">菜品名称</param>
        /// <param name="context">审核意见</param>
        /// <param name="matchName">活动名称</param>
        /// <param name="isPass">是否合格</param>
        /// <param name="url">链接</param>
        /// <returns>OK，No</returns>
        public string SentTemplate(string openId,string dishName, string context, string matchName,int isApply,string url)
        {
            try
            {
                var head="";
                var audit = "";
                var end = "";
                if (isApply == 1)
                {
                    head = "您上传的菜品已经审核通过";
                    context = "已成功为您充值10元话费";
                    end = "点击详情，去查看我的菜品！";
                }
                else
                {
                    head = "您上传的菜品未审核通过";
                    audit = "未审核通过";
                    end = "点击详情，去修改我的菜品！" ;
                }

                #region 报名审核结果
                //var json = "{\"touser\":\"" + openId + "\"," +
                //         "\"template_id\":\"2_8pRWBnNSV-fJoLkhWyHN4Fkh3qnOtzuKB7WPv17GY\"," +
                //    "\"url\":\"" + url+ "\"," +
                //         //url +
                //         "\"data\":{" +
                //                 "\"first\": {" +
                //                     "\"value\":\"您上传的菜品" + dishName + "" + head + "\"," +
                //                     "\"color\":\"#ff0000\"" +
                //                 "}," +
                //                 "\"keyword1\":{" +
                //                     "\"value\":\"" + matchName + "\"," +
                //                     "\"color\":\"#173177\"" +
                //                 "}," +
                //                "\"keyword2\":{" +
                //                     "\"value\":\"" + audit + "\"," +
                //                     "\"color\":\"#ff0000\"" +
                //                 "}," +
                //                 "\"keyword3\":{" +
                //                     "\"value\":\"" + DateTime.Now + "\"," +
                //                     "\"color\":\"#173177\"" +
                //                 "}," +
                //                 "\"remark\":{" +
                //                     "\"value\":\"" + end + "\"," +
                //                     "\"color\":\"#173177\"" +
                //                 "}" +
                //         "}" +
                //     "}"; 
                #endregion

                #region 审核结果提醒

                var json = "{\"touser\":\"" + openId + "\"," +
                                "\"template_id\":\"_o0HfY7H09532W0bUuZy8IYEbyXNBjP6YKiCQTXY0jQ\"," +
                           "\"url\":\"" + url + "\"," +
                    //url +
                                "\"data\":{" +
                                        "\"first\": {" +
                                            "\"value\":\"" + head + "\"," +
                                            "\"color\":\"#ff0000\"" +
                                        "}," +
                                        "\"keyword1\":{" +
                                            "\"value\":\"" + matchName + "\"," +
                                            "\"color\":\"#173177\"" +
                                        "}," +
                                       "\"keyword2\":{" +
                                            "\"value\":\"" + dishName + "\"," +
                                            "\"color\":\"#173177\"" +
                                        "}," +
                                        "\"keyword3\":{" +
                                            "\"value\":\"" +context + "\"," +
                                            "\"color\":\"#ff0000\"" +
                                        "}," +
                                        "\"keyword4\":{" +
                                            "\"value\":\"" + DateTime.Now + "\"," +
                                            "\"color\":\"#173177\"" +
                                        "}," +
                                        "\"remark\":{" +
                                            "\"value\":\"" + end + "\"," +
                                            "\"color\":\"#173177\"" +
                                        "}" +
                                "}" +
                            "}"; 
                #endregion

                PostJson("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + this.GetAccessToken(), json);

                return "OK";
            }
            catch (Exception)
            {
                return "No";
            }
           
        }
        #endregion


        #region 发送模板消息
        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public string PostJson(string url, string json)
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
                //return ret;
                var dd = ex.Message + "   " + ex.Source + "   " + ex.StackTrace;
                return ret;
            }

        } 
        #endregion

        #region 获取AccessToken
        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <returns></returns>
        public string GetAccessToken( )
        {
            string key = "AccessToken";
            if (CacheExtend.HasCache(key))
            {
                var dada = CacheExtend.GetCache<AccessToken>(key).access_token;
                return CacheExtend.GetCache<AccessToken>(key).access_token;
            }

            lock (_lock)
            {
                if (CacheExtend.HasCache(key))
                {
                    return CacheExtend.GetCache<AccessToken>(key).access_token;
                }
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
                                //LogHelper.WriteLog(ErrorManage.GetErrorInfo(token.errcode ?? -1));
                                var dd = token.errcode;
                            }
                        }
                        else
                        {
                            //LogHelper.WriteLog("Token 返回的值为NULL");
                        }
                    }
                }
                catch (Exception ex)
                {
                    //LogHelper.WriteLog(string.Format("{0}\n{1}\n{2}\n", ex.Message, ex.Source, ex.StackTrace));
                    var dd = ex.Message + "   " + ex.Source + "  " + ex.StackTrace;
                }
                return (token == null ? "" : token.access_token);
            }
        }
        #endregion
    }

    public class AccessToken : ErrorMsg
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
    }

    public class OAuthAccessToken : ErrorMsg
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
        public string openid { get; set; }
        public string scope { get; set; }
    }

    public class ErrorMsg
    {
        public int? errcode { get; set; }
        public string errmsg { get; set; }
    }

    public class ErrorManage
    {
        private static Dictionary<int, string> dic;

        #region 赋值

        static ErrorManage()
        {
            dic = new Dictionary<int, string>();

            dic.Add(-1, "系统繁忙 ");
            dic.Add(0, "请求成功 ");
            dic.Add(40001, "获取access_token时AppSecret错误，或者access_token无效 ");
            dic.Add(40002, "不合法的凭证类型 ");
            dic.Add(40003, "不合法的OpenID ");
            dic.Add(40004, "不合法的媒体文件类型");
            dic.Add(40005, "不合法的文件类型 ");
            dic.Add(40006, "不合法的文件大小 ");
            dic.Add(40007, "不合法的媒体文件id");
            dic.Add(40008, "不合法的消息类型");
            dic.Add(40009, "不合法的图片文件大小 ");
            dic.Add(40010, "不合法的语音文件大小");
            dic.Add(40011, "不合法的视频文件大小 ");
            dic.Add(40012, "不合法的缩略图文件大小");
            dic.Add(40013, "不合法的APPID");
            dic.Add(40014, "不合法的access_token");
            dic.Add(40015, "不合法的菜单类型 ");
            dic.Add(40016, "不合法的按钮个数 ");
            dic.Add(40017, "不合法的按钮个数 ");
            dic.Add(40018, "不合法的按钮名字长度");
            dic.Add(40019, "不合法的按钮KEY长度");
            dic.Add(40020, "不合法的按钮URL长度 ");
            dic.Add(40021, "不合法的菜单版本号 ");
            dic.Add(40022, "不合法的子菜单级数 ");
            dic.Add(40023, "不合法的子菜单按钮个数 ");
            dic.Add(40024, "不合法的子菜单按钮类型 ");
            dic.Add(40025, "不合法的子菜单按钮名字长度 ");
            dic.Add(40026, "不合法的子菜单按钮KEY长度 ");
            dic.Add(40027, "不合法的子菜单按钮URL长度 ");
            dic.Add(40028, "不合法的自定义菜单使用用户 ");
            dic.Add(40029, "不合法的oauth_code ");
            dic.Add(40030, "不合法的refresh_token");
            dic.Add(40031, "不合法的openid列表 ");
            dic.Add(40032, "不合法的openid列表长度 ");
            dic.Add(40033, "不合法的请求字符，不能包含\\uxxxx格式的字符 ");
            dic.Add(40035, "不合法的参数");
            dic.Add(40038, "不合法的请求格式");
            dic.Add(40039, "不合法的URL长度 ");
            dic.Add(40050, "不合法的分组id ");
            dic.Add(40051, "分组名字不合法 ");
            dic.Add(41001, "缺少access_token参数");
            dic.Add(41002, "缺少appid参数 ");
            dic.Add(41003, "缺少refresh_token参数 ");
            dic.Add(41004, "缺少secret参数 ");
            dic.Add(41005, "缺少多媒体文件数据 ");
            dic.Add(41006, "缺少media_id参数 ");
            dic.Add(41007, "缺少子菜单数据 ");
            dic.Add(41008, "缺少oauth code ");
            dic.Add(41009, "缺少openid ");
            dic.Add(42001, "access_token超时");
            dic.Add(42002, "refresh_token超时");
            dic.Add(42003, "oauth_code超时 ");
            dic.Add(43001, "需要GET请求");
            dic.Add(43002, "需要POST请求");
            dic.Add(43003, "需要HTTPS请求");
            dic.Add(43004, "需要接收者关注 ");
            dic.Add(43005, "需要好友关系");
            dic.Add(44001, "多媒体文件为空 ");
            dic.Add(44002, "POST的数据包为空 ");
            dic.Add(44003, "图文消息内容为空");
            dic.Add(44004, "文本消息内容为空 ");
            dic.Add(45001, "多媒体文件大小超过限制");
            dic.Add(45002, "消息内容超过限制 ");
            dic.Add(45003, "标题字段超过限制 ");
            dic.Add(45004, "描述字段超过限制 ");
            dic.Add(45005, "链接字段超过限制 ");
            dic.Add(45006, "图片链接字段超过限制 ");
            dic.Add(45007, "语音播放时间超过限制 ");
            dic.Add(45008, "图文消息超过限制 ");
            dic.Add(45009, "接口调用超过限制 ");
            dic.Add(45010, "创建菜单个数超过限制 ");
            dic.Add(45015, "回复时间超过限制 ");
            dic.Add(45016, "系统分组，不允许修改 ");
            dic.Add(45017, "分组名字过长 ");
            dic.Add(45018, "分组数量超过上限");
            dic.Add(46001, "不存在媒体数据 ");
            dic.Add(46002, "不存在的菜单版本 ");
            dic.Add(46003, "不存在的菜单数据 ");
            dic.Add(46004, "不存在的用户");
            dic.Add(47001, "解析JSON/XML内容错误");
            dic.Add(48001, "api功能未授权");
            dic.Add(50001, "用户未授权该api");

            //加解密时的错误码
            dic.Add(-40001, "签名验证错误");
            dic.Add(-40002, "xml解析失败");
            dic.Add(-40003, "sha加密生成签名失败");
            dic.Add(-40004, "AESKey 非法");
            dic.Add(-40005, "appid 校验错误");
            dic.Add(-40006, "AES 加密失败");
            dic.Add(-40007, "AES 解密失败");
            dic.Add(-40008, "公众平台发送的xml不合法");
            dic.Add(-40009, "base64加密异常");
            dic.Add(-40010, "base64解密异常");
            dic.Add(-40011, "公众帐号生成回包xml失败");
        }

        #endregion

        /// <summary>
        /// 获取错误信息
        /// </summary>
        public static string GetErrorInfo(int errcode)
        {
            string errmsg = "";

            try
            {
                errmsg = dic[errcode];
            }
            catch
            {
            }

            if (string.IsNullOrEmpty(errmsg))
            {
                errmsg = dic[0];
            }

            return errmsg;
        }
    }

    public class JsonHelper
    {
        #region 对象的序列化、反序列化

        /// <summary>
        /// 对象序列化成json
        /// </summary>
        /// <param name="toJsonValue">要转化成Json的对象</param>
        /// <returns>返回序列化后的字符串</returns>
        public static string ObjectToJson(object toJsonValue)
        {
            return toJsonValue == null ? "" : JsonConvert.SerializeObject(toJsonValue);
        }

        /// <summary>
        /// json反序列化成对象
        /// </summary>
        /// <typeparam name="T">实体对象</typeparam>
        /// <param name="json">反序列化字符串</param>
        /// <returns>返回反序列化后的实体T</returns>
        public static T JsonToObject<T>(string json)
        {
            var t = JsonConvert.DeserializeObject<T>(json);

            return t;
        }

        /// <summary>
        /// json反序列化成list
        /// </summary>
        /// <typeparam name="T">实体对象</typeparam>
        /// <param name="json">反序列化字符串</param>
        /// <returns>返回反序列化后的实体T</returns>
        public static List<T> JsonToList<T>(string json)
        {
            var t = JsonConvert.DeserializeObject<List<T>>(json);

            return t;
        }

        #endregion
    }
}
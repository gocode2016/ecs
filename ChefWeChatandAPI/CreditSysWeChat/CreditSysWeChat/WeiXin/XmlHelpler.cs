using CreditSysWeChat.WeiXin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CreditSysWeChat.WeiXin
{
    public class XmlHelpler
    {
        /// <summary>
        /// 取消关注事件
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="msg"></param>
        public static string GetUnSubscribeXml(MsgModel msg)
        {
            return "";
        }

        /// <summary>
        /// 关注事件XML
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="msg"></param>
        public static string GetSubscribeXml(MsgModel model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>");
            sb.Append("<ToUserName><![CDATA[" + model.FromUserName + "]]></ToUserName>");
            sb.Append("<FromUserName><![CDATA[" + model.ToUserName + "]]></FromUserName>");
            sb.Append("<CreateTime>" + BasicApi.LocalTimeToEpochTimeSeconds(DateTime.Now) + "</CreateTime>");
            sb.Append("<MsgType><![CDATA[text]]></MsgType>");
            sb.Append("<Content><![CDATA[" + model.TextModel.Content + "]]></Content>");
            sb.Append("<FuncFlag>0<FuncFlag>");
            sb.Append("</xml>");
            return sb.ToString();
        }

        /// <summary>
        /// 扫描带参数二维码
        /// </summary>
        /// <param name="msg"></param>
        public static string GetScanningParametersXml(MsgModel model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>");
            sb.Append("<ToUserName><![CDATA[" + model.FromUserName + "]]></ToUserName>");
            sb.Append("<FromUserName><![CDATA[" + model.ToUserName + "]]></FromUserName>");
            sb.Append("<CreateTime>" + BasicApi.LocalTimeToEpochTimeSeconds(DateTime.Now) + "</CreateTime>");
            sb.Append("<MsgType><![CDATA[event]]></MsgType>");
            sb.Append("<Event><![CDATA[" + model.EventModel.Event + "]]></Event>");
            sb.Append("<EventKey><![CDATA[" + model.EventModel.EventKeyModel.EventKey + "]]><EventKey>");
            sb.Append("<Ticket><![CDATA[" + model.EventModel.EventKeyModel.Ticket + "]]><Ticket>");
            sb.Append("</xml>");
            return sb.ToString();
        }

        /// <summary>
        /// 回复文本消息XML
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="msg"></param>
        public static string GetTextXml(MsgModel model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>");
            sb.Append("<ToUserName><![CDATA[" + model.FromUserName + "]]></ToUserName>");
            sb.Append("<FromUserName><![CDATA[" + model.ToUserName + "]]></FromUserName>");
            sb.Append("<CreateTime>" + BasicApi.LocalTimeToEpochTimeSeconds(DateTime.Now) + "</CreateTime>");
            sb.Append("<MsgType><![CDATA[text]]></MsgType>");
            sb.Append("<Content><![CDATA[" + model.TextModel.Content + "]]></Content>");
            sb.Append("<FuncFlag>0<FuncFlag>");
            sb.Append("</xml>");
            return sb.ToString();
        }

        /// <summary>
        /// 回复图片消息XML
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="msg"></param>
        public static string GetImageXml(MsgModel model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>");
            sb.Append("<ToUserName><![CDATA[" + model.FromUserName + "]]></ToUserName>");
            sb.Append("<FromUserName><![CDATA[" + model.ToUserName + "]]></FromUserName>");
            sb.Append("<CreateTime>" + BasicApi.LocalTimeToEpochTimeSeconds(DateTime.Now) + "</CreateTime>");
            sb.Append("<MsgType><![CDATA[" + model.MsgType + "]]></MsgType>");
            sb.Append("<Image>");
            sb.Append("<MediaId><![CDATA[" + model.ImageModel.MediaId + "]]></MediaId>");
            sb.Append("</Image>");
            sb.Append("</xml>");
            return sb.ToString();
        }

        /// <summary>
        /// 回复语音消息XML
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="msg"></param>
        public static string GetVoiceXml(MsgModel model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>");
            sb.Append("<ToUserName><![CDATA[" + model.FromUserName + "]]></ToUserName>");
            sb.Append("<FromUserName><![CDATA[" + model.ToUserName + "]]></FromUserName>");
            sb.Append("<CreateTime>" + BasicApi.LocalTimeToEpochTimeSeconds(DateTime.Now) + "</CreateTime>");
            sb.Append("<MsgType><![CDATA[" + model.MsgType + "]]></MsgType>");
            sb.Append("<Voice>");
            sb.Append("<MediaId><![CDATA[" + model.VoiceModel.MediaId + "]]></MediaId>");
            sb.Append("</Voice>");
            sb.Append("</xml>");
            return sb.ToString();
        }

        /// <summary>
        /// 回复视频消息XML
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="msg"></param>
        public static string GetVideoXml(MsgModel model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>");
            sb.Append("<ToUserName><![CDATA[" + model.FromUserName + "]]></ToUserName>");
            sb.Append("<FromUserName><![CDATA[" + model.ToUserName + "]]></FromUserName>");
            sb.Append("<CreateTime>" + BasicApi.LocalTimeToEpochTimeSeconds(DateTime.Now) + "</CreateTime>");
            sb.Append("<MsgType><![CDATA[" + model.MsgType + "]]></MsgType>");
            sb.Append("<Video>");
            sb.Append("<MediaId><![CDATA[" + model.VideoModel.MediaId + "]]></MediaId>");
            sb.Append("<Title><![CDATA[" + model.VideoModel.Title + "]]></Title>");
            sb.Append("<Description><![CDATA[" + model.VideoModel.Description + "]]></Description>");
            sb.Append("</Video>");
            sb.Append("</xml>");
            return sb.ToString();
        }

        /// <summary>
        /// 回复音乐消息XML
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="msg"></param>
        public static string GetMusicXml(MsgModel model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>");
            sb.Append("<ToUserName><![CDATA[" + model.FromUserName + "]]></ToUserName>");
            sb.Append("<FromUserName><![CDATA[" + model.ToUserName + "]]></FromUserName>");
            sb.Append("<CreateTime>" + BasicApi.LocalTimeToEpochTimeSeconds(DateTime.Now) + "</CreateTime>");
            sb.Append("<MsgType><![CDATA[" + model.MsgType + "]]></MsgType>");
            sb.Append("<Music>");
            sb.Append("<Title><![CDATA[" + model.MusicModel.Title + "]]></Title>");
            sb.Append("<Description><![CDATA[" + model.MusicModel.Description + "]]></Description>");
            sb.Append("<MusicUrl><![CDATA[" + model.MusicModel.MusicURL + "]]></MusicUrl>");
            sb.Append("<HQMusicUrl><![CDATA[" + model.MusicModel.HQMusicUrl + "]]></HQMusicUrl>");
            sb.Append("<ThumbMediaId><![CDATA[" + model.MusicModel.ThumbMediaId + "]]></ThumbMediaId>");
            sb.Append("</Music>");
            sb.Append("</xml>");
            return sb.ToString();
        }

        /// <summary>
        /// 回复图文消息XML
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="msg"></param>
        public static string GetArticlesXml(MsgModel model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>");
            sb.Append("<ToUserName><![CDATA[" + model.FromUserName + "]]></ToUserName>");
            sb.Append("<FromUserName><![CDATA[" + model.ToUserName + "]]></FromUserName>");
            sb.Append("<CreateTime>" + BasicApi.LocalTimeToEpochTimeSeconds(DateTime.Now) + "</CreateTime>");
            sb.Append("<MsgType><![CDATA[" + model.MsgType + "]]></MsgType>");
            sb.Append("<ArticleCount><![CDATA[" + model.Articles.Count + "]]></ArticleCount>");
            sb.Append("<Articles>");
            foreach (var item in model.Articles)
            {
                sb.Append("<item>");
                sb.Append("<Title><![CDATA[" + item.Title + "]]></Title>");
                sb.Append("<Description><![CDATA[" + item.Description + "]]></Description>");
                sb.Append("<PicUrl><![CDATA[" + item.PicUrl + "]]></PicUrl>");
                sb.Append("<Url><![CDATA[" + item.Url + "]]></Url>");
                sb.Append("</item>");
            }
            sb.Append("</Articles>");
            sb.Append("</xml>");
            return sb.ToString();
        }

        public static MsgModel GetMsg(string msg)
        {
            var model = new MsgModel();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(msg);
            XmlNodeList list = doc.GetElementsByTagName("xml");
            XmlNode xn = list[0];
            model.FromUserName = xn.SelectSingleNode("//FromUserName").InnerText;
            model.ToUserName = xn.SelectSingleNode("//ToUserName").InnerText;
            model.CreateTime = Convert.ToInt64(xn.SelectSingleNode("//CreateTime").InnerText);
            model.MsgType = xn.SelectSingleNode("//MsgType").InnerText;
            switch (model.MsgType.ToLower())
            {
                case "text":
                    model.MsgId = Convert.ToInt64(xn.SelectSingleNode("//MsgId").InnerText);
                    model.TextModel = new TextModel()
                    {
                        Content = xn.SelectSingleNode("//Content").InnerText
                    };
                    break;
                case "image":
                    model.MsgId = Convert.ToInt64(xn.SelectSingleNode("//MsgId").InnerText);
                    model.ImageModel = new ImageModel()
                    {
                        MediaId = xn.SelectSingleNode("//MediaId").InnerText,
                        PicUrl = xn.SelectSingleNode("//PicUrl").InnerText
                    };
                    break;
                case "voice":
                    model.MsgId = Convert.ToInt64(xn.SelectSingleNode("//MsgId").InnerText);
                    model.VoiceModel = new VoiceModel()
                    {
                        MediaId = xn.SelectSingleNode("//MediaId").InnerText,
                        Format = xn.SelectSingleNode("//Format").InnerText
                    };
                    break;
                case "video":
                    model.MsgId = Convert.ToInt64(xn.SelectSingleNode("//MsgId").InnerText);
                    model.VideoModel = new VideoModel()
                    {
                        MediaId = xn.SelectSingleNode("//MediaId").InnerText,
                        ThumbMediaId = xn.SelectSingleNode("//ThumbMediaId").InnerText
                    };
                    break;
                case "location":
                    model.MsgId = Convert.ToInt64(xn.SelectSingleNode("//MsgId").InnerText);
                    model.LocationModel = new LocationModel()
                    {
                        Location_Y = Convert.ToDouble(xn.SelectSingleNode("//Location_Y").InnerText),
                        Location_X = Convert.ToDouble(xn.SelectSingleNode("//Location_X").InnerText),
                        Label = xn.SelectSingleNode("//Label").InnerText,
                        Scale = xn.SelectSingleNode("//Scale").InnerText
                    };
                    break;
                case "link":
                    model.MsgId = Convert.ToInt64(xn.SelectSingleNode("//MsgId").InnerText);
                    model.LinkModel = new LinkModel()
                    {
                        Description = Convert.ToDouble(xn.SelectSingleNode("//Description").InnerText),
                        Title = Convert.ToDouble(xn.SelectSingleNode("//Title").InnerText),
                        Url = xn.SelectSingleNode("//Url").InnerText
                    };
                    break;
                case "event":
                    model.EventModel = new EventModel()
                    {
                        Event = xn.SelectSingleNode("//Event").InnerText
                    };
                    switch (model.EventModel.Event.ToLower())
                    {
                        case "subscribe"://用户未关注时，进行关注后的事件推送
                            string EventKey = string.Empty;
                            try
                            {
                                EventKey = xn.SelectSingleNode("//EventKey").InnerText;
                            }
                            catch (Exception)
                            {
                            }
                            if (!string.IsNullOrEmpty(EventKey))//扫描二维码
                            {
                                model.EventModel.EventKeyModel = new EventKeyModel()
                                {
                                    EventKey = EventKey,
                                    Ticket = xn.SelectSingleNode("//Ticket").InnerText
                                };
                            }
                            break;
                        case "unsubscribe": //取消关注、、scan                            
                            break;
                        case "scan": //用户已关注时的事件推送         
                            model.EventModel.EventKeyModel = new EventKeyModel()
                            {
                                EventKey = xn.SelectSingleNode("//EventKey").InnerText,
                                Ticket = xn.SelectSingleNode("//Ticket").InnerText
                            };
                            break;
                        case "location": //上报地理位置事件         
                            model.EventModel.LocationEventModel = new LocationEventModel()
                            {
                                Latitude = Convert.ToDouble(xn.SelectSingleNode("//Latitude").InnerText),
                                Longitude = Convert.ToDouble(xn.SelectSingleNode("//Longitude").InnerText),
                                Precision = Convert.ToDouble(xn.SelectSingleNode("//Precision").InnerText)
                            };
                            break;
                        case "click": //自定义菜单事件         
                            model.EventModel.ClickEventModel = new ClickEventModel()
                            {
                                EventKey = xn.SelectSingleNode("//EventKey").InnerText
                            };
                            break;
                        case "scancode_waitmsg"://扫码带提示
                            model.EventModel.ClickEventModel = new ClickEventModel()
                            {
                                EventKey = xn.SelectSingleNode("//EventKey").InnerText,
                                ScanResult = xn.SelectSingleNode("//ScanResult").InnerText
                            };
                            break;
                        case "scancode_push":////扫码推事件 
                            model.EventModel.ClickEventModel = new ClickEventModel()
                            {
                                EventKey = xn.SelectSingleNode("//EventKey").InnerText,
                                ScanResult = xn.SelectSingleNode("//ScanResult").InnerText
                            };
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

            return model;
        }
    }
}

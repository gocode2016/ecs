using CaiPinKuAPI.Common;
using CaiPinKuAPI.WeiXin.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;




namespace CaiPinKuAPI.WeiXin
{
    public class ReceiveMsg
    {
        public static void SwitchCase(HttpResponseBase Response, MsgModel msgModel, ResultCryptography ccg)
        {

            try
            {
                switch (msgModel.MsgType.ToLower())
                {
                    case "text":
                        ResponseRecevieText(Response, msgModel, ccg);
                        break;
                    case "image":
                        if (msgModel.ImageModel != null)
                            ResponseReceviePic(Response, msgModel, ccg);
                        break;
                    case "voice":
                        if (msgModel.VoiceModel != null)
                            ResponseRecevieAudio(Response, msgModel, ccg);
                        break;
                    case "video":
                        if (msgModel.VideoModel != null)
                            ResponseRecevieVideo(Response, msgModel, ccg);
                        break;
                    case "location":
                        if (msgModel.LocationModel != null)
                            ResponseLocaltonEvent(Response, msgModel, ccg);
                        break;
                    case "link":
                        if (msgModel.LinkModel != null)
                            ResponseRecevieLink(Response, msgModel, ccg);
                        break;
                    case "event":
                        //LogHelper.WriteLog(msgModel.EventModel.Event.ToLower());
                        switch (msgModel.EventModel.Event.ToLower())
                        {
                            case "subscribe"://用户未关注时，进行关注后的事件推送                            
                                if (msgModel.EventModel.EventKeyModel != null)
                                {
                                    ResponseSubscribeEvent(Response, msgModel, ccg);
                                }
                                else//关注
                                {
                                    ResponseNoEvent(Response, msgModel, ccg);
                                }
                                break;
                            case "unsubscribe": //取消关注  
                                ResponseUnSubscribeEvent(Response, msgModel, ccg);
                                break;
                            case "scan": //用户已关注时的事件推送         
                                if (msgModel.EventModel.EventKeyModel != null)
                                {
                                    ResponseScanSubscribeEvent(Response, msgModel, ccg);
                                };
                                break;
                            case "location": //上报地理位置事件         
                                if (msgModel.EventModel.LocationEventModel != null)
                                {
                                    //ResponseLocaltonEvent(Response, msgModel, ccg);
                                };
                                break;
                            case "click": //自定义菜单事件                                     
                                if (msgModel.EventModel.ClickEventModel != null)
                                {
                                    ResponseClickEvent(Response, msgModel, ccg);
                                };
                                break;
                            case "scancode_waitmsg": //扫码带提示                                     
                                if (msgModel.EventModel.ClickEventModel != null)
                                {
                                    ResponseAdvClickEvent(Response, msgModel, ccg);
                                };
                                break;
                            case "scancode_push": //扫码推事件                                     
                                if (msgModel.EventModel.ClickEventModel != null)
                                {
                                    ResponseAdvClickEvent(Response, msgModel, ccg);
                                };
                                break;
                            case "pic_sysphoto": //系统拍照发图                                     
                                if (msgModel.EventModel.ClickEventModel != null)
                                {

                                };
                                break;
                            case "pic_photo_or_album": //拍照或者相册发图                                     
                                if (msgModel.EventModel.ClickEventModel != null)
                                {

                                };
                                break;
                            case "pic_weixin": //微信相册发图                                     
                                if (msgModel.EventModel.ClickEventModel != null)
                                {

                                };
                                break;
                            case "location_select": //发送位置                                     
                                if (msgModel.EventModel.ClickEventModel != null)
                                {

                                };
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(string.Format("接收消息-{0}\n{1}\n{2}", ex.Message, ex.Source, ex.StackTrace));
            }
        }

        #region 加密内容

        private static string GetMsg(string remsg, ResultCryptography ccg)
        {
            if (ccg.HasCrypto())
            {
                string sEncryptMsg = "";

                WXBizMsgCrypt wxcpt = new WXBizMsgCrypt(ccg.sToken, ccg.sEncodingAESKey, ccg.sAppID);
                int ret = 0;
                ret = wxcpt.EncryptMsg(remsg, ccg.timestamp, ccg.nonce, ref sEncryptMsg);

                if (ret != 0)
                {
                    LogHelper.WriteLog("ERR: Encrypt fail:" + ErrorManage.GetErrorInfo(ret));
                    return "";
                }

                return sEncryptMsg;
            }

            return remsg;
        }

        #endregion

        #region 接收普通消息(XML格式)

        /// <summary>
        /// 被动请求，接收文本消息
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="msg"></param>
        public static void ResponseRecevieText(HttpResponseBase Response, MsgModel msg, ResultCryptography ccg)
        {
            //var usr = wx.GetWeiXinUser();
            msg.MsgType = "text";
            var textResponse = new TextModel() { Content = "您好,你的消息已收到,这是自动回复" };
            if (msg.TextModel.Content != null)
            {
                if (msg.TextModel.Content == "我是欣和人") 
                {
                    textResponse.Content = "欣和人的注册地址是：" + ConfigurationManager.AppSettings["RegistSalemanUrl"];           
                }
                else if (msg.TextModel.Content == "积分" || msg.TextModel.Content == "积分产品")
                {
                    textResponse.Content = "您可以点击菜单【积分活动】-【积分规则】查询哦~";
                }
                else if (msg.TextModel.Content == "打开首页")
                {
                    textResponse.Content = ConfigurationManager.AppSettings["WeiXinDomain"];      
                }
                else if (msg.TextModel.Content.Contains("推荐码") || msg.TextModel.Content.Contains("邀请码") || msg.TextModel.Content.Contains("注册") || msg.TextModel.Content.Contains("认证"))
                {
                    textResponse.Content = "您好，当前不需要注册码也可以注册平台会员享受会员权益哦~☺ 点击此链接注册http://jifenweixin.shinho.net.cn/#/component/register如果您需要参与扫描积分活动仍需要欣和酒店业务代表认证身份哦，仅限于酒店终端用户参加🌹，后期我们将开放平台自动认证功能，请静候佳音~"; 
                }
                else if (msg.TextModel.Content == "味达美")
                {
                    textResponse.Content = "1、请问您是怎么理解EC的？\r\n2、看到味达美您会联想到什么\r\n3、味达美臻品蚝油的卖点是什么？\r\n4、味达美明年山东规划的主打产品有哪些？";
                }
                //else if (msg.TextModel.Content == "乐爽泉城")
                //{
                //    textResponse.Content = "乐爽泉城报名地址：" + ConfigurationManager.AppSettings["RegistActivityUrl"];
                //}
                //else if (msg.TextModel.Content == "我要推荐")
                //{
                //    textResponse.Content = "注册地址：http://jifenweixin.shinho.net.cn/RegistMember/NewCreate?TuiJianId=" + msg.FromUserName;  //+ usr.UserId.ToString();
                //}
                //else if (msg.TextModel.Content == "我是经销商")
                //{
                //    textResponse.Content = "注册地址：http://jifenweixin.shinho.net.cn/RegistMember/DealerCreate";
                //}
            }
            msg.TextModel = textResponse;
            var result = XmlHelpler.GetTextXml(msg);
            Response.Write(GetMsg(result, ccg));
        }

        /// <summary>
        /// 被动请求，接收图片
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="msg"></param>
        public static void ResponseReceviePic(HttpResponseBase Response, MsgModel msg, ResultCryptography ccg)
        {
            msg.ImageModel.MediaId = "9c954bmqAZ2X7o3CdeLHiYxZfnCizh9MAdrNUS1Gn6shMhFbcYiLmauIkcxpJa5B";
            msg.TextModel = new TextModel() { Content = "您好,你的图片消息已收到,这是自动回复" };
            var remsg = XmlHelpler.GetImageXml(msg);
            Response.Write(GetMsg(remsg, ccg));
        }

        /// <summary>
        /// 被动请求，接收语音
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="msg"></param>
        public static void ResponseRecevieAudio(HttpResponseBase Response, MsgModel msg, ResultCryptography ccg)
        {
            msg.TextModel = new TextModel() { Content = "您好,你的语音消息已收到,这是自动回复" };
            var remsg = XmlHelpler.GetTextXml(msg);
            Response.Write(GetMsg(remsg, ccg));
        }

        /// <summary>
        /// 被动请求，接收视频
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="msg"></param>
        public static void ResponseRecevieVideo(HttpResponseBase Response, MsgModel msg, ResultCryptography ccg)
        {
            msg.TextModel = new TextModel() { Content = "您好,你的视频消息已收到,这是自动回复" };
            var remsg = XmlHelpler.GetTextXml(msg);
            Response.Write(GetMsg(remsg, ccg));
        }

        /// <summary>
        /// 被动请求，接收链接
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="msg"></param>
        public static void ResponseRecevieLink(HttpResponseBase Response, MsgModel msg, ResultCryptography ccg)
        {
            msg.TextModel = new TextModel() { Content = "您好,你的链接消息已收到,这是自动回复" };
            var remsg = XmlHelpler.GetTextXml(msg);
            Response.Write(GetMsg(remsg, ccg));
        }

        /// <summary>
        /// 被动请求，接收语音Recongnition
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="msg"></param>
        public static void ResponseRecevieRecongnitionAudio(HttpResponseBase Response, MsgModel msg, ResultCryptography ccg)
        {
            msg.TextModel = new TextModel() { Content = "您好,开户语音识别消息已收到,这是自动回复" };
            var remsg = XmlHelpler.GetTextXml(msg);
            Response.Write(GetMsg(remsg, ccg));
        }

        #endregion

        #region 接收事件推送(XML格式)

        /// <summary>
        /// 关注事件
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="msg"></param>
        public static void ResponseSubscribeEvent(HttpResponseBase Response, MsgModel msg, ResultCryptography ccg)
        {
            string openid = msg.FromUserName;
            string ret = BasicApi.RequestCreate(ConfigurationManager.AppSettings["AddFollowerOpenId"] + "?id=" + msg.EventModel.EventKeyModel.EventKey + "&openid=" + openid);
            var remsg = "";
            try
            {
                string str = msg.EventModel.EventKeyModel.EventKey;
                if (!string.IsNullOrEmpty(str))
                {
                    int Id = Convert.ToInt32(System.Text.RegularExpressions.Regex.Replace(str, @"[^0-9]+", ""));
                    if (Id < 10000 && Id > 0)
                    {
                        msg.MsgType = "news";
                        ArticelModel articel = new ArticelModel()
                        {
                            Title = "味达美厨师会员招募啦！点击图片即可注册！",
                            Description = "积分好礼、国内外学习考察、新品试用等你来！",
                            PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqsSZZuaafJUZfNibZ109B0CXbWpLSaKEdQdibozC0LOibu9Eshs0R1yfgFNOYvI0icb7ntDKXC2k06tVw/0?wx_fmt=jpeg",
                            Url = ConfigurationManager.AppSettings["WeiXinDomain"] + "component/register"
                        };
                        ArticelModel articel2 = new ArticelModel()
                        {
                            Title = "会员扫码须知",
                            Description = "会员扫码须知",
                            PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqtaGUZZoklAKp1sE9eqTK2Mia7RSpH0AyT3lP5BaxNJkiagobOIz3Gpe2ZQs3HYz8icFKw3wxic35KApQ/0?wx_fmt=jpeg",
                            Url = "http://jifenweixin.shinho.net.cn/#/component/activityrule"
                        };
                        ArticelModel articel3 = new ArticelModel()
                        {
                            Title = "即日起至2月10日，凡任务达标者即可瓜分20万积分红包！",
                            Description = "即日起至2月10日，凡任务达标者即可瓜分20万积分红包！",
                            PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqtIndjMF7wbHqDy3CVVvrmTHXAdA9QtewoUvqgazfLlCmfhjExN3HuHk7sbtgp6trxNqt64Z01uqA/0?wx_fmt=jpeg",
                            Url = "http://mp.weixin.qq.com/s/5H6_rnk86ai2fOs-ChI2Gg"
                        };

                        msg.Articles = new List<ArticelModel>();
                        msg.Articles.Add(articel);
                        msg.Articles.Add(articel2);
                        msg.Articles.Add(articel3);
                        remsg = XmlHelpler.GetArticlesXml(msg);
                    }
                    else
                    {
                        msg.MsgType = "news";
                        ArticelModel articel = new ArticelModel()
                        {
                            Title = "味达美厨师会员招募啦！点击图片即可注册！",
                            Description = "积分好礼、国内外学习考察、新品试用等你来！",
                            PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqsSZZuaafJUZfNibZ109B0CXbWpLSaKEdQdibozC0LOibu9Eshs0R1yfgFNOYvI0icb7ntDKXC2k06tVw/0?wx_fmt=jpeg",
                            Url = ConfigurationManager.AppSettings["WeiXinDomain"] + "component/register"
                        };

                        msg.Articles = new List<ArticelModel>();
                        msg.Articles.Add(articel);
                        remsg = XmlHelpler.GetArticlesXml(msg);
                    }
                }
                else
                {
                    msg.MsgType = "news";
                    ArticelModel articel = new ArticelModel()
                    {
                        Title = "味达美厨师会员招募啦！点击图片即可注册！",
                        Description = "积分好礼、国内外学习考察、新品试用等你来！",
                        PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqsSZZuaafJUZfNibZ109B0CXbWpLSaKEdQdibozC0LOibu9Eshs0R1yfgFNOYvI0icb7ntDKXC2k06tVw/0?wx_fmt=jpeg",
                        Url = ConfigurationManager.AppSettings["WeiXinDomain"] + "component/register"
                    };
                    ArticelModel articel2 = new ArticelModel()
                    {
                        Title = "会员扫码须知",
                        Description = "会员扫码须知",
                        PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqtaGUZZoklAKp1sE9eqTK2Mia7RSpH0AyT3lP5BaxNJkiagobOIz3Gpe2ZQs3HYz8icFKw3wxic35KApQ/0?wx_fmt=jpeg",
                        Url = "http://jifenweixin.shinho.net.cn/#/component/activityrule"
                    };
                    ArticelModel articel3 = new ArticelModel()
                    {
                        Title = "即日起至2月10日，凡任务达标者即可瓜分20万积分红包！",
                        Description = "即日起至2月10日，凡任务达标者即可瓜分20万积分红包！",
                        PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqtIndjMF7wbHqDy3CVVvrmTHXAdA9QtewoUvqgazfLlCmfhjExN3HuHk7sbtgp6trxNqt64Z01uqA/0?wx_fmt=jpeg",
                        Url = "http://mp.weixin.qq.com/s/5H6_rnk86ai2fOs-ChI2Gg"
                    };
                    msg.Articles = new List<ArticelModel>();
                    msg.Articles.Add(articel);
                    msg.Articles.Add(articel2);
                    msg.Articles.Add(articel3);
                    remsg = XmlHelpler.GetArticlesXml(msg);
                }
            }
            catch
            {
                msg.MsgType = "news";
                ArticelModel articel = new ArticelModel()
                {
                    Title = "味达美厨师会员招募啦！点击图片即可注册！",
                    Description = "积分好礼、国内外学习考察、新品试用等你来！",
                    PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqsSZZuaafJUZfNibZ109B0CXbWpLSaKEdQdibozC0LOibu9Eshs0R1yfgFNOYvI0icb7ntDKXC2k06tVw/0?wx_fmt=jpeg",
                    Url = ConfigurationManager.AppSettings["WeiXinDomain"] + "component/register"
                };
                ArticelModel articel2 = new ArticelModel()
                {
                    Title = "会员扫码须知",
                    Description = "会员扫码须知",
                    PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqtaGUZZoklAKp1sE9eqTK2Mia7RSpH0AyT3lP5BaxNJkiagobOIz3Gpe2ZQs3HYz8icFKw3wxic35KApQ/0?wx_fmt=jpeg",
                    Url = "http://jifenweixin.shinho.net.cn/#/component/activityrule"
                };
                ArticelModel articel3 = new ArticelModel()
                {
                    Title = "即日起至2月10日，凡任务达标者即可瓜分20万积分红包！",
                    Description = "即日起至2月10日，凡任务达标者即可瓜分20万积分红包！",
                    PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqtIndjMF7wbHqDy3CVVvrmTHXAdA9QtewoUvqgazfLlCmfhjExN3HuHk7sbtgp6trxNqt64Z01uqA/0?wx_fmt=jpeg",
                    Url = "http://mp.weixin.qq.com/s/5H6_rnk86ai2fOs-ChI2Gg"
                };
                msg.Articles = new List<ArticelModel>();
                msg.Articles.Add(articel);
                msg.Articles.Add(articel2);
                msg.Articles.Add(articel3);
                remsg = XmlHelpler.GetArticlesXml(msg);
            }

            Response.Write(GetMsg(remsg, ccg));
        }

        #endregion

        /// <summary>
        /// 关注无事件
        /// </summary>
        /// <param name="Response"></param>
        /// <param name="msg"></param>
        /// <param name="ccg"></param>
        public static void ResponseNoEvent(HttpResponseBase Response, MsgModel msg, ResultCryptography ccg)
        {
            var remsg = "";
            try
            {
                string str = msg.EventModel.EventKeyModel.EventKey;
                if (!string.IsNullOrEmpty(str))
                {
                    int Id = Convert.ToInt32(System.Text.RegularExpressions.Regex.Replace(str, @"[^0-9]+", ""));
                    if (Id < 30000 && Id > 0)
                    {
                        msg.MsgType = "news";
                        ArticelModel articel = new ArticelModel()
                        {
                            Title = "味达美厨师会员招募啦！点击图片即可注册！",
                            Description = "积分好礼、国内外学习考察、新品试用等你来！",
                            PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqsSZZuaafJUZfNibZ109B0CXbWpLSaKEdQdibozC0LOibu9Eshs0R1yfgFNOYvI0icb7ntDKXC2k06tVw/0?wx_fmt=jpeg",
                            Url = ConfigurationManager.AppSettings["WeiXinDomain"] + "component/register"
                        };
                        ArticelModel articel2 = new ArticelModel()
                        {
                            Title = "会员扫码须知",
                            Description = "会员扫码须知",
                            PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqtaGUZZoklAKp1sE9eqTK2Mia7RSpH0AyT3lP5BaxNJkiagobOIz3Gpe2ZQs3HYz8icFKw3wxic35KApQ/0?wx_fmt=jpeg",
                            Url = "http://jifenweixin.shinho.net.cn/#/component/activityrule"
                        };
                        ArticelModel articel3 = new ArticelModel()
                        {
                            Title = "即日起至2月10日，凡任务达标者即可瓜分20万积分红包！",
                            Description = "即日起至2月10日，凡任务达标者即可瓜分20万积分红包！",
                            PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqtIndjMF7wbHqDy3CVVvrmTHXAdA9QtewoUvqgazfLlCmfhjExN3HuHk7sbtgp6trxNqt64Z01uqA/0?wx_fmt=jpeg",
                            Url = "http://mp.weixin.qq.com/s/5H6_rnk86ai2fOs-ChI2Gg"
                        };
                        msg.Articles = new List<ArticelModel>();
                        msg.Articles.Add(articel);
                        msg.Articles.Add(articel2);
                        msg.Articles.Add(articel3);
                        remsg = XmlHelpler.GetArticlesXml(msg);
                    }
                    else
                    {
                        msg.MsgType = "news";
                        ArticelModel articel = new ArticelModel()
                        {
                            Title = "味达美厨师会员招募啦！点击图片即可注册！",
                            Description = "积分好礼、国内外学习考察、新品试用等你来！",
                            PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqsSZZuaafJUZfNibZ109B0CXbWpLSaKEdQdibozC0LOibu9Eshs0R1yfgFNOYvI0icb7ntDKXC2k06tVw/0?wx_fmt=jpeg",
                            Url = ConfigurationManager.AppSettings["WeiXinDomain"] + "component/register"
                        };
                        ArticelModel articel2 = new ArticelModel()
                        {
                            Title = "会员扫码须知",
                            Description = "会员扫码须知",
                            PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqtaGUZZoklAKp1sE9eqTK2Mia7RSpH0AyT3lP5BaxNJkiagobOIz3Gpe2ZQs3HYz8icFKw3wxic35KApQ/0?wx_fmt=jpeg",
                            Url = "http://jifenweixin.shinho.net.cn/#/component/activityrule"
                        };
                        ArticelModel articel3 = new ArticelModel()
                        {
                            Title = "即日起至2月10日，凡任务达标者即可瓜分20万积分红包！",
                            Description = "即日起至2月10日，凡任务达标者即可瓜分20万积分红包！",
                            PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqtIndjMF7wbHqDy3CVVvrmTHXAdA9QtewoUvqgazfLlCmfhjExN3HuHk7sbtgp6trxNqt64Z01uqA/0?wx_fmt=jpeg",
                            Url = "http://mp.weixin.qq.com/s/5H6_rnk86ai2fOs-ChI2Gg"
                        };
                        msg.Articles = new List<ArticelModel>();
                        msg.Articles.Add(articel);
                        msg.Articles.Add(articel2);
                        msg.Articles.Add(articel3);
                        remsg = XmlHelpler.GetArticlesXml(msg);
                    }
                }
                else
                {
                    msg.MsgType = "news";
                    ArticelModel articel = new ArticelModel()
                    {
                        Title = "味达美厨师会员招募啦！点击图片即可注册！",
                        Description = "积分好礼、国内外学习考察、新品试用等你来！",
                        PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqsSZZuaafJUZfNibZ109B0CXbWpLSaKEdQdibozC0LOibu9Eshs0R1yfgFNOYvI0icb7ntDKXC2k06tVw/0?wx_fmt=jpeg",
                        Url = ConfigurationManager.AppSettings["WeiXinDomain"] + "component/register"
                    };
                    ArticelModel articel2 = new ArticelModel()
                    {
                        Title = "会员扫码须知",
                        Description = "会员扫码须知",
                        PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqtaGUZZoklAKp1sE9eqTK2Mia7RSpH0AyT3lP5BaxNJkiagobOIz3Gpe2ZQs3HYz8icFKw3wxic35KApQ/0?wx_fmt=jpeg",
                        Url = "http://jifenweixin.shinho.net.cn/#/component/activityrule"
                    };
                    ArticelModel articel3 = new ArticelModel()
                    {
                        Title = "即日起至2月10日，凡任务达标者即可瓜分20万积分红包！",
                        Description = "即日起至2月10日，凡任务达标者即可瓜分20万积分红包！",
                        PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqtIndjMF7wbHqDy3CVVvrmTHXAdA9QtewoUvqgazfLlCmfhjExN3HuHk7sbtgp6trxNqt64Z01uqA/0?wx_fmt=jpeg",
                        Url = "http://mp.weixin.qq.com/s/5H6_rnk86ai2fOs-ChI2Gg"
                    };
                    msg.Articles = new List<ArticelModel>();
                    msg.Articles.Add(articel);
                    msg.Articles.Add(articel2);
                    msg.Articles.Add(articel3);
                    remsg = XmlHelpler.GetArticlesXml(msg);
                }
            }
            catch
            {
                msg.MsgType = "news";
                ArticelModel articel = new ArticelModel()
                {
                    Title = "味达美厨师会员招募啦！点击图片即可注册！",
                    Description = "积分好礼、国内外学习考察、新品试用等你来！",
                    PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqsSZZuaafJUZfNibZ109B0CXbWpLSaKEdQdibozC0LOibu9Eshs0R1yfgFNOYvI0icb7ntDKXC2k06tVw/0?wx_fmt=jpeg",
                    Url = ConfigurationManager.AppSettings["WeiXinDomain"] + "component/register"
                };
                ArticelModel articel2 = new ArticelModel()
                {
                    Title = "会员扫码须知",
                    Description = "会员扫码须知",
                    PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqtaGUZZoklAKp1sE9eqTK2Mia7RSpH0AyT3lP5BaxNJkiagobOIz3Gpe2ZQs3HYz8icFKw3wxic35KApQ/0?wx_fmt=jpeg",
                    Url = "http://jifenweixin.shinho.net.cn/#/component/activityrule"
                };
                ArticelModel articel3 = new ArticelModel()
                {
                    Title = "即日起至2月10日，凡任务达标者即可瓜分20万积分红包！",
                    Description = "即日起至2月10日，凡任务达标者即可瓜分20万积分红包！",
                    PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqtIndjMF7wbHqDy3CVVvrmTHXAdA9QtewoUvqgazfLlCmfhjExN3HuHk7sbtgp6trxNqt64Z01uqA/0?wx_fmt=jpeg",
                    Url = "http://mp.weixin.qq.com/s/5H6_rnk86ai2fOs-ChI2Gg"
                };
                msg.Articles = new List<ArticelModel>();
                msg.Articles.Add(articel);
                msg.Articles.Add(articel2);
                msg.Articles.Add(articel3);
                remsg = XmlHelpler.GetArticlesXml(msg);
            }
            
            Response.Write(GetMsg(remsg, ccg));
        }

        /// <summary>
        /// 取消关注事件
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="msg"></param>
        public static void ResponseUnSubscribeEvent(HttpResponseBase Response, MsgModel msg, ResultCryptography ccg)
        {
            var remsg = XmlHelpler.GetUnSubscribeXml(msg);
            Response.Write(GetMsg(remsg, ccg));
        }

        /// <summary>
        /// 扫描二维码，用户已关注时，进行关注后的事件推送
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="msg"></param>
        public static void ResponseScanSubscribeEvent(HttpResponseBase Response, MsgModel msg, ResultCryptography ccg)
        {
            string str = msg.EventModel.EventKeyModel.EventKey;
            if (!string.IsNullOrEmpty(str))
            {
                int Id = Convert.ToInt32(System.Text.RegularExpressions.Regex.Replace(str, @"[^0-9]+", ""));
                if (Id < 30000 && Id > 0)
                {
                    //msg.TextModel = new TextModel() { Content = "厨师大大，点击 http://jifenweixin.shinho.net.cn/RegistMember/Create?activityId=" + Id + "链接完成签到，新用户需先完成注册" }; 
                    msg.MsgType = "news";
                    ArticelModel articel = new ArticelModel()
                    {
                        Title = "味达美厨师会员招募啦！点击图片即可注册！",
                        Description = "积分好礼、国内外学习考察、新品试用等你来！",
                        PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqsSZZuaafJUZfNibZ109B0CXbWpLSaKEdQdibozC0LOibu9Eshs0R1yfgFNOYvI0icb7ntDKXC2k06tVw/0?wx_fmt=jpeg",
                        Url = ConfigurationManager.AppSettings["WeiXinDomain"] + "component/register"
                    };
                    ArticelModel articel2 = new ArticelModel()
                    {
                        Title = "会员扫码须知",
                        Description = "会员扫码须知",
                        PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqtaGUZZoklAKp1sE9eqTK2Mia7RSpH0AyT3lP5BaxNJkiagobOIz3Gpe2ZQs3HYz8icFKw3wxic35KApQ/0?wx_fmt=jpeg",
                        Url = "http://jifenweixin.shinho.net.cn/#/component/activityrule"
                    };
                    ArticelModel articel3 = new ArticelModel()
                    {
                        Title = "即日起至2月10日，凡任务达标者即可瓜分20万积分红包！",
                        Description = "即日起至2月10日，凡任务达标者即可瓜分20万积分红包！",
                        PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqtIndjMF7wbHqDy3CVVvrmTHXAdA9QtewoUvqgazfLlCmfhjExN3HuHk7sbtgp6trxNqt64Z01uqA/0?wx_fmt=jpeg",
                        Url = "http://mp.weixin.qq.com/s/5H6_rnk86ai2fOs-ChI2Gg"
                    };
                    msg.Articles = new List<ArticelModel>();
                    msg.Articles.Add(articel);
                    msg.Articles.Add(articel2);
                    msg.Articles.Add(articel3);
                }
            }
            //else
            //{
            //    ResponseNoEvent(Response, msg, ccg);
            //}
            var remsg = XmlHelpler.GetArticlesXml(msg);
            Response.Write(GetMsg(remsg, ccg));

        }

        /// <summary>
        /// 扫描二维码，用户未关注时，进行关注后的事件推送
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="msg"></param>
        public static void ResponseScanUnSubscribeEvent(HttpResponseBase Response, MsgModel msg, ResultCryptography ccg)
        {
            var remsg = "";
            string str = msg.EventModel.EventKeyModel.EventKey;
            if (!string.IsNullOrEmpty(str))
            {
                int Id = Convert.ToInt32(System.Text.RegularExpressions.Regex.Replace(str, @"[^0-9]+", ""));
                if (Id < 30000 && Id > 0)
                {
                    msg.MsgType = "news";
                    ArticelModel articel = new ArticelModel()
                    {
                        Title = "味达美厨师会员招募啦！点击图片即可注册！",
                        Description = "积分好礼、国内外学习考察、新品试用等你来！",
                        PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqsSZZuaafJUZfNibZ109B0CXbWpLSaKEdQdibozC0LOibu9Eshs0R1yfgFNOYvI0icb7ntDKXC2k06tVw/0?wx_fmt=jpeg",
                        Url = ConfigurationManager.AppSettings["WeiXinDomain"] + "component/register"
                    };
                    ArticelModel articel2 = new ArticelModel()
                    {
                        Title = "会员扫码须知",
                        Description = "会员扫码须知",
                        PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqtaGUZZoklAKp1sE9eqTK2Mia7RSpH0AyT3lP5BaxNJkiagobOIz3Gpe2ZQs3HYz8icFKw3wxic35KApQ/0?wx_fmt=jpeg",
                        Url = "http://jifenweixin.shinho.net.cn/#/component/activityrule"
                    };
                    ArticelModel articel3 = new ArticelModel()
                    {
                        Title = "即日起至2月10日，凡任务达标者即可瓜分20万积分红包！",
                        Description = "即日起至2月10日，凡任务达标者即可瓜分20万积分红包！",
                        PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqtIndjMF7wbHqDy3CVVvrmTHXAdA9QtewoUvqgazfLlCmfhjExN3HuHk7sbtgp6trxNqt64Z01uqA/0?wx_fmt=jpeg",
                        Url = "http://mp.weixin.qq.com/s/5H6_rnk86ai2fOs-ChI2Gg"
                    };
                    msg.Articles = new List<ArticelModel>();
                    msg.Articles.Add(articel);
                    msg.Articles.Add(articel2);
                    msg.Articles.Add(articel3);
                    remsg = XmlHelpler.GetArticlesXml(msg);
                }
                else
                {
                    msg.MsgType = "news";
                    ArticelModel articel = new ArticelModel()
                    {
                        Title = "味达美厨师会员招募啦！点击图片即可注册！",
                        Description = "积分好礼、国内外学习考察、新品试用等你来！",
                        PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqsSZZuaafJUZfNibZ109B0CXbWpLSaKEdQdibozC0LOibu9Eshs0R1yfgFNOYvI0icb7ntDKXC2k06tVw/0?wx_fmt=jpeg",
                        Url = ConfigurationManager.AppSettings["WeiXinDomain"] + "component/register"
                    };
                    ArticelModel articel2 = new ArticelModel()
                    {
                        Title = "会员扫码须知",
                        Description = "会员扫码须知",
                        PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqtaGUZZoklAKp1sE9eqTK2Mia7RSpH0AyT3lP5BaxNJkiagobOIz3Gpe2ZQs3HYz8icFKw3wxic35KApQ/0?wx_fmt=jpeg",
                        Url = "http://jifenweixin.shinho.net.cn/#/component/activityrule"
                    };
                    ArticelModel articel3 = new ArticelModel()
                    {
                        Title = "即日起至2月10日，凡任务达标者即可瓜分20万积分红包！",
                        Description = "即日起至2月10日，凡任务达标者即可瓜分20万积分红包！",
                        PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqtIndjMF7wbHqDy3CVVvrmTHXAdA9QtewoUvqgazfLlCmfhjExN3HuHk7sbtgp6trxNqt64Z01uqA/0?wx_fmt=jpeg",
                        Url = "http://mp.weixin.qq.com/s/5H6_rnk86ai2fOs-ChI2Gg"
                    };
                    msg.Articles = new List<ArticelModel>();
                    msg.Articles.Add(articel);
                    msg.Articles.Add(articel2);
                    msg.Articles.Add(articel3);
                    remsg = XmlHelpler.GetArticlesXml(msg);
                }
            }
            else
            {
                msg.MsgType = "news";
                ArticelModel articel = new ArticelModel()
                {
                    Title = "味达美厨师会员招募啦！点击图片即可注册！",
                    Description = "积分好礼、国内外学习考察、新品试用等你来！",
                    PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqsSZZuaafJUZfNibZ109B0CXbWpLSaKEdQdibozC0LOibu9Eshs0R1yfgFNOYvI0icb7ntDKXC2k06tVw/0?wx_fmt=jpeg",
                    Url = ConfigurationManager.AppSettings["WeiXinDomain"] + "component/register"
                };
                ArticelModel articel2 = new ArticelModel()
                {
                    Title = "会员扫码须知",
                    Description = "会员扫码须知",
                    PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqtaGUZZoklAKp1sE9eqTK2Mia7RSpH0AyT3lP5BaxNJkiagobOIz3Gpe2ZQs3HYz8icFKw3wxic35KApQ/0?wx_fmt=jpeg",
                    Url = "http://jifenweixin.shinho.net.cn/#/component/activityrule"
                };
                ArticelModel articel3 = new ArticelModel()
                {
                    Title = "即日起至2月10日，凡任务达标者即可瓜分20万积分红包！",
                    Description = "即日起至2月10日，凡任务达标者即可瓜分20万积分红包！",
                    PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqtIndjMF7wbHqDy3CVVvrmTHXAdA9QtewoUvqgazfLlCmfhjExN3HuHk7sbtgp6trxNqt64Z01uqA/0?wx_fmt=jpeg",
                    Url = "http://mp.weixin.qq.com/s/5H6_rnk86ai2fOs-ChI2Gg"
                };
                msg.Articles = new List<ArticelModel>();
                msg.Articles.Add(articel);
                msg.Articles.Add(articel2);
                msg.Articles.Add(articel3);
                remsg = XmlHelpler.GetArticlesXml(msg);
            }
            Response.Write(GetMsg(remsg, ccg));
        }

        /// <summary>
        /// 上报地理位置
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="msg"></param>
        public static void ResponseLocaltonEvent(HttpResponseBase Response, MsgModel msg, ResultCryptography ccg)
        {
            msg.TextModel = new TextModel() { Content = "上传地理位置成功！" };
            var remsg = XmlHelpler.GetTextXml(msg);
            //Response.Write(GetMsg(remsg, ccg));
        }

        /// <summary>
        /// 自定义菜单
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="msg"></param>
        public  static void ResponseClickEvent(HttpResponseBase Response, MsgModel msg, ResultCryptography ccg)
        {
            msg.TextModel = new TextModel() { Content = "推荐注册啦！复制此链接发送给好友，点击链接填写内容即可完成注册！：http://jifenweixin.shinho.net.cn/RegistMember/NewCreate?TuiJianId=" + msg.FromUserName };
            var remsg = XmlHelpler.GetTextXml(msg);
            Response.Write(GetMsg(remsg, ccg));
        }

        /// <summary>
        /// 扫一扫
        /// </summary>
        public static void ResponseAdvClickEvent(HttpResponseBase Response, MsgModel msg, ResultCryptography ccg)
        {
            string remsg = string.Empty;
            string openid = msg.FromUserName;
            //LogHelper.WriteLog("lalal" + ConfigurationManager.AppSettings["ScanCodeInfoUrl"] + "?url=" + System.Web.HttpUtility.UrlEncode(msg.EventModel.ClickEventModel.ScanResult) + "&openid=" + openid);
            //调用积分url
            string ret = BasicApi.RequestCreate(ConfigurationManager.AppSettings["ScanCodeInfoUrl"] + "?url=" + System.Web.HttpUtility.UrlEncode(msg.EventModel.ClickEventModel.ScanResult) + "&openid=" + openid);

            //LogHelper.WriteLog(ConfigurationManager.AppSettings["ScanCodeInfoUrl"]);

            msg.TextModel = new TextModel() { Content = ret };
            remsg = XmlHelpler.GetTextXml(msg);
            Response.Write(GetMsg(remsg, ccg));
        }


        public static void ResponseRecomendClickEvent(HttpResponseBase Response, MsgModel msg, ResultCryptography ccg)
        {
            string remsg = string.Empty;
            string openid = msg.FromUserName;

            //调用积分url
            //string ret = ConfigurationManager.AppSettings["TuiJianUrl"] + memberId.ToString();
            string ret = "";
            msg.TextModel = new TextModel() { Content = ret };
            remsg = XmlHelpler.GetTextXml(msg);
            Response.Write(GetMsg(remsg, ccg));
        }

    }
}

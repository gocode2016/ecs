using CreditSysWeChat.Common;
using CreditSysWeChat.WeiXin.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;





namespace CreditSysWeChat.WeiXin
{
    public class ReceiveMsg
    {
        //public static SqlHelper dataContext = new SqlHelper();
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
                                    ResponseLocaltonEvent(Response, msgModel, ccg);
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
                if (msg.TextModel.Content == "菜谱")
                {
                    textResponse.Content = "您的专属蚝油电子菜谱，请查收！http://jifenweixin.shinho.net.cn/?action=specialoyster";
                }
                else if (msg.TextModel.Content == "我是欣和人") 
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

                LogHelper.WriteLog("EventKey:" + str);
                if (!string.IsNullOrEmpty(str) || str.Contains("A"))
                {
                    if (str == "qrscene_subscribelog")
                    {
                        string sql = string.Format(@"INSERT INTO dbo.SubscribeLog
                                                            ( OpenId, SceneStr,Remark)
                                                    VALUES  ( '{0}','{1}','活动关注' ) ", msg.FromUserName, str);
                        SqlHelper2.ExecuteNonQuery(CommandType.Text, sql);
                        msg.MsgType = "news";
                        ArticelModel articel = new ArticelModel()
                        {
                            Title = "活动关注",
                            Description = "活动关注",
                            PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqumKFRaSlWT7zeDcnytdTxy4RiaKfPB8Dk2WKVaa8QgNlc658IlIMzmwibSRxufc7ekPiczic7Co8z2YQ/0?wx_fmt=jpeg",
                            Url = "https://vip.jfyf.com/xrf_game/2018October/Oct_CookMini/weixin.php"
                        };
                        ArticelModel artice2 = new ArticelModel()
                        {
                            Title = "我的订单",
                            Description = "我的订单",
                            PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqumKFRaSlWT7zeDcnytdTxyaXLibicZMGxDnB5PhxtZicajN1B2RMzzsGtCoz2B3D3RCP9ia1smYHNxqA/0?wx_fmt=jpeg",
                            Url = "http://jifenweixin.shinho.net.cn/?action=orderlist"
                        };
                        ArticelModel artice3 = new ArticelModel()
                        {
                            Title = "我的积分",
                            Description = "我的积分",
                            PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqumKFRaSlWT7zeDcnytdTxyeGAKcJRnkL9fMynvrnXibl7s18p2h2aicriatIXkdEkU68fjuXwy5Qenw/0?wx_fmt=jpeg",
                            Url = "http://jifenweixin.shinho.net.cn/?action=integraldetail"
                        };
                        msg.Articles = new List<ArticelModel>();
                        msg.Articles.Add(articel);
                        msg.Articles.Add(artice2);
                        msg.Articles.Add(artice3);
                        remsg = XmlHelpler.GetArticlesXml(msg);
                    }
                    else
                    {
                        int Id = Convert.ToInt32(System.Text.RegularExpressions.Regex.Replace(str, @"[^0-9]+", ""));

                        if (Id == 90001) //id=90001 20181020厨师节活动关注推送
                        {
                            string sql = string.Format(@"INSERT INTO dbo.SubscribeLog
                                                            ( OpenId, SceneId,Remark)
                                                    VALUES  ( '{0}',{1},'20181020厨师节活动关注' ) ", msg.FromUserName, Id);
                            SqlHelper2.ExecuteNonQuery(CommandType.Text, sql);
                            LogHelper.WriteLog(msg.FromUserName + ":20181020厨师节活动关注:" + Id.ToString());
                            msg.MsgType = "news";
                            ArticelModel articel = new ArticelModel()
                            {
                                Title = "您的专属电子菜谱，请查收！",
                                Description = "您的专属电子菜谱，请查收！",
                                PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqvRn28muC7icFe2qOnAalU1G5Be7eDt37iaPw0h8ibsFOYpzjreyTLnSmSAFq5aMUO0EUhwU2BIeJRPQ/0?wx_fmt=jpeg",
                                Url = "http://jifenweixin.shinho.net.cn/?action=dishstore"
                            };
                            ArticelModel artice2 = new ArticelModel()
                            {
                                Title = "点我注册",
                                Description = "点我注册",
                                PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqvRn28muC7icFe2qOnAalU1GINIWIUHogp03tKyhceKo0KETMAiaWnFeS5ic1M2BG8nzuHn4ltVnTheQ/0?wx_fmt=jpeg",
                                Url = ConfigurationManager.AppSettings["WeiXinDomain"] + "component/register"
                            };
                            ArticelModel artice3 = new ArticelModel()
                            {
                                Title = "马上抢红包活动入口",
                                Description = "马上抢红包活动入口",
                                PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBquqY17U580YTjjAFyNBBFUjGkJVkDLmTRZicHkyF6mOCL4nhCltMtODia7IJicuGgBZXWTaJfdKTVDtQ/0?wx_fmt=jpeg",
                                Url = "http://jifenweixin.shinho.net.cn/?action=scanhome"
                            };
                            msg.Articles = new List<ArticelModel>();
                            msg.Articles.Add(articel);
                            msg.Articles.Add(artice2);
                            msg.Articles.Add(artice3);
                            remsg = XmlHelpler.GetArticlesXml(msg);
                        }
                        else if (Id == 90002) //id=90002 20181020厨师节活动抽奖电子菜谱关注 推送
                        {
                            //                        string sql = string.Format(@"INSERT INTO dbo.SubscribeLog
                            //                                    ( OpenId, SceneId,Remark)
                            //                            VALUES  ( '{0}',{1},'20181020厨师节活动抽奖电子菜谱的关注' ) ", msg.FromUserName, Id);
                            //                        dataContext.ExecuteNonQuery(CommandType.Text, sql); 
                            LogHelper.WriteLog(msg.FromUserName + ":20181020厨师节活动关注:" + Id.ToString());
                            msg.MsgType = "news";
                            ArticelModel articel = new ArticelModel()
                            {
                                Title = "您的专属酸汤酱电子菜谱",
                                Description = "欣和餐饮服务欢迎您的到来",
                                PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqumKFRaSlWT7zeDcnytdTxymIDyVeM3oGVu3f5T7uPCG0fKhicodBkQAJlWSucuaV584BeWicOuZdMg/0?wx_fmt=jpeg",
                                Url = "http://jifenweixin.shinho.net.cn/?action=specialsoup"
                            };
                            msg.Articles = new List<ArticelModel>();
                            msg.Articles.Add(articel);
                            remsg = XmlHelpler.GetArticlesXml(msg);
                        }
                        else if (Id > 1 && Id < 3000)
                        {
                            remsg = ActivityExc(msg, Id);
                        }
                        else if (Id == 6666)
                        {
                            remsg = CookActivityExc(msg, Id);
                        }
                        else if (Id < 50000 && Id > 3000 && Id != 6666)
                        {
                            msg.MsgType = "news";
                            ArticelModel articel = new ArticelModel()
                            {
                                Title = "点击图片完成注册",
                                Description = "欣和餐饮服务欢迎您的到来",
                                PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqsSZZuaafJUZfNibZ109B0CXbWpLSaKEdQdibozC0LOibu9Eshs0R1yfgFNOYvI0icb7ntDKXC2k06tVw/0?wx_fmt=jpeg",
                                Url = ConfigurationManager.AppSettings["WeiXinDomain"] + "component/register"
                            };
                            ArticelModel artice2 = new ArticelModel()
                            {
                                Title = "码上抢红包活动入口",
                                Description = "码上抢红包活动入口",
                                PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBquqY17U580YTjjAFyNBBFUjGkJVkDLmTRZicHkyF6mOCL4nhCltMtODia7IJicuGgBZXWTaJfdKTVDtQ/0?wx_fmt=jpeg",
                                Url = "http://jifenweixin.shinho.net.cn/#/component/scanhome"
                            };
                            msg.Articles = new List<ArticelModel>();
                            msg.Articles.Add(articel);
                            msg.Articles.Add(artice2);
                            remsg = XmlHelpler.GetArticlesXml(msg);
                        }
                        else
                        {
                            msg.MsgType = "news";
                            ArticelModel articel = new ArticelModel()
                            {
                                //Title = "味达美厨师会员招募啦！点击图片即可注册！",
                                //Description = "积分好礼、国内外学习考察、新品试用等你来！",
                                //PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqsSZZuaafJUZfNibZ109B0CXbWpLSaKEdQdibozC0LOibu9Eshs0R1yfgFNOYvI0icb7ntDKXC2k06tVw/0?wx_fmt=jpeg",
                                //Url = ConfigurationManager.AppSettings["WeiXinDomain"] + "component/register"
                                //Title = "味达美厨师会员招募啦！点击图片即可注册！",
                                //Description = "积分好礼、国内外学习考察、新品试用等你来！",
                                //PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqsSZZuaafJUZfNibZ109B0CXbWpLSaKEdQdibozC0LOibu9Eshs0R1yfgFNOYvI0icb7ntDKXC2k06tVw/0?wx_fmt=jpeg",
                                //Url = ConfigurationManager.AppSettings["WeiXinDomain"] + "component/register"
                                Title = "您的专属蚝油电子菜谱，请查收！",
                                Description = "您的专属蚝油电子菜谱，请查收！",
                                PicUrl = "https://mmbiz.qpic.cn/mmbiz_png/uuwJXDpEBquGiaHze26HlEC1cGKfICxxCqvL27sgfuuN5Z5rVSt1OJIMdBf6nE8IKle0GneulRQzujoADgvOEUw/0?wx_fmt=png",
                                Url = "http://jifenweixin.shinho.net.cn/?action=specialoyster"
                            };
                            ArticelModel artice2 = new ArticelModel()
                            {
                                Title = "码上抢红包活动入口",
                                Description = "码上抢红包活动入口",
                                PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBquqY17U580YTjjAFyNBBFUjGkJVkDLmTRZicHkyF6mOCL4nhCltMtODia7IJicuGgBZXWTaJfdKTVDtQ/0?wx_fmt=jpeg",
                                Url = "http://jifenweixin.shinho.net.cn/#/component/scanhome"
                            };
                            msg.Articles = new List<ArticelModel>();
                            msg.Articles.Add(articel);
                            msg.Articles.Add(artice2);
                            remsg = XmlHelpler.GetArticlesXml(msg);
                        }
                    }
                }
                else  //没有EventKey
                {
                    msg.MsgType = "news";
                    ArticelModel articel = new ArticelModel()
                    {
                        //Title = "味达美厨师会员招募啦！点击图片即可注册！",
                        //Description = "积分好礼、国内外学习考察、新品试用等你来！",
                        //PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqsSZZuaafJUZfNibZ109B0CXbWpLSaKEdQdibozC0LOibu9Eshs0R1yfgFNOYvI0icb7ntDKXC2k06tVw/0?wx_fmt=jpeg",
                        //Url = ConfigurationManager.AppSettings["WeiXinDomain"] + "component/register"
                        Title = "您的专属蚝油电子菜谱，请查收！",
                        Description = "您的专属蚝油电子菜谱，请查收！",
                        PicUrl = "https://mmbiz.qpic.cn/mmbiz_png/uuwJXDpEBquGiaHze26HlEC1cGKfICxxCqvL27sgfuuN5Z5rVSt1OJIMdBf6nE8IKle0GneulRQzujoADgvOEUw/0?wx_fmt=png",
                        Url = "http://jifenweixin.shinho.net.cn/?action=specialoyster"
                    };
                    ArticelModel articel2 = new ArticelModel()
                    {
                        Title = "会员扫码须知",
                        Description = "会员扫码须知",
                        PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqtaGUZZoklAKp1sE9eqTK2Mia7RSpH0AyT3lP5BaxNJkiagobOIz3Gpe2ZQs3HYz8icFKw3wxic35KApQ/0?wx_fmt=jpeg",
                        Url = "http://jifenweixin.shinho.net.cn/?action=membernotice"
                    };
                    ArticelModel articel3 = new ArticelModel()
                    {
                        Title = "码上抢红包活动入口",
                        Description = "码上抢红包活动入口",
                        PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBquqY17U580YTjjAFyNBBFUjGkJVkDLmTRZicHkyF6mOCL4nhCltMtODia7IJicuGgBZXWTaJfdKTVDtQ/0?wx_fmt=jpeg",
                        Url = "http://jifenweixin.shinho.net.cn/#/component/scanhome"
                    };
                    msg.Articles = new List<ArticelModel>();
                    msg.Articles.Add(articel);
                    msg.Articles.Add(articel2);
                    msg.Articles.Add(articel3);
                    remsg = XmlHelpler.GetArticlesXml(msg);
                }
            }
            catch(Exception ex)
            {
                LogHelper.WriteLog("error" + ex.ToString());
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
                    Url = "http://jifenweixin.shinho.net.cn/?action=membernotice"
                };
                ArticelModel articel3 = new ArticelModel()
                {
                    Title = "码上抢红包活动入口",
                    Description = "码上抢红包活动入口",
                    PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBquqY17U580YTjjAFyNBBFUjGkJVkDLmTRZicHkyF6mOCL4nhCltMtODia7IJicuGgBZXWTaJfdKTVDtQ/0?wx_fmt=jpeg",
                    Url = "http://jifenweixin.shinho.net.cn/#/component/scanhome"
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
                msg.MsgType = "news";
                ArticelModel articel = new ArticelModel()
                {
                    //Title = "味达美厨师会员招募啦！点击图片即可注册！",
                    //Description = "积分好礼、国内外学习考察、新品试用等你来！",
                    //PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqsSZZuaafJUZfNibZ109B0CXbWpLSaKEdQdibozC0LOibu9Eshs0R1yfgFNOYvI0icb7ntDKXC2k06tVw/0?wx_fmt=jpeg",
                    //Url = ConfigurationManager.AppSettings["WeiXinDomain"] + "component/register"
                    Title = "您的专属蚝油电子菜谱，请查收！",
                    Description = "您的专属蚝油电子菜谱，请查收！",
                    PicUrl = "https://mmbiz.qpic.cn/mmbiz_png/uuwJXDpEBquGiaHze26HlEC1cGKfICxxCqvL27sgfuuN5Z5rVSt1OJIMdBf6nE8IKle0GneulRQzujoADgvOEUw/0?wx_fmt=png",
                    Url = "http://jifenweixin.shinho.net.cn/?action=specialoyster"
                };
                ArticelModel articel2 = new ArticelModel()
                {
                    Title = "会员扫码须知",
                    Description = "会员扫码须知",
                    PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqtaGUZZoklAKp1sE9eqTK2Mia7RSpH0AyT3lP5BaxNJkiagobOIz3Gpe2ZQs3HYz8icFKw3wxic35KApQ/0?wx_fmt=jpeg",
                    Url = "http://jifenweixin.shinho.net.cn/?action=membernotice"
                };
                ArticelModel articel3 = new ArticelModel()
                {
                    Title = "码上抢红包活动入口",
                    Description = "码上抢红包活动入口",
                    PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBquqY17U580YTjjAFyNBBFUjGkJVkDLmTRZicHkyF6mOCL4nhCltMtODia7IJicuGgBZXWTaJfdKTVDtQ/0?wx_fmt=jpeg",
                    Url = "http://jifenweixin.shinho.net.cn/#/component/scanhome"
                };
                msg.Articles = new List<ArticelModel>();
                msg.Articles.Add(articel);
                msg.Articles.Add(articel2);
                msg.Articles.Add(articel3);
                remsg = XmlHelpler.GetArticlesXml(msg);
                
            }
            catch(Exception ex)
            {
                LogHelper.WriteLog("error" + ex.ToString());
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
                    Url = "http://jifenweixin.shinho.net.cn/?action=membernotice"
                };
                ArticelModel articel3 = new ArticelModel()
                {
                    Title = "码上抢红包活动入口",
                    Description = "码上抢红包活动入口",
                    PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBquqY17U580YTjjAFyNBBFUjGkJVkDLmTRZicHkyF6mOCL4nhCltMtODia7IJicuGgBZXWTaJfdKTVDtQ/0?wx_fmt=jpeg",
                    Url = "http://jifenweixin.shinho.net.cn/#/component/scanhome"
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
        /// 扫描二维码，用户已关注时，进行关注后的事件推送
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="msg"></param>
        public static void ResponseScanSubscribeEvent(HttpResponseBase Response, MsgModel msg, ResultCryptography ccg)
        {
            var remsg = "";
            try
            { 
                string str = msg.EventModel.EventKeyModel.EventKey;
                LogHelper.WriteLog("EventKey:" + str);
                if (!string.IsNullOrEmpty(str) || str.Contains("A"))
                {
                    if (str == "qrscene_subscribelog")
                    {
                        string sql = string.Format(@"INSERT INTO dbo.SubscribeLog
                                                            ( OpenId, SceneStr,Remark)
                                                    VALUES  ( '{0}','{1}','活动关注' ) ", msg.FromUserName, str);
                        SqlHelper2.ExecuteNonQuery(CommandType.Text, sql);
                        msg.MsgType = "news";
                        ArticelModel articel = new ArticelModel()
                        {
                            Title = "活动关注",
                            Description = "活动关注",
                            PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqumKFRaSlWT7zeDcnytdTxy4RiaKfPB8Dk2WKVaa8QgNlc658IlIMzmwibSRxufc7ekPiczic7Co8z2YQ/0?wx_fmt=jpeg",
                            Url = "https://vip.jfyf.com/xrf_game/2018October/Oct_CookMini/weixin.php"
                        };
                        ArticelModel artice2 = new ArticelModel()
                        {
                            Title = "我的订单",
                            Description = "我的订单",
                            PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqumKFRaSlWT7zeDcnytdTxyaXLibicZMGxDnB5PhxtZicajN1B2RMzzsGtCoz2B3D3RCP9ia1smYHNxqA/0?wx_fmt=jpeg",
                            Url = "http://jifenweixin.shinho.net.cn/?action=orderlist"
                        };
                        ArticelModel artice3 = new ArticelModel()
                        {
                            Title = "我的积分",
                            Description = "我的积分",
                            PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqumKFRaSlWT7zeDcnytdTxyeGAKcJRnkL9fMynvrnXibl7s18p2h2aicriatIXkdEkU68fjuXwy5Qenw/0?wx_fmt=jpeg",
                            Url = "http://jifenweixin.shinho.net.cn/?action=integraldetail"
                        };
                        msg.Articles = new List<ArticelModel>();
                        msg.Articles.Add(articel);
                        msg.Articles.Add(artice2);
                        msg.Articles.Add(artice3);
                        remsg = XmlHelpler.GetArticlesXml(msg);
                    }
                    else
                    {
                        int Id = Convert.ToInt32(System.Text.RegularExpressions.Regex.Replace(str, @"[^0-9]+", ""));
                        if (Id == 90001) //id=90001 20181020厨师节活动关注推送
                        {
                            string sql = string.Format(@"INSERT INTO dbo.SubscribeLog
                                                            ( OpenId, SceneId,Remark)
                                                    VALUES  ( '{0}',{1},'20181020厨师节活动之前已关注的推送' ) ", msg.FromUserName, Id);
                            SqlHelper2.ExecuteNonQuery(CommandType.Text, sql);
                            LogHelper.WriteLog(msg.FromUserName + ":之前已关注的20181020厨师节活动关注:" + Id.ToString());
                            msg.MsgType = "news";
                            ArticelModel articel = new ArticelModel()
                            {
                                Title = "您的专属电子菜谱，请查收！",
                                Description = "您的专属电子菜谱，请查收！",
                                PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqvRn28muC7icFe2qOnAalU1G5Be7eDt37iaPw0h8ibsFOYpzjreyTLnSmSAFq5aMUO0EUhwU2BIeJRPQ/0?wx_fmt=jpeg",
                                Url = "http://jifenweixin.shinho.net.cn/?action=dishstore"  

                            };
                            ArticelModel artice2 = new ArticelModel()
                            {
                                Title = "点我注册",
                                Description = "点我注册",
                                PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqvRn28muC7icFe2qOnAalU1GINIWIUHogp03tKyhceKo0KETMAiaWnFeS5ic1M2BG8nzuHn4ltVnTheQ/0?wx_fmt=jpeg",
                                Url = ConfigurationManager.AppSettings["WeiXinDomain"] + "component/register"
                            };
                            ArticelModel artice3 = new ArticelModel()
                            {
                                Title = "马上抢红包活动入口",
                                Description = "马上抢红包活动入口",
                                PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBquqY17U580YTjjAFyNBBFUjGkJVkDLmTRZicHkyF6mOCL4nhCltMtODia7IJicuGgBZXWTaJfdKTVDtQ/0?wx_fmt=jpeg",
                                Url = "http://jifenweixin.shinho.net.cn/#/component/scanhome"
                            };
                            msg.Articles = new List<ArticelModel>();
                            msg.Articles.Add(articel);
                            msg.Articles.Add(artice2);
                            msg.Articles.Add(artice3);
                            remsg = XmlHelpler.GetArticlesXml(msg);
                        }
                        else if (Id == 90002) //id=90002 20181020厨师节活动抽奖电子菜谱关注 推送
                        {
                            //                        string sql = string.Format(@"INSERT INTO dbo.SubscribeLog
                            //                                    ( OpenId, SceneId,Remark)
                            //                            VALUES  ( '{0}',{1},'20181020厨师节活动抽奖电子菜谱的之前已关注的' ) ", msg.FromUserName, Id);
                            //                        dataContext.ExecuteNonQuery(CommandType.Text, sql);
                            LogHelper.WriteLog(msg.FromUserName + ":之前已关注的20181020厨师节活动关注:" + Id.ToString());
                            msg.MsgType = "news";
                            ArticelModel articel = new ArticelModel()
                            {
                                Title = "您的专属酸汤酱电子菜谱",
                                Description = "欣和餐饮服务欢迎您的到来",
                                PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqumKFRaSlWT7zeDcnytdTxymIDyVeM3oGVu3f5T7uPCG0fKhicodBkQAJlWSucuaV584BeWicOuZdMg/0?wx_fmt=jpeg",
                                Url = "http://jifenweixin.shinho.net.cn/?action=specialsoup"
                            };
                            msg.Articles = new List<ArticelModel>();
                            msg.Articles.Add(articel);
                            remsg = XmlHelpler.GetArticlesXml(msg);
                        }
                        else if (Id > 0 && Id < 3000)
                        {
                            remsg = ActivityExc(msg, Id);
                        }
                        else if (Id == 6666)
                        {
                            remsg = CookActivityExc(msg, Id);
                        }
                        if (Id < 50000 && Id > 3000 && Id != 6666)
                        {
                            //msg.TextModel = new TextModel() { Content = "厨师大大，点击 http://jifenweixin.shinho.net.cn/RegistMember/Create?activityId=" + Id + "链接完成签到，新用户需先完成注册" }; 
                            msg.MsgType = "news";
                            ArticelModel articel = new ArticelModel()
                            {
                                Title = "点击图片完成注册",
                                Description = "欣和餐饮服务欢迎您的到来",
                                PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqsSZZuaafJUZfNibZ109B0CXbWpLSaKEdQdibozC0LOibu9Eshs0R1yfgFNOYvI0icb7ntDKXC2k06tVw/0?wx_fmt=jpeg",
                                Url = ConfigurationManager.AppSettings["WeChat"] + "?action=registeredrecommend" + Id
                            };
                            ArticelModel articel2 = new ArticelModel()
                            {
                                Title = "会员扫码须知",
                                Description = "会员扫码须知",
                                PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqtaGUZZoklAKp1sE9eqTK2Mia7RSpH0AyT3lP5BaxNJkiagobOIz3Gpe2ZQs3HYz8icFKw3wxic35KApQ/0?wx_fmt=jpeg",
                                Url = "http://jifenweixin.shinho.net.cn/?action=membernotice"
                            };
                            ArticelModel articel3 = new ArticelModel()
                            {
                                Title = "码上抢红包活动入口",
                                Description = "码上抢红包活动入口",
                                PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBquqY17U580YTjjAFyNBBFUjGkJVkDLmTRZicHkyF6mOCL4nhCltMtODia7IJicuGgBZXWTaJfdKTVDtQ/0?wx_fmt=jpeg",
                                Url = "http://jifenweixin.shinho.net.cn/#/component/scanhome"
                            };
                            msg.Articles = new List<ArticelModel>();
                            msg.Articles.Add(articel);
                            msg.Articles.Add(articel2);
                            msg.Articles.Add(articel3);
                            remsg = XmlHelpler.GetArticlesXml(msg);
                        }

                    }
                }
                else  //没有EventKey
                {
                    msg.MsgType = "news";
                    ArticelModel articel = new ArticelModel()
                    {
                        //Title = "味达美厨师会员招募啦！点击图片即可注册！",
                        //Description = "积分好礼、国内外学习考察、新品试用等你来！",
                        //PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqsSZZuaafJUZfNibZ109B0CXbWpLSaKEdQdibozC0LOibu9Eshs0R1yfgFNOYvI0icb7ntDKXC2k06tVw/0?wx_fmt=jpeg",
                        //Url = ConfigurationManager.AppSettings["WeiXinDomain"] + "component/register"
                        Title = "您的专属蚝油电子菜谱，请查收！",
                        Description = "您的专属蚝油电子菜谱，请查收！",
                        PicUrl = "https://mmbiz.qpic.cn/mmbiz_png/uuwJXDpEBquGiaHze26HlEC1cGKfICxxCqvL27sgfuuN5Z5rVSt1OJIMdBf6nE8IKle0GneulRQzujoADgvOEUw/0?wx_fmt=png",
                        Url = "http://jifenweixin.shinho.net.cn/?action=specialoyster"
                    };
                    ArticelModel articel2 = new ArticelModel()
                    {
                        Title = "会员扫码须知",
                        Description = "会员扫码须知",
                        PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqtaGUZZoklAKp1sE9eqTK2Mia7RSpH0AyT3lP5BaxNJkiagobOIz3Gpe2ZQs3HYz8icFKw3wxic35KApQ/0?wx_fmt=jpeg",
                        Url = "http://jifenweixin.shinho.net.cn/?action=membernotice"
                    };
                    ArticelModel articel3 = new ArticelModel()
                    {
                        Title = "码上抢红包活动入口",
                        Description = "码上抢红包活动入口",
                        PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBquqY17U580YTjjAFyNBBFUjGkJVkDLmTRZicHkyF6mOCL4nhCltMtODia7IJicuGgBZXWTaJfdKTVDtQ/0?wx_fmt=jpeg",
                        Url = "http://jifenweixin.shinho.net.cn/#/component/scanhome"
                    };
                    msg.Articles = new List<ArticelModel>();
                    msg.Articles.Add(articel);
                    msg.Articles.Add(articel2);
                    msg.Articles.Add(articel3);
                    remsg = XmlHelpler.GetArticlesXml(msg);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("error"+ex.ToString());
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
                    Url = "http://jifenweixin.shinho.net.cn/?action=membernotice"
                };
                ArticelModel articel3 = new ArticelModel()
                {
                    Title = "码上抢红包活动入口",
                    Description = "码上抢红包活动入口",
                    PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBquqY17U580YTjjAFyNBBFUjGkJVkDLmTRZicHkyF6mOCL4nhCltMtODia7IJicuGgBZXWTaJfdKTVDtQ/0?wx_fmt=jpeg",
                    Url = "http://jifenweixin.shinho.net.cn/#/component/scanhome"
                };

                msg.Articles = new List<ArticelModel>();
                msg.Articles.Add(articel);
                msg.Articles.Add(articel2);
                msg.Articles.Add(articel3);
                remsg = XmlHelpler.GetArticlesXml(msg);
            }
           
            //else
            //{
            //    ResponseNoEvent(Response, msg, ccg);
            //}
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
        /// 上报地理位置
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="msg"></param>
        public static void ResponseLocaltonEvent(HttpResponseBase Response, MsgModel msg, ResultCryptography ccg)
        {
            msg.TextModel = new TextModel() { Content = "上传地理位置成功！" };
            var remsg = XmlHelpler.GetTextXml(msg);
            LogHelper.WriteLog(JsonHelper.ObjectToJson(msg));
            //Response.Write(GetMsg(remsg, ccg));
        }

        /// <summary>
        /// 自定义菜单
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="msg"></param>
        public  static void ResponseClickEvent(HttpResponseBase Response, MsgModel msg, ResultCryptography ccg)
        {
            msg.TextModel = new TextModel() { Content = "10月20日世界厨师日，好礼活动即将开启"};
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
            if (ret.Contains("x"))
            {
                //BasicApi.RequestCreate("http://jifenweixin.shinho.net.cn/#/component/scansuccess?integral=" + ret);
                ret = ret.Replace("x", "");
                msg.MsgType = "news";
                ArticelModel articel = new ArticelModel()
                {
                    Title = "好手气，拿大奖",
                    Description = "点击上文查看中奖情况",
                    PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqudkJX4sk2WDIYvqmibxToUKFtPcmLS2c2tnoXvoWoVgANLiaZPvEEAohSxdnTxPdIRSBDy10SPE1TA/0?wx_fmt=jpeg",
                    Url = ConfigurationManager.AppSettings["WeiXinDomain"] + "component/scansuccess?integral=" + ret
                };
                msg.Articles = new List<ArticelModel>();
                msg.Articles.Add(articel);
                remsg = XmlHelpler.GetArticlesXml(msg);
                Response.Write(GetMsg(remsg, ccg));

            }
            else
            {
                msg.TextModel = new TextModel() { Content = ret };
                remsg = XmlHelpler.GetTextXml(msg);
                Response.Write(GetMsg(remsg, ccg));
            }
            
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

        public static string ActivityExc(MsgModel msg, int id)
        {
            msg.MsgType = "news";
            //msg.TextModel = new TextModel() { Content = "厨大大们，点击 http://jifenweixin.shinho.net.cn/RegistMember/Create?activityId=" + Id + "链接完成签到，新用户需先完成注册" };
            ArticelModel articel = new ArticelModel()
            {
                Title = "点击上文完成签到",
                Description = "欣和餐饮服务欢迎您的到来！",
                PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqsynpxhFaMHaRGSNcnicINW3EVhsLcwqYFK4AUKyw5kyickELM1cmjxdT2RCWic6icrk3d9Xr4aAHcibUw/0?wx_fmt=jpeg",

                Url = ConfigurationManager.AppSettings["WeChat"] + "?action=signsuccess" + id
            };

            msg.Articles = new List<ArticelModel>();
            msg.Articles.Add(articel);
            string remsg = XmlHelpler.GetArticlesXml(msg);

            return remsg;
        }


        public static string CookActivityExc(MsgModel msg, int id)
        {
            msg.MsgType = "news";

            ArticelModel articel = new ArticelModel()
            {
                Title = "点击上文去领奖",
                Description = "欣和餐饮服务欢迎您的到来！",
                PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqvIrd3GiaYVq5lQJaxQC9O57a2S9vDfcjyh7qmOm1Ks1wcsgd25cprhj1AjNS0GQ79ibQOlGRiblCk6Q/0?wx_fmt=jpeg",

                Url = ConfigurationManager.AppSettings["WeChat"] + "?action=cookregister"
            };

            msg.Articles = new List<ArticelModel>();
            msg.Articles.Add(articel);
            string remsg = XmlHelpler.GetArticlesXml(msg);

            return remsg;
        }

    }
}

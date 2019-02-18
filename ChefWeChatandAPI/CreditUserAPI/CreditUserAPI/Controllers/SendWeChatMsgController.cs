using CreditUserAPI.Common;
using CreditUserAPI.Models;
using CreditUserAPI.WeiXin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace CreditUserAPI.Controllers
{
    public class SendWeChatMsgController : ApiController
    {
        private CreditContext db = new CreditContext();
        private SqlHelper dataContext = new SqlHelper();

        [HttpGet]
        public string SendMsg(int flag)
        {

            var sql = string.Format(@" SELECT MemberId,OpenId,SendFlag,SendDate 
                                       FROM SendMubanMsg WHERE MemberId<=38000 AND SendFlag=0  ORDER BY MemberId");

            DataTable dt = dataContext.ExecuteDataTable(CommandType.Text, sql);
            var json = "";
            string result = "";
            foreach (DataRow dr in dt.Rows)
            {
                string openid = dr["OpenId"].ToString();
                int memberid = Convert.ToInt32(dr["MemberId"]);

                json = "{\"touser\":\"" + openid + "\"," +
                        "\"template_id\":\"b0-aVm0y5Urmbup-cnCykHlYWHiuaGYy9DabB-ePetU\"," +
                        "\"url\":\"https://mp.weixin.qq.com/s/G20tKtK4vIvH1nOlwO3Zuw\"," +
                        "\"data\":{" +
                                "\"first\": {" +
                                    "\"value\":\"今天是厨师的节日，快来测测你是哪一个性感面？分享号召更多厨师兄弟来参与，让世界看见我们的精彩！\"," +
                                    "\"color\":\"#173177\"" +
                                "}," +
                                "\"keyword1\":{" +
                                    "\"value\":\"厨师千面 每一面都性感！\"," +
                                    "\"color\":\"#173177\"" +
                                "}," +
                                "\"keyword2\":{" +
                                        "\"value\":\"" + DateTime.Now + "\"," +
                                    "\"color\":\"#173177\"" +
                                "}," +
                                "\"remark\":{" +
                                    "\"value\":\"快点击详情，Apple Watch大奖赢回家！\"," +
                                    "\"color\":\"#173177\"" +
                                "}" +
                        "}" +
                    "}";
                result = SendMuBanMsg(json);
                if (result == "succ")
                {
                    sql = string.Format(@"UPDATE SendMubanMsg SET SendFlag =1 ,SendDate=GETDATE() WHERE MemberId = {0}", memberid);
                    dataContext.ExecuteNonQuery(CommandType.Text, sql);
                }
            }
            return "Excute Success";


////            var sql = string.Format(@"SELECT OpenId FROM dbo.RegistSaleMan a JOIN dbo.OpenIdAssociated b ON a.SalemanId=b.UserId 
////                                    WHERE a.IsEnable =0 AND a.RegistState  <>0  AND UserType =1  ORDER BY RegistDate DESC");
//            //var sql = string.Format("SELECT o.OpenId FROM RegistSaleMan AS rsm JOIN OpenIdAssociated o ON rsm.SalemanId=o.UserId where  RegistState = 2 and o.UserType = 1   ");
//            var sql = string.Format(@"SELECT o.OpenId FROM RegistMember as r JOIN OpenIdAssociated as o ON r.MemberId=o.UserId
//                                    WHERE  o.UserType = 2 and r.MemberId in (55921,54322)");

//            var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
//            string result = JsonConvert.SerializeObject(q);
//            List<SendMember> openIdList = new List<SendMember>();
//            openIdList = JsonConvert.DeserializeObject<List<SendMember>>(result);

//            var json = "";
//            if (openIdList != null)
//            {
//                foreach (var a in openIdList)
//                {
//                    if (!string.IsNullOrEmpty(a.OpenId))
//                    {
//                        json = "{\"touser\":\"" + a.OpenId + "\"," +
//                                "\"template_id\":\"b0-aVm0y5Urmbup-cnCykHlYWHiuaGYy9DabB-ePetU\"," +
//                                "\"url\":\"https://mp.weixin.qq.com/s/G20tKtK4vIvH1nOlwO3Zuw\"," +
//                                "\"data\":{" +
//                                        "\"first\": {" +
//                                            "\"value\":\"今天是厨师的节日，快来测测你是哪一个性感面？分享号召更多厨师兄弟来参与，让世界为我们喝彩！\"," +
//                                            "\"color\":\"#173177\"" +
//                                        "}," +
//                                        "\"keyword1\":{" +
//                                            "\"value\":\"厨师节快乐！\"," +
//                                            "\"color\":\"#173177\"" +
//                                        "}," +
//                                     "\"keyword2\":{" +
//                                             "\"value\":\"" + DateTime.Now + "\"," +
//                                            "\"color\":\"#173177\"" +
//                                        "}," +
//                                        "\"remark\":{" +
//                                            "\"value\":\"快点击详情，Apple Watch大奖赢回家！\"," +
//                                            "\"color\":\"#173177\"" +
//                                        "}" +
//                                "}" +
//                            "}";
//                      //  json = "{\"touser\":\"" + a.OpenId + "\"," +
//                      //    "\"template_id\":\"b0-aVm0y5Urmbup-cnCykHlYWHiuaGYy9DabB-ePetU\"," +
//                      //    "\"url\":\"http://mp.weixin.qq.com/s/5H6_rnk86ai2fOs-ChI2Gg\"," +
//                      //    "\"data\":{" +
//                      //            "\"first\": {" +
//                      //                "\"value\":\"恭喜您，在欣春迎家味摇一摇中中奖啦，现在去【我的账户-我的订单-未提交】中提交吧，填写您的收货地址，大礼包马上到！\"," +
//                      //                "\"color\":\"#173177\"" +
//                      //            "}," +
//                      //            "\"keyword1\":{" +
//                      //                "\"value\":\"欣春迎家味—领奖啦\"," +
//                      //                "\"color\":\"#173177\"" +
//                      //            "}," +
//                      //           "\"keyword2\":{" +
//                      //                "\"value\":\"" + DateTime.Now + "\"," +
//                      //                "\"color\":\"#173177\"" +
//                      //            "}," +
//                      //            "\"remark\":{" +
//                      //                "\"value\":\"务必快快的哦，再晚快递就停运啦！\"," +
//                      //                "\"color\":\"#173177\"" +
//                      //            "}" +
//                      //    "}" +
//                      //"}";
//                      //  json = "{\"touser\":\"" + a.OpenId + "\"," +
//                      //    "\"template_id\":\"b0-aVm0y5Urmbup-cnCykHlYWHiuaGYy9DabB-ePetU\"," +
//                      //    "\"url\":\"http://jifenweixin.shinho.net.cn/#/component/pageshare\"," +
//                      //    "\"data\":{" +
//                      //            "\"first\": {" +
//                      //                "\"value\":\"岁月会变，爱与温情不变，厨师平台给您送福利啦！\"," +
//                      //                "\"color\":\"#173177\"" + 
//                      //            "}," +
//                      //            "\"keyword1\":{" +
//                      //                "\"value\":\"转发厨师家味故事赚积分！\"," +
//                      //                "\"color\":\"#173177\"" +
//                      //            "}," +
//                      //           "\"keyword2\":{" +
//                      //                "\"value\":\"" + DateTime.Now + "\"," +
//                      //                "\"color\":\"#173177\"" +
//                      //            "}," +
//                      //            "\"remark\":{" +
//                      //                "\"value\":\"点击详情，积分赚起来！\"," +
//                      //                "\"color\":\"#173177\"" +
//                      //            "}" +
//                      //    "}" +
//                      //"}";
//                      SendMuBanMsg(json);
//                    }
//                }
//            }

//            return "Excute Success";
        }


        [HttpGet]
        public string SendMsgByXin()
        {
            //            var sql = string.Format(@"SELECT OpenId FROM dbo.RegistSaleMan a JOIN dbo.OpenIdAssociated b ON a.SalemanId=b.UserId 
            //                                    WHERE a.IsEnable =0 AND a.RegistState  <>0  AND UserType =1  ORDER BY RegistDate DESC");
            //var sql = string.Format("SELECT o.OpenId FROM RegistSaleMan AS rsm JOIN OpenIdAssociated o ON rsm.SalemanId=o.UserId where  RegistState = 2 and o.UserType = 1   ");
            var sql = string.Format(@" SELECT MemberId,OpenId FROM SendMubanMsg WHERE MemberId<11000 ORDER BY MemberId");

            DataTable dt = dataContext.ExecuteDataTable(CommandType.Text, sql);
            var json = "";
            string result = "";
            foreach (DataRow dr in dt.Rows)
            {
                string openid = dr["OpenId"].ToString();
                int memberid =  Convert.ToInt32(dr["OpenId"]);

                json = "{\"touser\":\"" + openid + "\"," +
                        "\"template_id\":\"b0-aVm0y5Urmbup-cnCykHlYWHiuaGYy9DabB-ePetU\"," +
                        "\"url\":\"https://mp.weixin.qq.com/s/G20tKtK4vIvH1nOlwO3Zuw\"," +
                        "\"data\":{" +
                                "\"first\": {" +
                                    "\"value\":\"今天是厨师的节日，快来测测你是哪一个性感面？分享号召更多厨师兄弟来参与，让世界看见我们的精彩！\"," +
                                    "\"color\":\"#173177\"" +
                                "}," +
                                "\"keyword1\":{" +
                                    "\"value\":\"厨师千面 每一面都性感！\"," +
                                    "\"color\":\"#173177\"" +
                                "}," +
                                "\"keyword2\":{" +
                                        "\"value\":\"" + DateTime.Now + "\"," +
                                    "\"color\":\"#173177\"" +
                                "}," +
                                "\"remark\":{" +
                                    "\"value\":\"快点击详情，Apple Watch大奖赢回家！\"," +
                                    "\"color\":\"#173177\"" +
                                "}" +
                        "}" +
                    "}";
                result =SendMuBanMsg(json);
                if (result == "succ")
                {
                    sql = string.Format(@"UPDATE SendMubanMsg SET SendFlag =1 ,SendDate=GETDATE() WHERE MemberId = {0}", memberid);
                    dataContext.ExecuteNonQuery(CommandType.Text, sql);
                }
            }
            return "Excute Success";
        }

        [HttpPost]
        public string ExcuteMemberPrize(dynamic requestData)
        {
            int drawPrizeId = requestData.DrawPrizeId;
            string prizeName = requestData.PrizeName;

            string sql = string.Format(" Update [DrawPrize] Set PrizeState = 1 Where DrawprizeId = {0}", drawPrizeId);
            dataContext.ExecuteNonQuery(CommandType.Text, sql);

            string msgSql = string.Format("SELECT o.OpenId FROM RegistMember as r JOIN OpenIdAssociated as o ON r.MemberId=o.UserId  WHERE MemberState=1 AND o.UserType = 2 and r.MemberId in (Select MemberId from MemberPrize Where DrawPrizeId = {0})", drawPrizeId);
            var q = dataContext.ExecuteDataTable(CommandType.Text, msgSql);
            string result = JsonConvert.SerializeObject(q);
            List<SendMember> openIdList = new List<SendMember>();
            openIdList = JsonConvert.DeserializeObject<List<SendMember>>(result);

            var json = "";
            if (openIdList != null)
            {
                foreach (var a in openIdList)
                {
                    if (!string.IsNullOrEmpty(a.OpenId))
                    {
                        json = "{\"touser\":\"" + a.OpenId + "\"," +
                                "\"template_id\":\"b0-aVm0y5Urmbup-cnCykHlYWHiuaGYy9DabB-ePetU\"," +
                                "\"url\":\"http://jifenweixin.shinho.net.cn?action=cookdays\"," +
                                "\"data\":{" +
                                        "\"first\": {" +
                                            "\"value\":\"中大奖啦，恭喜您抽中" + prizeName + "\"," +
                                            "\"color\":\"#173177\"" +
                                        "}," +
                                        "\"keyword1\":{" +
                                            "\"value\":\"无锡美食节-欣和抽奖活动\"," +
                                            "\"color\":\"#173177\"" +
                                        "}," +
                                     "\"keyword2\":{" +
                                             "\"value\":\"" + DateTime.Now + "\"," +
                                            "\"color\":\"#173177\"" +
                                        "}," +
                                        "\"remark\":{" +
                                            "\"value\":\"凭借注册手机号去领奖台领奖哦！\"," +
                                            "\"color\":\"#173177\"" +
                                        "}" +
                                "}" +
                            "}";
                        SendMuBanMsg(json);
                    }
                }
            }

            return "Exc Success";
        }

        /// <summary>
        /// 发模板消息
        /// </summary>
        /// <param name="Response"></param>
        /// <param name="msg"></param>
        public string SendMuBanMsg(string json)
        {
             return PostJson("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + BasicApi.GetAccessToken(), json);
        }

        /// <summary>
        /// post提交json
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        private string PostJson(string url, string json)
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

                return "succ";
            }
            catch (Exception ex)
            {
                //LogHelper.WriteLog(string.Format("{0}\n{1}\n{2}\n", ex.Message, ex.Source, ex.StackTrace));
                return "fail";
            }

        }
    }
}

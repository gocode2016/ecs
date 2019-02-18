using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace IntegralMallAPI.Common.ofpay
{
    public class OfpayInterface
    {
        private static string userid = System.Configuration.ConfigurationManager.AppSettings["OFCARDUserId"];
        private static string userpws = System.Configuration.ConfigurationManager.AppSettings["OFCARDUserpws"];
        private const string CHAR_SET = "UTF-8";
        private const string KeyStr = "SHINHOKEY";

        /// <summary>
        /// 自定义充值入口
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="cardnum">充值金额</param>
        /// <param name="requestid">商户订单号</param>
        /// <returns></returns>
        public static RetCode topup(string mobile, string cardnum, string sporder_id)
        {
            // SP编码
            string P0_userid = userid;
            // SP接入密码(为账户密码的MD5值)
            string P1_userpws = MD5Lower(userpws);
            // 所需提货商品的编码(快充：140101，慢充：170101，流量充值：230101 )
            string P2_cardid = "140101";
            // 可选面值
            string P3_cardnum = cardnum;
            // Sp商家的订单号
            string P4_sporder_id = sporder_id;
            // 订单时间 （yyyyMMddHHmmss 如：20070323140214）
            string P5_sporder_time = DateTime.Now.ToString("yyyyMMddHHmmss");
            // 手机号
            string P6_game_userid = mobile; 
            // MD5后字符串
            string P7_md5_str = MD5Upper(P0_userid + P1_userpws + P2_cardid + P3_cardnum + P4_sporder_id + P5_sporder_time + P6_game_userid + KeyStr);
            //订单充值成功后返回的URL地址，可为空（不参与MD5验算）
            string P8_ret_url = ConfigurationManager.AppSettings["WeiXinDomain"] + "/AgentTopup/OfpayNotify";
            //固定值为：6.0 (不参与MD5验证)
            string P9_version = "6.0";

            string url = string.Format("http://api2.ofpay.com/onlineorder.do?userid={0}&userpws={1}&cardid={2}&cardnum={3}&sporder_id={4}&sporder_time={5}&game_userid={6}&md5_str={7}&ret_url={8}&version={9}",
                P0_userid,P1_userpws,P2_cardid,P3_cardnum,P4_sporder_id,P5_sporder_time,P6_game_userid,P7_md5_str,P8_ret_url,P9_version);

            LogHelper.WriteLog("调用地址：" + url);

            // 调用充值接口
            var result = MyRequest(url);

            return result;
        }
        /// <summary>
        /// 加油卡充值
        /// </summary>
        /// <param name="cardNo">油卡号</param>
        /// <param name="cardTel">油卡联系方式</param>
        /// <param name="cardNum">金额</param>
        /// <param name="sporder_id">商户订单号</param>
        /// <returns></returns>
        public static RetCode Sinopec(string cardNo,string cardTel, string cardNum, string spOrderId)
        {
            // SP接入密码(为账户密码的MD5值)
            string userpwsMd5 = MD5Lower(userpws);
            // 所需提货商品的编码默认中石化(64127500（中石化任意充）64349102(（中石油任意充）)
            string cardid = "64127500";
            // 可选面值
            string cardnum = cardNum;
            // Sp商家的订单号
            string sporder_id = spOrderId;
            // 订单时间 （yyyyMMddHHmmss 如：20070323140214）
            string sporder_time = DateTime.Now.ToString("yyyyMMddHHmmss");
            // 卡号
            string game_userid = cardNo;
            //加油卡类型 （1:中石化、2:中石油；默认为1)
            var chargeType = "1";
            Regex matchCNPC = new Regex(@"^9\d{15}$");
            //如果是中石油
            if (matchCNPC.IsMatch(cardNo))
            {
                cardid = "64349102";
                chargeType = "2";
            }
            //持卡人手机号码
            var gasCardTel = cardTel;
            // MD5后字符串
            string md5_str = MD5Upper(userid + userpwsMd5 + cardid + cardnum + sporder_id + sporder_time + game_userid + KeyStr);
            //订单充值成功后返回的URL地址，可为空（不参与MD5验算）
            string ret_url = ConfigurationManager.AppSettings["WeiXinDomain"] + "/AgentTopup/OfpayNotify";
            //固定值为：6.0 (不参与MD5验证)
            string version = "6.0";

            string url = string.Format("http://api2.ofpay.com/sinopec/onlineorder.do?userid={0}&userpws={1}&cardid={2}&cardnum={3}&sporder_id={4}&sporder_time={5}&game_userid={6}&md5_str={7}&ret_url={8}&version={9}&gasCardTel={10}&chargeType={11}",
                userid, userpwsMd5, cardid, cardnum, sporder_id, sporder_time, game_userid, md5_str, ret_url, version, gasCardTel, chargeType);

            //CreditSys.Core.Common.LogHelper.WriteLog("调用地址：" + url);

            // 调用充值接口
            var result = MyRequest(url);            
            return result;
        }
        /// <summary>
        /// 卡密提取
        /// </summary>
        /// <param name="cardId">所需提货商品的编码(需和CP商品编码一一对应)</param>
        /// <param name="cardNum">所需提货商品的数量（1-10张）,cardnum不填默认为1</param>
        /// <param name="phone">收货手机号</param>
        /// <param name="spOrderId">Sp商家的订单号</param>
        /// <returns>卡号,密码,有效期</returns>
        public static RetCode TakeCard(string cardId, string cardNum, string phone, string spOrderId)
        {
            // SP接入密码(为账户密码的MD5值)
            string userpwsMd5 = MD5Lower(userpws);
            // 订单时间 （yyyyMMddHHmmss 如：20070323140214）
            string sporder_time = DateTime.Now.ToString("yyyyMMddHHmmss");
            // MD5后字符串
            string md5_str = MD5Upper(userid + userpwsMd5 + cardId + cardNum + spOrderId + sporder_time + KeyStr);
            //固定值为：6.0 (不参与MD5验证)
            string version = "6.0";

            string url = string.Format("http://api2.ofpay.com/order.do?userid={0}&userpws={1}&cardid={2}&cardnum={3}&sporder_id={4}&sporder_time={5}&md5_str={6}&phone={7}&version={8}",
                userid, userpwsMd5, cardId, cardNum, spOrderId, sporder_time, md5_str,phone, version);

            //CreditSys.Core.Common.LogHelper.WriteLog("调用地址：" + url);

            // 调用充值接口
            var result = MyRequest(url);            
            return result;
        }


        /// <summary>
        /// 调用API
        /// </summary>
        /// <param name="url"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        private static RetCode MyRequest(string url)
        {
            var model = new RetCode();
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                req.Method = "GET";
                using (WebResponse wr = req.GetResponse())
                {
                    HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();
                    StreamReader reader = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.Default);
                    string content = reader.ReadToEnd();
                    model = XmlHelpler.GetRetCode(content);
                }
            }
            catch(Exception e)
            {
                //CreditSys.Core.Common.LogHelper.WriteLog(e.Message);
                req.Abort();
                model.retcode = "0000";               
            }
            finally
            {
                req = null;
            }
            return model;
        }

        /// <summary>
        ///  获取MD5签名结果
        /// </summary>
        /// <param name="encypStr"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        private static byte[] MD5(string encypStr)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = Encoding.GetEncoding(CHAR_SET).GetBytes(encypStr);
            bs = md5.ComputeHash(bs);
            return bs;
        }

        /// <summary>
        /// 获取大写的MD5
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static string MD5Upper(string encypStr)
        {
            return BytesToHexString(MD5(encypStr), LowerOrUpper.大写);
        }

        /// <summary>
        /// 获取小写的MD5
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static string MD5Lower(string encypStr)
        {
            return BytesToHexString(MD5(encypStr), LowerOrUpper.小写);
        }

        /// <summary>
        /// 大小写转换
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static string BytesToHexString(byte[] bytes,LowerOrUpper val)
        {
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bytes)
            {
                if (val == LowerOrUpper.大写)
                {
                    s.Append(b.ToString("x2").ToUpper());
                }
                else if (val == LowerOrUpper.小写)
                {
                    s.Append(b.ToString("x2").ToLower());
                }
            }
            return s.ToString();
        }

      
    }

    public enum LowerOrUpper
    {
        小写=1,
        大写=2
    }
}

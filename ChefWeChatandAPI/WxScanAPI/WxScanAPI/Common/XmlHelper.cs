using System;using System.Collections.Generic;using System.Linq;using System.Web;using System.Text;
using WxScanAPI.Models;
using System.Xml;namespace WxScanAPI.Common{    public class XmlHelpler    {

        /// <summary>
        /// 获取现金红包的xml
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static string GetRedPackPayXml(IDictionary<string, string> dic)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>\n");

            sb.Append("<nonce_str>" + dic["nonce_str"] + "</nonce_str>\n");

            sb.Append("<mch_billno>" + dic["mch_billno"] + "</mch_billno>\n");

            sb.Append("<wxappid>" + dic["wxappid"] + "</wxappid>\n");

            sb.Append("<send_name>" + dic["send_name"] + "</send_name>\n");

            sb.Append("<re_openid>" + dic["re_openid"] + "</re_openid>\n");

            sb.Append("<total_amount>" + dic["total_amount"] + "</total_amount>\n");

            sb.Append("<total_num>" + dic["total_num"] + "</total_num>\n");

            sb.Append("<wishing>" + dic["wishing"] + "</wishing>\n");

            sb.Append("<client_ip>" + dic["client_ip"] + "</client_ip>\n");

            sb.Append("<act_name>" + dic["act_name"] + "</act_name>\n");

            sb.Append("<remark>" + dic["remark"] + "</remark>\n");

            sb.Append("<scene_id>" + dic["scene_id"] + "</scene_id>\n");

            sb.Append("<risk_info>" + dic["risk_info"] + "</risk_info>\n");

            sb.Append("<consume_mch_id>" + dic["consume_mch_id"] + "</consume_mch_id>\n");

            sb.Append("<sign>" + dic["sign"] + "</sign>\n");

            sb.Append("</xml>");
            return sb.ToString();
        }        /// <summary>        /// 获取企业付款到用户零钱的xml        /// </summary>        /// <param name="dic"></param>        /// <returns></returns>        public static string GetCorpPayXml(IDictionary<string,string> dic)        {             StringBuilder sb = new StringBuilder();
            sb.Append("<xml>\n");

            sb.Append("<mch_appid>" + dic["mch_appid"] + "</mch_appid>\n");

            sb.Append("<mchid>" + dic["mchid"] + "</mchid>\n");

            sb.Append("<nonce_str>" + dic["nonce_str"] + "</nonce_str>\n");

            sb.Append("<partner_trade_no>" + dic["partner_trade_no"] + "</partner_trade_no>\n");

            sb.Append("<openid>" + dic["openid"] + "</openid>\n");

            sb.Append("<check_name>" + dic["check_name"] + "</check_name>\n");

            sb.Append("<re_user_name>" + dic["re_user_name"] + "</re_user_name>\n");

            sb.Append("<amount>" + dic["amount"] + "</amount>\n");

            sb.Append("<desc>" + dic["desc"] + "</desc>\n");

            sb.Append("<spbill_create_ip>" + dic["spbill_create_ip"] + "</spbill_create_ip>\n");            sb.Append("<sign>" + dic["sign"] + "</sign>\n");            sb.Append("</xml>");            return sb.ToString();        }


        public static PayResultModel GetWxPayResult(string msg)
        {
            msg = msg.Replace("<![CDATA[", "").Replace("]]>", "");
            var model = new PayResultModel();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(msg);
            XmlNodeList list = doc.GetElementsByTagName("xml");
            XmlNode xn = list[0];
            model.return_code = xn.SelectSingleNode("//return_code").InnerText;  //SUCCESS/FAIL 此字段是通信标识，非交易标识，交易是否成功需要查看result_code来判断
            model.return_msg = xn.SelectSingleNode("//return_msg").InnerText;  //返回信息，如非空，为错误原因 
            if (model.return_code.Contains("SUCCESS"))
            {
                model.result_code = xn.SelectSingleNode("//result_code").InnerText;   //业务结果 SUCCESS/FAIL
                model.mch_appid = xn.SelectSingleNode("//mch_appid").InnerText;   //商户appid
                model.mchid = xn.SelectSingleNode("//mchid").InnerText;   //商户号
                if (model.result_code.Contains("SUCCESS"))
                {
                    model.partner_trade_no = xn.SelectSingleNode("//partner_trade_no").InnerText;//商户订单号
                    model.payment_no = xn.SelectSingleNode("//payment_no").InnerText;  //微信订单号
                    model.payment_time = xn.SelectSingleNode("//payment_time").InnerText;  //微信支付成功时间
                    model.nonce_str = xn.SelectSingleNode("//nonce_str").InnerText;   //随机字符串，不长于32位
                    //model.device_info = xn.SelectSingleNode("//device_info").InnerText;   //微信支付分配的终端设备号
                  
                }
                else
                {
                    model.err_code = xn.SelectSingleNode("//err_code").InnerText;  //错误代码
                    model.err_code_des = xn.SelectSingleNode("//err_code_des").InnerText;   //错误代码描述
                }
            }
            return model;
        }    }}
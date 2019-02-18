using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace IntegralMallAPI.Common.ofpay
{
    public class XmlHelpler
    {

        /// <summary>
        /// 获取返回的XML结果
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static RetCode GetRetCode(string str)
        {
            XElement d = XElement.Parse(str);
            var model = new RetCode();
            if (d.Element("err_msg") != null)
                model.err_msg = d.Element("err_msg").Value;
            if (d.Element("retcode") != null)
                model.retcode = d.Element("retcode").Value;
            if (d.Element("orderid") != null)
                model.orderid = d.Element("orderid").Value;
            if (d.Element("cardid") != null)
                model.cardid = d.Element("cardid").Value;
            if (d.Element("cardnum") != null)
                model.cardnum = d.Element("cardnum").Value;
            if (d.Element("ordercash") != null)
                model.ordercash = d.Element("ordercash").Value;
            if (d.Element("cardname") != null)
                model.cardname = d.Element("cardname").Value;
            if (d.Element("sporder_id") != null)
                model.sporder_id = d.Element("sporder_id").Value;
            if (d.Element("game_userid") != null)
                model.game_userid = d.Element("game_userid").Value;
            if (d.Element("game_state") != null)
                model.game_state = d.Element("game_state").Value;
            if (d.Element("cards") != null)
            {
                model.cardno = d.Element("cards").Element("card").Element("cardno").Value;
                model.cardpws = d.Element("cards").Element("card").Element("cardpws").Value;
                model.expiretime = d.Element("cards").Element("card").Element("expiretime").Value;
            }
            return model;
        }
    }
}

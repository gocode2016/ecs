using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketActivityAPI.WeiXin.Models
{
    public class QRcode : ErrorMsg
    {
        //二维码ticket
        public string ticket { get; set; }
        //有效时间
        public string expire_seconds { get; set; }
        //二维码地址
        public string url { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntegralMallAPI.Common.ofpay
{
    public class RetCode
    {
        public string err_msg { get; set; }
        public string retcode { get; set; }
        public string orderid { get; set; }
        public string cardid { get; set; }
        public string cardnum { get; set; }
        public string ordercash { get; set; }
        public string cardname { get; set; }
        public string sporder_id { get; set; }
        public string game_userid { get; set; }
        public string game_state { get; set; }
        public string cardno { get; set; }
        public string cardpws { get; set; }
        public string expiretime { get; set; }
    }
}

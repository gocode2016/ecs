using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WxScanAPI.Models
{
    public class RScanJson
    {
        public RScanJson()
        {
            this.result_status = "fail";
            this.code_status = "正常";
            this.return_code = 200;
        }
        public int return_code { get; set; }
        public string result_status { get; set; }
        public string code_status { get; set; }
        //扫码时间
        public string scan_date { get; set; }
        public string message { get; set; }
        //当次扫码获取的金额
        public double money { get; set; }
        /// <summary>
        /// 累计中奖未提现金额
        /// </summary>
        public double kTxMoney { get; set; }
        /// <summary>
        /// 是否可提现
        /// </summary>
        public int isKeTiXian { get; set; }
        //是否注册用户  0未注册  1已注册
        public int isRegist{ get; set; }
        public object data { get; set; }
    }

    public class RHistoryJson
    {
        public RHistoryJson()
        {
            this.result_status = "fail";
            this.return_code = 200;
        }
        public int return_code { get; set; }
        public string result_status { get; set; }
        //已经扫码次数
        public int scanCount { get; set; }
        public string message { get; set; }
        //已经提现的金额
        public double haveTxMoney { get; set; }
        /// <summary>
        /// 可以提现的金额
        /// </summary>
        public double kTxMoney { get; set; }
        /// <summary>
        /// 是否可提现
        /// </summary>
        public int isKeTiXian { get; set; }
        //是否注册用户  0未注册  1已注册
        public int isRegist { get; set; }
        public object data { get; set; }
    }

    public class RJson
    {
        public RJson()
        {
            this.return_code = 200;
            this.result_status = "fail";
        }
        public int return_code { get; set; }
        public string result_status { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }

}
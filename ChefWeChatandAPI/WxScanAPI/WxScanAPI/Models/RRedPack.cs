using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WxScanAPI.Models
{
    public class RRedPack
    {
        public RRedPack()
        {
        }

        /// <summary>
        /// 返回状态
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 返回状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 本次中奖金额
        /// </summary>
        public double Money { get; set; }
        /// <summary>
        /// 累计中奖金额
        /// </summary>
        public double TotalMoney  { get; set; }
        /// <summary>
        /// 是否可提现
        /// </summary>
        public int IsKeTiXian { get; set; }
    }
}
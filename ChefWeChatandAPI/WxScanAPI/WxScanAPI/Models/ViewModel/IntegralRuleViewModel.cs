using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WxScanAPI.Models
{
    public class IntegralRuleViewModel
    {
        public int RuleId { get; set; }
        public string RuleName { get; set; }
        public int GoodsId { get; set; }
        public int Integral { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int RuleState { get; set; }
        public DateTime CreateDate { get; set; }
        public string Remark { get; set; }
        public int RuleType { get; set; }
        public int ScanLimit { get; set; }
        public string GoodsName { get; set; }
    }
}
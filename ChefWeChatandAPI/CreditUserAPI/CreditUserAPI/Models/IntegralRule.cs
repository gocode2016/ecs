using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CreditUserAPI.Models
{
    /// <summary>
    /// 积分规则
    /// </summary>
    public class IntegralRule
    {
        [Key]
        public int IntegralRuleId { get; set; }
        /// <summary>
        /// 规则名称
        /// </summary>
        public string RuleName { get; set; }
        /// <summary>
        /// 积分产品名称
        /// </summary>
        public string ProduceName { get; set; }
        /// <summary>
        /// 积分产品规格
        /// </summary>
        public string ProduceNorms { get; set; }
        /// <summary>
        /// 规则开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 规则结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 规则状态
        /// </summary>
        public int RuleState { get; set; }
        /// <summary>
        /// 积分值
        /// </summary>
        public int Integral { get; set; }
        public float Price { get; set; }
        public string Unit { get; set; }
        public int IsActivity { get; set; }
        public string Remark { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntegralCardAPI.Models
{
    /// <summary>
    /// 活动规则表
    /// </summary>
    public class IntegralRuleChance
    {
        [Key]
        public int RuleChanceId { get; set; }
        /// <summary>
        /// 规则ID
        /// </summary>
        public int RuleId { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int Integral { get; set; }
        /// <summary>
        /// 投放数量
        /// </summary>
        public int PutNum { get; set; }
        /// <summary>
        /// 概率
        /// </summary>
        public int Chance { get; set; }
    }
}
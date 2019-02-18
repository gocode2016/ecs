using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IntegralMallAPI.Models
{
    /// <summary>
    /// 商城销售活动
    /// </summary>
    public class SaleActivity
    {
        /// <summary>
        /// 具体SKUID
        /// </summary>
        [Key]
        public int SkuId { get; set; }
        /// <summary>
        /// 活动开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 活动结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 售卖价格
        /// </summary>
        public int SalePrice { get; set; }
        /// <summary>
        /// 限购
        /// </summary>
        public int Limit { get; set; }
    }
}
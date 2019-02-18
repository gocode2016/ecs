using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntegralMallAPI.Models
{
    /// <summary>
    /// 订单详情
    /// </summary>
    public class OrderDetaile
    {
        [Key]
        public int OrderDetaileId { get; set; }
        /// <summary>
        /// 订单ID
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// SKUID
        /// </summary>
        public int SkuId { get; set; }
        /// <summary>
        /// sku名称
        /// </summary>
        public string SkuName { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// 订单购买数量
        /// </summary>
        public int Count { get; set; }
    }
}
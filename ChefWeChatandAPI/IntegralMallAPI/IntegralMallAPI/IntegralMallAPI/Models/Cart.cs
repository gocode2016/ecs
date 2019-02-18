using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntegralMallAPI.Models
{
    /// <summary>
    /// 购物车
    /// </summary>
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        /// <summary>
        /// 会员ID
        /// </summary>
        public int MemberId { get; set; }
        /// <summary>
        /// SKUID
        /// </summary>
        public int SkuId { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// sku名称
        /// </summary>
        public string SkuName { get; set; }
        /// <summary>
        /// 价格 此处价格为SKU+Count的价格
        /// </summary>
        public int Price { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegralMallAPI.Models
{
    /// <summary>
    /// SKU信息
    /// </summary>
    public class SKU
    {
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
        /// 物料编码
        /// </summary>
        public string SkuCode { get; set; }
        /// <summary>
        /// 规格ID
        /// </summary>
        public int NormsId { get; set; }
        /// <summary>
        /// 库存
        /// </summary>
        public int Stock { get; set; }
        /// <summary>
        /// 市场价格
        /// </summary>
        public double MarketPrice { get; set; }
        /// <summary>
        /// 具体销售价格
        /// </summary>
        public int Price { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 限购
        /// </summary>
        public int Limit { get; set; }
        /// <summary>
        /// 是否展示
        /// </summary>
        public int IsShow { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime AddDate { get; set; }
        /// <summary>
        /// 是否启禁用
        /// </summary>
        public int IsEnable { get; set; }

    }
}
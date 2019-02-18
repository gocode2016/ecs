using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntegralMallAPI.Models
{
    /// <summary>
    /// 商品明细
    /// </summary>
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 类目ID
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// 类目名称
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 商品价格，此价格仅为展示价格
        /// </summary>
        public string ProductPrice { get; set; }
        /// <summary>
        /// 商品详情
        /// </summary>
        public string ProductContent { get; set; }
        /// <summary>
        /// 商品图片
        /// </summary>
        public string ImgUrl { get; set; }
        /// <summary>
        /// 限购-省
        /// </summary>
        public string SaleProvince { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? AddDate { get; set; }
        /// <summary>
        /// 商品类型
        /// </summary>
        public int ProductType { get; set; } 
        /// <summary>
        /// 虚拟接口类型
        /// </summary>
        public int InventedType { get; set; }
        /// <summary>
        /// 是否启禁用
        /// </summary>
        public int IsEnable { get; set; }
    }
}
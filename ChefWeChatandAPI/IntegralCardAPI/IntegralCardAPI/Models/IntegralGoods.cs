using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntegralCardAPI.Models
{
    /// <summary>
    /// 积分商品
    /// </summary>
    public class IntegralGoods
    {
        [Key]
        public int GoodsId { get; set; }
        /// <summary>
        /// 商品编码
        /// </summary>
        public string GoodsNo { get; set; }
        /// <summary>
        /// 商品内部编码
        /// </summary>
        public string GoodsInterNo { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName { get; set; }
        /// <summary>
        /// 商品价格
        /// </summary>
        public double GoodsPrice { get; set; }
        /// <summary>
        /// 商品状态
        /// </summary>
        public int GoodsState { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 商品URL
        /// </summary>
        public string GoodsImageUrl { get; set; }
    }
}
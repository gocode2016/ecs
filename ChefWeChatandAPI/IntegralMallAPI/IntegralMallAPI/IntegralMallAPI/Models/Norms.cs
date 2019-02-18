using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntegralMallAPI.Models
{
    /// <summary>
    /// 商品规格
    /// </summary>
    public class Norms
    {
        /// <summary>
        /// 规格ID
        /// </summary>
        [Key]
        public int NormsId { get; set; }
        /// <summary>
        /// 商品id
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 规格名称
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 规格值
        /// </summary>
        public string TypeValue { get; set; }
        /// <summary>
        /// 是否启禁用
        /// </summary>
        public int IsEnable { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CreditUserAPI.Models
{
    /// <summary>
    /// 队员区域表
    /// 新需求，队员有多个省市区
    /// </summary>
    public class SalemanArea
    {
        [Key]
        public int SalemanAreaId { get; set; }
        /// <summary>
        /// 队员ID
        /// </summary>
        public int SalemanId { get; set; }
        /// <summary>
        /// 省ID
        /// </summary>
        public int? ProvinceId { get; set; }
        /// <summary>
        /// 省名称
        /// </summary>
        public string ProvinceName { get; set; }
        /// <summary>
        /// 市ID
        /// </summary>
        public int? CityId { get; set; }
        /// <summary>
        /// 市名称
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// 区ID
        /// </summary>
        public int? AreaId { get; set; }
        /// <summary>
        /// 区名称
        /// </summary>
        public string AreaName { get; set; }
    }
}
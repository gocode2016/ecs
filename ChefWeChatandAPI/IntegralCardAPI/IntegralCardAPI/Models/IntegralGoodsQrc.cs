using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntegralCardAPI.Models
{
    /// <summary>
    /// 商品积分二维码
    /// </summary>
    public class IntegralGoodsQrc
    {
        [Key]
        public int QrcId { get; set; }
        /// <summary>
        /// 二维码URL
        /// </summary>
        public string QrcUrl { get; set; }
        /// <summary>
        /// 流水号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 二维码包ID
        /// </summary>
        public int PackageId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 使用时间
        /// </summary>
        public DateTime? UseDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
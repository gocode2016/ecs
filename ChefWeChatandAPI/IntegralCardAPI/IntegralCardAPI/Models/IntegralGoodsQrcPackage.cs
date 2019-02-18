using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntegralCardAPI.Models
{
    /// <summary>
    /// 积分商品二维码包
    /// </summary>
    public class IntegralGoodsQrcPackage
    {
        [Key]
        public int PackageId { get; set; }
        /// <summary>
        /// 包名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        public int GoodsId { get; set; }
        /// <summary>
        /// 网站链接
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 规格包装
        /// </summary>
        public int Range { get; set; }
        /// <summary>
        /// 生产类别
        /// </summary>
        public string GoodsCode { get; set; }
        /// <summary>
        /// 生产工厂
        /// </summary>
        public int Source { get; set; }
        /// <summary>
        /// 月份
        /// </summary>
        public int March { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 生产状态
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 压缩包地址
        /// </summary>
        public string DownLoadUrl { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
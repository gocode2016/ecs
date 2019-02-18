using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CreditUserAPI.Models
{
    /// <summary>
    /// 队员表
    /// </summary>
    public class RegistSaleMan
    {
        [Key]
        public int SalemanId { get; set; }
        /// <summary>
        /// 队员省ID
        /// </summary>
        public int ProvinceId { get; set; }
        /// <summary>
        /// 队员省名称
        /// </summary>
        public string ProvinceName { get; set; }
        /// <summary>
        /// 队员市ID
        /// </summary>
        public int CityId { get; set; }
        /// <summary>
        /// 队员市名称
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// 队员区ID
        /// </summary>
        public int AreaId { get; set; }
        /// <summary>
        /// 队员区名称
        /// </summary>
        public string AreaName { get; set; }
        /// <summary>
        /// 队员大区ID
        /// </summary>
        public int? RegionId { get; set; }
        /// <summary>
        /// 队员大区名称
        /// </summary>
        public string RegionName { get; set; }
        /// <summary>
        /// 队员工号
        /// </summary>
        public string WorkNum { get; set; }
        /// <summary>
        /// 队员名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 队员电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 队员身份证号
        /// </summary>
        public string CardId { get; set; }
        /// <summary>
        /// 队员注册名称
        /// </summary>
        public DateTime RegistDate { get; set; }
        /// <summary>
        /// 队员状态
        /// </summary>
        public int RegistState { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 队员导入时间
        /// </summary>
        public DateTime? ImportDate { get; set; }
        /// <summary>
        /// 是否启禁用
        /// </summary>
        public int IsEnable { get; set; }
        /// <summary>
        /// 岗位
        /// </summary>
        public string Position { get; set; }
    }
}
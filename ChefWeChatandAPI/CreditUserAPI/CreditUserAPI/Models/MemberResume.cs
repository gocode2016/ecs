using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CreditUserAPI.Models
{
    /// <summary>
    /// 会员履历
    /// </summary>
    public class MemberResume
    {
        [Key]
        public int ResumeId { get; set; }
        /// <summary>
        /// 省ID
        /// </summary>
        public int ProvinceId { get; set; }
        /// <summary>
        /// 省名称
        /// </summary>
        public string ProvinceName { get; set; }
        /// <summary>
        /// 市ID
        /// </summary>
        public int CityId { get; set; }
        /// <summary>
        /// 市名称
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// 区ID
        /// </summary>
        public int AreaId { get; set; }
        /// <summary>
        /// 区名称
        /// </summary>
        public string AreaName { get; set; }
        /// <summary>
        /// 工作经历开始时间
        /// </summary>
        public DateTime? BeginTime { get; set; }
        /// <summary>
        /// 工作经历结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 会员id
        /// </summary>
        public int MemberId { get; set; }
        /// <summary>
        /// 酒店名称
        /// </summary>
        public string HotelName { get; set; }
        /// <summary>
        /// 酒店地址
        /// </summary>
        public string HotelAddress { get; set; }
        /// <summary>
        /// 从事工作
        /// </summary>
        public string Job { get; set; }
        /// <summary>
        /// 酒店编码
        /// </summary>
        public string HotelCode { get; set; }
        /// <summary>
        /// 完善时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
    }
}
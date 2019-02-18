using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MarketActivityAPI.Models
{
    /// <summary>
    /// 签到活动
    /// </summary>
    public class SignActivity
    {
        [Key]
        public int ActivityId { get; set; }
        public string ActName { get; set; }
        public int ActState { get; set; }
        public int Integral { get; set; }
        public string ActivityDesc { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string ActivityType { get; set; }
        public string ShareUrl { get; set; }
        public string ActivityCode { get; set; }
        public string ActivityRegion { get; set; }
        public int ActProvinceId { get; set; }
        public string ActProvinceName { get; set; }
        public int ActCityId { get; set; }
        public string ActCityName { get; set; }
        public DateTime RegistDate { get; set; }
    }
}
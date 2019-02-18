using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    /// <summary>
    /// 赛区表
    /// </summary>
    public class MatchRegion
    {
        [Key]
        public int MatchRegionId { get; set; }
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        public string AreaName { get; set; }
        public int ChefActivityId { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }

        public string CreatePerson { get; set; }
        public string UpdatePerson { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public string Remark { get; set; }
    }
}
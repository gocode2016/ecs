using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    /// <summary>
    /// 规格配置地区类
    /// </summary>
    public class SpecificationConfArea
    {
        public SpecificationConfArea() {
            this.CreateTime = DateTime.Now;
        }
        [Key]
        public int SpecificationConfAreaId { get; set; }
        public int SpecificationConfId { get; set; }
        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        //public string CreatePerson { get; set; }
        //public string UpdatePerson { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
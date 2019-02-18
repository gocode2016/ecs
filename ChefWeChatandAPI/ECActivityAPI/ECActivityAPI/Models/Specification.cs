using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    /// <summary>
    /// 产品规格类
    /// </summary>
    public class Specification
    {
        public Specification() {
            this.CreateTime = DateTime.Now;
        }
        [Key]
        public int SpecificationId { get; set; }
        public int ProductId { get; set; }
        public string Amount { get; set; }
        public string Unit { get; set; }
        public string SpecificationPicUrl { get; set; }
        public int VisitCount { get; set; }
        public int PraiseCount { get; set; }
        public string CreatePerson { get; set; }
        public string UpdatePerson { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
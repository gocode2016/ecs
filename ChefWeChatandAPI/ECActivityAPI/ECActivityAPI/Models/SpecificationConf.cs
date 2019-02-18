using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    /// <summary>
    /// 规格配置类
    /// </summary>
    public class SpecificationConf
    {
        public SpecificationConf() {
            this.CreateTime = DateTime.Now;
        }
        [Key]
        public int SpecificationConfId { get; set; }
        public int SpecificationId { get; set; }
        //物料编号
        public string MaterialNum { get; set; }
        public string Introduce { get; set; }
        public string CreatePerson { get; set; }
        public string UpdatePerson { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
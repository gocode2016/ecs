using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    /// <summary>
    /// 规格配置虚拟分类信息类
    /// </summary>
    public class SpecificationConfVC
    {
        public SpecificationConfVC () {
            this.CreateTime = DateTime.Now;
        }
        [Key]
        public int SpecificationConfVCId { get; set; }
        public int SpecificationConfId { get; set; }
        public int VCId { get; set; }
        public int PriorityId { get; set; }
        public string IsDisplay { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
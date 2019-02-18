using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    /// <summary>
    /// 虚拟分类 类
    /// </summary>
    public class VirtualClassify
    {
        public VirtualClassify(){
            this.CreateTime = DateTime.Now;
        }
        [Key]
        public int VCId { get; set; }
        public string VCName { get; set; }
        public string ShortIntroduce { get; set; }
        public string IsDisplay { get; set; }
        public int IsIntroduce { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
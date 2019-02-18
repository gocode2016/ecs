using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    /// <summary>
    /// 产品一级分类
    /// </summary>
    public class ProductFirst
    {
        public ProductFirst() {
            this.CreateTime = DateTime.Now;
        }
        [Key]
        public int ProductFirstId { get; set; }
        public string ProductFirstName { get; set; }
        public string ProductFirstIcon { get; set; }
        public int IsDisplay { get; set; }
        public int PriorityId { get; set; }
        public string CreatePerson { get; set; }
        public string UpdatePerson { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
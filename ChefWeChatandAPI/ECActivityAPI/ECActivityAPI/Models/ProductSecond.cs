using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    /// <summary>
    /// 产品二级分类
    /// </summary>
    public class ProductSecond
    {
        public ProductSecond() {
            this.CreateTime = DateTime.Now;
        }
        [Key]
        public int ProductSecondId { get; set; }
        public int ProductFirstId { get; set; }
        public string ProductSecondName { get; set; }
        public string IsDisplay { get; set; }
        public string CreatePerson { get; set; }
        public string UpdatePerson { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
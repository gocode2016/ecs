using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
   /// <summary>
   /// 产品库研发基本信息类
   /// </summary>
    public class ProductIdKuInfoRD
    {
        public ProductIdKuInfoRD() {
            this.CreateTime = DateTime.Now;
        }
        [Key]
        public int RDId { get; set; }
        public int ProductId { get; set; }
        public string RDStory { get; set; }
        public int IsDisPlay { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
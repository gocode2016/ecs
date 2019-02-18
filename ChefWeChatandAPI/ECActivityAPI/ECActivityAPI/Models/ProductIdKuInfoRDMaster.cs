using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    /// <summary>
    /// 产品库研发大师信息类
    /// </summary>
    public class ProductIdKuInfoRDMaster
    {
        public ProductIdKuInfoRDMaster() {
            this.CreateTime = DateTime.Now;
        }
        [Key]
        public int RDMasterId { get; set; }
        public int RDId { get; set; }
        public string MasterHeaderPicUrl { get; set; }
        public string MasterName { get; set; }
        public string MasterStation { get; set; }
        public string MasterHotal { get; set; }
        public int  IsDisPlay { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
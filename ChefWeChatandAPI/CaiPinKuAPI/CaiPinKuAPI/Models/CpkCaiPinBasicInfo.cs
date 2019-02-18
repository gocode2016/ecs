using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CaiPinKuAPI.Models
{
    public class CpkCaiPinBasicInfo
    {
        public CpkCaiPinBasicInfo()
        {
            this.CreateDate = DateTime.Now;
            this.IsEnable = 1;
            this.IsShow = 0;
        }
       
        [Key]
        public int CaiPinId { get; set; }
        public string CaiPinName { get; set; }
        public string VideoUrl { get; set; }
        public string VideoImage { get; set; }
        public int DaoShiId { get; set; }
        public int IsPublish { get; set; }
        public int IsEnable { get; set; }
        public int IsShow { get; set; }
        public DateTime CreateDate { get; set; }
        public string MaterialJson { get; set; }
        public string Remark { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CaiPinKuAPI.Models
{
    public class CpkZhuanTi
    {
        public CpkZhuanTi()
        {
            this.CreateDate = DateTime.Now;
            this.IsEnable = 1;
        }
        [Key]
        public int ZhuanTiId { get; set; }
        public string ZhuanTiName { get; set; }
        public string ZhuanTiURL { get; set; }
        public int IsEnable { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; } 
        public string Remark { get; set; }
    }

    public class CpkZhuanTiContent
    {
        public CpkZhuanTiContent()
        {
            this.CreateDate = DateTime.Now;
            this.IsEnable = 1;
        }
        [Key]
        public int ZtContentId { get; set; }
        public int ZhuanTiId { get; set; }
        public string Content { get; set; }
        public int IsEnable { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Remark { get; set; }

    }
}
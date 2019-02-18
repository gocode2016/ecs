using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WxScanAPI.Models
{
    public class RedPackScanRecord
    {
        public RedPackScanRecord()
        {
            this.PayId = 0;
            this.IsPay = 0;
        }
        [Key]
        public int Id { get; set; }
        public string OpenId { get; set; }
        public double Money { get; set; }
        public DateTime ScanDate { get; set; }
        public string ScanCode { get; set; }
        public int IsPay { get; set; }
        public int PayId { get; set; }
        public string Remark { get; set; }
    }
}
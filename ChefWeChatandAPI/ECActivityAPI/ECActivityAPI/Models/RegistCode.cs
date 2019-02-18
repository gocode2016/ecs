using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    /// <summary>
    /// 会员与队员关联表
    /// </summary>
    public class RegistCode
    {
        [Key]
        public int RegistCodeId { get; set; }
        public int SalemanId { get; set; }
        public int MemberId { get; set; }
        public int RegistCodeState { get; set; }
        public DateTime GenerDate { get; set; }
        public DateTime UseDate { get; set; }
    }
}
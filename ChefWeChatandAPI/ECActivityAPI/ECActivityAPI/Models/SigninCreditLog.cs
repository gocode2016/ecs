using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    /// <summary>
    /// 签到积分日志
    /// </summary>
    public class SigninCreditLog
    {
        public SigninCreditLog()
        {
            this.CreateTime = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }
        public int MemberId { get; set; }
        public DateTime SigninData { get; set; }
        public int IsDraw { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
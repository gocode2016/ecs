using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    /// <summary>
    /// 签到积分
    /// </summary>
    public class SigninCredit
    {
        public SigninCredit()
        {
            this.CreateTime = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int CreditsCount { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
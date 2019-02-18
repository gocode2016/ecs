using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CommonAPI.Models
{
    /// <summary>
    /// 常规抽奖日志
    /// </summary>
    public class LuckDrawLog
    {
        [Key]
        public int DrawId { get; set; }
        public int MemberId { get; set; }
        public string DrawName { get; set; }
        public DateTime? DrawTime { get; set; }
        public int IsShare { get; set; }
    }
}
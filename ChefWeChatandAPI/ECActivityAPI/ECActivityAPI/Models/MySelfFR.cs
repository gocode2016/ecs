using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    /// <summary>
    /// 个人菜品库推荐调料
    /// </summary>
    public class MySelfFR
    {
        [Key]
        public int MySelfFRId { get; set; }
        public string ActivityName { get; set; }
        public string FRName { get; set; }
        public string FRPicUrl { get; set; }
        public int FRCount { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
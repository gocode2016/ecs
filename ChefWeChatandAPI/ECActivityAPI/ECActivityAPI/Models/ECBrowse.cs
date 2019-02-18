using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    /// <summary>
    /// EC活动浏览数据总统计
    /// </summary>
    public class ECBrowse
    {
        [Key]
        public int Id { get; set; }
        public string ECURL { get; set; }
        public int PV { get; set; }
        public int UV { get; set; }
        public int Transpond { get; set; }
    }
}
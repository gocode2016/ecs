using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    /// <summary>
    /// 页面名称
    /// </summary>
    public class URLName
    {
        public int Id { get; set; }
        public string URL { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
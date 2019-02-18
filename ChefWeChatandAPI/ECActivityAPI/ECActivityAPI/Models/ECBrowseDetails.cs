using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    /// <summary>
    /// EC活动信息浏览详情
    /// </summary>
    public class ECBrowseDetails
    {
        public ECBrowseDetails() {
            this.CreateTime = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }
        public string ECURL { get; set; }
        public string PageName { get; set; }

        public string PageShort { get; set; }
        public string PageDetail { get; set; }
        public string PageType { get; set; }
        public string Parameter { get; set; }
        public string OpenId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
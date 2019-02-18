using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CommonAPI.Models
{
    /// <summary>
    /// 转发详情类
    /// </summary>
    public class ECWXTranspondDetails
    {
        public ECWXTranspondDetails() {
            this.CreateTime = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }
        public string ECBrowseURL { get; set; }
        public string Parameter { get; set; }
        public string OpenId { get; set; }
        public int TranspondType { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
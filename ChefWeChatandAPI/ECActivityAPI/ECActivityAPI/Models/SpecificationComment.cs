using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    /// <summary>
    /// 规格留言类
    /// </summary>
    public class SpecificationComment
    {
        public SpecificationComment()
        {
            this.CreateTime = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }
        public int SpecificationId { get; set; }
        public string OpenId { get; set; }
        public string MemberName { get; set; }
        public string HeadPicUrl { get; set; }
        public string Comment { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
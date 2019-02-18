using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    public class SpecificationPraiseLog
    {
        public SpecificationPraiseLog() {
            this.CreateTime = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }
        public int SpecificationId { get; set; }
        public string OpenId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
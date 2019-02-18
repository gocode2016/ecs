using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MarketActivityAPI.Models
{
    public class SignInLog
    {
        [Key]
        public int SignId { get; set; }
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public int UserId { get; set; }
        public string OpenId { get; set; }
        public DateTime? SignDate { get; set; }
        public int? IsShare { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CreditUserAPI.Models
{
    public class IntegralQrc
    {
        [Key]
        public int IntegralId { get; set; }
        public int IntegralRuleId { get; set; }
        public int Count { get; set; }
        public string QrcUrl { get; set; }
        public int QrcState { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
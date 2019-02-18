using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    public class SpecificationVCDatePraiseLog
    {
        [Key]
        public int Id { get; set; }
        public int SpecificationVCDateId { get; set; }
        //public int VCId { get; set; }
        public string OpenId { get; set; }
        public DateTime CreateTime { get; set; }

    }
}
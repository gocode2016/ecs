using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    public class SpecificationVCDate
    {
        [Key]
        public int Id { get; set; }
        public int SpecificationId { get; set; }
        public int VCId { get; set; }
        public int VisitCount { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
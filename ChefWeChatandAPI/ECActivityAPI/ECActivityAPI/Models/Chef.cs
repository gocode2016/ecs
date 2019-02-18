using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    public class Chef
    {
        public Chef() {
            this.CreateTime = DateTime.Now;
        }

        [Key]
        public int ChefId { get; set; } 
        [Required]
        public int MemberId { get; set; }
        [Required]
        public int ChefActivityId { get; set; }
        [Required]
        public int MatchRegionId { get; set; }
        //报名途径
        public string ApplyType { get; set; }
        public string ReferrerName { get; set; }
        public DateTime Birthday { get; set; }
        public string ChefPicUrl { get; set; }
      
        public string ChefHotelPicUrl { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
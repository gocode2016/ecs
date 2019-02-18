using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    public class FlavourRec
    {
        [Key]
        public int FlavourRecId { get; set; }
        [Required]
        public int ChefActivityId { get; set; }
         [Required]
        public string  FlavourName { get; set; }
         [Required]
        public string FlavourPicUrl { get; set; }
         [Required]
        public int FlavourType { get; set; }
         [Required]
        public DateTime CreateTime { get; set; }
         [Required]
        public DateTime UpdateTime { get; set; }
    }
}
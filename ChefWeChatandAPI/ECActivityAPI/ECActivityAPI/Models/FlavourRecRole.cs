using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    public class FlavourRecRole
    {
        public FlavourRecRole() {
            this.CreateTime = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public int RoleId { get; set; }
         [Required]
        public int FlavourRecId { get; set; }
         [Required]
        public int DishId { get; set; }
         public string Unit { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        

    }
}
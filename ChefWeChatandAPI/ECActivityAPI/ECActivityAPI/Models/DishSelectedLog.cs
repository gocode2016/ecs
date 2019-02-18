using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    /// <summary>
    /// 菜品投票Log
    /// </summary>
    public class DishSelectedLog
    {
        public DishSelectedLog() {
            this.CreateTime = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public int DishId { get; set; }
        [Required]
        public string OpenId { get; set; }
        [Required]
        public int DishType { get; set; }


        public int ChefActivityId { get; set; }
        [Required]

        public DateTime CreateTime { get; set; }
    }
}
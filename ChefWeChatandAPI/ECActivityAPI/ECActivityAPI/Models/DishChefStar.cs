using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    /// <summary>
    /// 星厨菜品
    /// </summary>
    public class DishChefStar
    {
        [Key]
        public int DishChefStarId { get; set; }
        [Required]
        public int ChefStarId { get; set; }
        
        public string DishStory { get; set; }
        [Required]
        public string DishPicUrl { get; set; }
       
        public string DishName { get; set; }
        [Required]
        public int DishType { get; set; }
        [Required]
        public string CreatePerson { get; set; }
        [Required]
        public DateTime CreateTime { get; set; }
        [Required]
        public string UpdatePerson { get; set; }
        [Required]
        public DateTime UpdateTime { get; set; }
        public int VisitCount { get; set; }
        public int ShareCount { get; set; }
        public int PrasieCount { get; set; }
    }
}
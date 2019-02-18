using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    /// <summary>
    /// 星厨
    /// </summary>
    public class ChefStar
    {
        public ChefStar() {
            this.CreateTime = DateTime.Now;
        }
        [Key]
        public int ChefStarId { get; set; }
        [Required]
        public int ChefActivityId { get; set; }
        [Required]
        public string Introduce { get; set; }
        [Required]
        public string PicUrl { get; set; }
        [Required]
        public string ChefStarName { get; set; }
        [Required]
        public string HeadPicUrl { get; set; }
        [Required]
        public string ChefStarPosition { get; set; }
        [Required]
        public string HotelName { get; set; }
        [Required]
        public string CityName { get; set; }
        [Required]
        public int IsDisplay { get; set; }
        [Required]
        public int PriorityId { get; set; }
        [Required]
        public string CreatePerson { get; set; }
        [Required]
        public string UpdatePerson { get; set; }
        [Required]
        public DateTime CreateTime { get; set; }
        [Required]
        public DateTime UpdateTime { get; set; }
       
    }
}
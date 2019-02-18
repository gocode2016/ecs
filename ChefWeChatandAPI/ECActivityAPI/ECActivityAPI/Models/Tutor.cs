using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    public class Tutor
    {
        public Tutor(){
            this.CreatePerson = "1";
         }
        [Key]
        public int TutorId { get; set; }
        [Required]
        public int ChefActivityId { set; get; }
        [Required]
        public string Introduce { set; get; }
        [Required]
        public string PicUrl { set; get; }
        [Required]
        public string HeadPicUrl { get; set; }
        [Required]
        public string TutorName { set; get; }
        public string TutorComment { set; get; }
        [Required]
        public string AreaName { get; set; }
        [Required]
        public string TutorPosition { set; get; }
        [Required]
        public string TutorHotel { set; get; }
        [Required]
        public int PriorityId { get; set; }
        [Required]
        public int IsDisplay { get; set; }
        [Required]
        public int IsDel { set; get; }
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
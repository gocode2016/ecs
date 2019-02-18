using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    public class DishTutor
    {
        [Key]
        public int TutorDishId { set; get; }
        public int TutorId { set; get; }
        public string DishStory { set; get; }
        public string DishPicUrl { set; get; }
        public string DishName { set; get; }
        public int DishType { set; get; }
        public string CreatePerson { set; get; }
        public string UpdatePerson { set; get; }
        public DateTime CreateTime { set; get; }
        public DateTime UpdateTime { set; get; }
        public int VisitCount { set; get; }
        public int ShareCount { set; get; }
        public int PrasieCount { set; get; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    public class DishChef
    {
        public DishChef() {
            this.CreateTime = DateTime.Now;
            this.IsApply = 0;
            this.IsSelected = 0;
        }
        [Key]
        public int DishChefId { get; set; }
        [Required]
        public int ChefId { get; set; }
        
        public string DishStory { get; set; }
        
        public string DishPicUrl { get; set; }
       
        public string DishName { get; set; }
        [Required]
        public int DishType { get; set; }
        
        public DateTime CreateTime { get; set; }
       
        public DateTime UpdateTime { get; set; }
        [Required]
        //是否通过初步晒选  0没有，1，通过,2,待审核
        public int IsApply { get; set; }
        public string ApplyPerson { get; set; }
        public DateTime ApplyTime { get; set; }
        [Required]
        //是否入选菜品  0没有（默认值），1，通过，2，待审核
        public int IsSelected { get; set; }
        public string SelectedPerson { get; set; }
        public DateTime SelectedTime { get; set; }
        public int VisitCount { get; set; }
        public int PrasieCount { get; set; }
        public int SelectedCount { get; set; }
        public int ShareCount { get; set; }
    }
}
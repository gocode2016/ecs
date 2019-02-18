using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    /// <summary>
    /// 菜品步骤
    /// </summary>
    public class DishStep
    {
        public DishStep(){
            this.CreateTime = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public int DishId { get; set; }
         [Required]
        public int StepId { get; set; }
         [Required]
        public string StepName { get; set; }
        public string OpenId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
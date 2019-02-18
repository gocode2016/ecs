using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
   /// <summary>
   /// 培训交流
   /// </summary>
    public class CultivateInterflow
    {
        public CultivateInterflow(){
            this.CreateTime = DateTime.Now;
        }
        [Key]
        public int CuInterId { get; set; }
         [Required]
        public string CurriculumName { get; set; }
         [Required]
        public int CurriculumType { get; set; }
         [Required]
        public string Url { get; set; }
         [Required]
        public int Priority { get; set; }
         [Required]
        public int IsDisplay { get; set; }
         [Required]
        public string Introduce { get; set; }
         [Required]
        public int PraiseCount { get; set; }
        public int VisitCount { get; set; }
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
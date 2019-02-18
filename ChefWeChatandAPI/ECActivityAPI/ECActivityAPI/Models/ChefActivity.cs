using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    public class ChefActivity
    {
        public ChefActivity() 
        {
            this.CreateTime = DateTime.Now;
        }
        [Key]
        public int ChefActivityId { get; set; }
        
        public string ChefActivityName { get; set; }
       
        public string CreatePerson { get; set; }
       
        public string UpdatePerson { get; set; }
        
        public DateTime CreateTime { get; set; }
     
        public DateTime UpdateTime { get; set; }
       
        public int IsDel { get; set; }
       
        public int IsEnable { get; set; }
    }
}
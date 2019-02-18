using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    public class DishVisitLog
    {
        public DishVisitLog() 
        {
            this.CreateTime = DateTime.Now;
        }
        [Key]
        public int Id { set; get; }
        public int DishId { set; get; }
        public string OpenId { set; get; }
        public int DishType { set; get; }
        public DateTime CreateTime { set; get; }
    }
}
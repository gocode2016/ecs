using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    public class DishPrasieLog
    {
        public DishPrasieLog() {
            this.CreateTime = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }
        public int DishId { get; set; }
        public string OpenId { get; set; }
        public int DishType { set; get; }
        public DateTime CreateTime { get; set; }
    }
}
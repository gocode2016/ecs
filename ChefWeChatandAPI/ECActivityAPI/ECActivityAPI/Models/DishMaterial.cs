using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    /// <summary>
    /// 菜品调料
    /// </summary>
    public class DishMaterial
    {
        public DishMaterial() {
            this.CreateTime = DateTime.Now;
        }
        [Key]
        public int Id { set; get; }
        public int DishId { set; get; }
        public string Material { set; get; }
        public string Unit { get; set; }
        public int MaterialType { set; get; }
        public string OpenId { set; get; }
        public DateTime CreateTime { set;get; }
    }
}
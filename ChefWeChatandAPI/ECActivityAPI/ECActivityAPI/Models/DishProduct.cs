using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    /// <summary>
    /// 产品库菜品类
    /// </summary>
    public class DishProduct
    {
        public DishProduct() {
            this.CreateTime = DateTime.Now;
        }
        [Key]
        public int DishProductId { get; set; }
        public int ProductId { get; set; }
        public string DishStory { get; set; }
        public string DishPicUrl { get; set; }
        public string DishName { get; set; }
        public int DishType { get; set; }
        public string IsDisplay { get; set; }
        public int VisitCount { get; set; }
        public int ShareCount { get; set; }
        public int PrasieCount { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public string CreatePerson { get; set; }
        public string UpdatePerson { get; set; }
    }
}
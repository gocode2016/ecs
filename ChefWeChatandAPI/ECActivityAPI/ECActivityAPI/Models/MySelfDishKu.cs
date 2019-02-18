using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    /// <summary>
    /// 个人菜品库
    /// </summary>
    public class MySelfDishKu
    {
        [Key]
        public int MySelfDishId { get; set; }
        public int MemberId { get; set; }
        public string DishStory { get; set; }
        public string DishPicUrl { get; set; }
        public string DishName { get; set; }
        public string PhoneNum { get; set; }
        public int DishType { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpateTime { get; set; }
        public int IsApply { get; set; }
        public DateTime ApplyTime { get; set; }
        public int VisitCount { get; set; }
        public int PrasieCount { get; set; }
        public int ShareCount { get; set; }
    }
}
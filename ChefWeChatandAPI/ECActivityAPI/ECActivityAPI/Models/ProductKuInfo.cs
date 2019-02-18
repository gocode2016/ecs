using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    /// <summary>
    /// 产品库类
    /// </summary>
    public class ProductKuInfo
    {
        public ProductKuInfo() {
            this.CreateTime = DateTime.Now;
        }
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductFirstId { get; set; }
        public int ProductSecondId { get; set; }
        public string BrandName { get; set; }
        public string ProductBasicInfo { get; set; }
        public string ProductFeature { get; set; }
        public string ProductPicURL { get; set; }
        public string ProductValue { get; set; }
        public string CookingSkill { get; set; }
        public int IsDisplay { get; set; }
        public int PriorityId { get; set; }
        public string CreatePerson { get; set; }
        public string UpdatePerson { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }


    }
}
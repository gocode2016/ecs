using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    /// <summary>
    /// 产品库推荐调料类
    /// </summary>
    public class DishProductRF
    {
        public DishProductRF() {
            this.CreateTime = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }
        public int DishProductId { get; set; }
        public int SpecificationConfVCId { get; set; }
        public int SpecificationConfId { get; set; }
        public int ProductId { get; set; }
        public int VCId { get; set; }
        public int SpecificationId { get; set; }
        public string VCName { get; set; }
        public int Amount { get; set; }
        public string Unit { get; set; }
        public string ProductName { get; set; }
        public string Used { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
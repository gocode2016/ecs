using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CommonAPI.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ProductPrice { get; set; }
        public string ProductContent { get; set; }
        public string ImgUrl { get; set; }
        public string SaleProvince { get; set; }
        public DateTime? AddDate { get; set; }
        public int ProductType { get; set; }
        public int InventedType { get; set; }
        public int IsEnable { get; set; }
    }
}
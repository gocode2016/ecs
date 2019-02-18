using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntegralCardAPI.Models
{

    public class GoodsIntegral
    {
        [Key]
        public int GoodsIntegralId { get; set; }
        public int GoodsId { get; set; }
        public string GoodsCode { get; set; }
        public int Integral { get; set; }
        public DateTime CreateDate { get; set; }
        public string Remark { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    /// <summary>
    /// 生意贡献度临时表20180424
    /// </summary>
    public class BusinessContribution
    {
        [Key]
        public int Id { get; set; }
        public string GoodsName { get; set; }
        public double Price { get; set; }
        public string Unit { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
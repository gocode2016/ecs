using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CommonAPI.Models
{
    public class DrawPrize
    {
        [Key]
        public int DrawPrizeId { get; set; }
        public string DrawName { get; set; }
        public string PrizeName { get; set; }
        public int PrizeNum { get; set; }
        public string PrizeImg { get; set; }
        public string PrizeBatch { get; set; }
        public int IsDelete { get; set; }
        public int PrizeState { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
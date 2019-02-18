using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CommonAPI.Models
{
    /// <summary>
    /// 欣春赢家味抽奖奖品情况
    /// </summary>
    public class PrizeInfo
    {
        [Key]
        public int Id { get; set; }
        public int PrizeRank { get; set; }
        public string PrizeName { get; set; }
        public int Count { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CommonAPI.Models
{
    /// <summary>
    /// 欣春赢家味抽奖获奖情况
    /// </summary>
    public class PrizeInfoWinner
    {
        public PrizeInfoWinner() {
            this.CreateTime = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int PrizeRank { get; set; }
        public int PrizeProbabilityNum { get; set; }
        public DateTime CreateDay { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
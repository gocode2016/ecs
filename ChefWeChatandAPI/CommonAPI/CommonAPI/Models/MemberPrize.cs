using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CommonAPI.Models
{
    public class MemberPrize
    {
        [Key]
        public int MemberPrizeId { get; set; }
        public string PrizeBatch { get; set; }
        public int DrawPrizeId { get; set; }
        public int MemberId { get; set; }
        public int IsEnable { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegralCardAPI.Models
{
    public class RankingsViewModel
    {
        public RankingsViewModel()
        {
            LastGetTime = DateTime.Now;
        }

        public int MemberId { get; set; }
        public string HeadImgUrl { get; set; }
        public int IntegralNum { get; set; }
        public int Ranking { get; set; }
        public string MemberTelePhone { get; set; }
        public int? CityId { get; set; }
        public DateTime LastGetTime { get; set; }
    }
}
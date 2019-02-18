using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    public class Banner
    {
        [Key]
        public int BannerId { get; set; }

        public int ChefActivityId { get; set; }

        public int MatchRegionId { get; set; }
        public string PicUrl { get; set; }
        public string Link { get; set; }
        public int PriorityId { get; set; }
        public int IsDisplay { get; set; }
        public int IsDel { get; set; }
        public int BannerType { get; set; }

        public string CreatePerson { get; set; }
        public string UpdatePerson { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

    }
}
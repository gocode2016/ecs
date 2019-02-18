using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    public class RegistMember
    {
        [Key]
        public int MemberId { get; set; }

        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public string ImgUrl { get; set; }
        public string HotelAddress { get; set; }
        public string MemberName { get; set; }
        public string MemberTelePhone { get; set; }
        public string HotelName { get; set; }
        public string HotelCode { get; set; }
        public string Position { get; set; }
        public string PositionType { get; set; }
        public int RecommendId { get; set; }
        public int UpdateId { get; set; }
        public DateTime RegistDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int MemberState { get; set; }
        public int TotalIntegral { get; set; }
        public int LeaveIntegral { get; set; }
        public string Remark { get; set; }
        public string MemberCode { get; set; }
        public DateTime MemberCodeTime { get; set; }
        public int RoleId { get; set; }
        public int IsEnable { get; set; }
    }
}
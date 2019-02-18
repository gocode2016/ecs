using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    /// <summary>
    /// 会员积分详情
    /// </summary>
    public class MemberIntegralDetail
    {
        [Key]
        public int IntegralId { get; set; }
        public int MemberId { get; set; }
        public int IntegralNum { get; set; }
        public DateTime CreatDate { get; set; }
        public int IntegralType { get; set; }
        public string IntegralSource { get; set; }
        public DateTime InvalidDate { get; set; }
        public string Remark { get; set; }
        public string ScanMemberCode { get; set; }
        public string ScanHotelName { get; set; }
    }
}
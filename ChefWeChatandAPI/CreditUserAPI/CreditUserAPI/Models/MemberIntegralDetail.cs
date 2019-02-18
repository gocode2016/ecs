using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CreditUserAPI.Models
{
    /// <summary>
    /// 会员积分明细
    /// </summary>
    public class MemberIntegralDetail
    {
        [Key]
        public int IntegralId { get; set; }
        /// <summary>
        /// 会员id
        /// </summary>
        public int MemberId { get; set; }
        /// <summary>
        /// 积分数量
        /// </summary>
        public int IntegralNum { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatDate { get; set; }
        /// <summary>
        /// 操作类别 1为增加 2为减
        /// </summary>
        public int IntegralType { get; set; }
        /// <summary>
        /// 操作来源
        /// </summary>
        public string IntegralSource { get; set; }
        public DateTime? InvalidDate { get; set; }
        /// <summary>
        /// 操作备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 扫码酒店编码
        /// </summary>
        public string ScanMemberCode { get; set; }
        /// <summary>
        /// 扫码酒店名称
        /// </summary>
        public string ScanHotelName { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OutsideAPI.Models
{
    /// <summary>
    /// 会员表
    /// </summary>
    public class RegistMember
    {
        [Key]
        public int MemberId { get; set; }

        /// <summary>
        /// 酒店-省ID
        /// </summary>
        public int? ProvinceId { get; set; }
        /// <summary>
        /// 酒店-省名称
        /// </summary>
        public string ProvinceName { get; set; }
        /// <summary>
        /// 酒店-市ID
        /// </summary>
        public int? CityId { get; set; }
        /// <summary>
        /// 酒店-市名称
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// 酒店-区ID
        /// </summary>
        public int? AreaId { get; set; }
        /// <summary>
        /// 酒店-区名称
        /// </summary>
        public string AreaName { get; set; }
        /// <summary>
        /// 会员头像
        /// </summary>
        public string ImgUrl { get; set; }
        /// <summary>
        /// 酒店地址
        /// </summary>
        public string HotelAddress { get; set; }
        /// <summary>
        /// 会员名称
        /// </summary>
        public string MemberName { get; set; }
        /// <summary>
        /// 会员电话
        /// </summary>
        public string MemberTelePhone { get; set; }
        /// <summary>
        /// 酒店名称
        /// </summary>
        public string HotelName { get; set; }
        /// <summary>
        /// 酒店编码
        /// </summary>
        public string HotelCode { get; set; }
        /// <summary>
        /// 具体职业
        /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// 职业类型
        /// </summary>
        public string PositionType { get; set; }
        /// <summary>
        /// 所属队员ID 若为99则为实名认证注册
        /// </summary>
        public int? RecommendId { get; set; }
        /// <summary>
        /// 修改信息者ID
        /// </summary>
        public int? UpdateId { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? RegistDate { get; set; }
        /// <summary>
        /// 更新资料时间
        /// </summary>
        public DateTime? UpdateDate { get; set; }
        /// <summary>
        /// 会员状态
        /// </summary>
        [DefaultValue(0)]
        public int MemberState { get; set; }
        /// <summary>
        /// 会员总积分
        /// </summary>
        [DefaultValue(0)]
        public int? TotalIntegral { get; set; }
        /// <summary>
        /// 会员剩余积分
        /// </summary>
        [DefaultValue(0)]
        public int LeaveIntegral { get; set; }
        /// <summary>
        /// 备注 及注册来源
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 客户编码
        /// </summary>
        public string MemberCode { get; set; }
        /// <summary>
        /// 完善客户编码时间
        /// </summary>
        public DateTime? MemberCodeTime { get; set; }
        /// <summary>
        /// 角色ID 已不用
        /// </summary>
        [DefaultValue(1)]
        public int? RoleId { get; set; }
        /// <summary>
        /// 推荐人ID
        /// </summary>
        public int? MemberRecId { get; set; }
        /// <summary>
        /// 是否启禁用
        /// </summary>
        [DefaultValue(0)]
        public int? IsEnable { get; set; }
    }
}
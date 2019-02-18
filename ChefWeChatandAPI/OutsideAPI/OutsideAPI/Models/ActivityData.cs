using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OutsideAPI.Models
{
    public class ActivityData
    {
        /// <summary>
        /// 微信openid
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 岗位类型
        /// </summary>
        public string PositionType { get; set; }
        /// <summary>
        /// 岗位
        /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// 酒店或者门店名称
        /// </summary>
        public string HotelOrMdName { get; set; }
        /// <summary>
        /// 酒店或者门店地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 微信昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 微信头像url
        /// </summary>
        public string HeadImageUrl { get; set; }
        /// <summary>
        /// 第三方抽奖所得奖品 如六月鲜200ml
        /// </summary>
        public string PrizeName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ProvinceId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ProvinceName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int CityId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int AreaId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AreaName { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public string AddDate { get; set; }
    }
}
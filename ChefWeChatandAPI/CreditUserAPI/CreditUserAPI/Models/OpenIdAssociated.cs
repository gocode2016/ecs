using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CreditUserAPI.Models
{
    /// <summary>
    /// 微信OpenID表
    /// </summary>
    public class OpenIdAssociated
    {
        [Key]
        public int AssociatedId { get; set; }
        /// <summary>
        /// 微信唯一ID  OPENID
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 微信昵称
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 微信头像
        /// </summary>
        public string HeadImgUrl { get; set; }
        /// <summary>
        /// 会员ID / 队员ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 用户类别1为队员 2位会员
        /// </summary>
        public int UserType { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CreditUserAPI.Models
{
    /// <summary>
    /// 会员实名认证
    /// </summary>
    public class MemberRealAuth
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MemberId { get; set; }
        /// <summary>
        /// 认证资料图片1
        /// </summary>
        public string AuthImg1 { get; set; }
        /// <summary>
        /// 认证资料图片2
        /// </summary>
        public string AuthImg2 { get; set; }
        /// <summary>
        /// 认证状态 0为未认证 1为已认证 -1为不通过
        /// </summary>
        public int? AuthState { get; set; }
        /// <summary>
        /// 审核备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 认证时间
        /// </summary>
        public DateTime? AuthTime { get; set; }
    }
}
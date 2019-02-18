using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CreditUserAPI.Models
{
    /// <summary>
    /// 注册码
    /// </summary>
    public class RegistCode
    {
        /// <summary>
        /// 注册码
        /// </summary>
        [Key]
        public int RegistCodeId { get; set; }
        /// <summary>
        /// 队员ID
        /// </summary>
        public int SalemanId { get; set; }
        /// <summary>
        /// 会员ID  0为注册码未使用
        /// </summary>
        [DefaultValue(0)]
        public int MemberId { get; set; }
        /// <summary>
        /// 注册码状态 0为未使用 1为已使用 -1为已转移
        /// </summary>
        public int RegistCodeState { get; set; }
        /// <summary>
        /// 生成时间
        /// </summary>
        public DateTime GenerDate { get; set; }
        /// <summary>
        /// 使用时间
        /// </summary>
        public DateTime? UseDate { get; set; }
    }
}
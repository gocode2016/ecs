using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CreditUserAPI.Models
{
    /// <summary>
    /// 批量转移会员
    /// </summary>
    public class TransferModel
    {
        /// <summary>
        /// 会员id
        /// </summary>
        public int MemberId { get; set; }
        /// <summary>
        /// 旧队员ID
        /// </summary>
        public int OldSalemanId { get; set; }
        /// <summary>
        /// 新队员ID
        /// </summary>
        public int NewSalemanId { get; set; }
    }
}
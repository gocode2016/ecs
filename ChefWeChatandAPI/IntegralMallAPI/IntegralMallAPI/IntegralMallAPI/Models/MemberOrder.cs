using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntegralMallAPI.Models
{
    /// <summary>
    /// 会员订单
    /// </summary>
    public class MemberOrder
    {
        [Key]
        public int OrderId { get; set; }
        /// <summary>
        /// 会员ID
        /// </summary>
        public int MemberId { get; set; }
        /// <summary>
        /// 订单地址
        /// </summary>
        public string OrderAddress { get; set; }
        /// <summary>
        /// 下单人员名称
        /// </summary>
        public string OrderName { get; set; }
        /// <summary>
        /// 订单电话
        /// </summary>
        public string OrderTelephone { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public string OrderState { get; set; }
        /// <summary>
        /// 订单价格
        /// </summary>
        public int OrderPrice { get; set; }
        /// <summary>
        /// 订单类型1为实物  2为虚拟
        /// </summary>
        public int OrderType { get; set; }
        /// <summary>
        /// 虚拟接口类型 1为欧飞充值
        /// </summary>
        public int InventedType { get; set; }
        /// <summary>
        /// 物流单号
        /// </summary>
        public string LogisticsNo { get; set; }
        /// <summary>
        /// 物流公司类别
        /// </summary>
        public string LogisticsType { get; set; }
        /// <summary>
        /// 订单来源
        /// </summary>
        public string OrderFrom { get; set; }
        /// <summary>
        /// 订单备注
        /// </summary>
        public string OrderRemark { get; set; }
        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime AddDate { get; set; }

    }
}
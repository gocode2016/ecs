using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegralMallAPI.Models
{
    public class RequestOrderInfo
    {
        public int MemberId { get; set; }
        public string OrderAddress { get; set; }
        public string OrderName { get; set; }
        public string OrderTelephone { get; set; }
        /// <summary>
        /// 订单状态 备货中 已取消 未提交 已发货
        /// </summary>
        public string OrderState { get; set; }
        public int OrderPrice { get; set; }
        /// <summary>
        /// 订单类型 1实物, 2虚拟商品
        /// </summary>
        public int OrderType { get; set; }
        /// <summary>
        /// 是否是虚拟商品  1是虚拟商品 如话费,0是实物商品如味极鲜
        /// </summary>
        public int InventedType { get; set; }
        public string OrderFrom { get; set; }
        public string OrderRemark { get; set; }
        public List<OrderDetaile> OrderDetaile { get; set; }
    }
}
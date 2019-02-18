using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CommonAPI.Models
{
    /// <summary>
    /// 欣春赢家味抽奖从这里没有注册的会员是否去注册
    /// </summary>
    public class PrizeOpenIdHomeTasteDraw
    {
        public PrizeOpenIdHomeTasteDraw() {
            this.CreateTime = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }
        public string OpenId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
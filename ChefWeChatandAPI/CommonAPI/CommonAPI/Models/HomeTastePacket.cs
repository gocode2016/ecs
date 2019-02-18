using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CommonAPI.Models
{
    /// <summary>
    /// 201802月份随机红包类
    /// </summary>
    public class HomeTastePacket
    {
        public HomeTastePacket() {
            this.CreateTime = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string OpenId { get; set; }
        public int RedPacket { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    /// <summary>
    /// 个人菜品库
    /// </summary>
    public class MySelfFRRole
    {
        public MySelfFRRole()
        {
            this.CreateTime=DateTime.Now;
        }
        [Key]
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int MySelfDishKuId { get; set; }
        public int MySelfFRId { get; set; }
        public string Unit { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
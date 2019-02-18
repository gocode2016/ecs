using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    /// <summary>
    /// 培训交流留言
    /// </summary>
    public class CultivateComment
    {
        public CultivateComment() {
            this.CreateTime = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }
        public int CuInterId { get; set; }
        public string OpenId { get; set; }
        public string HeadPicUrl { get; set; }
        public string NickName { get; set; }
        public string Comment { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
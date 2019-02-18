using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    /// <summary>
    /// 星厨留言
    /// </summary>
    public class ChefStarComment
    {
        public ChefStarComment()
        {
            this.CreateTime = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public int ChefStarId { get; set; }
        [Required]
        public string OpenId { get; set; }
        [Required]
        public string MemberName { get; set; }
        [Required]
        public string HeadPicUrl { get; set; }
        [Required]
        public string Comment { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
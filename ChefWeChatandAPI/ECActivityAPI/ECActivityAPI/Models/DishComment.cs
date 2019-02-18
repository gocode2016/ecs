using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    public class DishComment
    {
        public DishComment() {
            this.CreateTime = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public int DishId { get; set; }
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
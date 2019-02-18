using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    /// <summary>
    /// 讲师
    /// </summary>
    public class Lecturer
    {
        public Lecturer() {
            this.CreateTime = DateTime.Now;
        }
        [Key]
        public int LecturerId { get; set; }
        [Required]
        public int CuInterId { get; set; }
        [Required]
        public string HeadPicUrl { get; set; }
        [Required]
        public string LecturerName { get; set; }
        [Required]
        public string Post { get; set; }
        [Required]
        public string HotelName { get; set; }
        [Required]
        public DateTime CreateTime { get; set; }
    }
}
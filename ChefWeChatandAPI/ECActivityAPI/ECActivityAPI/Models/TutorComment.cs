using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    public class TutorComment
    {
        [Key]
        public int TutorCommentId { get; set; }
        public int TutorId { get; set; }
        public string OpenId { get; set; }
        public string MemberName { get; set; }
        public string HeadPicUrl { get; set; }
        public string Comment { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
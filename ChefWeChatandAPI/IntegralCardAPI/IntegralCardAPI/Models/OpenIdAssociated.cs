using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntegralCardAPI.Models
{
    public class OpenIdAssociated
    {
        [Key]
        public int AssociatedId { get; set; }
        public string OpenId { get; set; }
        public string Nickname { get; set; }
        public string HeadImgUrl { get; set; }
        public int UserId { get; set; }
        public int UserType { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
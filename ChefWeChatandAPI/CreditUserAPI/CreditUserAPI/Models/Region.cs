using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CreditUserAPI.Models
{
    /// <summary>
    /// 队员大区
    /// </summary>
    public class Region
    {
        /// <summary>
        /// 大区ID
        /// </summary>
        [Key]
        public int RegionId { get; set; }
        /// <summary>
        /// 大区名称
        /// </summary>
        public string RegionName { get; set; }
        public DateTime? CreateDate { get; set; }

    }
}
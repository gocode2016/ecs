using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntegralMallAPI.Models
{
    /// <summary>
    /// 商城类目
    /// </summary>
    public class Category
    {
        /// <summary>
        /// 类目ID
        /// </summary>
        [Key]
        public int CategoryId { get; set; }
        /// <summary>
        /// 类目名称
        /// </summary>
        public string CategoryName { get; set; }
        public int ParentId { get; set; }
    }
}
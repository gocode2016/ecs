using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Models
{
    /// <summary>
    /// 培训交流攒
    /// </summary>
    public class CultivatePraise
    {
        public CultivatePraise()
        {
            this.CreateTime=DateTime.Now;
        }
        [Key]
        public int Id { get; set; }
        
        public int CuInterId { get; set; }
        public string OpenId { get; set; }
        public DateTime CreateTime { get; set; }

    }
    public class CultivateVisit 
    {
        public CultivateVisit()
        {
            this.CreateTime=DateTime.Now;
        }
        [Key]
        public int Id { get; set; }
        
        public int CuInterId { get; set; }
        public string OpenId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
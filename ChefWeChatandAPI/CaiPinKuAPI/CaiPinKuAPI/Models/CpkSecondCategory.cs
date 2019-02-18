using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CaiPinKuAPI.Models
{
    public class CpkSecondCategory
    {
        public CpkSecondCategory()
        {
            this.CreateDate = DateTime.Now;
            this.IsEnable = 1;
        }
        [Key]
         public int SecondId { get; set; }
         public string SecondName { get; set; }
         public int FirstId { get; set; }
         public int IsPublish { get; set; }
         public int IsEnable { get; set; }
         public DateTime? CreateDate { get; set; }
         public string Remark { get; set; }
    }
}
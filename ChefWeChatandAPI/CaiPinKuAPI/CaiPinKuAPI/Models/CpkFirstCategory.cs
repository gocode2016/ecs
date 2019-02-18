using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System;

namespace CaiPinKuAPI.Models
{
    public class CpkFirstCategory
    {
        public CpkFirstCategory()
        {
            this.CreateDate = DateTime.Now;
        }

        [Key]
        public int FirstId { get; set; }
        public string FirstName { get; set; }
        public string Remark { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
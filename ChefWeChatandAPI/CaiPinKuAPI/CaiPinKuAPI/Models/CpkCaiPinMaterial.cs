using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System;

namespace CaiPinKuAPI.Models
{
    /// <summary>
    /// 菜品调料
    /// </summary>
    public class CpkCaiPinMaterial
    {
        public CpkCaiPinMaterial()
        {
            this.CreateDate = DateTime.Now;
            this.ProductId = 0;
            this.ParentId = 0;
        }
        [Key]
        public int MaterialId { set; get; }
        public int CaiPinId { set; get; }
        public string Material { set; get; }
        public int ProductId { set; get; }
        public string Amount { get; set; }
        public string Unit { get; set; }
        public int MaterialType { set; get; }
        public int ParentId { set; get; }
        public DateTime CreateDate { set; get; }
        public string Remark { get; set; }
    }
}
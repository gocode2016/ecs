using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaiPinKuAPI.Models
{
    public class RZhuanTi
    {
        public int ZhuanTiId { get; set; }
        public string ZhuanTiName { get; set; }
        public string ZhuanTiUrl { get; set; }
        public int ZtContentId { get; set; }
        public string Content { get; set; }
        public object CaiPinList { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaiPinKuAPI.Models
{
    public class RCpkCaiPinStep1
    {
        public int CaiPinId { get; set; }
        public string CaiPinName { get; set; }
        public string VideoUrl { get; set; }
        public string VideoImage { get; set; }
        public string Images { get; set; }
        public object CategoryList { get; set; }
        public object BanKuai { get; set; }
    }
    public class RCpkCaterory
    {
        public int FirstId { get; set; }
        public string FirstName { get; set; }
        public object SecondList { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaiPinKuAPI.Models
{
    public class RequestSaleManModel
    {
        public int IndexPage { get; set; }
        public int PageSize { get; set; }
        public int RegionId { get; set; }
        public int RegistState { get; set; }
        public string SearchInfo { get; set; }
        public DateTime? RegistBeginTime { get; set; }
        public DateTime? RegistEndTime { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegralMallAPI.Models
{
    /// <summary>
    /// 物流信息实体类，来源第三方
    /// </summary>
    public class PostalDeliveryViewModel
    {
        public string message { get; set; }
        public string nu { get; set; }
        public string ischeck { get; set; }
        public string condition { get; set; }
        public string com { get; set; }
        public string status { get; set; }
        public string state { get; set; }
        public List<LogisticsDetailViewModel> data { get; set; }
    }
    public class LogisticsDetailViewModel
    {
        public DateTime time { get; set; }
        public DateTime ftime { get; set; }
        public string context { get; set; }
        public string location { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaiPinKuAPI.Models
{
    public class ReturnJson
    {
        public ReturnJson()
        {
            this.status = "fail";
            this.totalpage = 0;
            this.totalcount = 0;
            this.code = 200;
        }
        public int totalpage {get; set;}
        public int totalcount {get; set;}
        public int code { get; set; }
        public string status { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }

    public class RJson
    {
        public RJson()
        {
            this.result_status = "fail";
            this.return_code = 200;
        }
        public int return_code { get; set; }
        public string result_status { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }
}
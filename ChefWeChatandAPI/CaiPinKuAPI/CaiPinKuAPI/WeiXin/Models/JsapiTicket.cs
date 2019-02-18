using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaiPinKuAPI.WeiXin.Models
{
    public class JsapiTicket : ErrorMsg
    {
        public string ticket { get; set; }
        public int expires_in { get; set; }
    }
}

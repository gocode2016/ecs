﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreditUserAPI.WeiXin.Models
{
    public class UserInfo : ErrorMsg
    {
        public string openid { get; set; }
        public string nickname { get; set; }
        public string sex { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string headimgurl { get; set; }
        public string[] privilege { get; set; }
        public string unionid { get; set; }

    }



}

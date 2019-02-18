using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CreditUserAPI.Models
{
    public class RequestMemberBySaleman
    {
        public int IndexPage { get; set; }

        public int PageSize { get; set; }

        public int SalemanId { get; set; }

        public int MemberState { get; set; }

        public string MemberName { get; set; }

        public int IntegralOB { get; set; }
        
        public int ActiveOB { get; set; }
    }
}
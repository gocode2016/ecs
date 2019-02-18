using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CreditUserAPI.Models
{
    public class RequestMemberData
    {
        public int IndexPage { get; set; }
        public int PageSize { get; set; }
        public int Identity { get; set; }
        public int? SalemanId { get; set; }
        public int CodeState { get; set; }
        public string SearchInfo { get; set; }
        public int? AuthState { get; set; }
        public DateTime? RegistBeginTime { get; set; }
        public DateTime? RegistEndTime { get; set; }

    }
}
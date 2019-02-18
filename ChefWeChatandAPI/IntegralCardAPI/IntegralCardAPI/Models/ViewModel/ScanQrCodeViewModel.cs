using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegralCardAPI.Models
{
    public class ScanQrCodeViewModel
    {
        public bool IsSuccess { get; set; }
        public string Msg { get; set; }
        public int RiseIntegral { get; set; }
        public int Rank { get; set; }
        public int RiseRank { get; set; }
        public string ReturnUrl { get; set; }
        public int TotalIntegral { get; set; }
        public int RowNumber { get; set; }
    }
}
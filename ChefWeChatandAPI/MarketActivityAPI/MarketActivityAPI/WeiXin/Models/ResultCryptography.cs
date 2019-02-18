using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketActivityAPI.WeiXin.Models
{
    public class ResultCryptography
    {
        public string encrypttype { get; set; }
        public string timestamp { get; set; }
        public string nonce { get; set; }

        public string sToken { get; set; }
        public string sAppID { get; set; }
        public string sEncodingAESKey { get; set; }

        public bool HasCrypto()
        {
            if (!string.IsNullOrEmpty(encrypttype))
            {
                if (encrypttype.ToLower() == "aes")
                {
                    return true;
                }
            }
            return false;
        }
    }
}

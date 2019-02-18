using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaiPinKuAPI.Common
{
    public class EncryptHelper
    {
        /// <summary>
        /// 混淆字符串
        /// </summary>
        /// <param name="encryptString">混淆字符串</param>
        public static string Confusion(string content)
        {
            //混淆方法将字符串两段a和b，反转a和b的顺序，再返回b+a
            try
            {
                if (content.Length == 18)
                {
                    var strPrev = content.Substring(0, 9);
                    var strAfter = content.Substring(9);
                    var strNew = new string(strAfter.Reverse().ToArray()) + new string(strPrev.Reverse().ToArray());
                    return strNew;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;


namespace WxScanAPI.Common
{
    public static class StringOperate
    {
        public static string Str2MD5(string sStr)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sStr, "MD5");
        }
        /// <summary>
        /// 32位MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Md5Hash(string input)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString().ToUpper();
        }



        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRandomString(int length)
        {
            const string key = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            if (length < 1)
                return string.Empty;

            Random rnd = new Random();
            byte[] buffer = new byte[8];

            ulong bit = 31;
            ulong result = 0;
            int index = 0;
            StringBuilder sb = new StringBuilder((length / 5 + 1) * 5);

            while (sb.Length < length)
            {
                rnd.NextBytes(buffer);

                buffer[5] = buffer[6] = buffer[7] = 0x00;
                result = BitConverter.ToUInt64(buffer, 0);

                while (result > 0 && sb.Length < length)
                {
                    index = (int)(bit & result);
                    sb.Append(key[index]);
                    result = result >> 5;
                }
            }
            return sb.ToString();
        }
        public static string MakeSign(IDictionary<string, string> para, string apiKey)
        {
                        //如果需要POST数据       
            if (!(para == null || para.Count == 0))
            {
                para.Add("key", apiKey);
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in para.Keys)
                {
                    if (i > 0)
                    {
                        if (!string.IsNullOrEmpty(para[key]))
                        {
                            buffer.AppendFormat("&{0}={1}", key, para[key]);
                            continue;
                        }
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, para[key]);
                    }
                    i++;
                }
                return buffer.ToString();
            }
            else
            {
                return "";
            }
        }
    }
}
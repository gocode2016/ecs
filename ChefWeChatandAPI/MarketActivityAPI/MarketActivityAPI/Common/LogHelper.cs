using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MarketActivityAPI.Common
{

    public class LogHelper
    {
        public static string path =System.Web.Hosting.HostingEnvironment.MapPath("~/") + "log.txt";

        /// <summary>
        /// 写日志(用于跟踪)
        /// </summary>
        public static void WriteLog(string strMemo,string filename="")
        {
            if (string.IsNullOrEmpty(filename))
            {
                filename = path;
            }
            StreamWriter sr = null;
            try
            {
                if (!File.Exists(filename))
                {
                    sr = File.CreateText(filename);
                }
                else
                {
                    sr = File.AppendText(filename);
                }

                strMemo = "\r\n--------------" + DateTime.Now + "----------------\r\n" + strMemo;
                sr.WriteLine(strMemo);
            }
            catch
            {
            }
            finally
            {
                if (sr != null)
                    sr.Close();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CaiPinKuAPI.Common
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


        /// 日志文件记录
        /// </summary>
        /// <param name="msg">写入信息</param>
        public static void WriteMsgByDay(string msg)
        {
            try
            {
                //string path = Directory.GetCurrentDirectory()+"\\log";
                string path = System.AppDomain.CurrentDomain.BaseDirectory + "\\log";
                if (!Directory.Exists(path))//判断是否有该文件
                    Directory.CreateDirectory(path);
                string logFileName = path + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";//生成日志文件
                if (!File.Exists(logFileName))//判断日志文件是否为当天
                {
                    FileStream fs = new FileStream(logFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        writer.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "" + msg);
                        writer.Flush();
                        writer.Close();
                    }
                }
                else
                {
                    FileStream fs = new FileStream(logFileName, FileMode.Append, FileAccess.Write, FileShare.Write);
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        writer.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "" + msg);
                        writer.Flush();
                        writer.Close();
                    }
                }
            }
            catch (Exception e)
            {
                string path = System.AppDomain.CurrentDomain.BaseDirectory + "\\log";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                string logFileName = path + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
                if (!File.Exists(logFileName))
                    File.Create(logFileName);
                StreamWriter writer = File.AppendText(logFileName);
                writer.WriteLine(DateTime.Now.ToString("日志记录错误HH:mm:ss") + " " + e.Message + " " + msg);
                writer.Flush();
                writer.Close();
            }
        }
    }
}

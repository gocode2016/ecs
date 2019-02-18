using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace WxScanAPI.Common
{
    public class QrCodeHelper
    {
         /// <summary>
        /// 生成二维码并返回地址
        /// </summary>
        /// <param name="qrcontent">生成二维码的内容</param>   
        /// <param name="modulesize">二维码黑白线条的宽度</param>
        /// <returns>二维码字节数组</returns>   
        public static string ToUrl(string content, int modulesize = 8)
        {
            var basePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var qrCodePath = Convert.ToString(ConfigurationManager.AppSettings["QrCodeStorePath"]);            
            var fileName = string.Format("{0}.png", Guid.NewGuid().ToString());
            var serverPath = qrCodePath + fileName;
            var filePath = basePath + serverPath;
            var savePath = basePath + qrCodePath;
            var qrCode = Renderer(content, Brushes.Black, Brushes.White, modulesize);
            //如果目录不存在则创建 
            if (!System.IO.Directory.Exists(savePath))
            {
                System.IO.Directory.CreateDirectory(savePath);
            }
            using (FileStream file = new FileStream(filePath, FileMode.Create))
            {
                try
                {
                    file.Write(qrCode, 0, qrCode.Length);
                }
                catch (Exception)
                {
                    serverPath = string.Empty;
                }
            }
            return serverPath;
        }
        /// <summary>
        /// 生成二维码并返回地址
        /// </summary>
        /// <param name="qrcontent">生成二维码的内容</param>   
        /// <param name="modulesize">二维码黑白线条的宽度</param>
        /// <returns>二维码字节数组</returns>   
        public static byte[] ToBytes(string content, int modulesize = 8)
        {
            var qrCode = Renderer(content, Brushes.Black, Brushes.White, modulesize);
            return qrCode;
        }
        /// <summary>
        /// 生成二维码并返回地址
        /// </summary>
        /// <param name="qrcontent">生成二维码的内容</param>   
        /// <param name="modulesize">二维码黑白线条的宽度</param>
        /// <returns>二维码字节数组</returns>   
        public static byte[] ToBytes(string content, string logoPath, int modulesize = 8)
        {
            Bitmap bitmap = new Bitmap(logoPath);
            var qrCode = Renderer(content, Brushes.Black, Brushes.White, modulesize, logo: bitmap);
            return qrCode;
        }
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="qrcontent">生成二维码的内容</param>
        /// <param name="darkbrush">二维码线条的颜色</param>
        /// <param name="lightbrush">二维码线条空白的颜色</param>
        /// <param name="modulesize">二维码图片的大小（1=41px）</param>
        /// <param name="quietzonemodules">二维码外围空白</param>
        /// <returns>二维码字节数组</returns>   
        public static byte[] Renderer(
            string content, 
            Brush darkbrush, 
            Brush lightbrush, 
            int modulesize = 8,
            QuietZoneModules quietzonemodules = QuietZoneModules.Two, 
            Bitmap logo = null)
        {            
            //创建二维码生成类，ErrorCorrectionLevel为容错级别
            QrEncoder encoder = new QrEncoder(ErrorCorrectionLevel.H);
            QrCode qrCode;
            //生成二维码            
            encoder.TryEncode(content, out qrCode); 
            //FixedModuleSize参数1:二维码黑白线条的宽度
            //FixedModuleSize参数2，QuietZoneModules:二维码外围空白
            GraphicsRenderer render = new GraphicsRenderer(
                new FixedModuleSize(modulesize, QuietZoneModules.Zero),
                darkbrush, lightbrush);
            //将二维码写入到内存中
            byte[] byteQrCode = new byte[] { };
            using (MemoryStream ms = new MemoryStream())
            {
                render.WriteToStream(qrCode.Matrix, ImageFormat.Png, ms);                
                if (logo != null)
                {
                    var qrCodeImage = new Bitmap((Image)new Bitmap(ms));
                    using (var g = Graphics.FromImage(qrCodeImage))
                    {
                        //g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高); 
                        g.DrawImage(qrCodeImage, 0, 0, qrCodeImage.Width, qrCodeImage.Height);
                        //g.FillRectangle(System.Drawing.Brushes.White, imgBack.Width / 2 - img.Width / 2 - 1, imgBack.Width / 2 - img.Width / 2 - 1,1,1);//相片四周刷一层黑色边框  
                        g.DrawImage(logo, qrCodeImage.Width / 2 - logo.Width / 2, qrCodeImage.Width / 2 - logo.Width / 2, logo.Width, logo.Height);
                        GC.Collect();
                        using (MemoryStream msNew = new MemoryStream()) 
                        {                                                        
                            qrCodeImage.Save(msNew, ImageFormat.Png);
                            byteQrCode = msNew.ToArray();
                        }                  
                    }
                }
                else
                {
                    byteQrCode = ms.ToArray();
                }
            }
            return byteQrCode;
        }
    }
    
}
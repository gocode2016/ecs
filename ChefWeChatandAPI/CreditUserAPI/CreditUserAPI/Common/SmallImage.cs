using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;

namespace CreditUserAPI.Common
{
    public class SmallImage
    {
        /// <summary>
        /// 得到缩略图，指定像素
        /// </summary>
        /// <param name="fileName">源文件路径</param>
        /// <param name="newFile">新文件路径</param>
        /// <param name="maxHeight">缩略图高</param>
        /// <param name="maxWidth">缩略图宽</param>
        /// <param name="qualityNum">图片质量1-100</param>
        /// <param name="mode">缩放模式，CUT裁剪不失真，HW定宽高有可能变形，W定宽，H定高</param>
        public static void SendSmallImage(string fileName, string newFile, int maxHeight, int maxWidth, long qualityNum, string mode)
        {
            Image img = Image.FromFile(fileName);
            System.Drawing.Imaging.ImageFormat thisFormat = img.RawFormat;
            int towidth = maxWidth;
            int toheight = maxHeight;
            int x = 0;
            int y = 0;
            int ow = img.Width;
            int oh = img.Height;
            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）                
                    break;
                case "W"://指定宽，高按比例                    
                    toheight = img.Height * maxWidth / img.Width;
                    break;
                case "H"://指定高，宽按比例
                    towidth = img.Width * maxHeight / img.Height;
                    break;
                case "CUT"://指定高宽裁减（不变形）                
                    if ((double)img.Width / (double)img.Height > (double)towidth / (double)toheight)
                    {
                        oh = img.Height;
                        ow = img.Height * towidth / toheight;
                        y = 0;
                        x = (img.Width - ow) / 2;
                    }
                    else
                    {
                        ow = img.Width;
                        oh = img.Width * maxHeight / towidth;
                        x = 0;
                        y = (img.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }
            Bitmap outBmp = new Bitmap(towidth, toheight);
            Graphics g = Graphics.FromImage(outBmp);
            // 设置画布的描绘质量
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(img, new Rectangle(0, 0, towidth, toheight), x, y, ow, oh, GraphicsUnit.Pixel);
            g.Dispose();
            // 以下代码为保存图片时,设置压缩质量
            EncoderParameters encoderParams = new EncoderParameters();
            long[] quality = new long[1];
            quality[0] = qualityNum;//图片质量1-100
            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;
            //获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象.
            ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo jpegICI = null;
            for (int index = 0; index < arrayICI.Length; index++)
            {
                if (arrayICI[index].FormatDescription.Equals("JPEG"))
                {
                    jpegICI = arrayICI[index];
                    //设置JPEG编码
                    break;
                }
            }
            if (jpegICI != null)
            {
                outBmp.Save(newFile, jpegICI, encoderParams);
            }
            else
            {
                outBmp.Save(newFile, thisFormat);
            }
            img.Dispose();
            outBmp.Dispose();
        }
    }
}
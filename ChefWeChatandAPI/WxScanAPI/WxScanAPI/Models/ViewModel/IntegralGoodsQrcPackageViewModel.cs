using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WxScanAPI.Models
{
    public enum PackageSource
    {
        欣和企业 = 10,
        味达美工厂 = 11,
        神州1工厂 = 12,
        蓬莱工厂 = 13,
        济南欣昌工厂 = 14,
        济南宜和 = 15,
        味达美开发区工厂 = 16,
        烟台欣尚 = 17
    }
    public enum PackageMarch
    {
        一月 = 1,
        二月 = 2,
        三月 = 3,
        四月 = 4,
        五月 = 5,
        六月 = 6,
        七月 = 7,
        八月 = 8,
        九月 = 9,
        十月 = 10,
        十一月 = 11,
        十二月 = 12
    }
    public enum PackageState
    {
        已完成 = 1,
        未完成 = 2
    }
    public enum QrCodeState
    {
        未使用 = 1,
        已使用 = 2
    }
    public class IntegralGoodsQrcPackageViewModel
    {
        [Column]
        public int PackageId { get; set; }
        [Column]
        public string Name { get; set; }
        [Column]
        public int GoodsId { get; set; }
        [Column]
        public string Url { get; set; }
        [Column]
        public int Range { get; set; }
        [Column]
        public string GoodsCode { get; set; }
        [Column]
        public PackageSource Source { get; set; }
        [Column]
        public PackageMarch March { get; set; }
        [Column]
        public int Num { get; set; }
        [Column]
        public PackageState State { get; set; }
        [Column]
        public string Remark { get; set; }
        [Column]
        public string DownLoadUrl { get; set; }
        [Column]
        public DateTime CreateDate { get; set; }
        public static IDictionary<int, string> PackageRange()
        {
            IDictionary<int, string> dic = new Dictionary<int, string>();
            dic.Add(10, "160ml");
            dic.Add(15, "460ml");
            dic.Add(16, "500ml");

            dic.Add(20, "1L");
            dic.Add(23, "1.78L");
            dic.Add(24, "1.8L");
            dic.Add(26, "2L");
            dic.Add(28, "3.78L");

            dic.Add(41, "600g");
            dic.Add(42, "605g");
            dic.Add(43, "630g");
            dic.Add(46, "2.2kg");
            dic.Add(47, "2.3kg");
            dic.Add(60, "5kg");
            dic.Add(62, "6.18kg");
            return dic;
        }
    }
}
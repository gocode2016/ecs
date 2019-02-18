using CommonAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Amazon.S3;
using WebGrease;

using CommonAPI.Common;

namespace CommonAPI.Controllers
{
    public class ImageController : ApiController
    {

        private CreditContext db = new CreditContext();

        #region 后台图片上传
        /// <summary>
        /// 图片上传API
        /// </summary>
        /// <param name="img_type">
        /// 图片类型：
        /// img_type = "Member"   用户类
        /// img_type = "Product"  商城类
        /// img_type = "Cook"     菜品类
        /// img_type = "Activity" 活动类
        /// </param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ImgUpload(string img_type)
        {
            string ROOT_PATH = string.Empty;
            string ROOT_PATH2 = string.Empty;

            switch (img_type)
            {
                case "CaiPinKu":
                    ROOT_PATH = HttpContext.Current.Server.MapPath("~/UploadFiles/CaiPinKu");
                    break;
                case "Member":
                    ROOT_PATH = HttpContext.Current.Server.MapPath("~/UploadFiles/Member");
                    break;
                case "Product":
                    ROOT_PATH = HttpContext.Current.Server.MapPath("~/UploadFiles/Product");
                    ROOT_PATH2 = HttpContext.Current.Server.MapPath("~/UploadFiles/SmallProduct");
                    break;
                case "Cook":
                    ROOT_PATH = HttpContext.Current.Server.MapPath("~/UploadFiles/Cook");
                    break;
                case "Activity":
                    ROOT_PATH = HttpContext.Current.Server.MapPath("~/UploadFiles/Activity");
                    break;
                case "Tools":
                    ROOT_PATH = HttpContext.Current.Server.MapPath("~/UploadFiles/Tools");
                    break;
                case "Produce":
                    ROOT_PATH = HttpContext.Current.Server.MapPath("~/UploadFiles/Produce");
                    break;
                default:
                    return "Request Error";
            }

            //ROOT_PATH = HttpContext.Current.Server.MapPath("~/UploadFiles/");

            List<Resource> resources = new List<Resource>();
            // 采用MultipartMemoryStreamProvider
            var provider = new MultipartMemoryStreamProvider();

            //读取文件数据
            await Request.Content.ReadAsMultipartAsync(provider);

            foreach (var item in provider.Contents)
            {
                // 判断是否是文件
                if (item.Headers.ContentDisposition.FileName != null)
                {
                    //获取到流
                    var ms = item.ReadAsStreamAsync().Result;
                    //进行流操作 
                    using (var br = new BinaryReader(ms))
                    {
                        if (ms.Length <= 0)
                            break;
                        //读取文件内容到内存中
                        var data = br.ReadBytes((int)ms.Length);
                        //Create
                        //当前时间作为ID
                        Resource resource = new Resource() { Id = DateTime.Now.ToString("yyyyMMddHHmmssffff", DateTimeFormatInfo.InvariantInfo) };

                        //Info
                        FileInfo info = new FileInfo(item.Headers.ContentDisposition.FileName.Replace("\"", ""));
                        //文件类型
                        resource.Type = info.Extension.Substring(1).ToLower();
                        //Write
                        try
                        {
                            //文件存储地址
                            string dirPath = Path.Combine(ROOT_PATH);
                            if (!Directory.Exists(dirPath))
                            {
                                Directory.CreateDirectory(dirPath);
                            }

                            File.WriteAllBytes(Path.Combine(dirPath, resource.Id + "." + resource.Type), data);
                            resources.Add(resource);

                            //调用S3服务
                            ROOT_PATH += "\\" + resource.Id + "." + resource.Type;
                            S3Unit.UploadFile(img_type, ROOT_PATH);
                        } 
                        catch (Exception ex) 
                        {
                            return ex.Message;
                        }
                        if (img_type == "Product")
                        {
                            ROOT_PATH += "\\" + resource.Id + "." + resource.Type;
                            ROOT_PATH2 += "\\" + resource.Id + "." + resource.Type;
                            //return ROOT_PATH + "|" + ROOT_PATH2;

                            //SmallImage.SendSmallImage(ROOT_PATH, ROOT_PATH2, 200, 200, 100, "CUT");
                        }
                    }
                }
            }
            if (resources.Count == 0)
                return "request error";
            else if (resources.Count == 1)
                return JsonConvert.SerializeObject(resources.FirstOrDefault());
            else
                return JsonConvert.SerializeObject(resources);
        }

        #endregion

        [HttpGet]
        public string SmallImg()
        {
            List<string> ImgList = (from o in db.Product
                                    where !string.IsNullOrEmpty(o.ImgUrl)
                                    select o.ImgUrl).ToList();

            string ROOT_PATH = string.Empty;
            string ROOT_PATH2 = string.Empty;

            foreach (string item in ImgList)
            {
                //ROOT_PATH += "\\" + resource.Id + "." + resource.Type;
                //S3Unit.UploadFile(img_type, ROOT_PATH);
                //string a = item.Replace("http://shkapi4uat.shinho.net.cn/ecs/common/UploadFiles/Product/", "");
                //ROOT_PATH = HttpContext.Current.Server.MapPath("~/UploadFiles/Product/" + a);
                //ROOT_PATH2 = HttpContext.Current.Server.MapPath("~/UploadFiles/SmallProduct/" + a);
                //SmallImage.SendSmallImage(ROOT_PATH, ROOT_PATH2, 200, 200, 100, "CUT");
            }

            return "success";
        }

    } 
}



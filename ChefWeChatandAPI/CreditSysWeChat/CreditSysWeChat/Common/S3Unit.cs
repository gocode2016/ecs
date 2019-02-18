using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System.Configuration;
using Amazon.CognitoIdentity;
using Newtonsoft.Json;

namespace CreditSysWeChat.Common
{
    public static class S3Unit
    {
        
        static string bucketName = System.Configuration.ConfigurationManager.AppSettings["S3_BUCKET"];
        static string endpoint = System.Configuration.ConfigurationManager.AppSettings["ENDPOINT"];
        static string accesskeyId = System.Configuration.ConfigurationManager.AppSettings["ACCESSKEYID"];
        static string accessKeySecret = System.Configuration.ConfigurationManager.AppSettings["SECRETKEY"];

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="s3Folder"></param>
        /// <param name="localFilePath"></param>
        public static void UploadFile(string s3Folder, string localFilePath)
        {
            FileInfo fileInf = new FileInfo(localFilePath);

            //设置存储路径及上传区域
            CognitoAWSCredentials credentials = new CognitoAWSCredentials(
                endpoint,
                RegionEndpoint.CNNorth1
               );
            LogHelper.WriteLog("111");
            try
            {
                //创建客户端及上传文件
                using (var s3Client = new AmazonS3Client(RegionEndpoint.CNNorth1))
                {
                    var request = new Amazon.S3.Model.PutObjectRequest()
                    { 
                        BucketName = bucketName,
                        Key = s3Folder + "/" + fileInf.Name,
                        FilePath = localFilePath,
                        //ContentType = "text/plain"
                    };
                    //request.Metadata.Add("x-amz-meta-title", "someTitle");
                    PutObjectResponse response2 = s3Client.PutObject(request);
                    LogHelper.WriteLog("上传图片信息：" + JsonConvert.SerializeObject(response2));
                }
            }
            catch (AmazonS3Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="s3Folder"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        //public static string DownloadFile(string s3Folder, string filename)
        //{
        //    try
        //    {
        //        using (var s3Client = new AmazonS3Client())
        //        {
        //            var request = new Amazon.S3.Model.GetObjectRequest()
        //            {
        //                BucketName = bucketName,
        //                Key = s3Folder + "/" + filename
        //            };
        //            using (var response = s3Client.GetObject(request))
        //            {
        //                response.WriteResponseStreamToFile(srm_config.TEMP_FILE_PATH + filename);
        //            }
        //        }
        //    }
        //    catch (AmazonS3Exception)
        //    {
        //        throw;
        //    }

        //    return srm_config.TEMP_FILE_PATH + filename;
        //}

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="s3Folder"></param>
        /// <param name="filename"></param>
        public static void DeleteFile(string s3Folder, string filename)
        {
            using (var s3Client = new AmazonS3Client())
            {
                DeleteObjectRequest deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = bucketName,
                    Key = s3Folder + "/" + filename
                };
                try
                {
                    s3Client.DeleteObject(deleteObjectRequest);
                }
                catch (AmazonS3Exception)
                {
                    throw;
                }
            }
        }

    }
}
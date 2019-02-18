using IntegralCardAPI.Common;
using IntegralCardAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace IntegralCardAPI.Controllers
{
    /// <summary>
    /// 积分二维码及二维码包处理
    /// </summary>
    public class IntegralGoodsQrcController : ApiController
    {
        private IntegralCardContext db = new IntegralCardContext();
        private SqlHelper dataContext = new SqlHelper();

        /// <summary>
        /// 产品二维码列表
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetGoodsQrcList(dynamic requestData)
        {
            string name = requestData.Name;
            string code = requestData.Code;
            int range = requestData.Range;
            int source = requestData.Source;
            int march = requestData.March;
            int state = requestData.State;

            int beginPage = (requestData.IndexPage - 1) * requestData.PageSize + 1;
            int endPage = requestData.IndexPage * requestData.PageSize;

            try
            {
                var sql = SqlParamHelper.Builder
                           .Append("Select ip.*, ROW_NUMBER() over(order by ip.PackageId desc) as nums from IntegralGoodsQrcPackage as ip");

                if (!string.IsNullOrEmpty(name))
                {
                    sql.Where("Name like '%" + name + "%'");
                }
                else if (!string.IsNullOrEmpty(code))
                {
                    sql.Where("GoodsCode = '" + code + "'");
                }
                else if (range > 0)
                {
                    sql.Where("Range = " + range + "");
                }
                else if (source > 0)
                {
                    sql.Where("Source = " + source + "");
                }
                else if (march > 0)
                {
                    sql.Where("March = " + march + "");
                }
                else if (state > 0)
                {
                    sql.Where("State = " + state + "");
                }

                var pageSql = SqlParamHelper.Builder
                 .Append("select * from (" + sql.SQL + ") as package")
                 .Append("where package.nums >= " + beginPage + " and package.nums <= " + endPage + "");

                var q = dataContext.ExecuteDataTable(CommandType.Text, pageSql.SQL, sql.Arguments);
                string query = JsonConvert.SerializeObject(q);

                //计算分页
                var countSql = string.Format("select Count(*) from(" + sql.SQL + ")as packageCount");
                var count = dataContext.ExecuteScalar(CommandType.Text, countSql);

                return "{ \"Count\":\"" + count + "\",\"data\":" + query + "}";

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="requestData"></param>
        [HttpPost]
        public string Generate(dynamic requestData)
        {
            try
            {
                string query = JsonConvert.SerializeObject(requestData);
                IntegralGoodsQrcPackage qrc = JsonConvert.DeserializeObject<IntegralGoodsQrcPackage>(query);

                Thread t = new Thread(new ParameterizedThreadStart(AsyncGenerate));
                t.Start(qrc);
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
            return "Excute Success";
        }
        
        /// <summary>
        /// 异步执行生成二维码
        /// </summary>
        /// <param name="obj"></param>
        protected void AsyncGenerate(object obj)
        {
            ExcGenerate(obj as IntegralGoodsQrcPackage);
        }

        /// <summary>
        /// 执行生成二维码包
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool ExcGenerate(IntegralGoodsQrcPackage obj)
        {
            try
            {
                //压缩包存储路径
                var basePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                var qrCodePath = Convert.ToString(ConfigurationManager.AppSettings["QrCodeStorePath"]);
                var fileName = string.Format("{0}.xls", Guid.NewGuid().ToString());
                var serverPath = qrCodePath + fileName;
                var filePath = basePath + serverPath;
                //查询商品
                IntegralGoods goods = (from g in db.IntegralGoods
                                       where g.GoodsId == obj.GoodsId
                                       select g).FirstOrDefault<IntegralGoods>();

                //var goods = _integralGoodsRepository.Find(obj.GoodsId);
                obj.Name = goods.GoodsName;
                obj.GoodsCode = goods.GoodsNo;
                obj.State = 2;
                obj.CreateDate = DateTime.Now;
                obj.Url = Convert.ToString(ConfigurationManager.AppSettings["QrCodeUrl"]) + "/";
                obj.DownLoadUrl = serverPath;

                //添加一条压缩包记录
                db.IntegralGoodsQrcPackage.Add(obj);
                db.SaveChanges();

                //安全码的模板
                //{规格}{商品类别}{工厂}{月份}{流水号}
                var snTmpl = ((int)obj.Range).ToString()
                    + obj.GoodsCode
                    + ((int)obj.Source).ToString()
                    + ((int)obj.March).ToString().PadLeft(2, '0');
                List<string> qrCodeList = new List<string>();

                //生成二维码
                for (int i = 1; i <= obj.Num; i++)
                {
                    IntegralGoodsQrc qrc = new IntegralGoodsQrc();
                    qrc.State = 1;
                    qrc.PackageId = obj.PackageId;
                    qrc.CreateDate = DateTime.Now;
                    qrc.QrcUrl = "";
                    qrc.Code = "";

                    //添加一条二维码记录
                    db.IntegralGoodsQrc.Add(qrc);
                    db.SaveChanges();
                    
                    //安全码
                    qrc.Code = snTmpl + qrc.QrcId.ToString().PadLeft(10, '0');
                    //安全码需混淆
                    //{域名}?p={本批次ID}&sn={安全码}   
                    qrc.QrcUrl = string.Format("{0}?sn={1}", obj.Url, EncryptHelper.Confusion(qrc.Code));
                    //更改二维码的值
                    SetQrcCode(qrc.QrcId, qrc.QrcUrl, qrc.Code);
                    //生成一个二维码
                    var code = QrCodeHelper.ToBytes(qrc.QrcUrl);
                    qrCodeList.Add(qrc.QrcUrl);
                }

                //生成Excel
                ExcelExtend.SaveExcel<string>(qrCodeList, qrCodePath, fileName);

                obj.State = 1;
                //_repository.Update(obj);
                EntityState statebefore = db.Entry(obj).State;
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();

                //将生成的所有二维码打包
                //CompressHelper.ToZip(qrCodeList, filePath, "PNG");

                return true;
            }
            catch (Exception ex)
            {
                string a = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 设置二维码
        /// </summary>
        /// <param name="qrcId">二维码ID</param>
        /// <param name="url">二维码链接</param>
        /// <param name="code">编码</param>
        private void SetQrcCode(int qrcId, string url, string code)
        {
            string sql = string.Format("UPDATE dbo.IntegralGoodsQrc SET QrcUrl = '{0}', Code = '{1}' WHERE QrcId = {2}", url, code, qrcId);
            dataContext.ExecuteNonQuery(CommandType.Text, sql);
        }
    }
}

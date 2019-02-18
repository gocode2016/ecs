using ECActivityAPI.Common;
using ECActivityAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace ECActivityAPI.Controllers
{
    /// <summary>
    ///产品库API
    /// </summary>
    public class ProductController : ApiController
    {
        private CreditContext db = new CreditContext();
        private SqlHelper dataContext = new SqlHelper();
        private ImgHandle imgHandle = new ImgHandle();
        private ImgHandle imgHandel = new ImgHandle();
        #region 后台--web

        #region 增加产品一级分类--产品库后台--web
        /// <summary>
        /// 增加产品一级分类--产品库后台--web
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddProductFirst(dynamic requestData)
        {
            try
            {
                ProductFirst productFirst = JsonConvert.DeserializeObject<ProductFirst>(JsonConvert.SerializeObject(requestData));
                productFirst.UpdateTime = productFirst.CreateTime;
                productFirst.UpdatePerson = productFirst.CreatePerson;
                var sql = string.Format(@"INSERT INTO [dbo].[ProductFirst]  ([ProductFirstName] ,[CreatePerson]  ,[UpdatePerson] ,[CreateTime] ,[UpdateTime])  VALUES  ( '{0}','{1}' ,'{2}' ,'{3}' ,'{4}')", productFirst.ProductFirstName, productFirst.CreatePerson, productFirst.UpdatePerson, productFirst.CreateTime, productFirst.UpdateTime);

                dataContext.ExecuteNonQuery(CommandType.Text, sql);

                return "OK";
            }
            catch (Exception)
            {
                return "No";
            }
        }

        #endregion

        #region 获取产品一级分类列表--产品库后台--web
        /// <summary>
        /// 获取产品一级分类列表--产品库后台--web
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetProductFirst(dynamic requestData)
        {
            try
            {
                var q = (from v in db.ProductFirst
                         select v);
                var count = q.Count();
                int pageIndex = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.PageIndex));
                int pageSize = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.PageSize));

                var lastq = q.OrderByDescending(v => v.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize);

                return "{\"Count\":" + count + ",\"Info\":" + JsonConvert.SerializeObject(lastq, DateTimeConvent.DateTimehh()) + "}";
            }
            catch (Exception)
            {
                return "No";
            }
        }

        #endregion

        #region 获取产品一级类别全部--产品库后台--web
        /// <summary>
        /// 获取产品一级类别全部--产品库后台--web
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetProductFirstAll()
        {
            try
            {
                var q = (from v in db.ProductFirst
                         select new
                         {
                             ProductFirstId = v.ProductFirstId,
                             ProductFirstName = v.ProductFirstName
                         });
                return JsonConvert.SerializeObject(q);
            }
            catch (Exception)
            {
                return "No";
            }
        }

        #endregion

        #region 修改产品一级分类--产品库后台--web
        /// <summary>
        /// 修改产品一级分类--产品库后台--web
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string UpdateProductFirst(dynamic requestData)
        {
            try
            {
                ProductFirst pF = JsonConvert.DeserializeObject<ProductFirst>(JsonConvert.SerializeObject(requestData));

                var sql = string.Format("UPDATE [dbo].[ProductFirst] SET [ProductFirstName] = '{0}', [UpdatePerson] ='{1}',[UpdateTime] = '{2}' WHERE ProductFirstId={3}", pF.ProductFirstName, pF.UpdatePerson, DateTime.Now, pF.ProductFirstId);
                dataContext.ExecuteNonQuery(CommandType.Text, sql);
                return "OK";
            }
            catch (Exception)
            {
                return "No";
            }
        }

        #endregion

        #region 增加,修改产品二级分类--产品库后台--web
        /// <summary>
        /// 增加,修改产品二级分类--产品库后台--web
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddProductSecond(dynamic requestData)
        {
            try
            {
                List<ProductSecond> listPS = JsonConvert.DeserializeObject<List<ProductSecond>>(JsonConvert.SerializeObject(requestData));

                for (int i = 0; i < listPS.Count; i++)
                {
                    ProductSecond pS = listPS[i];
                    if (pS.ProductSecondId == 0)
                    {
                        pS.UpdatePerson = pS.CreatePerson;
                        pS.UpdateTime = pS.CreateTime;
                        db.ProductSecond.Add(pS);

                    }
                    else
                    {
                        var sql = string.Format("Update [dbo].ProductSecond set ProductSecondName='{0}' ,UpdatePerson='{1}',UpdateTime='{2}' where   ProductSecondId={3}", pS.ProductSecondName, pS.UpdatePerson, DateTime.Now, pS.ProductSecondId);
                        dataContext.ExecuteNonQuery(CommandType.Text, sql);
                    }
                }
                db.SaveChanges();

                return "OK";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 根据产品一级分类获取二级分类--产品库后台--web
        /// <summary>
        /// 根据产品一级分类获取二级分类--产品库后台--web
        /// </summary>
        /// <param name="requesData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetProductSecond(dynamic requestData)
        {
            try
            {
                int productFirstId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.ProductFirstId));
                var q = (from v in db.ProductSecond
                         where v.ProductFirstId == productFirstId 
                         select v);
                return JsonConvert.SerializeObject(q, DateTimeConvent.DateTimehh());
            }
            catch (Exception)
            {
                return "No";
            }
        }

        #endregion



        #region 增加,修改产品库产品信息（包括规格信息）--产品库后台--web
        /// <summary>
        /// 增加,修改产品库产品信息（包括规格信息）--产品库后台--web
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public string AddProductKuInfo(dynamic requestData)
        {
            try
            {
                ProductKuInfo productKuInfo = JsonConvert.DeserializeObject<ProductKuInfo>(JsonConvert.SerializeObject(requestData));

                int pId = productKuInfo.ProductId;

               //没有做好封装方法
                #region 富文本图片转换

                #region 产品基本信息
                if (productKuInfo.ProductBasicInfo.IndexOf("base64") > 0)
                {
                    string d = productKuInfo.ProductBasicInfo;

                    foreach (string a in imgHandel.GetHtmlImageUrlList(d))
                    {
                        if (a.IndexOf(ImgHandle.DNS) < 0)
                        {
                            string[] asplit = a.Split(',');
                            string imgtype = asplit[0].Substring(11, asplit[0].Length - 18);
                            string filepath = imgHandel.Base64StringToImage(asplit[1], imgtype, "/pic/ProductKuInfo");
                            d = d.Replace(a, filepath);
                        }
                    }
                    productKuInfo.ProductBasicInfo = d;
                }
                #endregion

                #region 产品特性
                if (productKuInfo.ProductFeature.IndexOf("base64") > 0)
                {
                    string d = productKuInfo.ProductFeature;

                    foreach (string a in imgHandel.GetHtmlImageUrlList(d))
                    {
                        if (a.IndexOf(ImgHandle.DNS) < 0)
                        {
                            string[] asplit = a.Split(',');
                            string imgtype = asplit[0].Substring(11, asplit[0].Length - 18);
                            string filepath = imgHandel.Base64StringToImage(asplit[1], imgtype, "/pic/ProductKuInfo");
                            d = d.Replace(a, filepath);
                        }
                    }
                    productKuInfo.ProductFeature = d;
                }
                #endregion

                #region 产品价值
                if (productKuInfo.ProductValue.IndexOf("base64") > 0)
                {
                    string d = productKuInfo.ProductValue;

                    foreach (string a in imgHandel.GetHtmlImageUrlList(d))
                    {
                        if (a.IndexOf(ImgHandle.DNS) < 0)
                        {
                            string[] asplit = a.Split(',');
                            string imgtype = asplit[0].Substring(11, asplit[0].Length - 18);
                            string filepath = imgHandel.Base64StringToImage(asplit[1], imgtype, "/pic/ProductKuInfo");
                            d = d.Replace(a, filepath);
                        }
                    }
                    productKuInfo.ProductValue = d;
                }
                #endregion

                #region 烹饪技巧
                if (productKuInfo.CookingSkill.IndexOf("base64") > 0)
                {
                    string d = productKuInfo.CookingSkill;

                    foreach (string a in imgHandel.GetHtmlImageUrlList(d))
                    {
                        if (a.IndexOf(ImgHandle.DNS) < 0)
                        {
                            string[] asplit = a.Split(',');
                            string imgtype = asplit[0].Substring(11, asplit[0].Length - 18);
                            string filepath = imgHandel.Base64StringToImage(asplit[1], imgtype, "/pic/ProductKuInfo");
                            d = d.Replace(a, filepath);
                        }
                    }
                    productKuInfo.CookingSkill = d;
                }
                #endregion

                #endregion

                //如果是零则表示增加
                if (pId == 0)
                {

                    productKuInfo.UpdatePerson = productKuInfo.CreatePerson;
                    productKuInfo.UpdateTime = productKuInfo.CreateTime;
                    db.ProductKuInfo.Add(productKuInfo);
                    db.SaveChanges();

                }
                //如果不是0则表示修改
                if (pId != 0)
                {
                    var q = (from v in db.ProductKuInfo
                             where v.ProductId == pId
                             select v).FirstOrDefault();
                    if (q == null)
                        return "No";
                    else
                    {
                        q.ProductName = productKuInfo.ProductName;
                        q.ProductFirstId = productKuInfo.ProductFirstId;
                        q.ProductSecondId = productKuInfo.ProductSecondId;
                        q.BrandName = productKuInfo.BrandName;
                        q.ProductBasicInfo = productKuInfo.ProductBasicInfo;
                        q.ProductFeature = productKuInfo.ProductFeature;
                        q.ProductPicURL = productKuInfo.ProductPicURL;
                        q.ProductValue = productKuInfo.ProductValue;
                        q.CookingSkill = productKuInfo.CookingSkill;
                        q.UpdatePerson = productKuInfo.UpdatePerson;
                        q.UpdateTime = DateTime.Now;
                        productKuInfo.ProductId = -1;
                        db.Entry<ProductKuInfo>(q).State = EntityState.Modified;
                        db.SaveChanges();
                        productKuInfo.ProductId = q.ProductId;
                    }

                }
                pId = productKuInfo.ProductId;

                List<Specification> sPList = JsonConvert.DeserializeObject<List<Specification>>(JsonConvert.SerializeObject(requestData.Specification));
                for (int i = 0; i < sPList.Count; i++)
                {
                    //如果等于0表示增加产品分类
                    if (sPList[i].SpecificationId == 0)
                    {
                        sPList[i].ProductId = pId;
                        sPList[i].UpdateTime = sPList[i].CreateTime;
                        var insertSql = string.Format(@"INSERT INTO [dbo].[Specification] ([ProductId],[Amount],[Unit] ,[CreatePerson],[UpdatePerson] ,[CreateTime],[UpdateTime],VisitCount) VALUES({0},{1},'{2}','{3}','{4}','{5}','{6}',0)", sPList[i].ProductId, sPList[i].Amount, sPList[i].Unit, sPList[i].CreatePerson, sPList[i].CreatePerson, sPList[i].CreateTime, sPList[i].CreateTime);
                        dataContext.ExecuteNonQuery(CommandType.Text, insertSql);
                    }
                    //表示修改
                    else
                    {
                        sPList[i].UpdateTime = DateTime.Now;
                        var updateSql = string.Format(@"UPDATE [dbo].[Specification] SET [Amount] ={0},[Unit] ='{1}',[UpdatePerson] ='{2}' ,[UpdateTime]='{3}'  where SpecificationId={4}", sPList[i].Amount, sPList[i].Unit, sPList[i].UpdatePerson, sPList[i].UpdateTime, sPList[i].SpecificationId);
                        dataContext.ExecuteNonQuery(CommandType.Text, updateSql);
                    }
                }
                return "{\"ProductId\":" + pId + "}";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 获取产品库列表--产品库后台--web
        /// <summary>
        ///  获取产品库列表--产品库后台--web
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetPKIList(dynamic requestData)
        {

            try
            {
                int pFId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.ProductFirstId));

                string pName = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.ProductName));

                //页数
                int pageIndex = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.PageIndex));

                var sql = string.Format(@"select PKI.ProductId,PKI.ProductName,ST.Specification,PF.ProductFirstId,PF.ProductFirstName,PS.ProductSecondName,PKI.CreateTime,PKI.UpdateTime from ProductKuInfo as PKI
left join 
------规格反转列--------
(SELECT ProductId , stuff((select '  '+convert(nvarchar,Amount)+Unit  from [Specification] where A.ProductId=ProductId for xml path('')),1,1,'') Specification from [Specification] as A group by ProductId) as ST  ON PKI.ProductId=ST.ProductId
left join ProductFirst as PF on PKI.ProductFirstId=PF.ProductFirstId
Left Join ProductSecond as PS on PKI.ProductSecondId=PS.ProductSecondId
where 1=1");
                if (pFId > 0)
                    sql = sql + "  and  PF.ProductFirstId=" + pFId;
                if (!string.IsNullOrEmpty(pName))
                    sql = sql + "  and  PKI.ProductName   like '%" + pName + "%'";

                sql += "  order by PKI.UpdateTime desc";

                var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
                int totalCount = q.Rows.Count;

                if (pageIndex > 0)
                    q = imgHandle.GetPagedTable(q, pageIndex, 10);

                return "{\"totalCount\":" + totalCount + ",\"Info\":" + JsonConvert.SerializeObject(q, DateTimeConvent.DateTimehh()) + "}";
            }
            catch (Exception)
            {
                return "No";
            }
        }


        #endregion

        #region 根据一个产品ID获取产品全部信息--产品库后台--web
        /// <summary>
        /// 根据一个产品ID获取产品全部信息--产品库后台--web
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetPKIOne(dynamic requestData)
        {
            try
            {
                int pFId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.ProductId));
                var pFInfo = (from v in db.ProductKuInfo
                              where v.ProductId == pFId
                              select v).FirstOrDefault();
                var pFirst = (from v in db.ProductFirst
                              select new
                              {
                                  ProductFirstId = v.ProductFirstId,
                                  ProductFirstName = v.ProductFirstName
                              });
                var sTion = (from v in db.Specification
                             where v.ProductId == pFInfo.ProductId
                             select new
                             {
                                 SpecificationId = v.SpecificationId,
                                 ProductId = v.ProductId,
                                 Amount = v.Amount,
                                 Unit = v.Unit,
                                 UpdatePerson = v.UpdatePerson,
                                 CreatePerson = v.CreatePerson
                             });
                return "{\"ProductKuInfo\":" + JsonConvert.SerializeObject(pFInfo, DateTimeConvent.DateTimehh()) + ",\"ProductFirst\":" + JsonConvert.SerializeObject(pFirst, DateTimeConvent.DateTimehh()) + ",\"Specification\":" + JsonConvert.SerializeObject(sTion) + "}";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion



        #region 增加虚拟类分类--产品库后台--web
        /// <summary>
        /// 增加虚拟类分类--产品库后台--web
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddVirtualClassify(dynamic requestData)
        {
            try
            {
                VirtualClassify vc = JsonConvert.DeserializeObject<VirtualClassify>(JsonConvert.SerializeObject(requestData));
                vc.UpdateTime = vc.CreateTime;
                db.VirtualClassify.Add(vc);
                db.SaveChanges();
                return "OK";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 获取虚拟类分类列表--产品库后台--web
        /// <summary>
        /// 获取虚拟类分类列表--产品库后台--web
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetVCList()
        {
            try
            {
                var q = (from v in db.VirtualClassify
                         select new { 
                            VCId= v.VCId,
                           VCName= v.VCName,
                            IsDisplay = v.IsDisplay,
                            IsIntroduce=v.IsIntroduce
                         });

                return JsonConvert.SerializeObject(q, DateTimeConvent.DateTimehh());
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 修改单个虚拟类分类--产品库后台--web
        /// <summary>
        /// 修改单个虚拟类分类--产品库后台--web
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string UpdateVC(dynamic requestData)
        {
            try
            {
                VirtualClassify vc = JsonConvert.DeserializeObject<VirtualClassify>(JsonConvert.SerializeObject(requestData));
                var q = (from v in db.VirtualClassify
                         where v.VCId == vc.VCId
                         select v).FirstOrDefault();
                q.VCName = vc.VCName;
                q.IsIntroduce = vc.IsIntroduce;
                q.UpdateTime = DateTime.Now;
                vc.VCId = 0;
                db.Entry<VirtualClassify>(q).State = EntityState.Modified;
                db.SaveChanges();
                return "OK";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion


        #region 根据产品信息Id获取规格信息列表--产品库后台--web
        /// <summary>
        /// 根据产品信息Id获取规格信息列表--产品库后台--web
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetSpecificationOnly(dynamic requestData)
        {
            try
            {
                int pId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.ProductId));
                if (pId < 1)
                    return "-100";
                var q = (from v in db.Specification
                         where v.ProductId == pId
                         select v);
                return JsonConvert.SerializeObject(q);
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 修改规格上传图片信息--产品库后台--web
        /// <summary>
        /// 修改规格上传图片信息--产品库后台--web
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string UpdateSPPic(dynamic requestData)
        {
            try
            {
                Specification s = JsonConvert.DeserializeObject<Specification>(JsonConvert.SerializeObject(requestData));
                var sql = string.Format(@"Update  Specification set  SpecificationPicUrl='{0}',  UpdatePerson='{1}' , UpdateTime='{2}'  where  SpecificationId={3}", s.SpecificationPicUrl, s.UpdatePerson, DateTime.Now, s.SpecificationId);
                dataContext.ExecuteNonQuery(CommandType.Text, sql);
                return "OK";
            }
            catch (Exception)
            {
                return "No";
            }
        }

        #endregion


        #region 增加，修改规格配置信息--规格信息--web
        /// <summary>
        /// 增加，修改规格配置信息--规格信息--web
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddSpecificationConf(dynamic requestData)
        {

            try
            {
                #region 增加或修改配置信息基本信息
                SpecificationConf sConf = JsonConvert.DeserializeObject<SpecificationConf>(JsonConvert.SerializeObject(requestData));

                #region 研发故事富文图片本转换
                if (sConf.Introduce.IndexOf("base64") > 0)
                {
                    string d = sConf.Introduce;

                    foreach (string a in imgHandel.GetHtmlImageUrlList(d))
                    {
                        if (a.IndexOf(ImgHandle.DNS) < 0)
                        {
                            string[] asplit = a.Split(',');
                            string imgtype = asplit[0].Substring(11, asplit[0].Length - 18);
                            string filepath = imgHandel.Base64StringToImage(asplit[1], imgtype, "/pic/Specification");
                            d = d.Replace(a, filepath);
                        }
                    }
                    sConf.Introduce = d;
                }
                #endregion

                int sConfId = sConf.SpecificationConfId;
                
                //增加规格配置
                if (sConfId == 0)
                {
                    sConf.UpdateTime = sConf.CreateTime;
                    db.SpecificationConf.Add(sConf);
                    db.SaveChanges();

                }
                //修改规格配置
                if (sConfId != 0)
                {
                    var q = (from v in db.SpecificationConf
                             where v.SpecificationConfId == sConf.SpecificationConfId
                             select v).FirstOrDefault();
                    q.MaterialNum = sConf.MaterialNum;
                    q.Introduce = sConf.Introduce;
                    q.UpdateTime = DateTime.Now;
                    q.UpdatePerson = sConf.UpdatePerson;
                    sConf.SpecificationConfId = 0;
                    db.Entry<SpecificationConf>(q).State = EntityState.Modified;
                    db.SaveChanges();
                    sConf.SpecificationConfId = q.SpecificationConfId;
                }
                #endregion

                sConfId = sConf.SpecificationConfId;

                #region 增加虚拟推荐类

                //在20180411 跟需求方确认王菲时，这个虚拟推荐根本没有，没有必要修改，每次增加及时删除即可
                var deletesqlVC = string.Format("DELETE FROM [dbo].[SpecificationConfVC] where SpecificationConfId={0}", sConfId);
                dataContext.ExecuteNonQuery(CommandType.Text, deletesqlVC);

                List<SpecificationConfVC> sConfVCList = JsonConvert.DeserializeObject<List<SpecificationConfVC>>(JsonConvert.SerializeObject(requestData.SpecificationConfVC));
                for (int i = 0; i < sConfVCList.Count; i++)
                {
                    var sConfVC = sConfVCList[i];
                        sConfVC.UpdateTime = sConfVC.CreateTime;
                        sConfVC.SpecificationConfId = sConfId;
                        db.SpecificationConfVC.Add(sConfVC);

                    #region 初始化看过数据
                        var qDate = (from v in db.SpecificationVCDate
                                     where v.SpecificationId == sConf.SpecificationId && v.VCId == sConfVC.VCId
                                     select v).FirstOrDefault();
                        if (qDate == null)
                        {
                            var addData = string.Format("INSERT INTO [dbo].[SpecificationVCDate]([SpecificationId],[VCId] ,[VisitCount] ,[CreateTime],[UpdateTime])VALUES ( {0},{1},0,'{2}','{2}')", sConf.SpecificationId, sConfVC.VCId,DateTime.Now);
                            dataContext.ExecuteNonQuery(CommandType.Text,addData);
                        }

                    #endregion
                }

                #endregion

                #region 增加或修改配置地区信息

                //在20180411 跟需求方确认王菲时，这个地址问题只是展示，没有必要修改，每次增加及时删除即可
                var deletesqlArea = string.Format("DELETE FROM [dbo].[SpecificationConfArea] where SpecificationConfId={0}", sConfId);
                dataContext.ExecuteNonQuery(CommandType.Text, deletesqlArea);
                
                List<SpecificationConfArea> sConfAList = JsonConvert.DeserializeObject<List<SpecificationConfArea>>(JsonConvert.SerializeObject(requestData.SpecificationConfArea));
                for (int i = 0; i < sConfAList.Count; i++)
                {
                    var sConfA = sConfAList[i];
                    //if (sConfA.SpecificationConfAreaId == 0)
                    //{
                        sConfA.SpecificationConfId = sConfId;
                        sConfA.UpdateTime = sConfA.CreateTime;
                        db.SpecificationConfArea.Add(sConfA);
                    //}
                    //else
                    //{
                    //    var sql = string.Format("Update SpecificationConfArea set ProvinceId={0},ProvinceName='{1}' ,  UpdateTime='{2}' where    SpecificationConfAreaId={3}", sConfA.ProvinceId, sConfA.ProvinceName, DateTime.Now, sConfA.SpecificationConfAreaId);
                    //    dataContext.ExecuteNonQuery(CommandType.Text, sql);
                    //}
                }


                #endregion

                db.SaveChanges();

                return "OK";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 获取规格配置信息--规格信息--web
        /// <summary>
        /// 获取规格配置信息--规格信息--web
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetSpecificationConf(dynamic requestData)
        {
            try
            {
                int sConfId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.SpecificationId));
                var qSConfInfo = (from v in db.SpecificationConf
                                  where v.SpecificationId == sConfId
                                  select new
                                  {
                                      SpecificationId = v.SpecificationId,
                                      SpecificationConfId = v.SpecificationConfId,
                                      MaterialNum = v.MaterialNum,
                                      Introduce = v.Introduce,
                                      CreatePerson = v.CreatePerson,
                                      UpdatePerson = v.UpdatePerson
                                  }).FirstOrDefault();
                if (qSConfInfo == null)
                    return "-100";
                var qSConfVCInfo = (from v in db.SpecificationConfVC
                                    join vc in db.VirtualClassify on v.VCId equals vc.VCId into temp
                                    from tt in temp.DefaultIfEmpty()
                                    where v.SpecificationConfId == qSConfInfo.SpecificationConfId
                                    select new
                                    {
                                        SpecificationConfId = v.SpecificationConfId,
                                        //SpecificationConfVCId = v.SpecificationConfVCId,
                                        VCId = v.VCId,
                                        IsDisplay = v.IsDisplay,
                                        PriorityId = v.PriorityId,
                                        VCName = tt.VCName,
                                        IsIntroduce = tt.IsIntroduce
                                    });
                var qSConfAreaInfo = (from v in db.SpecificationConfArea
                                      where v.SpecificationConfId == qSConfInfo.SpecificationConfId
                                      select new
                                      {
                                          SpecificationConfId = v.SpecificationConfId,
                                          //SpecificationConfAreaId = v.SpecificationConfAreaId,
                                          ProvinceId = v.ProvinceId,
                                          ProvinceName = v.ProvinceName
                                      });
                return "{\"SConfInfo\":" + JsonConvert.SerializeObject(qSConfInfo) + ",\"SConfVCInfo\":" + JsonConvert.SerializeObject(qSConfVCInfo) + ",\"SConfAreaInfo\":" + JsonConvert.SerializeObject(qSConfAreaInfo) + "}";
            }
            catch (Exception)
            {
                return "No";
            }
        }


        #endregion



        #region 增加,修改研发信息--研发信息--web
        /// <summary>
        /// 研发信息增加--研发信息--web
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string AddProductIdKuInfoRD(dynamic requestData)
        {
            try
            {
                ProductIdKuInfoRD pKIRD = JsonConvert.DeserializeObject<ProductIdKuInfoRD>(JsonConvert.SerializeObject(requestData));


                #region 研发故事富文图片本转换
                if (pKIRD.RDStory.IndexOf("base64") > 0)
                {
                    string d = pKIRD.RDStory;

                    foreach (string a in imgHandel.GetHtmlImageUrlList(d))
                    {
                        if (a.IndexOf(ImgHandle.DNS) < 0)
                        {
                            string[] asplit = a.Split(',');
                            string imgtype = asplit[0].Substring(11, asplit[0].Length - 18);
                            string filepath = imgHandel.Base64StringToImage(asplit[1], imgtype, "/pic/RD");
                            d = d.Replace(a, filepath);
                        }
                    }
                    pKIRD.RDStory = d;
                }
                #endregion

                int rdid = pKIRD.RDId;

                #region 增加，修改研发基本信息
                //表示增加
                if (rdid == 0)
                {
                    pKIRD.UpdateTime = pKIRD.CreateTime;
                    db.ProductIdKuInfoRD.Add(pKIRD);
                    db.SaveChanges();
                }
                if (rdid > 0)
                {
                    var q = (from v in db.ProductIdKuInfoRD
                             where v.RDId == rdid
                             select v).FirstOrDefault();

                    q.RDStory = pKIRD.RDStory;
                    q.IsDisPlay = pKIRD.IsDisPlay;
                    q.UpdateTime = DateTime.Now;
                    pKIRD.RDId = 0;
                    db.Entry<ProductIdKuInfoRD>(q).State = EntityState.Modified;
                    db.SaveChanges();
                    pKIRD.RDId = q.RDId;
                }
                #endregion

                rdid = pKIRD.RDId;

                #region 增加，修改研发大厨信息

                var sql = string.Format("delete [dbo].[ProductIdKuInfoRDMaster] where RDId= {0}", rdid);
                dataContext.ExecuteNonQuery(CommandType.Text,sql);


                List<ProductIdKuInfoRDMaster> masterList = JsonConvert.DeserializeObject<List<ProductIdKuInfoRDMaster>>(JsonConvert.SerializeObject(requestData.RDMaster));

                for (int i = 0; i < masterList.Count; i++)
                {
                    var master = masterList[i];
                    master.RDId = rdid;
                    master.RDMasterId = 0;
                    //if (master.RDMasterId == 0)
                    //{
                        master.UpdateTime = master.CreateTime;
                        db.ProductIdKuInfoRDMaster.Add(master);
                        db.SaveChanges();
                    //}
                    //else
                    //{
                    //    var q = (from v in db.ProductIdKuInfoRDMaster
                    //             where v.RDMasterId == master.RDMasterId
                    //             select v).FirstOrDefault();
                    //    q.MasterHeaderPicUrl = master.MasterHeaderPicUrl;
                    //    q.MasterName = master.MasterName;
                    //    q.MasterStation = master.MasterStation;
                    //    q.MasterHotal = master.MasterHotal;
                    //    q.UpdateTime = DateTime.Now;
                    //    master.RDMasterId = 0;
                    //    db.Entry<ProductIdKuInfoRDMaster>(q).State = EntityState.Modified;
                    //    db.SaveChanges();
                    //}
                }
                #endregion

                return "{\"RDId\":"+rdid+"}";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 获取研发信息,研发大师信息--研发信息--web
        /// <summary>
        /// 获取研发信息--研发信息--web
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetProductIdKuInfoRD(dynamic requestData)
        {
            try
            {
                int productId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.ProductId));
                var qRDInfo = (from v in db.ProductIdKuInfoRD
                               where v.ProductId == productId
                               select new
                               {
                                   RDId = v.RDId,
                                   ProductId = v.ProductId,
                                   RDStory = v.RDStory,
                                   IsDisPlay = v.IsDisPlay
                               }).FirstOrDefault();
                if (qRDInfo == null)
                    return "-100";
                var qRDMasterInfo = (from v in db.ProductIdKuInfoRDMaster
                                     where v.RDId == qRDInfo.RDId
                                     select new
                                     {
                                         RDMasterId = v.RDMasterId,
                                         RDId = v.RDId,
                                         MasterHeaderPicUrl = v.MasterHeaderPicUrl,
                                         MasterName = v.MasterName,
                                         MasterStation = v.MasterStation,
                                         MasterHotal = v.MasterHotal
                                     });

                return "{\"RDInfo\":" + JsonConvert.SerializeObject(qRDInfo) + ",\"RDMaster\":" + JsonConvert.SerializeObject(qRDMasterInfo) + "}";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion



        #region 获取产品库推荐使用的调料--应用菜品--web
        /// <summary>
        /// 获取产品库推荐使用的调料--应用菜品--web
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetSDish(dynamic requestData)
        {
            try
            {
                int pId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.ProductId));
//                var sql = string.Format(@" select sVC.SpecificationConfVCId,sVC.SpecificationConfId,sVC.VCId,s.SpecificationId,P.ProductId,VC.VCName,s.Amount,s.Unit,P.ProductName from SpecificationConfVC as sVC
//  left join VirtualClassify as VC on sVC.VCId=VC.VCId
//  left join SpecificationConf as sConf on sVC.SpecificationConfId=sConf.SpecificationConfId
//  left join Specification as s on sConf.SpecificationId=s.SpecificationId
//  left join ProductKuInfo as P on s.ProductId=P.ProductId
//  where s.ProductId={0}", pId);
                //var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
                var q = (from v in db.ProductKuInfo
                         where v.ProductId == pId
                         select new {
                             FlavourRecId = v.ProductId,
                             ProductName=v.ProductName
                         });
                return JsonConvert.SerializeObject(q);
            }
            catch (Exception)
            {
                return "No";
            }
        }


        #endregion

        #region 增加，修改产品库菜品信息--应用菜品--web
        /// <summary>
        /// 增加，修改产品库菜品信息--应用菜品--web
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddDishProduct(dynamic requestData)
        {
            try
            {
                DishProduct dishProduct = JsonConvert.DeserializeObject<DishProduct>(JsonConvert.SerializeObject(requestData));


                #region 研发故事富文图片本转换
                if (dishProduct.DishStory.IndexOf("base64") > 0)
                {
                    string d = dishProduct.DishStory;

                    foreach (string a in imgHandel.GetHtmlImageUrlList(d))
                    {
                        if (a.IndexOf(ImgHandle.DNS) < 0)
                        {
                            string[] asplit = a.Split(',');
                            string imgtype = asplit[0].Substring(11, asplit[0].Length - 18);
                            string filepath = imgHandel.Base64StringToImage(asplit[1], imgtype, "/pic/DishProduct");
                            d = d.Replace(a, filepath);
                        }
                    }
                    dishProduct.DishStory = d;
                }
                #endregion


                int dPId = dishProduct.DishProductId;

                #region 增加，修改产品库菜品基本信息
                if (dPId == 0)
                {
                    dishProduct.UpdateTime = dishProduct.CreateTime;
                    db.DishProduct.Add(dishProduct);
                    db.SaveChanges();
                    dPId = dishProduct.DishProductId;
                }
                else
                {
                    var q = (from v in db.DishProduct
                             where v.DishProductId == dishProduct.DishProductId
                             select v).FirstOrDefault();
                    q.DishStory = dishProduct.DishStory;
                    q.DishPicUrl = dishProduct.DishPicUrl;
                    q.DishName = dishProduct.DishName;
                    q.UpdatePerson = dishProduct.UpdatePerson;
                    q.IsDisplay = dishProduct.IsDisplay;
                    q.UpdateTime = DateTime.Now;
                    dishProduct.DishProductId = 0;
                    db.Entry<DishProduct>(q).State = EntityState.Modified;
                    db.SaveChanges();
                    dPId = q.DishProductId;
                }

                #endregion

                #region 增加产品库菜品调料

                var middleDishMaterial = requestData.DishMaterial;
                List<DishMaterial> addDishMaterial = new List<DishMaterial>();
                for (int i = 0; i < middleDishMaterial.Count; i++)
                {
                    addDishMaterial.Add(JsonConvert.DeserializeObject<DishMaterial>(JsonConvert.SerializeObject(middleDishMaterial[i])));
                }
                //删除原来调料
                string deleteDishMaterial = string.Format(@"DELETE FROM [dbo].[DishMaterial] where DishId={0}", dPId);
                dataContext.ExecuteNonQuery(CommandType.Text, deleteDishMaterial);

                for (int i = 0; i < addDishMaterial.Count; i++)
                {
                    string insert = string.Format(@"INSERT INTO  [dbo].[DishMaterial] ([DishId]
           ,[Material]  ,[Unit],[MaterialType]  ,[CreateTime])  VALUES   ({0} ,'{1}' ,'{2}' ,{3} ,'{4}' )  ", dPId, addDishMaterial[i].Material, addDishMaterial[i].Unit, addDishMaterial[i].MaterialType, addDishMaterial[i].CreateTime);
                    dataContext.ExecuteNonQuery(CommandType.Text, insert);
                }
                #endregion

                #region 增加产品库菜品步骤

                var middleDishStep = requestData.DishStep;

                List<DishStep> addDishStep = new List<DishStep>();
                for (int i = 0; i < middleDishStep.Count; i++)
                {
                    addDishStep.Add(JsonConvert.DeserializeObject<DishStep>(JsonConvert.SerializeObject(middleDishStep[i])));
                }
                //删除
                string deleteDishStep = string.Format(@"DELETE FROM [dbo].[DishStep] where DishId={0}", dPId);
                dataContext.ExecuteNonQuery(CommandType.Text, deleteDishStep);

                for (int i = 0; i < addDishStep.Count; i++)
                {
                    string insert = string.Format("INSERT INTO  DishStep (DishId,StepId,StepName,CreateTime) VALUES  ({0} ,{1} ,'{2}' ,'{3}' ) ", dPId, i + 1, addDishStep[i].StepName, addDishStep[i].CreateTime);
                    dataContext.ExecuteNonQuery(CommandType.Text, insert);
                }

                #endregion

                #region 增加产品库推荐调料
                var middleFlavourRecRole = requestData.FlavourRecRole;

                List<FlavourRecRole> disRFList = JsonConvert.DeserializeObject<List<FlavourRecRole>>(JsonConvert.SerializeObject(middleFlavourRecRole));
                var sql = string.Format(@"DELETE FROM [dbo].[FlavourRecRole]  WHERE  DishId={0} ", dPId);

                dataContext.ExecuteNonQuery(CommandType.Text,sql);

                for (int i = 0; i < disRFList.Count; i++)
                {
                    disRFList[i].DishId = dPId;
                    disRFList[i].UpdateTime = DateTime.Now;
                    db.FlavourRecRole.Add(disRFList[i]);
                }
                db.SaveChanges();

                #endregion

                return "OK";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 根据产品库菜品ID获取菜品信息--应用菜品--web
        /// <summary>
        /// 根据产品库菜品ID获取菜品信息--应用菜品--web
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetDishProduct(dynamic requestData)
        {
            try
            {
                int dId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.DishProductId));
                if (dId < 1)
                    return "-100";
                var qDishProduct = (from v in db.DishProduct
                                    where v.DishProductId == dId
                                    select new
                                    {
                                        DishProductId = v.DishProductId,
                                        ProductId = v.ProductId,
                                        DishStory = v.DishStory,
                                        DishPicUrl = v.DishPicUrl,
                                        DishName = v.DishName,
                                        IsDisplay = v.IsDisplay,
                                        CreatePerson = v.CreatePerson,
                                        UpdatePerson = v.UpdatePerson
                                    }).FirstOrDefault();
                if (qDishProduct == null)
                    return "No";
                var qDishMaterial = (from v in db.DishMaterial
                                     where v.DishId == dId
                                     select new
                                     {
                                         //DishId=v.DishId,
                                         Material = v.Material,
                                         Unit = v.Unit,
                                         MaterialType = v.MaterialType
                                     });
                var qDishStep = (from v in db.DishStep
                                 where v.DishId == dId
                                 select new
                                 {
                                     //DishId=v.DishId,
                                     StepId = v.StepId,
                                     StepName = v.StepName
                                 });
                //var qDishProductRF = (from v in db.DishProductRF
                //         where v.DishProductId == dId
                //         select new {
                //             SpecificationConfVCId=v.SpecificationConfVCId,
                //             SpecificationConfId=v.SpecificationConfId,
                //             VCId=v.VCId,
                //             SpecificationId=v.SpecificationId,
                //             ProductId=v.ProductId,
                //             VCName=v.VCName,
                //             Amount=v.Amount,
                //             Unit=v.Unit,
                //             ProductName=v.ProductName,
                //             Used=v.Used
                //         });
                var sql = string.Format(@" select FR.DishId,FR.FlavourRecId,FR.Unit,PKI.ProductName from FlavourRecRole as FR
  left join ProductKuInfo as PKI on FR.FlavourRecId=PKI.ProductId
  where FR.DishId={0}", dId);
                var qDishProductRF = dataContext.ExecuteDataTable(CommandType.Text, sql);
                return "{\"DishProduct\":" + JsonConvert.SerializeObject(qDishProduct) + ",\"DishMaterial\":" + JsonConvert.SerializeObject(qDishMaterial) + ",\"DishStep\":" + JsonConvert.SerializeObject(qDishStep) + ",\"DishProductRF\":" + JsonConvert.SerializeObject(qDishProductRF) + "}";
            }
            catch (Exception)
            {
                return "No";
            }
        }

        #endregion

        #region 获取研发菜品列表--应用菜品--web
        /// <summary>
        /// 获取研发菜品列表--应用菜品--web
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetRDDishProductList(dynamic requestData)
        {
            try
            {
                //产品Id
                int pId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.ProductId));
                if (pId < 1)
                    return "-100";
                //菜品名字
                string dishName = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.DishName));


                //是否显示
                string display = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.IsDisplay));

                //页数
                int pageIndex = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.PageIndex));

                var q = (from v in db.DishProduct
                         where v.ProductId == pId
                         select new
                         {
                             DishProductId = v.DishProductId,
                             ProductId = v.ProductId,
                             DishStory = v.DishStory,
                             DishPicUrl = v.DishPicUrl,
                             DishName = v.DishName,
                             IsDisplay = v.IsDisplay,
                             CreateTime = v.CreateTime
                         });
                int count = q.Count();
                if (!string.IsNullOrEmpty(dishName))
                    q = q.Where(v => v.DishName.Contains(dishName));
                if (display == "true" || display == "false")
                    q = q.Where(v => v.IsDisplay == display);
                var lastq = q;
                if (pageIndex != 0)
                    lastq = q.OrderByDescending(v => v.CreateTime).Skip((pageIndex - 1) * 10).Take(10);

                return "{\"Count\":" + count + ",\"Info\":" + JsonConvert.SerializeObject(lastq, DateTimeConvent.DateTimehh()) + "}";
            }
            catch (Exception)
            {
                return "No";
            }

            //return null;
        }
        #endregion

        #endregion

        #region 微信端

        #region 获取一级分类--产品库调味--wx
        /// <summary>
        ///  获取一级分类--产品库调味--wx
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetProductFirst()
        {
            try
            {
                var q = (from v in db.ProductFirst
                         where v.IsDisplay == 1
                         orderby v.PriorityId 
                         select new
                         {
                             ProductFirstId = v.ProductFirstId,
                             ProductFirstName = v.ProductFirstName,
                             ProductFirstIcon = v.ProductFirstIcon,
                             
                         });
                return JsonConvert.SerializeObject(q);
            }
            catch (Exception)
            {
                return "No";
            }
        }

        #endregion

        #region 获取虚拟分类--产品库调味--wx
        /// <summary>
        /// 获取虚拟分类--产品库调味--wx
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetVC()
        {
            var  qVC = (from v in db.VirtualClassify
                       where v.IsDisplay == "true"
                       select new
                       {
                           VCId = v.VCId,
                           VCName = v.VCName,
                           ShortIntroduce = v.ShortIntroduce,
                          IsIntroduce= v.IsIntroduce
                       });

            return JsonConvert.SerializeObject(qVC);
        }
        #endregion

        #region 根据虚拟分类Id获取规格--产品库调味--wx
        /// <summary>
        /// 根据虚拟分类Id获取规格--产品库调味--wx
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetSpecificationByVCId(dynamic requestData)
        {
            try
            {
                int VCId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.VCId));
                string OpenId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.OpenId));
                var sql = string.Format(@"select top 6
s.SpecificationId,s.ProductId,s.SpecificationPicUrl,s.Amount,s.Unit,P.ProductName,s.VisitCount,sC.Introduce
,case when L.Id is null then 'f' else 't'  end as IsPraise
,P.ProductFirstId
from Specification as s
left join SpecificationConf as sC on s.SpecificationId=sC.SpecificationId
left join SpecificationConfVC as sCVC on sC.SpecificationConfId=sCVC.SpecificationConfId
left join ProductKuInfo as P on s.ProductId=P.ProductId
left join (select * from  SpecificationPraiseLog where OpenId='{1}'  )as L on L.SpecificationId=s.SpecificationId
where sCVC.VCId={0} and sCVC.IsDisplay='true'
order by sCVC.PriorityId", VCId, OpenId);
                var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
                return JsonConvert.SerializeObject(q);
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 根据虚拟分类Id获取规格列表--产品库虚拟列表--wx
        /// <summary>
        /// 根据虚拟分类Id获取规格列表--产品库虚拟列表--wx
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetSpecificationByVCIdList(dynamic requestData)
        {
            try
            {
                int VCId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.VCId));
                string OpenId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.OpenId));
                int pageIndex = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.PageIndex));

                //根据哪个排序
                string rank = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.Rank));

                //升序还是降序
                string isDesc = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.IsDesc));

                var sql = string.Format(@"select 
s.SpecificationId,s.ProductId,s.SpecificationPicUrl,s.Amount,s.Unit,P.ProductName,s.VisitCount,sC.Introduce
,case when L.Id is null then 'f' else 't'  end as IsPraise
,P.ProductFirstId
from Specification as s
left join SpecificationConf as sC on s.SpecificationId=sC.SpecificationId
left join SpecificationConfVC as sCVC on sC.SpecificationConfId=sCVC.SpecificationConfId
left join ProductKuInfo as P on s.ProductId=P.ProductId
left join (select * from  SpecificationPraiseLog where OpenId='{1}'   )as L on L.SpecificationId=s.SpecificationId
where sCVC.VCId={0} and sCVC.IsDisplay='true'
", VCId, OpenId);

                if (rank == "count" || rank == "time")
                {

                    if (isDesc == "f" || isDesc == "t")
                    {
                        if (rank == "count" && isDesc == "f")
                            sql += "   order  by   s.VisitCount  asc";
                        if (rank == "count" && isDesc == "t")
                            sql += "   order  by   s.VisitCount  desc";
                        if (rank == "time" && isDesc == "f")
                            sql += "   order  by  s.UpdateTime  asc";
                        if (rank == "time" && isDesc == "t")
                            sql += "   order  by  s.UpdateTime  desc";
                    }
                }
                else
                    sql += "  order  by   s.VisitCount  desc";

                var q = dataContext.ExecuteDataTable(CommandType.Text, sql);

                int count = q.Rows.Count;
                if (pageIndex > 0)
                    q = imgHandle.GetPagedTable(q, pageIndex, 6);

                return "{\"Count\":" + count + ",\"Info\":" + JsonConvert.SerializeObject(q) + "}";
            }
            catch (Exception)
            {
                return "No";
            }
        }

        #endregion


        #region 根据一级分类获取二级分类--一级分类列表页--wx
        /// <summary>
        /// 根据一级分类获取二级分类--一级分类列表页--wx
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetProductSecondWX(dynamic requestData)
        {
            try
            {
                int pFirst = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.ProductFirstId));
                var q = (from v in db.ProductSecond
                         where v.ProductFirstId == pFirst && v.IsDisplay == "true"
                         select new
                         {
                             ProductFirstId = v.ProductFirstId,
                             ProductSecondId = v.ProductSecondId,
                             ProductSecondName = v.ProductSecondName
                         });

                return JsonConvert.SerializeObject(q);
            }
            catch (Exception)
            {
                return "No";
            }
        }
        
        #endregion

        #region 获取一级分类规格列表--一级分类列表页--wx
        /// <summary>
        /// 获取一级分类规格列表--一级分类列表页--wx
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetProductFirstList(dynamic requestData)
        {
            try
            {
                int pSId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.ProductSecondId));
                 int pFId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.ProductFirstId));

                string OpenId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.OpenId));
                int pageIndex = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.PageIndex));

                

                //根据哪个排序
                string rank = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.Rank));

                //升序还是降序
                string isDesc = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.IsDesc));
                var sql = string.Format(@"select s.SpecificationId,s.ProductId,s.Amount,s.Unit,PKI.ProductName,s.SpecificationPicUrl,s.VisitCount
,PKI.ProductFirstId ,PKI.ProductSecondId
,case when L.Id is null then 'f' else 't'  end as IsPraise
, Display.IsDisplay
from  Specification as s
left join ProductKuInfo as PKI on s.ProductId=PKI.ProductId
left join (select * from  SpecificationPraiseLog where OpenId='{0}'  )as L on L.SpecificationId=s.SpecificationId
left join SpecificationConf as sConf on s.SpecificationId=sConf.SpecificationId
left join ( select SpecificationConfId,sum(IsDisplay) as IsDisplay from( select SpecificationConfId , case IsDisplay when 'true' then 1 else  0 end as IsDisplay  from SpecificationConfVC ) as A
  group by SpecificationConfId) as Display on sConf.SpecificationConfId=Display.SpecificationConfId
where Display.IsDisplay>0 
and  PKI.ProductFirstId={1}", OpenId, pFId);
                if (pSId > 0)
                    sql += "   and    PKI.ProductSecondId=" + pSId;

                if (rank == "count" || rank == "time")
                {

                    if (isDesc == "f" || isDesc == "t")
                    {
                        if (rank == "count" && isDesc == "f")
                            sql += "   order  by   s.VisitCount  asc";
                        if (rank == "count" && isDesc == "t")
                            sql += "   order  by   s.VisitCount  desc";
                        if (rank == "time" && isDesc == "f")
                            sql += "   order  by  s.UpdateTime  asc";
                        if (rank == "time" && isDesc == "t")
                            sql += "   order  by  s.UpdateTime  desc";
                    }
                }
                else
                    sql += "  order  by   s.VisitCount  desc";

                var q = dataContext.ExecuteDataTable(CommandType.Text, sql);

                int count = q.Rows.Count;
                if (pageIndex > 0)
                    q = imgHandle.GetPagedTable(q, pageIndex, 6);

                return "{\"Count\":" + count + ",\"Info\":" + JsonConvert.SerializeObject(q) + "}";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 获取规格基本信息--产品、规格、详情页--wx
        /// <summary>
        /// 获取规格基本信息--产品、规格、详情页--wx
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetProductDetail(dynamic requestData)
        {
            try
            {
                Specification sP = JsonConvert.DeserializeObject<Specification>(JsonConvert.SerializeObject(requestData));
                string openId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.OpenId));

                int pFId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.ProductFirstId));

                var sInfo = (from v in db.Specification
                             where v.SpecificationId == sP.SpecificationId
                             select new
                             {
                                 SpecificationId = v.SpecificationId,
                                 VisitCount = v.VisitCount,
                                 PraiseCount = v.PraiseCount,
                                 SpecificationPicUrl = v.SpecificationPicUrl,
                                 Amount = v.Amount,
                                 Unit = v.Unit
                             }).FirstOrDefault();

                var IsPaise = (from v in db.SpecificationPraiseLog
                               where v.SpecificationId == sP.SpecificationId && v.OpenId == openId
                               select v).FirstOrDefault();
                string isPaise = "f";
                if (IsPaise != null)
                    isPaise = "t";

                var pInfo = (from v in db.ProductKuInfo
                             where v.ProductId == sP.ProductId
                             select new
                             {
                                 ProductId = v.ProductId,
                                 ProductName = v.ProductName,
                                 BrandName = v.BrandName,
                                 ProductFeature = v.ProductFeature,
                                 ProductValue = v.ProductValue,
                                 CookingSkill = v.CookingSkill,
                                 ProductBasicInfo = v.ProductBasicInfo
                             }).FirstOrDefault();

                var pAreaSql = string.Format(@"select s.SpecificationId,sArea.ProvinceName from  SpecificationConfArea as sArea
  left join SpecificationConf as sConf on sArea.SpecificationConfId=sConf.SpecificationConfId
  left join Specification as s on s.SpecificationId=sConf.SpecificationId
  where s.SpecificationId={0}", sP.SpecificationId);
                var qdateTable = dataContext.ExecuteDataTable(CommandType.Text, pAreaSql);

                //研发信息
                var RDInfo = (from v in db.ProductIdKuInfoRD
                              where v.ProductId == sP.ProductId && v.IsDisPlay == 1
                              select v).FirstOrDefault();

                //相关菜品
                var dishInfo = (from v in db.DishProduct
                                where v.ProductId == sP.ProductId && v.IsDisplay == "true"
                                select new
                                {
                                    ProductId = v.ProductId,
                                    DishProductId = v.DishProductId,
                                    DishName = v.DishName,
                                    v.DishPicUrl
                                }).Take(6);

                //相关产品
                var sql = string.Format(@"select s.SpecificationId,s.ProductId,s.Amount,s.Unit,PKI.ProductName,s.SpecificationPicUrl,s.VisitCount
,PKI.ProductFirstId ,PKI.ProductSecondId
,case when L.Id is null then 'f' else 't'  end as IsPraise
, Display.IsDisplay
from  Specification as s
left join ProductKuInfo as PKI on s.ProductId=PKI.ProductId
left join (select * from  SpecificationPraiseLog where OpenId='{0}'  )as L on L.SpecificationId=s.SpecificationId
left join SpecificationConf as sConf on s.SpecificationId=sConf.SpecificationId
left join ( select SpecificationConfId,sum(IsDisplay) as IsDisplay from( select SpecificationConfId , case IsDisplay when 'true' then 1 else  0 end as IsDisplay  from SpecificationConfVC ) as A
  group by SpecificationConfId) as Display on sConf.SpecificationConfId=Display.SpecificationConfId
where Display.IsDisplay>0 
and  PKI.ProductFirstId={1}", openId, pFId);
               
                var Product = dataContext.ExecuteDataTable(CommandType.Text, sql);

                var lastq = imgHandle.GetPagedTable(Product, 1, 6);





                if (RDInfo != null)
                {
                    //研发大师
                    var RDMaster = (from v in db.ProductIdKuInfoRDMaster
                                    where v.RDId == RDInfo.RDId
                                    select new
                                    {
                                        MasterHeaderPicUrl = v.MasterHeaderPicUrl,
                                        MasterName = v.MasterName,
                                        MasterHotal = v.MasterHotal,
                                        MasterStation = v.MasterStation
                                    });

                    return "{\"Specification\":" + JsonConvert.SerializeObject(sInfo) + ",\"IsPaise\":\"" + isPaise + "\",\"ProductKuInfo\":" + JsonConvert.SerializeObject(pInfo) + ",\"Area\":" + JsonConvert.SerializeObject(qdateTable) + ",\"RDInfo\":" + JsonConvert.SerializeObject(RDInfo) + ",\"RDMaster\":" + JsonConvert.SerializeObject(RDMaster) + ",\"DishInfo\":" + JsonConvert.SerializeObject(dishInfo) + ",\"ProductInfo\":" + JsonConvert.SerializeObject(lastq) + "}";
                }

                return "{\"Specification\":" + JsonConvert.SerializeObject(sInfo) + ",\"IsPaise\":\"" + isPaise + "\",\"ProductKuInfo\":" + JsonConvert.SerializeObject(pInfo) + ",\"Area\":" + JsonConvert.SerializeObject(qdateTable) + ",\"RDInfo\":" + JsonConvert.SerializeObject(RDInfo) + ",\"RDMaster\":\"[]\",\"DishInfo\":" + JsonConvert.SerializeObject(dishInfo) + ",\"ProductInfo\":" + JsonConvert.SerializeObject(lastq) + "}";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        
        #endregion

        #region 增加规格点赞数据--产品、规格、详情页--wx
        /// <summary>
        /// 增加规格点赞数据--产品、规格、详情页--wx
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddSpecificationPraiseLog(dynamic requestData)
        {
            try
            {
                SpecificationPraiseLog sL = JsonConvert.DeserializeObject<SpecificationPraiseLog>(JsonConvert.SerializeObject(requestData));
                var q = (from v in db.SpecificationPraiseLog
                         where v.SpecificationId == sL.SpecificationId && v.OpenId == sL.OpenId
                         select v).FirstOrDefault();
                if (q == null)
                {
                    db.SpecificationPraiseLog.Add(sL);
                    db.SaveChanges();
                    var sql = string.Format(@"UPDATE [dbo].[Specification]SET [PraiseCount] = [PraiseCount]+1 ,[UpdateTime] = '{0}' WHERE SpecificationId={1}", DateTime.Now, sL.SpecificationId);
                    dataContext.ExecuteNonQuery(CommandType.Text, sql);
                }
                var qPraise = (from v in db.Specification
                               where v.SpecificationId == sL.SpecificationId
                               select v).FirstOrDefault();

                return "{\"PraiseCount\":" + qPraise.PraiseCount + "}";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 增加规格看过量--产品、规格、详情页--wx
        /// <summary>
        /// 增加规格看过量--产品、规格、详情页--wx
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddSpecificationVisitLog(dynamic requestData)
        {
            try
            {
                SpecificationVisitLog sL = JsonConvert.DeserializeObject<SpecificationVisitLog>(JsonConvert.SerializeObject(requestData));
                //var q = (from v in db.SpecificationVisitLog
                //         where v.SpecificationId == sL.SpecificationId && v.OpenId == sL.OpenId
                //         select v).FirstOrDefault();
                //if (q == null)
                //{
                    db.SpecificationVisitLog.Add(sL);
                    db.SaveChanges();
                    var sql = string.Format(@"UPDATE [dbo].[Specification]SET [VisitCount] = [VisitCount]+1 ,[UpdateTime] = '{0}' WHERE SpecificationId={1}", DateTime.Now, sL.SpecificationId);
                    dataContext.ExecuteNonQuery(CommandType.Text, sql);
                //}
                Thread.Sleep(500);
                var qPraise = (from v in db.Specification
                               where v.SpecificationId == sL.SpecificationId
                               select v).FirstOrDefault();

                return "{\"VisitCount\":" + qPraise.VisitCount + "}";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion


        #region 增加规格留言数据--产品、规格、详情页--wx
        /// <summary>
        /// 增加规格留言数据--产品、规格、详情页--wx
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddSpecificationComment(dynamic requestData)
        {
            try
            {
                SpecificationComment sC = JsonConvert.DeserializeObject<SpecificationComment>(JsonConvert.SerializeObject(requestData));
                db.SpecificationComment.Add(sC);
                db.SaveChanges();

                var q = (from v in db.SpecificationComment
                         where v.SpecificationId == sC.SpecificationId
                         select v);
                var count = q.Count();

                var lastq = q.OrderByDescending(v => v.CreateTime).Skip(0).Take(3);

                //查找HotelName
                var lastResult = (from v in lastq
                                  join p in db.OpenIdAssociated on v.OpenId equals p.OpenId
                                  join n in db.RegistMember on p.UserId equals n.MemberId
                                  select new
                                  {
                                      HeadPicUrl = v.HeadPicUrl,
                                      MemebName = v.MemberName,
                                      Comment = v.Comment,
                                      CreateTime = v.CreateTime,
                                      HotelName = n.HotelName
                                  });

                return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(lastResult, DateTimeConvent.DateTimehh()) + "}";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 查询产品留言数据--产品、规格、详情页--wx
        /// <summary>
        ///  查询产品留言数据--产品、规格、详情页--wx
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetSpecificationComment(dynamic requestData)
        {
            try
            {
                SpecificationComment sC = JsonConvert.DeserializeObject<SpecificationComment>(JsonConvert.SerializeObject(requestData));
                int index = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.Index));
                var q = (from v in db.SpecificationComment
                         where v.SpecificationId == sC.SpecificationId
                         select v);
                var count = q.Count();

                var lastq = q.OrderByDescending(v => v.CreateTime).Skip((index - 1) *3).Take(3);

                //查找HotelName
                var lastResult = (from v in lastq
                                  join p in db.OpenIdAssociated on v.OpenId equals p.OpenId
                                  join n in db.RegistMember on p.UserId equals n.MemberId
                                  select new
                                  {
                                      HeadPicUrl = v.HeadPicUrl,
                                      MemebName = v.MemberName,
                                      Comment = v.Comment,
                                      CreateTime = v.CreateTime,
                                      HotelName = n.HotelName
                                  });

                return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(lastResult, DateTimeConvent.DateTimehh()) + "}";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 获取产品菜品详细信息--产品库菜品详情（做成通用接口）--wx
        /// <summary>
        /// 获取产品菜品详细信息--产品库菜品详情（做成通用接口）--wx
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetDishProductWX(dynamic requestData)
        {
            try
            {
                int dishId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.DishId));
                string openId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.OpenId));
                var qInfo = (from v in db.DishProduct
                             where v.DishProductId == dishId && v.IsDisplay == "true"
                             select new
                             {
                                 DishName = v.DishName,
                                 DishStory = v.DishStory,
                                 DishPicUrl = v.DishPicUrl,
                                 VisitCount = v.VisitCount,
                                 PrasieCount = v.PrasieCount
                             }).FirstOrDefault();

                var qMaterial = (from v in db.DishMaterial
                                 where v.DishId == dishId
                                 select new
                                 {
                                     Material = v.Material,
                                     Unit=v.Unit,
                                     MaterialType = v.MaterialType
                                 });
                var qStep = (from v in db.DishStep
                             where v.DishId == dishId
                             select new
                             {
                                 StepId = v.StepId,
                                 StepName = v.StepName
                             });
                string isPrasie = "f";

                var prasie = (from v in db.DishPrasieLog
                              where v.OpenId == openId && v.DishId == dishId
                              select v).FirstOrDefault();
                if (prasie != null)
                    isPrasie = "t";


                string isCollect = "f";
                var collect = (from v in db.DishCollectLog
                               where v.OpenId == openId && v.DishId == dishId
                               select v).FirstOrDefault();
                if (collect != null)
                    isCollect = "t";


                var sql = string.Format(@" select FR.DishId,FR.FlavourRecId,FR.Unit,PKI.ProductName from FlavourRecRole as FR
  left join ProductKuInfo as PKI on FR.FlavourRecId=PKI.ProductId
  where FR.DishId={0}", dishId);
                var qDishProductRF = dataContext.ExecuteDataTable(CommandType.Text, sql);
                return "{\"DishProduct\":" + JsonConvert.SerializeObject(qInfo) + ",\"DishMaterial\":" + JsonConvert.SerializeObject(qMaterial) + ",\"DishStep\":" + JsonConvert.SerializeObject(qStep) + ",\"DishProductRF\":" + JsonConvert.SerializeObject(qDishProductRF) + ",\"IsPrasie\":\"" + isPrasie + "\",\"IsCollect\":\"" + isCollect + "\"}";
            }
            catch (Exception)
            {
                return "No";
            }
        }
       

        #endregion

        #region 增加产品菜品点赞--产品库菜品详情（做成通用接口）--wx
        /// <summary>
        /// 增加产品菜品点赞--产品库菜品详情（做成通用接口）--wx
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddDishProductPrasie(dynamic requestData)
        {
            try
            {
                DishPrasieLog dL = JsonConvert.DeserializeObject<DishPrasieLog>(JsonConvert.SerializeObject(requestData));

                var q = (from v in db.DishPrasieLog
                         where v.DishId == dL.DishId && v.OpenId == dL.OpenId
                         select v).FirstOrDefault();
                if (q == null)
                {
                    db.DishPrasieLog.Add(dL);
                    db.SaveChanges();
                    var sql = string.Format(@"Update DishProduct set  PrasieCount=PrasieCount+1 , UpdatePerson='{0}'  where  DishProductId={1}", DateTime.Now, dL.DishId);
                    dataContext.ExecuteNonQuery(CommandType.Text, sql);
                }
                var qprasie = (from v in db.DishProduct
                               where v.DishProductId == dL.DishId
                               select v).FirstOrDefault();
                return "{\"PrasieCount\":" + qprasie.PrasieCount + "}";
            }
            catch (Exception)
            {
                return "No";
            }

        }
        #endregion

        #region 增加产品菜品浏览次数--产品库菜品详情（做成通用接口）--wx
        /// <summary>
        /// 增加产品菜品浏览次数--产品库菜品详情（做成通用接口）--wx
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddDishVisit(dynamic requestData)
        {
            try
            {
                DishVisitLog dL = JsonConvert.DeserializeObject<DishVisitLog>(JsonConvert.SerializeObject(requestData));
                db.DishVisitLog.Add(dL);
                db.SaveChanges();
                var sql = string.Format(@"Update DishProduct set  VisitCount=VisitCount+1 , UpdatePerson='{0}'  where  DishProductId={1}", DateTime.Now, dL.DishId);
                dataContext.ExecuteNonQuery(CommandType.Text, sql);
                Thread.Sleep(400);
                var qprasie = (from v in db.DishProduct
                               where v.DishProductId == dL.DishId
                               select v).FirstOrDefault();
                return "{\"VisitCount\":" + qprasie.VisitCount + "}";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 增加产品菜品留言--产品库菜品详情（做成通用接口）--wx
        /// <summary>
        /// 增加产品菜品留言--产品库菜品详情（做成通用接口）--wx
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddDishComment(dynamic requestData)
        {
            try
            {
                DishComment dC = JsonConvert.DeserializeObject<DishComment>(JsonConvert.SerializeObject(requestData));
                db.DishComment.Add(dC);
                db.SaveChanges();

                var q = (from v in db.DishComment
                         where v.DishId == dC.DishId
                         select v);
                var count = q.Count();

                var lastq = q.OrderByDescending(v => v.CreateTime).Skip(0).Take(3);
                return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(lastq, DateTimeConvent.DateTimehh()) + "}";
            }
            catch (Exception)
            {
                return "No";
            }
        }

        #endregion

        #region 查询产品菜品留言--产品库菜品详情（做成通用接口）--wx
        /// <summary>
        /// 查询产品菜品留言--产品库菜品详情（做成通用接口）--wx
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetDishComment(dynamic requestData)
        {
            try
            {
                int  dishId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.DishId));
                int index = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.Index));
                //db.DishComment.Add(dC);
                //db.SaveChanges();

                var q = (from v in db.DishComment
                         where v.DishId == dishId
                         select v);
                var count = q.Count();

                var lastq = q.OrderByDescending(v => v.CreateTime).Skip((index - 1) * 3).Take(3);
                return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(lastq, DateTimeConvent.DateTimehh()) + "}";
            }
            catch (Exception)
            {
                return "No";
            }
        }

        #endregion

        #region 增加或取消收藏--产品库菜品详情（做成通用接口）--wx
        /// <summary>
        /// 增加或取消收藏--产品库菜品详情（做成通用接口）--wx
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddDishCollect(dynamic requestData)
        {
            try
            {
                DishCollectLog dCL = JsonConvert.DeserializeObject<DishCollectLog>(JsonConvert.SerializeObject(requestData));
                var q = (from v in db.DishCollectLog
                         where v.DishId == dCL.DishId && v.OpenId == dCL.OpenId
                         select v).FirstOrDefault();
                string isCollect = "t";
                if (q == null)
                {
                    db.DishCollectLog.Add(dCL);
                    db.SaveChanges();
                }
                else
                {
                    db.DishCollectLog.Remove(q);
                    db.SaveChanges();
                    isCollect = "f";
                }

                return "{\"IsCollect\":\"" + isCollect + "\"}";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion



        #endregion
    }
}

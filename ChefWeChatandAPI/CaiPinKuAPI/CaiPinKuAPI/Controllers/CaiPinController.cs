using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CaiPinKuAPI.Common;
using CaiPinKuAPI.Models;
using Newtonsoft.Json;
using System.Data;


namespace CaiPinKuAPI.Controllers
{
    /// <summary>
    /// 后台菜品管理
    /// </summary>
    public class CaiPinController : ApiController
    {
        private CreditContext db = new CreditContext();
        private SqlHelper dataContext = new SqlHelper();


        #region 菜品库后台接口
        /// <summary>
        /// 获取富文本信息类型,用于前端下拉值
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetRichTextType()
        {
            ReturnJson r = new ReturnJson();
            string sql = "SELECT Id,Type FROM dbo.CpkRichTextType ORDER BY Id";
            var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
            return JsonConvert.SerializeObject(q);
        }


        /// <summary>
        /// 获取产品列表,用于前端下拉值
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetProducts()
        {
            ReturnJson r = new ReturnJson();
            string sql = "SELECT ProductId,ProductName FROM dbo.ProductKuInfo ORDER BY ProductId";
            var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
            return JsonConvert.SerializeObject(q);
        }

        /// <summary>
        /// 获取产品列表,用于前端下拉值
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetMaterialUnit()
        {
            ReturnJson r = new ReturnJson();
            string sql = "SELECT Id,Unit FROM dbo.CpkUnit ORDER BY Id";
            var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
            return JsonConvert.SerializeObject(q);
        }

        /// <summary>
        /// 获取创建菜品第一步时的信息
        /// </summary>
        /// <param name="CaiPinId"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetStep1(int CaiPinId)
        {
            ReturnJson r = new ReturnJson();
            List<RCpkCaiPinStep1> list = new List<RCpkCaiPinStep1>();
            string sql = string.Format(@"SELECT CaiPinId,CaiPinName,VideoUrl,VideoImage,Images 
                                        FROM CpkCaiPinBasicInfo
                                        WHERE CaiPinId = {0}", CaiPinId);
            var data = dataContext.ExecuteDataTable(CommandType.Text, sql);
            var a = DataTableToList.ConvertTo<RCpkCaiPinStep1>(data).FirstOrDefault();
            if (a != null)
            {
                a.CategoryList = GetCategoryById(CaiPinId);
                a.BanKuai = GetBanKuaiByCpId(CaiPinId);
                return JsonConvert.SerializeObject(a);
            }
            else
            {
                return "";
            }
        }


       /// <summary>
       /// 根据菜品id获取所属分类
       /// </summary>
       /// <param name="CaiPinId"></param>
       /// <returns></returns>
        public object GetCategoryById(int CaiPinId)
        {
            string sql = string.Format(@"
                                         SELECT DISTINCT c.FirstId ,c.FirstName 
                                         FROM dbo.CpkCaiPinCategory a 
                                         JOIN  dbo.CpkSecondCategory b ON a.SecondCategoryId = b.SecondId 
                                         JOIN dbo.CpkFirstCategory c ON c.FirstId = b.FirstId
                                         WHERE b.IsEnable =1 AND a.CaiPinId ={0};"
                                        , CaiPinId);
            var rCpkCateroryList = DataTableToList.ConvertTo<RCpkCaterory>(dataContext.ExecuteDataTable(CommandType.Text, sql));  //菜品所属一级分类
            foreach (var item in rCpkCateroryList)
            {
               

                sql = string.Format(@"SELECT  b.SecondId,b.SecondName  FROM dbo.CpkCaiPinCategory a 
                                        JOIN  dbo.CpkSecondCategory b ON a.SecondCategoryId = b.SecondId 
                                        JOIN dbo.CpkFirstCategory c ON c.FirstId = b.FirstId
                                        WHERE b.IsEnable =1 AND a.CaiPinId ={0} AND c.FirstId = {1}", CaiPinId, item.FirstId);

                item.SecondList = dataContext.ExecuteDataTable(CommandType.Text, sql);
            }
            return rCpkCateroryList;
        }

        public object GetBanKuaiByCpId(int CaiPinId)
        {
            string sql = string.Format(@"SELECT b.BanKuaiId,b.IndexShow FROM dbo.CpkCaiPinBasicInfo a 
                                        JOIN dbo.CpkCpBkRelation b ON b.CaiPinId = a.CaiPinId
                                        JOIN dbo.CpkBanKuai c ON c.BanKuaiId = b.BanKuaiId 
                                         WHERE a.IsEnable =1 AND c.IsEnable =1 AND a.CaiPinId ={0};"
                             , CaiPinId);
            DataTable dt = dataContext.ExecuteDataTable(CommandType.Text, sql);  //菜品所属一级分类
            return dt;
        }

        /// <summary>
        /// 获取菜品所在的板块id
        /// </summary>
        /// <param name="CaiPinId">菜品id</param>
        /// <returns>板块id</returns>
        public int[] GetBanKuaiById(int CaiPinId)
        {
            string sql = string.Format(@"SELECT b.BanKuaiId FROM dbo.CpkCaiPinBasicInfo a 
                                        JOIN dbo.CpkCpBkRelation b ON b.CaiPinId = a.CaiPinId
                                        JOIN dbo.CpkBanKuai c ON c.BanKuaiId = b.BanKuaiId 
                                         WHERE a.IsEnable =1 AND c.IsEnable =1 AND a.CaiPinId ={0};"
                                        , CaiPinId);
            DataTable dt = dataContext.ExecuteDataTable(CommandType.Text, sql);  //菜品所属一级分类
            int[] c = new int[dt.Rows.Count]; //二级分类数组
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    int i = 0;
                    foreach (DataRow row2 in dt.Rows)
                    {
                        c[i] = Convert.ToInt16(row2[0]);
                        i++;
                    }
                }
            }
            return c;
        }

        /// <summary>
        /// 根据查询条件查询
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string Query(dynamic requestData)
        {
            LogHelper.WriteMsgByDay("CaiPinController-Query-log:" + requestData.ToString());
            ReturnJson r = new ReturnJson();
            int page = requestData.page;
            int pagesize = requestData.pagesize;
            string sqlwhere = "";
            if (!string.IsNullOrEmpty(requestData.startdate.ToString()))
            {
                sqlwhere += " and a.CreateDate >='" + requestData.startdate.ToString() + "'";
            }
            if (!string.IsNullOrEmpty(requestData.enddate.ToString()))
            {
                sqlwhere += " and a.CreateDate <='" + requestData.enddate.ToString() + "'";
            }
            if (!string.IsNullOrEmpty(requestData.keyword.ToString()))
            {
                sqlwhere += string.Format(@"and (CaiPinName like '%{0}%' or b.FenLeiName like '%{0}%' or c.FenLeiName like '%{0}%' 
                                                 or d.FenLeiName like '%{0}%'  or BanKuaiName like '%{0}%' )", requestData.keyword.ToString());
            }
            string sql = string.Format(@"SELECT row_number() over(order by a.CaiPinId DESC) RowId,a.CaiPinId,a.CaiPinName,b.FenLeiName CaiXi, c.FenLeiName CaiShi,d.FenLeiName ShiCai,
                                        a.VideoUrl,a.VideoImage,   CASE a.IsPublish WHEN 1 THEN '已发布' ELSE '未发布' END IsPublish,
                                        ISNULL(k.ScCount,0) ScCount,ISNULL(k.LlCount,0) LlCount,ISNULL( k.DzCount,0) DzCount,ISNULL(h.LyCount,0) LyCount,
                                         a.BanKuaiName
                                        FROM CpkCaiPinBasicInfo a 
                                        LEFT JOIN ViewCpkCaiPinFenZu b ON a.CaiPinId = b.CaiPinId AND b.FirstId =1
                                        LEFT JOIN ViewCpkCaiPinFenZu c ON a.CaiPinId = c.CaiPinId AND c.FirstId =2
                                        LEFT JOIN ViewCpkCaiPinFenZu d ON a.CaiPinId = d.CaiPinId AND d.FirstId =3
                                        LEFT JOIN ViewCpkTongji   k  ON a.CaiPinId = k.RecordKeyId AND   ModuleName = '菜品库' AND RecordKeyType='菜品' 
                                        LEFT JOIN(SELECT KeyId,COUNT(1) LyCount FROM dbo.CpkLiuYan WHERE  IsEnable =1 AND KeyType = '菜品' GROUP BY KeyId) h ON h.KeyId = a.CaiPinId 
                                        WHERE a.IsEnable =1  {0} ", sqlwhere);
            var sqlpage = PageHelper.GetPagerSql(page, pagesize, sql); //分页sql
            var q = dataContext.ExecuteDataTable(CommandType.Text, sqlpage); //分页的数据
            var totalcount = dataContext.GetCount(CommandType.Text, sql); //总条数
            var toatlpage = PageHelper.GetTotalPage(totalcount, pagesize);
            string data = JsonConvert.SerializeObject(q);
            r.status = "succ";
            r.totalcount = totalcount;
            r.totalpage = toatlpage;
            r.data = JsonConvert.DeserializeObject(data.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            return JsonConvert.SerializeObject(r);
        }






        /// <summary>
        /// 获取创建菜品第二步时的信息
        /// </summary>
        /// <param name="CaiPinId"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetStep2(int CaiPinId)
        {
            ReturnJson r = new ReturnJson();
            string sql = string.Format(@"SELECT b.TypeId,c.Type,b.Content ,a.HotelLongitude,a.HotelAtitude,b.Display
                                        FROM  dbo.CpkCaiPinBasicInfo a JOIN CpkCaiPinRichInfo b ON b.CaiPinId = a.CaiPinId
                                        JOIN CpkRichTextType c ON b.TypeId = c.Id
                                            WHERE a.CaiPinId = {0} ORDER BY TypeId", CaiPinId);
            var data = dataContext.ExecuteDataTable(CommandType.Text, sql);
            return JsonConvert.SerializeObject(data);
        }


        /// <summary>
        /// 获取创建菜品第三步时的信息
        /// </summary>
        /// <param name="CaiPinId"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetStep3(int CaiPinId)
        {
            ReturnJson r = new ReturnJson();
            string sql = string.Format(@"SELECT MaterialJson FROM dbo.CpkCaiPinBasicInfo WHERE  CaiPinId = {0}", CaiPinId);
            var data = dataContext.ExecuteScalar(CommandType.Text, sql);
            if (data != null)
            {
                return data.ToString();
            }
            else
            {
                return "";
            }
        }



        /// <summary>
        /// 创建新的菜品第一步:菜的分类、板块、图片、视频图片
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string CreateOrEditStep1(dynamic requestData)
        {
            LogHelper.WriteMsgByDay("CaiPinController-CreateOrEditStep1-log:" + requestData.ToString());
            ReturnJson r = new ReturnJson();
            string action = requestData.Action; //接口类型  分Create Edit
            int caipinid = requestData.CaiPinId; //菜品id  新增是为0 
            string caipinname = requestData.CaiPinName; //菜品名称
            string videourl = requestData.VideoUrl;//视频地址
            string secondid = requestData.SecondId;//二级分类id
            var bankuai = requestData.BanKuai;//板块id
            string videoimageurl = requestData.VideoImageUrl;//预览图片预览时的图片url
            var images = requestData.ImageUrlList;//菜品图片url
            string sql = "";
            if (action == "Create")
            {
                sql = string.Format("SELECT 1 FROM dbo.CpkCaiPinBasicInfo WHERE CaiPinName = '{0}' ", caipinname);
                int c = dataContext.GetCount(CommandType.Text, sql);
                if (c == 0) //判断是否存在
                {
                    //新增菜品基本信息
                    CpkCaiPinBasicInfo model = new CpkCaiPinBasicInfo { CaiPinName = caipinname, VideoUrl = videourl, VideoImage = videoimageurl };
                    db.CpkCaiPinBasicInfo.Add(model);
                    db.SaveChanges();
                    caipinid = model.CaiPinId;
                    r.message += action + "菜品基本信息成功;";
                    r.data = caipinid;  //返回前端菜品id,用于之后菜品信息保存
                }
                else
                {
                    r.message += "该菜品名称已存在;";
                    return JsonConvert.SerializeObject(r);
                }
            }
            else if (action == "Edit")
            {
                //更新菜品基本信息
                sql = string.Format(@"UPDATE dbo.CpkCaiPinBasicInfo SET CaiPinName = '{0}',VideoUrl='{1}',VideoImage = '{2}' 
                                      WHERE CaiPinId = {3}",caipinname,videourl,videoimageurl,caipinid);
                dataContext.ExecuteNonQuery(CommandType.Text, sql);
                r.message += action + "菜品基本信息成功;";
            }
            else
            {
                r.message += "Action参数有误";
                return JsonConvert.SerializeObject(r);
            }

            //不管新增还是编辑 都先删除菜品所属分类
            if (!string.IsNullOrEmpty(secondid) && caipinid != 0)
            {
                sql = string.Format(@"DELETE FROM dbo.CpkCaiPinCategory WHERE CaiPinId = {0};
                                        INSERT INTO dbo.CpkCaiPinCategory
                                                ( CaiPinId,SecondCategoryId )
                                        SELECT {0},SecondId FROM dbo.CpkSecondCategory WHERE SecondId IN ({1})", caipinid, secondid);
                dataContext.ExecuteNonQuery(CommandType.Text, sql);
                r.message += "分类"+action+"成功;";
            }

            //不管新增还是编辑 都先删除菜品所属板块 
            if (caipinid != 0)
            {

                string values = "";
                string bankuaiids = "";
                foreach (var item in bankuai)
                {
                    string bankuaiid = item["Id"];
                    string indexShow = item["IndexShow"];
                    values += string.Format(@"({0},{1},{2}),",caipinid,bankuaiid,indexShow);
                    bankuaiids += bankuaiid + ",";
                }
                if (!string.IsNullOrEmpty(values))
                {
                    values = values.TrimEnd(',') + ";";
                    bankuaiids = bankuaiids.TrimEnd(',');
                    sql = string.Format(@"DELETE FROM dbo.CpkCpBkRelation WHERE CaiPinId  = {0};
                                    INSERT INTO dbo.CpkCpBkRelation
                                            ( CaiPinId ,
                                                BanKuaiId,IndexShow ) VALUES
                                    {1}
                                    
                                    UPDATE dbo.CpkCaiPinBasicInfo SET BanKuaiName = STUFF((SELECT ','+  BanKuaiName FROM dbo.CpkBanKuai WHERE BanKuaiId IN ({2})  for xml path('')),1,1,'')
                                    WHERE CaiPinId = {0}
                                    ", caipinid,values, bankuaiids);
                    dataContext.ExecuteNonQuery(CommandType.Text, sql);
                    r.message += "板块" + action + "成功;";
                }
//                bankuaiids = bankuaiids.TrimEnd(',') + ";";
//                sql = string.Format(@"DELETE FROM dbo.CpkCpBkRelation WHERE CaiPinId  = {0};
//                                    INSERT INTO dbo.CpkCpBkRelation
//                                            ( CaiPinId ,
//                                                BanKuaiId )
//                                    SELECT {0},BanKuaiId FROM dbo.CpkBanKuai WHERE BanKuaiId IN ({1});
//                                    
//                                    UPDATE dbo.CpkCaiPinBasicInfo SET BanKuaiName = STUFF((SELECT ','+  BanKuaiName FROM dbo.CpkBanKuai WHERE BanKuaiId IN ({1})  for xml path('')),1,1,'')
//                                    WHERE CaiPinId = {0}
//                                    ", caipinid, bankuaiid);

            }
            //不管新增还是编辑 都先删除菜品图片(最多6张)
            if (images != null && caipinid != 0)
            {
                sql = string.Format("DELETE FROM CpkCaiPinImages WHERE CaiPinId  = {0};",caipinid);
                string urls = "";
                foreach (string item in images)
                {
                    sql += string.Format(@"INSERT INTO dbo.CpkCaiPinImages( CaiPinId ,ImageUrl) VALUES  ( {0} ,'{1}');", caipinid, item.ToString());
                    //拼接图片地址
                    urls += item.ToString() + ",";
                }
                sql += string.Format("UPDATE CpkCaiPinBasicInfo SET Images = '{0}' WHERE CaiPinId = {1} ", urls.Substring(0, urls.Length - 1), caipinid);
                dataContext.ExecuteNonQuery(CommandType.Text, sql);
                r.message += "菜品图片" + action + "成功;";
            }
            r.status = "succ";
            r.data = caipinid;
            return JsonConvert.SerializeObject(r);
        }


        /// <summary>
        /// 创建菜品第二步：菜品故事、作者介绍、旺店介绍
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string CreateOrEditStep2(dynamic requestData)
        {
            LogHelper.WriteMsgByDay("CaiPinController-CreateOrEditStep2-log:" + requestData.ToString());
            ReturnJson r = new ReturnJson(); 
            try
            {
                string sql = "";
                string action = requestData.Action; //接口类型  分Create Edit
                var richlist = requestData.List;
                int caipinid = requestData.CaiPinId;
                int dyrqShow = requestData.dyrqShow;
                string hotelLongitude = requestData.HotelLongitude;
                string hotelAtitude = requestData.HotelAtitude;
                //编辑时先删除
                if (action == "Edit")
                {
                    sql = string.Format(@"UPDATE CpkCaiPinBasicInfo SET HotelLongitude ='{1}',HotelAtitude = '{2}',DyrqShow = '{3}' WHERE CaiPinId = {0} ;
                                      DELETE FROM CpkCaiPinRichInfo WHERE CaiPinId = {0}", caipinid, hotelLongitude, hotelAtitude, dyrqShow);
                    dataContext.ExecuteNonQuery(CommandType.Text, sql);
                }
                foreach (var item in richlist)
                {
                    int typeid = item["TypeId"];
                    int display = item["Display"];
                    string content = item["Content"].ToString().Replace("\"", "'");
                    LogHelper.WriteMsgByDay("CaiPinController-content-log:" + content);
                    if (content.IndexOf("base64") > 0)
                    {
                        foreach (string a in CaiPinKuAPI.Common.ImageHandle.GetHtmlImageUrlList(content))
                        {
                            LogHelper.WriteMsgByDay("CaiPinController-string a-log:" + a);
                            LogHelper.WriteMsgByDay("CaiPinController-Common.ImageHandle.DNS-log:" + Common.ImageHandle.DNS);
                            if (a.IndexOf(Common.ImageHandle.DNS) < 0)
                            {
                                string[] asplit = a.Split(',');
                                LogHelper.WriteMsgByDay("CaiPinController-asplit-log:" + asplit.ToString());
                                string imgtype = asplit[0].Substring(11, asplit[0].Length - 18);
                                LogHelper.WriteMsgByDay("CaiPinController-imgtype-log:" + imgtype);
                                string filepath = Common.ImageHandle.Base64StringToImage(asplit[1], imgtype, "/Images");
                                content = content.Replace(a, filepath);
                            }
                        }
                    }
                    sql = string.Format(@"INSERT INTO dbo.CpkCaiPinRichInfo
                                                        ( CaiPinId ,TypeId ,Content,Display )
                                                VALUES  ({0},{1},'{2}',{3}) ", caipinid, typeid, content.Replace("'", "''"), display);
                    dataContext.ExecuteNonQuery(CommandType.Text, sql);
                    r.message += "富文本类型" + typeid.ToString() + action + "成功;";
                }
                r.status = "succ";
            }
            catch (Exception ex)
            {
                r.message = ex.ToString();
                LogHelper.WriteMsgByDay("CaiPinController-CreateOrEditStep2-error:" + ex.ToString());
            }
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 创建菜品第三步:主料辅料调料 做菜步骤
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string CreateOrEditStep3(dynamic requestData)
        {
            LogHelper.WriteMsgByDay("CaiPinController-CreateOrEditStep3-log:" + requestData.ToString());
            ReturnJson r = new ReturnJson();

            string action = requestData.Action; //接口类型  分Create Edit
            int caipinid = requestData.CaiPinId;
            string steps = requestData.Steps;

            //保存json字符串,微信端展示的时候直接用这个json,不用再拼接了.
            string sql = string.Format(@"UPDATE dbo.CpkCaiPinBasicInfo SET MaterialJson = '{0}',Steps= '{1}' WHERE CaiPinId = '{2}' ", requestData.ToString().Replace("\r\n", ""),steps, caipinid);
            dataContext.ExecuteNonQuery(CommandType.Text, sql);

            //编辑时先删除
            if (action == "Edit")
            {
                sql = string.Format(@"DELETE FROM CpkCaiPinMaterial WHERE CaiPinId = {0}", caipinid);
                dataContext.ExecuteNonQuery(CommandType.Text, sql);
            }

            var zhuliao = requestData.ZhuLiao;
            var fuliao = requestData.FuLiao;
            var tiaoliao = requestData.TiaoLiao;
     
            string name = "";
            string amount = "";
            string unit = "";
            int productid = 0;  //前端手动填写的调料productid=0 ,下拉选择的为实际productid
            int materialtype = 0;  //主料1   辅料2   调料3
            int display = 1; //显示方式  1单列显示 2双列显示

            //保存主料
            display = zhuliao.Display;
            foreach (var item in zhuliao.Data)
            {
                name = item["Name"];
                amount = item["Amount"];
                unit = item["Unit"];
                materialtype = 1;

                sql = string.Format(@"INSERT INTO CpkCaiPinMaterial
                        ( CaiPinId ,Material ,ProductId ,Amount ,Unit ,MaterialType,Display)  VALUES
                        ( {0},'{1}',{2},'{3}','{4}',{5},{6})", caipinid, name, productid, amount, unit, materialtype, display);
                dataContext.ExecuteNonQuery(CommandType.Text, sql);
            }
            r.message += "主料" + action + "成功;";

            //保存辅料
            display = fuliao.Display;
            foreach (var item in fuliao.Data)
            {
                name = item["Name"];
                amount = item["Amount"];
                unit = item["Unit"];
                materialtype = 2;
                sql = string.Format(@"INSERT INTO CpkCaiPinMaterial
                        ( CaiPinId ,Material ,ProductId ,Amount ,Unit ,MaterialType,Display)  VALUES
                        ( {0},'{1}',{2},'{3}','{4}',{5},{6})", caipinid, name, productid, amount, unit, materialtype, display);
                dataContext.ExecuteNonQuery(CommandType.Text, sql);
            }
            r.message += "辅料" + action + "成功;";

            //保存调料
            
            foreach (var a in tiaoliao)
            {
                name = a["Name"];
                amount = a["Amount"];
                unit = a["Unit"];
                productid = a["ProductId"];
                materialtype = 3;

                CpkCaiPinMaterial model = new CpkCaiPinMaterial { CaiPinId = caipinid, Material = name, ProductId = productid,                                      Amount = amount, Unit = unit, MaterialType = materialtype };
                db.CpkCaiPinMaterial.Add(model);
                db.SaveChanges();

                //保存组合调料
                //调料可能是组合起来的,比如糖醋汁由糖 生抽 老抽 醋 料酒 组合的。
                //保存在同一张表 组合调料的ParentId为刚保存的model.MaterialId
                var items = a["Items"];  
                if (model.MaterialId != 0 && items !=null)
                { 
                    foreach(var item in items)
                    {
                        CpkCaiPinMaterial m = new CpkCaiPinMaterial { CaiPinId = caipinid, Material = item["Name"],
                                                                        ProductId = item["ProductId"], Amount = item["Amount"], 
                                                                        Unit = item["Unit"], MaterialType = materialtype ,ParentId = model.MaterialId};
                        db.CpkCaiPinMaterial.Add(m);
                        db.SaveChanges();
                    }
                }
            }
            r.message += "调料" + action + "成功;";
            r.status = "succ";
            return JsonConvert.SerializeObject(r);
        }




        /// <summary>
        /// 菜品发布与不发布
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string Publish(dynamic requestData)
        {
            LogHelper.WriteMsgByDay("CaiPinController-Publish-log:" + requestData.ToString());
            ReturnJson r = new ReturnJson();
            int action = requestData.Flag;
            string caipinId = requestData.CaiPinId;
            if (action == 0 || action == 1)
            {
                if (!string.IsNullOrEmpty(caipinId))
                {
                    var sql = string.Format(@"UPDATE CpkCaiPinBasicInfo SET IsPublish = {0} WHERE CaiPinId  IN ({1})", action, caipinId);
                    int a = dataContext.ExecuteNonQuery(CommandType.Text, sql);
                    r.status = "succ";
                    r.message = a.ToString() + (action == 1 ? "条发布成功" : "条已取消发布");
                }
                else
                {
                    r.message = "CaiPinId参数不能为空";
                }
            }
            else
            {
                r.message = "Flag参数只能为0或1";
            }
            return JsonConvert.SerializeObject(r);
        }
        /// <summary>
        /// 菜品启用与删除
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string Enable(dynamic requestData)
        {
            LogHelper.WriteMsgByDay("CaiPinController-Enable-log:" + requestData.ToString());
            ReturnJson r = new ReturnJson();
            int action = requestData.Flag;
            string caipinId = requestData.CaiPinId;
            if (action == 0 || action == 1)
            {
                if (!string.IsNullOrEmpty(caipinId))
                {
                    var sql = string.Format("UPDATE CpkCaiPinBasicInfo SET IsEnable = {0} WHERE CaiPinId  IN ({1}) ", action, caipinId);
                    int a = dataContext.ExecuteNonQuery(CommandType.Text, sql);
                    r.status = "succ";
                    r.message = a.ToString() + (action == 1 ? "条已启用" : "条已删除");
                }
                else
                {
                    r.message = "CaiPinId参数不能为空";
                }
            }
            else
            {
                r.message = "Flag参数只能为0或1";
            }
            return JsonConvert.SerializeObject(r);
        }

        #endregion

    }
}

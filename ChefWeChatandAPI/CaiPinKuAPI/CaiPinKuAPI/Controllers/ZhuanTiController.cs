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
using System.Data.Entity.Migrations;
using System.Configuration;

namespace CaiPinKuAPI.Controllers
{
    /// <summary>
    /// 后台专题管理
    /// </summary>
    public class ZhuanTiController : ApiController
    {
        private CreditContext db = new CreditContext();
        private SqlHelper dataContext = new SqlHelper();
        /// <summary>
        /// 根据专题id获取信息
        /// </summary>
        /// <param name="ZhuanTiId">专题id</param>
        /// <returns></returns>
        [HttpGet]
        public string GetById(int ZhuanTiId)
        {
            ReturnJson r = new ReturnJson();

            string sql = string.Format(@"SELECT a.ZhuanTiId,a.ZhuanTiName,a.ZhuanTiURL,b.ZtContentId,b.Content 
                                        FROM dbo.CpkZhuanTi a JOIN dbo.CpkZhuanTiContent b ON b.ZhuanTiId = a.ZhuanTiId AND b.IsEnable=1
                                        WHERE a.IsEnable = 1 AND a.ZhuanTiId = {0}", ZhuanTiId);
            var data = dataContext.ExecuteDataTable(CommandType.Text, sql);
            var ztList = DataTableToList.ConvertTo<RZhuanTi>(data);
            foreach (var item in ztList)
            {
                item.CaiPinList = GetCaiPinByZhuanTiContentId(item.ZtContentId);
            }
            return JsonConvert.SerializeObject(ztList);
        }
        /// <summary>
        /// 获取专题内容关联的菜品
        /// </summary>
        /// <param name="ZhuanTiId">专题内容id</param>
        /// <returns></returns>
        public object GetCaiPinByZhuanTiContentId(int ZtContentId)
        {
            string sql = string.Format(@"SELECT a.CaiPinId,a.CaiPinName, a.Images FROM dbo.CpkCaiPinBasicInfo a 
                                        JOIN dbo.CpkCpZtContentRelation b ON b.CaiPinId = a.CaiPinId
                                        WHERE  a.IsEnable =1  AND  b.ZtContentId = {0}", ZtContentId);
            var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
            return q;
        }
        
        /// <summary>
        /// 获取专题下的板块id  不用了 po逻辑想错了
        /// </summary>
        /// <param name="ZhuanTiId"></param>
        /// <returns></returns>
        public int[] GetBanKuaiByZhuanTiId(int ZhuanTiId)
        {
            string sql = string.Format(@"SELECT b.BanKuaiId FROM dbo.CpkZhuanTi a 
                                        JOIN dbo.CpkZtBkRelation b ON b.ZhuanTiId = a.ZhuanTiId
                                        JOIN dbo.CpkBanKuai c ON c.BanKuaiId = b.BanKuaiId WHERE a.ZhuanTiId = {0}
                                        AND a.IsEnable = 1 AND c.IsEnable =1"
                                        , ZhuanTiId);
            DataTable dt = dataContext.ExecuteDataTable(CommandType.Text, sql);  
            int[] c = new int[dt.Rows.Count]; 
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
            LogHelper.WriteMsgByDay("ZhuanTiController-Query-log:" + requestData.ToString());
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
                sqlwhere += string.Format(@"and (a.ZhuanTiName like '%{0}%')", requestData.keyword.ToString());
            }
            string sql = string.Format(@"SELECT row_number() over(order by a.ZhuanTiId DESC) RowId,a.ZhuanTiId,a.ZhuanTiName,
                                        '{1}'+CONVERT(VARCHAR(20),a.ZhuanTiId) ZhuanTiUrl,
                                        ISNULL(k.LlCount,0) LlCount,ISNULL(k.DzCount,0) DzCount,ISNULL(k.ScCount,0) ScCount,ISNULL(d.LyCount,0) LyCount
                                        FROM dbo.CpkZhuanTi a
                                        LEFT JOIN ViewCpkTongji   k  ON a.ZhuanTiId = k.RecordKeyId AND   k.ModuleName = '菜品库' AND RecordKeyType='专题'    --浏览数量  人气
                                        LEFT JOIN (SELECT KeyId,COUNT(1) LyCount FROM dbo.CpkLiuYan 
			                                        WHERE IsEnable =1 AND ParentId =0 AND KeyType = '专题' GROUP BY KeyId
			                                        ) d ON d.KeyId = a.ZhuanTiId 
                                        WHERE a.IsEnable =1   {0} ", sqlwhere, ConfigurationManager.AppSettings["CpkZhuanTiUrl"].ToString());
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
        /// 创建
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string CreateOrEdit(dynamic requestData)
        {
            LogHelper.WriteMsgByDay("ZhuanTiController-CreateOrEdit-log:" + requestData.ToString());
            ReturnJson r = new ReturnJson();
            int zhuantiid = requestData.ZhuanTiId;  //专题id,新建时为0,编辑时为实际id
            string action = requestData.Action; //接口类型  分Create Edit
            string zhuantiname = requestData.ZhuanTiName; //专题名称
            var contentList = requestData.ContentList;   //专题内容列表
            if (string.IsNullOrEmpty(zhuantiname))
            {
                r.message = "专题名称不能为空";
            }
            else if (zhuantiname.Length > 14)
            {
                zhuantiname = zhuantiname.Substring(0, 14);
            }
            else
            {
                string sql = "";
                sql = string.Format("SELECT 1 FROM dbo.CpkZhuanTi WHERE ZhuanTiName = '{0}' ", zhuantiname);
                int c = dataContext.GetCount(CommandType.Text, sql);
                if (action == "Create")
                {
                    if (c > 0) //判断是否存在
                    {
                        r.message = "该专题名称已存在";
                    }
                }
                else
                {
                    if (c > 1) //判断是否存在
                    {
                        r.message = "该专题名称已存在";
                    }
                }
                
                if (string.IsNullOrEmpty(r.message))   //以上验证都通过时
                {
                    //现将该专题已存在的内容及菜品关系 逻辑删除
                    sql = string.Format(@"UPDATE dbo.CpkZhuanTiContent SET IsEnable = 0 WHERE ZhuanTiId ={0}", zhuantiid);
                    SqlHelper2.ExecuteNonQuery(CommandType.Text, sql);
                    foreach (var item in contentList)   //循环内容列表
                    {
                        string content = item["Content"].ToString().Replace("\"", "'");  //富文本内容
                        string caiPinId = item["CaiPinId"]; //内容关联的菜品
                        //LogHelper.WriteMsgByDay("CaiPinController-content-log:" + content);
                        if (content.IndexOf("base64") > 0)
                        {
                            foreach (string a in ImageHandle.GetHtmlImageUrlList(content))
                            {
                                //LogHelper.WriteMsgByDay("CaiPinController-string a-log:" + a);
                                //LogHelper.WriteMsgByDay("CaiPinController-Common.ImageHandle.DNS-log:" + Common.ImageHandle.DNS);
                                if (a.IndexOf(Common.ImageHandle.DNS) < 0)
                                {
                                    string[] asplit = a.Split(',');
                                    //LogHelper.WriteMsgByDay("CaiPinController-asplit-log:" + asplit.ToString());
                                    string imgtype = asplit[0].Substring(11, asplit[0].Length - 18);
                                    //LogHelper.WriteMsgByDay("CaiPinController-imgtype-log:" + imgtype);
                                    string filepath = Common.ImageHandle.Base64StringToImage(asplit[1], imgtype, "/Images");
                                    content = content.Replace(a, filepath);
                                }
                            }
                        }
                        //新增编辑都是如下逻辑,因为编辑时上面已逻辑删除之前的内容
                        //新增专题基本信息
                        CpkZhuanTi m = new CpkZhuanTi();
                        m.ZhuanTiId = zhuantiid;
                        m.ZhuanTiName = zhuantiname;
                        m.UpdateDate = DateTime.Now;
                        db.CpkZhuanTi.AddOrUpdate(m); //根据zhuantiid新增或者编辑
                        db.SaveChanges();
                        zhuantiid = m.ZhuanTiId;
                        //新增专题内容

                        CpkZhuanTiContent mContent = new CpkZhuanTiContent();
                        mContent.ZtContentId = 0;
                        mContent.ZhuanTiId = zhuantiid;
                        mContent.Content = content;
                        db.CpkZhuanTiContent.AddOrUpdate(mContent);
                        db.SaveChanges();
//                        sql = string.Format(@"INSERT INTO dbo.CpkZhuanTiContent
//                                                    ( ZhuanTiId , Content)
//                                            VALUES  ( {0} , '{1}' ); select SCOPE_IDENTITY() ", zhuantiid, content);
//                        int ztContentId = Convert.ToInt32(SqlHelper2.ExecuteScalar(CommandType.Text, sql));
                        int ztContentId = mContent.ZtContentId;
                        //新增专题内容关联的菜品
                        sql = string.Format(@"INSERT INTO dbo.CpkCpZtContentRelation
                                                    ( ZtContentId , CaiPinId )
                                              SELECT {0},CaiPinId FROM dbo.CpkCaiPinBasicInfo WHERE CaiPinId IN ({1});
                                              DELETE FROM dbo.CpkZhuanTiContent WHERE ZhuanTiId ={2} AND IsEnable = 0
                                        ", ztContentId, caiPinId,zhuantiid);
                        SqlHelper2.ExecuteNonQuery(CommandType.Text, sql);
                    }
                    r.status = "succ";
                    r.message = action + "成功";
                }
            }
            return JsonConvert.SerializeObject(r);
        }
        /// <summary>
        /// 启用与删除
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string Enable(dynamic requestData)
        {
            ReturnJson r = new ReturnJson();
            int action = requestData.Flag;
            string zhuantiid = requestData.ZhuanTiId;
            if (action == 0 || action == 1)
            {
                if (!string.IsNullOrEmpty(zhuantiid))
                {
                    var sql = string.Format("UPDATE dbo.CpkZhuanTi SET IsEnable =  {0} WHERE ZhuanTiId IN ({1})", action, zhuantiid);
                    int a = dataContext.ExecuteNonQuery(CommandType.Text, sql);
                    r.status = "succ";
                    r.message = a.ToString() + (action == 1 ? "条已启用" : "条已删除");
                }
                else
                {
                    r.message = "ZhuanTiId参数不能为空";
                }
            }
            else
            {
                r.message = "Flag参数只能为0或1";
            }
            return JsonConvert.SerializeObject(r);
        }
    }
}

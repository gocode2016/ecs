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
    /// 后台板块管理
    /// </summary>
    public class BanKuaiController : ApiController
    {
        private CreditContext db = new CreditContext();
        private SqlHelper dataContext = new SqlHelper();

        /// <summary>
        /// 获取板块信息,用于前端下拉值
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Get()
        {
            ReturnJson r = new ReturnJson();
            string sql = "SELECT BanKuaiId,BanKuaiName FROM dbo.CpkBanKuai WHERE IsEnable =1 ORDER BY BanKuaiId";
            var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
            return JsonConvert.SerializeObject(q);
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string Create(dynamic requestData)
        {
            LogHelper.WriteMsgByDay("BanKuaiController-Create-log:" + requestData.ToString());
            ReturnJson r = new ReturnJson();
            string bankuainame = requestData.BanKuaiName;
            if (bankuainame.Length > 0 && bankuainame.Length <=14)
            {
                string sql = string.Format(@"SELECT * FROM dbo.CpkBanKuai WHERE BanKuaiName = '{0}'", bankuainame);
                int count = SqlHelper2.GetCountByCountSql(sql);
                if (count == 0)
                {
                    sql = string.Format(@"INSERT INTO dbo.CpkBanKuai( BanKuaiName )VALUES  ('{0}')", bankuainame);
                    dataContext.ExecuteNonQuery(CommandType.Text, sql);
                    r.status = "succ";
                    r.message = "创建成功";
                }
                else
                {
                    r.message = "该板块已存在";
                }
            }
            else if (bankuainame.Length > 14)
            {
                r.message = "板块名称长度已超过14";
            }
            else
            {
                r.message = "BanKuaiName不能为空";
            }
            return JsonConvert.SerializeObject(r);
        }



        /// <summary>
        /// 根据查询条件获取板块
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string Query(dynamic requestData)
        {
            LogHelper.WriteMsgByDay("BanKuaiController-Query-log:" + requestData.ToString());
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
                sqlwhere += " and a.BanKuaiName like '%" + requestData.keyword.ToString() + "%'";
            }
            var sql = string.Format(@"SELECT  row_number() over(order by a.BanKuaiId) RowId ,a.BanKuaiId,a.BanKuaiName,a.CreateDate,
		                               CASE a.IsPublish WHEN 1 THEN '已发布' ELSE '未发布' END IsPublish,
                                       ISNULL( b.CpCount,0) CpCount,ISNULL(c.SpCount,0) SpCount,ISNULL(d.ZtCount,0) ZtCount
                                    FROM dbo.CpkBanKuai a
	                                    LEFT JOIN (
		                                    SELECT BanKuaiId, COUNT(1) AS CpCount
		                                    FROM CpkCpBkRelation t1
			                                    JOIN dbo.CpkCaiPinBasicInfo t2 ON t2.CaiPinId = t1.CaiPinId
		                                    WHERE t2.IsEnable = 1
		                                    GROUP BY BanKuaiId
	                                    ) b
	                                    ON b.BanKuaiId = a.BanKuaiId  --菜品数量
	                                    LEFT JOIN (
		                                    SELECT BanKuaiId, COUNT(1) AS SpCount
		                                    FROM CpkCpBkRelation t1
			                                    JOIN dbo.CpkCaiPinBasicInfo t2 ON t2.CaiPinId = t1.CaiPinId
		                                    WHERE t2.IsEnable = 1 AND LEN(VideoUrl)>0 
		                                    GROUP BY BanKuaiId
	                                    ) c ON c.BanKuaiId = b.BanKuaiId  --视频数量
	 
	                                    LEFT JOIN (SELECT BanKuaiId,COUNT(1) AS ZtCount
				                                     FROM dbo.CpkZhuanTi t1 JOIN dbo.CpkZtBkRelation t2 ON t2.ZhuanTiId = t1.ZhuanTiId
			                                         WHERE t1.IsEnable = 1 GROUP BY BanKuaiId
				                                     ) d  ON d.BanKuaiId = a.BanKuaiId    --专题数量
                                        WHERE a.IsEnable =1 
                                    {0}", sqlwhere);
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
        /// 发布与不发布
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string Publish(dynamic requestData)
        {
            ReturnJson r = new ReturnJson();
            int action = requestData.Flag;
            string bankuaiId = requestData.BanKuaiId;
            if (action == 0 || action == 1)
            {
                if (!string.IsNullOrEmpty(bankuaiId))
                {
                    var sql = string.Format("UPDATE dbo.CpkBanKuai SET IsPublish =  {0} WHERE BanKuaiId IN ({1})", action, bankuaiId);
                    int a = dataContext.ExecuteNonQuery(CommandType.Text, sql);
                    r.status = "succ";
                    r.message = a.ToString() + (action == 1 ? "条发布成功" : "条已取消发布");
                }
                else
                {
                    r.message = "BanKuaiId参数不能为空";
                }
            }
            else
            {
                r.message = "Flag参数只能为0或1";
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
            string bankuaiId = requestData.BanKuaiId;
            if (action == 0 || action == 1)
            {
                if (!string.IsNullOrEmpty(bankuaiId))
                {
                    var sql = string.Format("UPDATE dbo.CpkBanKuai SET IsEnable =  {0} WHERE BanKuaiId IN ({1})", action, bankuaiId);
                    int a = dataContext.ExecuteNonQuery(CommandType.Text, sql);
                    r.status = "succ";
                    r.message = a.ToString() + (action == 1 ? "条已启用" : "条已删除");
                }
                else
                {
                    r.message = "SecondId参数不能为空";
                }
            }
            else
            {
                r.message = "Flag参数只能为0或1";
            }
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string Edit(dynamic requestData)
        {
            ReturnJson r = new ReturnJson();
            int bankuaiId = requestData.BanKuaiId;
            string bankuainame = requestData.BanKuaiName;

            if (bankuainame.Length > 0 && bankuainame.Length <= 14)
            {
                var sql = string.Format(@"UPDATE dbo.CpkBanKuai SET BanKuaiName = '{0}' ,UpdateDate = GETDATE()  WHERE BanKuaiId = {1} ; ", bankuainame, bankuaiId);
                int a = dataContext.ExecuteNonQuery(CommandType.Text, sql);
                if (a > 0)
                {
                    r.message = "更新成功";
                    r.status = "succ";
                }
                else
                {
                    r.message = "更新失败";
                }
            }
            else if (bankuainame.Length > 14)
            {
                r.message = "板块名称长度已超过14";
            }
            else
            {
                r.message = "BanKuaiName不能为空";
            }
            return JsonConvert.SerializeObject(r);
        }
    }
}

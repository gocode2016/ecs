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
    /// 后台分类管理
    /// </summary>
    public class CategoryController : ApiController
    {
        private CreditContext db = new CreditContext();
        private SqlHelper dataContext = new SqlHelper();


        /// <summary>
        /// 根据查询条件获取分类
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string Query(dynamic requestData)
        {
            ReturnJson r = new ReturnJson();
            int page = requestData.page;
            int pagesize = requestData.pagesize;
            string sqlwhere = "";
            if ( !string.IsNullOrEmpty(requestData.startdate.ToString()))
            {
                sqlwhere += " and b.CreateDate >='" + requestData.startdate.ToString()+"'";
            }
            if (!string.IsNullOrEmpty(requestData.enddate.ToString()))
            {
                sqlwhere += " and b.CreateDate <='" + requestData.enddate.ToString() + "'";
            }
            if (!string.IsNullOrEmpty(requestData.keyword.ToString()))
            {
                sqlwhere += " and (a.FirstName like '%" + requestData.keyword.ToString() + "%' or b.SecondName like '%" + requestData.keyword.ToString() + "%')";
            }
            var sql = string.Format(@"SELECT  row_number() over(order by a.FirstId,b.SecondId) RowId,a.FirstId,
                                    a.FirstName,b.SecondId,b.SecondName,b.CreateDate,CASE b.IsPublish WHEN 1 THEN '已发布' ELSE '未发布' END IsPublish,ISNULL(c.total,0 ) Total
                                    FROM dbo.CpkFirstCategory  a JOIN dbo.CpkSecondCategory  b ON b.FirstId = a.FirstId
                                    LEFT JOIN (SELECT SecondCategoryId,COUNT(1) total 
                                                FROM CpkCaiPinCategory GROUP BY SecondCategoryId) c ON b.SecondId =  c.SecondCategoryId
                                    WHERE b.IsEnable = 1 
                                    {0}", sqlwhere);
            var sqlpage = PageHelper.GetPagerSql(page, pagesize, sql); //分页sql
            var q = dataContext.ExecuteDataTable(CommandType.Text, sqlpage); //分页的数据
            var totalcount = dataContext.GetCount(CommandType.Text,sql ); //总条数
            var toatlpage = PageHelper.GetTotalPage(totalcount, pagesize);
            string data = JsonConvert.SerializeObject(q);
            r.status = "succ";
            r.totalcount = totalcount;
            r.totalpage = toatlpage;
            r.data = JsonConvert.DeserializeObject(data.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 创建一级分类
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string CreateFirst(dynamic requestData)
        {
            ReturnJson r = new ReturnJson();
            string q = JsonConvert.SerializeObject(requestData.List);
            List<CpkFirstCategory> model = JsonConvert.DeserializeObject<List<CpkFirstCategory>>(q);
            foreach(var item in model)
            {
                var sql = string.Format("Select 1 from CpkFirstCategory Where FirstName = '{0}'", item.FirstName);
                var count = dataContext.GetCount(CommandType.Text, sql);
                if (count == 0)
                {
                    db.CpkFirstCategory.Add(item);
                    db.SaveChanges();
                }
                else
                { 
                    r.message += item.FirstName + "已存在;";
                }
                if (string.IsNullOrEmpty(r.message))
                {
                    r.status = "succ";
                    r.message = "添加成功";
                }
            }
           return JsonConvert.SerializeObject(r);
        }



        /// <summary>
        /// 获取一级分类
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetFirst()
        {
            ReturnJson r = new ReturnJson();
            string sql = "SELECT FirstId,FirstName FROM dbo.CpkFirstCategory ORDER BY FirstId";
            var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
            string data = JsonConvert.SerializeObject(q);
            r.status = "succ";
            r.data = JsonConvert.DeserializeObject(data.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            return JsonConvert.SerializeObject(r);
        }


        /// <summary>
        /// 根据一级获取二级分类
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetSecondByFirst(int FirstId = 0)
        {
            ReturnJson r = new ReturnJson();
            string sql = string.Format(@"SELECT SecondId,SecondName FROM dbo.CpkSecondCategory 
                                        WHERE IsEnable = 1  AND FirstId = {0} ORDER BY SecondId", FirstId);
            var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
            return JsonConvert.SerializeObject(q);
        }


        /// <summary>
        /// 创建一级分类
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string CreateSecond(dynamic requestData)
        {
            ReturnJson r = new ReturnJson();
            string q = JsonConvert.SerializeObject(requestData.List);
            List<CpkSecondCategory> model = JsonConvert.DeserializeObject<List<CpkSecondCategory>>(q);
            foreach (var item in model)
            {
                var sql = string.Format(@"SELECT 1 FROM dbo.CpkFirstCategory  a JOIN dbo.CpkSecondCategory  b ON b.FirstId = a.FirstId
                                            WHERE b.SecondName = '{0}' ", item.SecondName);
                var count = dataContext.GetCount(CommandType.Text, sql);
                if (count == 0)
                {
                    db.CpkSecondCategory.Add(item);
                    db.SaveChanges();
                }
                else
                {
                    r.status = "fail";
                    r.message += item.SecondName + "已存在";
                }
                if (string.IsNullOrEmpty(r.message))
                {
                    r.status = "succ";
                    r.message = "添加成功";
                }
            }
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
            string SecondId = requestData.SecondId;
            if (action == 0 || action == 1)
            {
                if (!string.IsNullOrEmpty(SecondId))
                {
                    var sql = string.Format("UPDATE dbo.CpkSecondCategory SET IsPublish =  {0} WHERE SecondId IN ({1})", action, SecondId);
                    int a = dataContext.ExecuteNonQuery(CommandType.Text, sql);
                    r.status = "succ";
                    r.message = a.ToString() + (action == 1 ? "条发布成功" : "条已取消发布");
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
        /// 启用与删除
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string Enable(dynamic requestData)
        {
            ReturnJson r = new ReturnJson();
            int action = requestData.Flag;
            string SecondId = requestData.SecondId;
            if (action == 0 || action == 1)
            {
                if (!string.IsNullOrEmpty(SecondId))
                {
                    var sql = string.Format("UPDATE dbo.CpkSecondCategory SET IsEnable =  {0} WHERE SecondId IN ({1})", action, SecondId);
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
            int firstid = requestData.FirstId;  
            int secondid = requestData.SecondId;
            string firstname = requestData.FirstName;
            string secondname = requestData.SecondName;
            if (!string.IsNullOrEmpty(firstname) && !string.IsNullOrEmpty(secondname))
            {
//                var sql = string.Format(@"UPDATE dbo.CpkFirstCategory SET FirstName = '{0}'  WHERE FirstId = {1} ;
//                                          UPDATE dbo.CpkSecondCategory SET SecondName = '{2}' WHERE SecondId = {3}", firstname, firstid, secondname, secondid);
//                dataContext.ExecuteNonQuery(CommandType.Text, sql);
                string sql = string.Format(@"SELECT FirstId FROM dbo.CpkFirstCategory WHERE FirstName = '{0}'", firstname);
                DataTable dt = SqlHelper2.ExecuteDataTable(sql);
                if (dt.Rows.Count>0) //判断是否有该名称的一级分类
                {
                    //sql = string.Format("SELECT  COUNT(1) FROM dbo.CpkSecondCategory WHERE FirstId ={0} AND SecondName = '{1}'", dt.Rows[0]["FirstId"].ToString(), secondname);
                    //if (SqlHelper2.GetCountByCountSql(sql) > 0) //判断是否该一级分类下已存在该二级分类
                    //{
                    //    r.message = "已存在改分类";
                    //}

                    sql = string.Format(@"UPDATE dbo.CpkSecondCategory SET SecondName = '{0}',FirstId ={1} WHERE SecondId = {2}", secondname, dt.Rows[0]["FirstId"].ToString(), secondid);
                    dataContext.ExecuteNonQuery(CommandType.Text, sql);
                    r.message = "更新成功";
                    r.status = "succ";

                }
                else
                {
                    r.message = "不存在该名称的一级分类";
                }

            }
            else
            {
                r.message = "分类名称不能为空";
            }
            return JsonConvert.SerializeObject(r);
        }
    }
}

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
//using CSRedis;

namespace CaiPinKuAPI.Controllers
{
    /// <summary>
    /// 菜品库微信端
    /// </summary>
    public class WeiXinController : ApiController
    {
        private CreditContext db = new CreditContext();
        private SqlHelper dataContext = new SqlHelper();
        //RedisClient redis = new RedisClient("redis007-sshop-uat.sfqn68.0001.cnn1.cache.amazonaws.com.cn", 6379);


        [HttpGet]
        public string GetGuiGeListByProductId(int ProductId = 0,string OpenId ="")
        {
            string sql = string.Format(@"SELECT s.SpecificationId, s.ProductId, s.Amount, s.Unit, PKI.ProductName
	                                        , s.SpecificationPicUrl, s.VisitCount, PKI.ProductFirstId, PKI.ProductSecondId
	                                        , CASE 
		                                        WHEN L.Id IS NULL THEN 'f'
		                                        ELSE 't'
	                                        END AS IsPraise, Display.IsDisplay
                                        FROM Specification s
	                                         JOIN ProductKuInfo PKI ON s.ProductId = PKI.ProductId
	                                        LEFT JOIN (
		                                        SELECT Id,SpecificationId
		                                        FROM SpecificationPraiseLog
		                                        WHERE OpenId = '{0}'
	                                        ) L ON L.SpecificationId = s.SpecificationId
	                                        LEFT JOIN SpecificationConf sConf ON s.SpecificationId = sConf.SpecificationId
	                                        LEFT JOIN (
		                                        SELECT SpecificationConfId, SUM(IsDisplay) AS IsDisplay
		                                        FROM (
			                                        SELECT SpecificationConfId
				                                        , CASE IsDisplay
					                                        WHEN 'true' THEN 1
					                                        ELSE 0
				                                        END AS IsDisplay
			                                        FROM SpecificationConfVC
		                                        ) A
		                                        GROUP BY SpecificationConfId
	                                        ) Display
	                                        ON sConf.SpecificationConfId = Display.SpecificationConfId
                                        WHERE Display.IsDisplay > 0 AND PKi.ProductId ={1}", OpenId, ProductId);
            var r = SqlHelper2.ExecuteDataTable(sql);
            return JsonConvert.SerializeObject(r);
        }


        /// <summary>
        /// 菜品库微信端 首页加载各个板块内容
        /// </summary>
        /// <param name="openid">微信openid</param>
        /// <returns></returns>
        [HttpGet]
        public string Index(string openid = "")
        {
            string sql = string.Format(@"select BanKuaiId,BanKuaiName,CaiPinId,HotelLongitude,HotelAtitude,CaiPinName,Images,Max(LlCount) LlCount,MAX(Content) Sbly,
		                                        VideoImage,VideoUrl,UpdateDate ,IsCollect
                                        FROM (select *,ROW_NUMBER() over(partition by BanKuaiId order by UpdateDate desc) rowNum
                                        from (SELECT a.CaiPinId,a.HotelLongitude,a.HotelAtitude, a.CaiPinName,a.Images,a.VideoImage,a.VideoUrl,a.UpdateDate,c.BanKuaiId,c.BanKuaiName,
                                        ISNULL(k.LlCount,0) LlCount,r.Content, CASE g.RecordAction WHEN 'collect' THEN 1 ELSE 0 END IsCollect ,'' PCaiPuUrl
                                        FROM dbo.CpkCaiPinBasicInfo a 
                                        JOIN dbo.CpkCpBkRelation b ON b.CaiPinId = a.CaiPinId  AND b.IndexShow = 1  
                                        JOIN dbo.CpkBanKuai c ON c.BanKuaiId = b.BanKuaiId 
                                        LEFT JOIN dbo.CpkCaiPinRichInfo r ON r.CaiPinId = a.CaiPinId AND r.TypeId = 4 --  TypeId 类别在CpkRichTextType中
                                        LEFT JOIN ViewCpkTongji   k  ON a.CaiPinId = k.RecordKeyId AND k.RecordKeyType='菜品'
                                        LEFT JOIN dbo.CpkTongJi g ON a.CaiPinId=g.RecordKeyId  
						                                        AND g.RecordKeyType='菜品' AND g.RecordAction='collect' AND g.OpenId ='{0}'  
                                        WHERE  a.IsEnable =1 AND a.IsPublish =1 AND c.IsEnable =1 AND c.IsPublish =1 ) 
                                        t ) as s 
                                        WHERE s.rowNum <= 6
                                        GROUP by BanKuaiId,HotelLongitude,HotelAtitude,BanKuaiName,CaiPinId,CaiPinName,Images,VideoImage,VideoUrl,UpdateDate,IsCollect
                                        ORDER by BanKuaiId,UpdateDate desc
                                        ", openid);
            var r = SqlHelper2.ExecuteDataTable(sql);
            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 获取当月人气板块的菜品   这个板块单独的逻辑
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetDyrq(string openid = "")
        {

            string sql = string.Format(@"SELECT TOP 3 a.CaiPinId,a.CaiPinName,a.Images,b.Content Sbly,
		                                         ISNULL(k.LlCount,0) LlCount,  --浏览数量
                                        CASE g.RecordAction WHEN 'collect' THEN 1 ELSE 0 END IsCollect ,--是否收藏
	                                        ISNULL(k.LlCount,0)*0.2+ISNULL(k.ScCount,0)*0.8 RQ ----当月人气计算逻辑  浏览量*0.2 + 收藏量*0.8
                                        FROM 
                                        dbo.CpkCaiPinBasicInfo a
                                        LEFT JOIN dbo.CpkCaiPinRichInfo b ON b.CaiPinId = a.CaiPinId AND b.TypeId = 1
                                        LEFT JOIN ViewCpkTongji   k  ON a.CaiPinId = k.RecordKeyId AND k.RecordKeyType='菜品' 
                                        LEFT JOIN dbo.CpkTongJi g ON a.CaiPinId=g.RecordKeyId  
					                                        AND g.RecordKeyType='菜品' AND g.RecordAction='collect' AND g.OpenId ='{0}'  
                                        WHERE CONVERT(VARCHAR(7),a.CreateDate,121) =  CONVERT(VARCHAR(7),GETDATE(),121) 
                                        ORDER BY RQ DESC", openid);
            var r = SqlHelper2.ExecuteDataTable(sql);
            return JsonConvert.SerializeObject(r);

        }
        /// <summary>
        /// 菜品库微信端 首页获取全部分类(菜谱)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetAllCategory()
        {
            string sql = string.Format(@"SELECT FirstId,FirstName 
                                        FROM dbo.CpkFirstCategory  
                                        WHERE FirstName IN('菜系','菜式','食材') ORDER BY FirstId"); //根据用户需求只显示菜系 菜式 食材三种
            var q = DataTableToList.ConvertTo<RCategory>(dataContext.ExecuteDataTable(CommandType.Text, sql));
            List<RCategory> rlist = new List<RCategory>();
            foreach (var item in q)
            {
//                sql = string.Format(@"SELECT c.SecondId,c.SecondName,COUNT(1) Total FROM dbo.CpkCaiPinBasicInfo a 
//                                JOIN dbo.CpkCaiPinCategory b  ON b.CaiPinId = a.CaiPinId
//                                JOIN dbo.CpkSecondCategory c  ON c.SecondId=b.SecondCategoryId 
//                                WHERE a.IsEnable=1 AND  a.IsPublish =1  AND c.IsEnable =1 AND  c.IsPublish =1  AND c.FirstId ={0}
//                                GROUP BY c.SecondId,c.SecondName", item.FirstId);
                sql = string.Format(@"SELECT a.SecondId,a.SecondName, SUM(CASE WHEN b.CaiPinId IS NULL THEN 0 ELSE 1 END)  Total 
                                        FROM CpkSecondCategory a 
                                        LEFT JOIN CpkCaiPinCategory b ON a.SecondId=b.SecondCategoryId
                                        LEFT JOIN CpkCaiPinBasicInfo c ON c.CaiPinId = b.CaiPinId AND c.IsEnable =1 AND  c.IsPublish =1
                                        WHERE a.IsEnable=1 AND  a.IsPublish =1  AND a.FirstId ={0}
                                        GROUP BY a.SecondId,a.SecondName", item.FirstId);
                DataTable dt2 = dataContext.ExecuteDataTable(CommandType.Text, sql);
                item.SecondList = dt2;
                item.Total = dt2.Rows.Count > 0 ? Convert.ToInt16(dt2.Compute("Sum(Total)", "true")) : 0;
                rlist.Add(item);
            }
            return JsonConvert.SerializeObject(rlist);
        }


        /// <summary>
        /// 菜品库微信端 根据二级分类获取菜品数量
        /// </summary>
        /// <param name="SecondId">二级分类id</param>
        /// <returns></returns>
        [HttpGet]
        public string GetCaiPinCountBySecondId(string SecondId)
        {
            string sqlwhere = "";
            if (!string.IsNullOrEmpty(SecondId))
            {
                sqlwhere = string.Format(" and c.SecondId IN ({0})", SecondId);
            }
            string sql = string.Format(@"SELECT COUNT(1)
                                        FROM dbo.CpkCaiPinBasicInfo a 
                                        JOIN dbo.CpkCaiPinCategory b ON b.CaiPinId = a.CaiPinId
                                        JOIN dbo.CpkSecondCategory c ON c.SecondId = b.SecondCategoryId
                                        WHERE a.IsEnable =1 AND  a.IsPublish =1 AND c.IsEnable = 1 AND  c.IsPublish =1 {0}  ", sqlwhere);
            return SqlHelper2.ExecuteScalar(CommandType.Text, sql).ToString();
        }

        /// <summary>
        /// 根据二级分类获取菜品列表
        /// </summary>
        /// <param name="requestData">post数据</param>
        /// <returns></returns>
        [HttpPost]
        public string GetCaiPinListBySecondId(dynamic requestData)
        {
            LogHelper.WriteMsgByDay("WeiXinController-GetCaiPinListBySecondId-log:" + requestData.ToString());
            ReturnJson r = new ReturnJson();
            string secondId = requestData.SecondId;
            string orderBy = requestData.OrderBy;
            string openid = requestData.OpenId;
            int page = requestData.page;
            int pagesize = requestData.pagesize;
            if (orderBy == "RenQi")
            {
                orderBy = "t.LlCount DESC ,t.UpdateDate DESC";
            }
            else
            {
                orderBy = "t.UpdateDate DESC ,t.LlCount DESC";
            }
            string sqlwhere = "";
            if (!string.IsNullOrEmpty(secondId))
            {
                sqlwhere =string.Format(" and c.SecondId IN ({0})",secondId);
            }
            string sql = string.Format(@"SELECT row_number() over(order by {0}) RowId ,t.* FROM  (
                                        SELECT  DISTINCT  a.UpdateDate,a.CaiPinId,a.CaiPinName,a.Images, ISNULL(k.LlCount,0) LlCount,
                                        ISNULL(s.IsCollect,0) IsCollect,ISNULL(s.IsDianZan,0) IsDianZan
                                        FROM dbo.CpkCaiPinBasicInfo a 
                                        JOIN dbo.CpkCaiPinCategory b ON b.CaiPinId = a.CaiPinId
                                        JOIN dbo.CpkSecondCategory c ON c.SecondId = b.SecondCategoryId
                                        LEFT JOIN ViewCpkTongji   k  ON a.CaiPinId = k.RecordKeyId  AND RecordKeyType='菜品'  
                                        LEFT JOIN dbo.ViewCpkDzOrCollect s ON s.RecordKeyId=a.CaiPinId  AND s.OpenId = '{2}' AND s.RecordKeyType='菜品'  
                                        WHERE a.IsEnable =1 AND  a.IsPublish =1 AND c.IsEnable = 1  AND  c.IsPublish =1 {1}  ) t", orderBy, sqlwhere, openid);
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
        /// 首页顶部或者每个版块顶部通过关键字查找菜品名称搜索
        /// </summary>
        /// <param name="requestData">post数据</param>
        /// <returns></returns>
        [HttpPost]
        public string GetCaiPinNameListByParms(dynamic requestData)
        {
            LogHelper.WriteMsgByDay("WeiXinController-GetCaiPinNameListByParms-log:" + requestData.ToString());
            string cpName = requestData.CpName;
            if (string.IsNullOrEmpty(cpName))
            {
                return "[]";
            }

            string bkName = requestData.BkName;
            string sql = "";
            if (string.IsNullOrEmpty(bkName))
            {
                sql = string.Format(@"SELECT CaiPinId,CaiPinName 
                                        FROM dbo.CpkCaiPinBasicInfo 
                                        WHERE IsEnable =1 AND  IsPublish =1 AND  CaiPinName LIKE '%{0}%' ORDER BY UpdateDate desc ", cpName);
                var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
                return JsonConvert.SerializeObject(q);
            }
            else
            {
                sql = string.Format(@"SELECT a.CaiPinId,CaiPinName 
                                    FROM dbo.CpkCaiPinBasicInfo a 
                                    JOIN dbo.CpkCpBkRelation b ON b.CaiPinId = a.CaiPinId
                                    JOIN dbo.CpkBanKuai c ON c.BanKuaiId = b.BanKuaiId
                                    WHERE a.IsEnable =1 AND  a.IsPublish =1 AND c.IsEnable =1 AND c.IsPublish =1 AND
	                                    a.CaiPinName LIKE '%{0}%'  AND c.BanKuaiName = '{1}' ORDER BY a.UpdateDate  desc ", cpName, bkName);
                var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
                return JsonConvert.SerializeObject(q);
            }
        }

        /// <summary>
        /// 根据查询条件拼接sql语句
        /// </summary>
        /// <param name="caiPinName"></param>
        /// <param name="banKuaiName"></param>
        /// <param name="secondId"></param>
        /// <param name="orderBy"></param>
        private string GetCaiPinListSqlByParms(string caiPinName,string banKuaiName,string secondId,string orderBy,string openid = "")
        {
            string sql = "";

            //根据二级目录查询
            if (!string.IsNullOrEmpty(secondId))
            {
                if (orderBy == "RenQi")
                {
                    orderBy = "t.LlCount DESC ,t.UpdateDate DESC";
                }
                else
                {
                    orderBy = "t.UpdateDate DESC ,t.LlCount DESC";
                }
                sql = string.Format(@"SELECT row_number() over(order by {0}) RowId ,t.* FROM  (
                                        SELECT  DISTINCT  a.UpdateDate,a.CaiPinId,a.HotelLongitude,a.HotelAtitude,a.CaiPinName,a.Images, ISNULL(k.LlCount,0) LlCount,ISNULL(k.BfCount,0) BfCount,
					                                  ISNULL(s.IsCollect,0) IsCollect,ISNULL(s.IsDianZan,0) IsDianZan
                                            FROM dbo.CpkCaiPinBasicInfo a 
                                        JOIN dbo.CpkCaiPinCategory b ON b.CaiPinId = a.CaiPinId
                                        JOIN dbo.CpkSecondCategory c ON c.SecondId = b.SecondCategoryId
                                        LEFT JOIN ViewCpkTongji   k  ON a.CaiPinId = k.RecordKeyId AND k.RecordKeyType='菜品'  
                                        LEFT JOIN dbo.ViewCpkDzOrCollect s ON s.RecordKeyId=a.CaiPinId  AND s.OpenId = '{2}' AND s.RecordKeyType='菜品'     
                                        WHERE a.IsEnable =1 AND a.IsPublish =1 AND c.IsEnable = 1 AND c.IsPublish =1  and c.SecondId IN ({1})  ) t  ", orderBy, secondId, openid);
            }
            else if (!string.IsNullOrEmpty(caiPinName) && string.IsNullOrEmpty(banKuaiName)) //只根据菜品名称查询菜品列表
            {
                if (orderBy == "RenQi")
                {
                    orderBy = "t.LlCount DESC,t.UpdateDate DESC";
                }
                else
                {
                    orderBy = "t.UpdateDate DESC,t.LlCount DESC";
                }
                sql = string.Format(@"SELECT row_number() over(order BY {0}) RowId,t.* FROM (SELECT  
                            c.CaiPinId,c.HotelLongitude,c.HotelAtitude,c.CaiPinName,c.VideoUrl,c.VideoImage,c.Images,ISNULL(k.LlCount,0) LlCount,c.BanKuaiName,
                            c.UpdateDate  ,ISNULL(k.BfCount,0) BfCount,ISNULL(s.IsCollect,0) IsCollect,ISNULL(s.IsDianZan,0) IsDianZan 
                            FROM  dbo.CpkCaiPinBasicInfo c 
                            LEFT JOIN ViewCpkTongji   k  ON c.CaiPinId = k.RecordKeyId AND   ModuleName = '菜品库' AND RecordKeyType='菜品'    --浏览数量  人气
                             LEFT JOIN dbo.ViewCpkDzOrCollect s ON s.RecordKeyId=c.CaiPinId  AND s.OpenId = '{2}' AND s.RecordKeyType='菜品'  
                            WHERE 1=1 AND c.IsEnable =1 AND c.IsPublish =1 AND c.CaiPinName LIKE '%{1}%' ) t ", orderBy, caiPinName, openid);
            }
            else if (!string.IsNullOrEmpty(banKuaiName))  //查询板块内的菜品列表
            {
                string sqlwhere = "";
                if (!string.IsNullOrEmpty(caiPinName))
                {
                    sqlwhere += string.Format(" and c.CaiPinName Like '%{0}%'", caiPinName);
                }

                if (!string.IsNullOrEmpty(banKuaiName))
                {
                    sqlwhere += string.Format(" and a.BanKuaiName = '{0}'", banKuaiName);
                }
                if (orderBy == "RenQi")
                {
                    orderBy = "t.LlCount DESC,t.BfCount DESC,t.UpdateDate DESC";
                }
                else if (orderBy == "BfCount")
                {
                    orderBy = "t.BfCount DESC,t.LlCount DESC,t.UpdateDate DESC";
                }
                else
                {
                    orderBy = "t.UpdateDate DESC,t.BfCount DESC,t.LlCount DESC";
                }
                sql = string.Format(@"SELECT row_number() over(order BY {0}) RowId,t.* FROM (SELECT  c.CaiPinId,c.HotelLongitude,c.HotelAtitude,c.CaiPinName,c.VideoUrl,c.VideoImage,
                                            c.Images,ISNULL(k.LlCount,0) LlCount,c.BanKuaiName,
	                                         f.Content Sbly,c.UpdateDate  ,ISNULL(k.BfCount,0) BfCount,
                                            ISNULL(s.IsCollect,0) IsCollect,ISNULL(s.IsDianZan,0) IsDianZan,'' PCaiPuUrl
                                        FROM dbo.CpkBanKuai a  
                                        JOIN dbo.CpkCpBkRelation b ON b.BanKuaiId = a.BanKuaiId
                                        JOIN dbo.CpkCaiPinBasicInfo c ON c.CaiPinId = b.CaiPinId
		                                LEFT JOIN dbo.CpkCaiPinRichInfo f ON f.CaiPinId =c.CaiPinId AND f.TypeId =  4    --上榜理由
                                        LEFT JOIN ViewCpkTongji   k    ON c.CaiPinId = k.RecordKeyId AND   ModuleName = '菜品库' AND RecordKeyType='菜品'   --播放数量
                                        LEFT JOIN dbo.ViewCpkDzOrCollect s ON s.RecordKeyId=c.CaiPinId AND s.OpenId = '{2}'  AND s.RecordKeyType='菜品'  
                                        WHERE 1=1   AND a.IsEnable =1 AND a.IsPublish =1 AND c.IsEnable =1 AND c.IsPublish =1 {1}) t ", orderBy, sqlwhere, openid);
                    
            }
            return sql;
        }


        /// <summary>
        /// 通过板块名称分页获取包含的菜品
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetCaiPinListByParms(dynamic requestData)
        {
            LogHelper.WriteMsgByDay("WeiXinController-GetCaiPinListByParms-log:" + requestData.ToString());
            ReturnJson r = new ReturnJson();
            try
            {
                int page = requestData.page;  //当前页数
                int pagesize = requestData.pagesize;   //每页条数
                string caiPinName = requestData.CaiPinName;  //板块里搜索菜品
                string banKuaiName = requestData.BanKuaiName; //版块名称
                string secondId = requestData.SecondId;  //二级目录id
                string orderBy = requestData.OrderBy;   //排序方式
                string openid = requestData.OpenId;

                string sql = GetCaiPinListSqlByParms(caiPinName, banKuaiName, secondId, orderBy, openid);

                if (string.IsNullOrEmpty(sql))
                {
                    r.status = "succ";
                    r.totalcount = 0;
                    r.totalpage = 0;
                }
                else
                {
                    var sqlpage = PageHelper.GetPagerSql(page, pagesize, sql); //分页sql
                    var q = dataContext.ExecuteDataTable(CommandType.Text, sqlpage); //分页的数据
                    var totalcount = dataContext.GetCount(CommandType.Text, sql); //总条数
                    var toatlpage = PageHelper.GetTotalPage(totalcount, pagesize);
                    string data = JsonConvert.SerializeObject(q);
                    r.status = "succ";
                    r.totalcount = totalcount;
                    r.totalpage = toatlpage;
                    r.data = JsonConvert.DeserializeObject(data.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
                }
                return JsonConvert.SerializeObject(r);
            }
            catch (Exception ex)
            {
                LogHelper.WriteMsgByDay("GetCaiPinListByParms-error:"+ex.ToString());
                r.status = "fail";
                r.message = ex.ToString();
                return JsonConvert.SerializeObject(r);
            }
        }

        /// <summary>
        /// 获取星厨星菜板块的菜品列表
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetXcxcCaiPinList(dynamic requestData)
        {
            LogHelper.WriteMsgByDay("WeiXinController-GetXcxcCaiPinList-log:" + requestData.ToString());
            ReturnJson r = new ReturnJson();
            try
            {
                int page = requestData.page;  //当前页数
                int pagesize = requestData.pagesize;   //每页条数
                string caiPinName = requestData.CaiPinName;  //板块里搜索菜品
                string openid = requestData.OpenId;
                string type = requestData.Type;

                string sqlwhere = "";
                string sql = "";
                if (!string.IsNullOrEmpty(caiPinName))
                {
                    sqlwhere = " AND b.DishName like '%" + caiPinName + "%'";
                }
                if (type == "星厨团")
                {
                    sql = string.Format(@"SELECT row_number() over(order by UpdateTime) RowId ,t.* FROM
                                            (
                                            SELECT a.ChefStarName ChefName,a.ChefStarId ChefId,a.PicUrl,a.HeadPicUrl,a.ChefStarPosition Position,
                                                   a.HotelName,b.DishChefStarId CaiPinId, b.DishName,b.DishType,b.DishPicUrl,b.VisitCount LyCount,b.UpdateTime,
	                                                ISNULL(s.IsCollect,0) IsCollect, ISNULL(s.IsDianZan,0) IsDianZan
                                            FROM dbo.ChefStar a 
                                            JOIN DishChefStar b					ON b.ChefStarId = a.ChefStarId
                                            LEFT JOIN dbo.ViewCpkTongji k		ON k.RecordKeyId = b.DishChefStarId AND k.RecordKeyType = '菜品' 
                                            LEFT JOIN dbo.ViewCpkDzOrCollect s  ON s.RecordKeyId = b.DishChefStarId AND s.OpenId='{0}'  AND s.RecordKeyType = '菜品' 
                                            WHERE 1=1 {1}) t", openid, sqlwhere);
                }
                else  //导师团
                {
                     sql = string.Format(@"SELECT row_number() over(order by UpdateTime) RowId ,t.* FROM
                                            (
                                            SELECT a.TutorName ChefName,a.TutorId ChefId,a.PicUrl,a.HeadPicUrl,a.TutorPosition Position,
                                                   a.TutorHotel HotelName,b.TutorDishId CaiPinId,b.DishName,b.DishType ,b.DishPicUrl,b.VisitCount LyCount,b.UpdateTime,
	                                               ISNULL(s.IsCollect,0) IsCollect, ISNULL(s.IsDianZan,0) IsDianZan
                                            FROM dbo.Tutor a 
                                            JOIN dbo.DishTutor b					ON b.TutorId = a.TutorId
                                            LEFT JOIN dbo.ViewCpkTongji k		ON k.RecordKeyId = b.TutorDishId AND k.RecordKeyType = '菜品' 
                                            LEFT JOIN dbo.ViewCpkDzOrCollect s  ON s.RecordKeyId = b.TutorDishId AND s.OpenId='{0}'  AND s.RecordKeyType = '菜品' 
                                            WHERE 1=1 {1}) t", openid, sqlwhere);
                }
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
            catch (Exception ex)
            {
                LogHelper.WriteMsgByDay("GetStarCaiPinList-error:" + ex.ToString());
                r.status = "fail";
                r.message = ex.ToString();
                return JsonConvert.SerializeObject(r);
            }
        }



        /// <summary>
        /// 获取星厨 导师的菜品列表
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetTutorCaiPinList(dynamic requestData)
        {
            LogHelper.WriteMsgByDay("WeiXinController-GetTutorCaiPinList-log:" + requestData.ToString());
            ReturnJson r = new ReturnJson();
            try
            {
                int page = requestData.page;  //当前页数
                int pagesize = requestData.pagesize;   //每页条数
                string caiPinName = requestData.CaiPinName;  //板块里搜索菜品
                string openid = requestData.OpenId;

                string sqlwhere = "";
                if (!string.IsNullOrEmpty(caiPinName))
                {
                    sqlwhere = " AND b.DishName like '%" + caiPinName + "%'";
                }

                string sql = string.Format(@"SELECT row_number() over(order by UpdateTime) RowId ,t.* FROM
                                            (
                                            SELECT a.TutorName ,a.PicUrl,a.HeadPicUrl,a.TutorPosition,
	                                                a.TutorHotel,b.DishName,b.DishPicUrl,b.VisitCount LyCount,b.UpdateTime,
	                                                ISNULL(s.IsCollect,0) IsCollect, ISNULL(s.IsDianZan,0) IsDianZan
                                            FROM dbo.Tutor a 
                                            JOIN dbo.DishTutor b					ON b.TutorId = a.TutorId
                                            LEFT JOIN dbo.ViewCpkTongji k		ON k.RecordKeyId = b.TutorDishId AND k.RecordKeyType = '菜品' 
                                            LEFT JOIN dbo.ViewCpkDzOrCollect s  ON s.RecordKeyId = b.TutorDishId AND s.OpenId='{0}'  AND s.RecordKeyType = '菜品' 
                                            WHERE 1=1 {1}) t", openid, sqlwhere);
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
            catch (Exception ex)
            {
                LogHelper.WriteMsgByDay("GetStarCaiPinList-error:" + ex.ToString());
                r.status = "fail";
                r.message = ex.ToString();
                return JsonConvert.SerializeObject(r);
            }
        }


        /// <summary>
        /// 根据菜品id获取基本信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetCaiPinBasicInfoById(string caiPinId,string openid = "")
        {
            if (string.IsNullOrEmpty(caiPinId))
            {
                return "caipinid不能为空";
            }
            else
            {
                string sql = string.Format(@"
	                                SELECT a.CaiPinId,a.CaiPinName,b.FenLeiName CaiXi, c.FenLeiName CaiShi,c.FenLeiId CaiShiSecondIds,d.FenLeiName ShiCai,
                                        a.VideoUrl,a.VideoImage,a.Images,  CASE a.IsPublish WHEN 1 THEN '已发布' ELSE '未发布' END IsPublish,
                                        ISNULL(k.ScCount,0) ScCount,ISNULL(k.LlCount,0) LlCount,ISNULL(k.DzCount,0) DzCount,ISNULL(h.LyCount,0) LyCount,
                                            a.BanKuaiName,ISNULL(s.IsCollect,0) IsCollect,  ISNULL(s.IsDianZan,0) IsDianZan 
                                    FROM CpkCaiPinBasicInfo a 
                                    LEFT JOIN ViewCpkCaiPinFenZu b ON a.CaiPinId = b.CaiPinId AND b.FirstId =1
                                    LEFT JOIN ViewCpkCaiPinFenZu c ON a.CaiPinId = c.CaiPinId AND c.FirstId =2
                                    LEFT JOIN ViewCpkCaiPinFenZu d ON a.CaiPinId = d.CaiPinId AND d.FirstId =3
                                    LEFT JOIN ViewCpkTongji   k    ON a.CaiPinId = k.RecordKeyId  AND k.RecordKeyType='菜品'     
                                    LEFT JOIN ViewCpkDzOrCollect s ON a.CaiPinId = s.RecordKeyId  AND s.OpenId = '{0}'  AND s.RecordKeyType='菜品'   
                                    LEFT JOIN(SELECT KeyId,COUNT(1) LyCount FROM dbo.CpkLiuYan WHERE KeyId = 1 AND IsEnable =1 GROUP BY KeyId) h ON h.KeyId = a.CaiPinId 
                                    WHERE  a.CaiPinId = {1} AND  a.IsEnable =1   AND a.IsPublish =1
                                    ", openid,caiPinId);
                var q = dataContext.ExecuteDataTable(CommandType.Text, sql); 
                return JsonConvert.SerializeObject(q);
            }
        }

        /// <summary>
        /// 根据菜品id获取富文本信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetCaiPinRichInfoById(string caiPinId)
        {
            if (string.IsNullOrEmpty(caiPinId))
            {
                return "caipinid不能为空";
            }
            else
            {
                string sql = string.Format(@"SELECT b.TypeId,c.Type,b.Content ,a.HotelLongitude,a.HotelAtitude
                                        FROM  dbo.CpkCaiPinBasicInfo a JOIN CpkCaiPinRichInfo b ON b.CaiPinId = a.CaiPinId
                                        JOIN CpkRichTextType c ON b.TypeId = c.Id
                                        WHERE a.CaiPinId = {0} ORDER BY TypeId", caiPinId);
                var data = dataContext.ExecuteDataTable(CommandType.Text, sql);
                return JsonConvert.SerializeObject(data);
            }

        }

        /// <summary>
        /// 根据菜品id获取调料步骤信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetCaiPinMaterialById(string caiPinId)
        {
            ReturnJson r = new ReturnJson();
            string sql = string.Format(@"SELECT MaterialJson FROM dbo.CpkCaiPinBasicInfo WHERE  CaiPinId = {0}", caiPinId);
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
        /// 增加留言信息
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddLiuYanInfo(dynamic requestData)
        {
            LogHelper.WriteMsgByDay("WeiXinController-AddLiuYanInfo-log:" + requestData.ToString());
            string keyType = requestData.KeyType;
            string keyId = requestData.KeyId;
            string parentId = requestData.ParentId;
            string liuYan = requestData.LiuYan;
            string openId = requestData.OpenId;
            if (string.IsNullOrEmpty(openId) || string.IsNullOrEmpty(liuYan) ||
                string.IsNullOrEmpty(keyId) || string.IsNullOrEmpty(keyType))
            {
                return "参数不能为空";
            }
            else
            {
                string sql = string.Format(@"INSERT INTO dbo.CpkLiuYan
                                            ( ParentId ,KeyId ,KeyType ,OpenId , LiuYan )
                                            VALUES ({0},{1},'{2}','{3}','{4}')", parentId, keyId, keyType, openId, liuYan);
                SqlHelper2.ExecuteNonQuery(CommandType.Text, sql);
                return "succ";
            } 
        }
        /// <summary>
        /// 获取板块或者菜品留言
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetLiuYanInfo(dynamic requestData)
        {
            int page = requestData.page;
            int pagesize = requestData.pagesize;
            string keyType = requestData.KeyType;
            string keyId = requestData.KeyId;
            if (string.IsNullOrEmpty(keyType) || string.IsNullOrEmpty(keyId))
            {
                return "参数不能为空";
            }
            else
            {
                string sql = string.Format(@"SELECT row_number() over(order BY CreateDate) RowId,t.* 
                                            FROM (SELECT  a.Id,a.KeyId,a.OpenId,a.LiuYan, c.Nickname,c.HeadImgUrl,r.HotelName, a.CreateDate,COUNT(b.Id) ReplyCount 
                                                    FROM CpkLiuYan a
                                                    LEFT JOIN CpkLiuYan b ON a.Id=b.ParentId
		                                            JOIN dbo.OpenIdAssociated c ON c.OpenId = a.OpenId
		                                            JOIN dbo.RegistMember r ON r.MemberId = c.UserId AND c.UserType =2
                                                  WHERE a.ParentId=0 AND a.KeyId= {0} AND a.KeyType = '{1}'
                                                  GROUP BY a.Id,a.KeyId,a.OpenId,a.LiuYan ,a.CreateDate ,c.Nickname,c.HeadImgUrl,r.HotelName
                                                 ) t", keyId, keyType);
                String sqlpage = PageHelper.GetPagerSql(page, pagesize, sql); //分页sql
                var data = SqlHelper2.ExecuteDataTable(sqlpage); //分页的数据
                var totalcount = SqlHelper2.GetCountByNormalSql (sql); //总条数
                var toatlpage = PageHelper.GetTotalPage(totalcount, pagesize);
                return JsonConvert.SerializeObject(data);
            }
        }
        /// <summary>
        /// 获取回复留言的数据
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetLiuYanReplyInfo(dynamic requestData)
        {
            LogHelper.WriteMsgByDay("WeiXinController-GetLiuYanReplyInfo-log:" + requestData.ToString());
            int page = requestData.page;
            int pagesize = requestData.pagesize;
            string keyType = requestData.KeyType;
            string keyId = requestData.KeyId;
            string parentId = requestData.ParentId;
            if (string.IsNullOrEmpty(keyType) || string.IsNullOrEmpty(keyId) || string.IsNullOrEmpty(parentId))
            {
                return "参数不能为空";
            }
            else
            {
                string sql = string.Format(@"SELECT row_number() over(order BY CreateDate) RowId,t.* 
                                             FROM ( SELECT  a.Id,a.KeyId,a.OpenId,a.LiuYan, c.Nickname,c.HeadImgUrl,r.HotelName, a.CreateDate,COUNT(b.Id) ReplyCount 
                                                    FROM CpkLiuYan a
                                                    LEFT JOIN CpkLiuYan b ON a.Id=b.ParentId
		                                            JOIN dbo.OpenIdAssociated c ON c.OpenId = a.OpenId
		                                            JOIN dbo.RegistMember r ON r.MemberId = c.UserId AND c.UserType =2
                                                    WHERE a.ParentId={0} AND a.KeyId= {1} AND a.KeyType = '{2}' 
                                                    GROUP BY a.Id,a.KeyId,a.OpenId,a.LiuYan ,a.CreateDate,c.Nickname,c.HeadImgUrl,r.HotelName
                                                   ) t", parentId, keyId, keyType);
                String sqlpage = PageHelper.GetPagerSql(page, pagesize, sql); //分页sql
                var data = SqlHelper2.ExecuteDataTable(sqlpage); //分页的数据
                var totalcount = SqlHelper2.GetCountByNormalSql(sql); //总条数
                var toatlpage = PageHelper.GetTotalPage(totalcount, pagesize);
                return JsonConvert.SerializeObject(data);
            }
        }

        /// <summary>
        /// 增加例如浏览、收藏、点赞、播放记录
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddActionRecord(dynamic requestData)
        {
            LogHelper.WriteMsgByDay("WeiXinController-AddActionRecord-log:" + requestData.ToString());
            try
            {
                string openid = requestData.OpenId;
                string modulename = requestData.ModuleName;
                string recordAction = requestData.RecordAction;  //dianzan  collect scan share
                string recordKeyType = requestData.RecordKeyType;
                string recordKeyId = requestData.RecordKeyId;
                string actionFlag = requestData.ActionFlag;
                //redis.Select(3);
                //string rediskey = "Ecs:Caipinku:" + recordaction + ":CaipinId:" + RecordKeyId;
                //if (redis.Exists(rediskey))
                //{
                //    int count = Convert.ToInt32(redis.Get("Ecs:Caipinku:Collect:CaipinId:" + RecordKeyId));
                //    redis.Set("Ecs:Caipinku:Collect:CaipinId:" + RecordKeyId, count + 1);
                //}   
                string sql = "";
                if (!string.IsNullOrEmpty(modulename) && !string.IsNullOrEmpty(openid) &&
                    !string.IsNullOrEmpty(recordAction) && !string.IsNullOrEmpty(recordKeyId) && (actionFlag == "0" || actionFlag == "1"))
                {

                    if (recordAction == "dianzan" || recordAction == "collect")
                    {
                        if (actionFlag == "0")
                        {
                            sql = string.Format(@"DELETE FROM CpkTongJi 
                                                 WHERE ModuleName = '{0}' AND OpenId ='{1}' 
                                                AND RecordKeyId ='{2}' AND RecordAction ='{3}' AND RecordKeyType ='{4}' ",
                                                  modulename, openid, recordKeyId, recordAction, recordKeyType);

                        }
                        else
                        {
                            sql = string.Format(@"DELETE FROM CpkTongJi 
                                                 WHERE ModuleName = '{0}' AND OpenId ='{1}' 
                                                AND RecordKeyId ='{2}'   AND RecordKeyType ='{3}' AND RecordAction ='{4}' ;

                                                INSERT INTO dbo.CpkTongJi
                                            ( ModuleName ,OpenId ,RecordKeyId ,RecordKeyType ,RecordAction  ,IsEnable )
                                            VALUES  ('{0}','{1}','{2}','{3}','{4}',{5})",
                                                  modulename, openid, recordKeyId, recordKeyType, recordAction, actionFlag);
                        }
                    }
                    else
                    {
                        sql = string.Format(@"INSERT INTO dbo.CpkTongJi
                                        ( ModuleName ,OpenId ,RecordKeyId ,RecordKeyType ,RecordAction  ,IsEnable)
                                        VALUES  ('{0}','{1}','{2}','{3}','{4}',{5})",
                                              modulename, openid, recordKeyId, recordKeyType, recordAction, 1);
                    }
                    if (!string.IsNullOrEmpty(sql))
                    {
                        dataContext.ExecuteNonQuery(CommandType.Text, sql);
                        return "succ";
                    }
                    else
                    {
                        return "参数值有误";
                    }
                }
                else
                {
                    return "参数值有误";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        } 
    }
}

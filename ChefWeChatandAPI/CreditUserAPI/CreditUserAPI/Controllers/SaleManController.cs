using CreditUserAPI.Common;
using CreditUserAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace CreditUserAPI.Controllers
{
    /// <summary>
    /// 队员部分API
    /// </summary>
    public class SaleManController : ApiController
    {
        private CreditContext db = new CreditContext();
        private SqlHelper dataContext = new SqlHelper();

        #region 欣和队员新增
        /// <summary>
        /// 欣和队员新增
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns>队员名称</returns>
        [HttpPost]
        public string AddRegistSaleMan(dynamic requestData)
        {
            OpenIdAssociated openbase = new OpenIdAssociated();

            RegistSaleMan saleman = new RegistSaleMan();
            saleman.ProvinceId = requestData.ProvinceId;
            saleman.ProvinceName = requestData.ProvinceName;
            saleman.CityId = requestData.CityId;
            saleman.CityName = requestData.CityName;
            saleman.AreaId = requestData.AreaId;
            saleman.AreaName = requestData.AreaName;
            //saleman.RegionId = requestData.RegionId;
            //saleman.RegionName = requestData.RegionName;
            saleman.Position = requestData.Position;
            saleman.Telephone = requestData.Telephone;

            string sql = string.Format("SELECT COUNT(1) total FROM dbo.RegistSaleMan WHERE Telephone = '{0}'", saleman.Telephone);
            int salemancount = Convert.ToInt16(dataContext.ExecuteScalar(CommandType.Text, sql));
            if (salemancount == 0)
            {
                string tempPositon = "STL、RI、WSI、CSI"; //这四个职位的工号必填
                if (tempPositon.Contains(saleman.Position))
                {
                    string workNum = requestData.WorkNum;
                    if (string.IsNullOrEmpty(workNum))
                    {
                        return "该职位必须填写工号";
                    }
                }
                saleman.WorkNum = requestData.WorkNum;
                saleman.Name = requestData.Name;

                saleman.CardId = requestData.CardId;
                saleman.RegistDate = DateTime.Now;
                saleman.RegistState = 0;
                saleman.Remark = "";
                saleman.ImportDate = null;
                saleman.IsEnable = 0;


                db.RegistSaleMan.Add(saleman);
                db.SaveChanges();

                #region 获取用户OpenId
                if (requestData.OpenId != null)
                {
                    openbase.OpenId = requestData.OpenId;
                    openbase.UserId = saleman.SalemanId;
                    openbase.UserType = 1;
                    openbase.Nickname = requestData.Nickname;
                    openbase.HeadImgUrl = requestData.HeadImgUrl;
                    openbase.CreateDate = DateTime.Now;

                    db.OpenIdAssociated.Add(openbase);
                    db.SaveChanges();
                }
                #endregion

                return saleman.SalemanId.ToString();
            }
            else
            {
                return "该手机号已注册";
            }
        } 
        #endregion

        #region 分页查询队员
        /// <summary>
        /// 分页查询队员
        /// </summary>
        /// <param name="requestData">查询队员的参数</param>
        /// <returns></returns>
        [HttpPost]
        public string GetSaleManPageList(dynamic requestData)
        {
            string query = JsonConvert.SerializeObject(requestData);
            RequestSaleManModel model = JsonConvert.DeserializeObject<RequestSaleManModel>(query);

            int beginPage = (model.IndexPage - 1) * model.PageSize + 1;
            int endPage = model.IndexPage * model.PageSize;

            var nestSql = SqlParamHelper.Builder 
                .Append("select *, ROW_NUMBER() over(order by SalemanId desc) as num from RegistSaleMan as rs");

            if (model.RegistState > -1)
            {
                nestSql.Where(" rs.RegistState = " + model.RegistState + "");
            }
            if (model.RegionId > 0)
            {
                nestSql.Where(" rs.RegionId = " + model.RegionId + "");
            }
            if (!string.IsNullOrEmpty(model.SearchInfo))
            {
                nestSql.Where(" rs.WorkNum like '%" + model.SearchInfo + "%' or rs.Name like '%" + model.SearchInfo + "%' or rs.Telephone like '%" + model.SearchInfo + "%'");
            }
            if (!string.IsNullOrEmpty(model.RegistBeginTime.ToString()) && !string.IsNullOrEmpty(model.RegistEndTime.ToString()))
            {
                nestSql.Where(" rs.RegistDate >= '" + model.RegistBeginTime + "' and rs.RegistDate <= '" + model.RegistEndTime + "'");
            }
            //nestSql.Where(" rs.IsEnable = 0");
            var sql = SqlParamHelper.Builder
                .Append("select * from (" + nestSql.SQL + ") as saleman")
                .Append("where saleman.num between " + beginPage + " and " + endPage + "");

            var q = dataContext.ExecuteDataTable(CommandType.Text, sql.SQL, sql.Arguments);

            #region 计算总页数
            var countSql = string.Format("select Count(*) from(" + nestSql.SQL + ")as salemanCount");
            var count = dataContext.ExecuteScalar(CommandType.Text, countSql);

            //int row = Convert.ToInt32(count);
            //int pageCount = model.PageSize;
            //int sum = (row - 1) / pageCount + 1;//这样就计算好了页码数量，逢1进1   
            #endregion

            return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(q) + "}";

        } 
        #endregion

        #region 根据工号获取队员
        /// <summary>
        /// 根据工号获取队员
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetSaleManById(dynamic requestData)
        {
            string workNum = requestData.WorkNum;
            string sql = string.Format("select * from RegistSaleMan where WorkNum = '{0}'", workNum.Trim());
            var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
            return JsonConvert.SerializeObject(q);
        } 
        #endregion

        #region 队员修改信息
        /// <summary>
        /// 队员修改信息
        /// </summary>
        /// <param name="requestData">队员表的所有字段</param>
        /// <returns></returns>
        [HttpPost]
        public int UpdateSaleMan(dynamic requestData)
        {
            string query = JsonConvert.SerializeObject(requestData);
            RegistSaleMan saleman = JsonConvert.DeserializeObject<RegistSaleMan>(query);
            EntityState statebefore = db.Entry(saleman).State;
            db.Entry(saleman).State = EntityState.Modified;

            return db.SaveChanges();
        } 
        #endregion

        #region 获取队员详细信息
        /// <summary>
        /// 获取队员详细信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetSaleManInfo(dynamic requestData)
        {
            try
            {
                int salemanId = requestData.SalemanId;
                RegistSaleMan model = db.RegistSaleMan.Find(salemanId);
                var q = JsonConvert.SerializeObject(model);

                string sql = string.Format("Select * from [SalemanArea] Where SalemanId = {0}", salemanId);
                var area = dataContext.ExecuteDataTable(CommandType.Text, sql);

                return "{ \"Data\":" + JsonConvert.SerializeObject(q) + ", \"Area\":" + JsonConvert.SerializeObject(area) + "}";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        } 
        #endregion

        #region 批量审核队员
        /// <summary>
        /// 批量审核队员
        /// </summary>
        /// <param name="salemanIdList">队员ID数组</param>
        /// <returns></returns>
        [HttpPost]
        public string ReviewSaleman(string[] salemanIdList)
        {
            try
            {
                string sql = string.Empty;
                for (int i = 0; i < salemanIdList.Length; i++)
                {
                    sql = string.Format("Update RegistSaleMan Set RegistState = 1 Where SalemanId = {0}", salemanIdList[i]);
                    dataContext.ExecuteNonQuery(CommandType.Text, sql);
                }
            }
            catch (Exception ex)
            {
                return "Exc error";
            }

            return "true";
        } 
        #endregion

        #region 批量转移厨师
        /// <summary>
        /// 批量转移厨师
        /// </summary>
        /// <param name="salemanId">队员id</param>
        /// <param name="memberList">厨师ID列表</param>
        /// <returns></returns>
        [HttpPost]
        public string TransferMember(int salemanId, string[] memberList)
        {
            string updateSql = string.Empty;
            string modifySql = string.Empty;
            string addSql = string.Empty;

            if (memberList.Length <= 0 || salemanId <= 0) 
            {
                return "Request Parameter Error";
            }
            //string[] memberList = new string [2]{"10000","10003"};
            try
            {
                for (int i = 0; i < memberList.Length; i++)
                {
                    updateSql = string.Format("Update RegistCode Set MemberId = -1 Where MemberId = {0}", memberList[i]);
                    dataContext.ExecuteNonQuery(CommandType.Text, updateSql);
                    modifySql = string.Format("Update RegistMember Set RecommendId = {0} Where MemberId = {1}", salemanId, memberList[i]);
                    dataContext.ExecuteNonQuery(CommandType.Text, modifySql);
                    addSql = string.Format("Insert into RegistCode values(" + salemanId + "," + memberList[i] + ",2,'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')");
                    dataContext.ExecuteNonQuery(CommandType.Text, addSql);
                }
            }
            catch (Exception ex)
            {
                return "error";
            }

            return "Success";
        } 
        #endregion

        #region 队员推广日历
        /// <summary>
        /// 队员推广日历
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetSalemanContribute(dynamic requestData)
        {
            int month = requestData.Month;
            int year = requestData.Year;
            int salemanId = requestData.SalemanId;

            //if ((month == 0 && year == 0) || salemanId == 0)
            //{
            //    return "Request Parameter Error";
            //}

            if (month > 0 && year > 0)
            {
                var sql = SqlParamHelper.Builder
                        .Append("Select DATEPART(DD, UseDate) as [Day], COUNT(*) as Num  from RegistCode")
                        .Append("where DATEPART(MM, UseDate) = " + month + " and DATEPART(YY, UseDate) = " + year + " and SalemanId = " + salemanId + " and MemberId <> 0")
                        .Append("group by DATEPART(DD, UseDate)");
                var q = dataContext.ExecuteDataTable(CommandType.Text, sql.SQL);

                string sqlMonth = string.Format("Select COUNT(*) from RegistCode where DATEPART(YY, UseDate) = {2} and DATEPART(MM, UseDate) = {0} and SalemanId = {1} and MemberId <> 0", month, salemanId, year);
                var monthCount = dataContext.ExecuteScalar(CommandType.Text, sqlMonth);

                string sqlDay = string.Format("Select COUNT(*) from RegistCode where DATEPART(YY, UseDate) = {0} and DATEPART(MM, UseDate) = {1} and DATEPART(DD, UseDate) = {2} and SalemanId = {3} and MemberId <> 0", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, salemanId);
                var dayCount = dataContext.ExecuteScalar(CommandType.Text, sqlDay);

                return "{ \"MonthCount\":\"" + monthCount + "\",\"DayCount\":\"" + dayCount + "\",\"data\":" + JsonConvert.SerializeObject(q) + "}";
            }
            if (year > 0)
            {
                var sql = SqlParamHelper.Builder
                        .Append("Select DATEPART(MM, UseDate) as [Month], COUNT(*) as Num from RegistCode ")
                        .Append("where DATEPART(YY, UseDate) = " + year + " and SalemanId = " + salemanId + " and MemberId <> 0")
                        .Append("group by DATEPART(MM, UseDate)");

                var q = dataContext.ExecuteDataTable(CommandType.Text, sql.SQL);

                string sqlYear = string.Format("Select COUNT(*) from RegistCode where DATEPART(YY, UseDate) = {0} and SalemanId = {1} and MemberId <> 0", year, salemanId);
                var count = dataContext.ExecuteScalar(CommandType.Text, sqlYear);

                return "{ \"YearCount\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(q) + "}";
            }

            return "Excute Error";
        } 
        #endregion

        /// <summary>
        /// 根据所在区域获取队员
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        [HttpPost]
        public string FindSalemanByArea(int areaId)
        {
            if (areaId <= 0)
            {
                return "Request Error";
            }
            string sql = string.Format("Select Name, ProvinceName, CityName, AreaName, Telephone  from RegistSaleMan where AreaId = {0}", areaId);

            var q = dataContext.ExecuteDataTable(CommandType.Text, sql);

            return JsonConvert.SerializeObject(q);
        }

        /// <summary>
        /// 新增队员区域
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddSalemanArea(dynamic requestData)
        {
            try
            {
                string query = JsonConvert.SerializeObject(requestData);
                SalemanArea model = JsonConvert.DeserializeObject<SalemanArea>(query);
                db.SalemanArea.Add(model);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "Exc Success";
        }

        /// <summary>
        /// 删除队员区域
        /// </summary>
        /// <param name="SalemanAreaId"></param>
        /// <returns></returns>
        [HttpPost]
        public string RemoveSalemanArea(int? SalemanAreaId)
        {
            if (SalemanAreaId == null || SalemanAreaId == 0)
            {
                return "Exc Error";
            }

            string sql = string.Format("Delete SalemanArea where SalemanAreaId = {0}", SalemanAreaId);
            dataContext.ExecuteNonQuery(CommandType.Text, sql);

            return "Exc Success";
        }

    } 
}

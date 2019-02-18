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
using System.Web.Http;

namespace CreditUserAPI.Controllers
{
    /// <summary>
    /// 厨师详细信息API
    /// </summary>
    public class MemberProfileController : ApiController
    {
        private CreditContext db = new CreditContext();
        private SqlHelper dataContext = new SqlHelper();

        #region 完善客户信息
        /// <summary>
        /// 完善客户信息
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddMemberProfile(string requestData)
        {
            string query = JsonConvert.SerializeObject(requestData);
            MemberProfile model = JsonConvert.DeserializeObject<MemberProfile>(query);
            db.MemberProfile.Add(model);
            db.SaveChanges();
            return model.MemberId.ToString();
        } 
        #endregion

        #region 查询会员的所有信息
        /// <summary>
        /// 查询会员的所有信息
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetMemberProfile(dynamic requestData)
        {

            string sql = string.Format("select r.*, rs.Name, rp.*, a.AuthImg1, a.AuthImg2, a.AuthTime, MemberActiveState from RegistMember as r left join MemberProfile as rp on r.MemberId = rp.MemberId left join RegistSaleMan as rs on r.RecommendId = rs.SalemanId left join MemberRealAuth as a on r.MemberId = a.MemberId LEFT JOIN (SELECT m.MemberId,CASE WHEN DATEDIFF(DAY,MAX(m.CreatDate),GETDATE()) < 20 THEN 1 ELSE 2 END AS MemberActiveState FROM MemberIntegralDetail AS m GROUP BY m.MemberId) AS mid ON mid.MemberId = r.MemberId where r.MemberId = {0}", requestData.MemberId);
            var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
            return JsonConvert.SerializeObject(q);
        } 
        #endregion

        #region 微信端完善资料-基本信息
        /// <summary>
        /// 微信端完善资料-基本信息
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string UpdateMemberInfo(dynamic requestData)
        {
            try
            {

                #region 会员信息表字段修改
                var memberSql = SqlParamHelper.Builder
                    .Append("Update RegistMember Set ")
                    .Append(" ImgUrl = '" + requestData.ImgUrl + "',")
                    .Append(" MemberTelePhone = '" + requestData.MemberTelePhone + "'")
                    .Append(" Where MemberId = " + requestData.MemberId + "");

                dataContext.ExecuteNonQuery(CommandType.Text, memberSql.SQL);
                #endregion

                #region 会员资料表字段修改
                var profileSql = SqlParamHelper.Builder
                   .Append("Update MemberProfile Set Sex = " + requestData.Sex + ",")
                   .Append(" Nation = '" + requestData.Nation + "',")
                   .Append(" Birthday = '" + Convert.ToDateTime(requestData.Birthday) + "',")
                   .Append(" Nature = '" + requestData.Nature + "',")
                   .Append(" HomeProvinceId = " + requestData.ProvinceId + ",")
                   .Append(" HomeProvinceName = '" + requestData.ProvinceName + "',")
                   .Append(" HomeCityId = " + requestData.CityId + ",")
                   .Append(" HomeCityName = '" + requestData.CityName + "',")
                   .Append(" HomeAreaId = " + requestData.AreaId + ",")
                   .Append(" HomeAreaName = '" + requestData.AreaName + "',")
                   .Append(" HomeAddress = '" + requestData.HomeAddress + "',")
                   .Append(" Hobbys = '" + requestData.Hobbys + "',")
                   .Append(" IsMarry = " + requestData.IsMarry + ",")
                   .Append(" ChineseLv = '" + requestData.ChineseLv + "',")
                   .Append(" IsJob = " + requestData.IsJob + "")
                   .Append(" Where MemberId = " + requestData.MemberId + "");

                dataContext.ExecuteNonQuery(CommandType.Text, profileSql.SQL);
                #endregion

            }
            catch (Exception ex)
            {
                return ex.Message + "|" + ex.Source;
            }
            return "Exc Success";
        } 
        #endregion

        #region 微信端完善资料-从厨理念
        /// <summary>
        /// 微信端完善资料-从厨理念
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string UpdateCookIdea(dynamic requestData)
        {
            try
            {
                var profileSql = SqlParamHelper.Builder
                        .Append("Update MemberProfile Set FoodDemand = '" + requestData.FoodDemand + "',")
                        .Append(" JobReason = '" + requestData.JobReason + "',")
                        .Append(" IsShare = " + requestData.IsShare + "")
                        .Append(" Where MemberId = " + requestData.MemberId + "");
                dataContext.ExecuteNonQuery(CommandType.Text, profileSql.SQL);
            }
            catch (Exception ex)
            {
                return ex.Message + "|" + ex.Source;
            }
            return "Exc Success";
        } 
        #endregion

        #region 微信端完善资料-职业技能
        /// <summary>
        /// 微信端完善资料-职业技能
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string UpdateJobSkill(dynamic requestData)
        {
            try
            {
                var profileSql = SqlParamHelper.Builder
                .Append("Update MemberProfile Set Educations = '" + requestData.Educations + "',")
                .Append(" Major = '" + requestData.Major + "',")
                .Append(" Purchaser = '" + requestData.Purchaser + "',")
                .Append(" Speciality = '" + requestData.Speciality + "',")
                .Append(" School = '" + requestData.School + "',")
                .Append(" Teacher = '" + requestData.Teacher + "',")
                .Append(" ExpertInCuisine = '" + requestData.ExpertInCuisine + "',")
                .Append(" SuccessReasons = '" + requestData.SuccessReasons + "',")
                .Append(" Represent = '" + requestData.Represent + "',")
                .Append(" Development = '" + requestData.Development + "',")
                .Append(" WishCity = '" + requestData.WishCity + "',")
                .Append(" ChoicePower = '" + requestData.ChoicePower + "',")
                .Append(" IsCtrlTime = '" + requestData.IsCtrlTime + "',")
                .Append(" IsManyExp = " + requestData.IsManyExp + ",")
                .Append(" JoinDate = '" + Convert.ToDateTime(requestData.JoinDate) + "'")
                .Append(" Where MemberId = " + requestData.MemberId + "");

                dataContext.ExecuteNonQuery(CommandType.Text, profileSql.SQL);
            }
            catch (Exception ex)
            {
                return ex.Message + "|" + ex.Source;
            }
            return "Exc Success";
        } 
        #endregion

        #region 微信端完善资料-荣誉资格
        /// <summary>
        /// 微信端完善资料-荣誉资格
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string UpdateHonor(dynamic requestData)
        {

            try
            {
                var profileSql = SqlParamHelper.Builder
                       .Append("Update MemberProfile Set MatchName = '" + requestData.MatchName + "',")
                       .Append(" CardId = '"+ requestData.CardId +"',")
                       .Append(" MatchLv = '" + requestData.MatchLv + "',")
                       .Append(" MatchNum = '" + requestData.MatchNum + "',")
                       .Append(" Qualifications = '" + requestData.Qualifications + "',")
                       .Append(" DietitianLv = '" + requestData.DietitianLv + "',")
                       .Append(" ExposureCount = '" + requestData.ExposureCount + "',")
                       .Append(" Honor = '" + requestData.Honor + "'")
                       .Append(" Where MemberId = " + requestData.MemberId + "");

                dataContext.ExecuteNonQuery(CommandType.Text, profileSql.SQL);
            }
            catch (Exception ex)
            {
                return ex.Message + "|" + ex.Source;
            }

            return "Exc Success";
        } 
        #endregion

        #region 微信端完善资料-自我介绍
        /// <summary>
        /// 微信端完善资料-自我介绍
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public string UpdateIntroduction(dynamic requestData)
        {


            var profileSql = SqlParamHelper.Builder
                .Append("Update MemberProfile Set Introduction = '" + requestData.Introduction + "'")
                .Append(" Where MemberId = " + requestData.MemberId + "");

            dataContext.ExecuteNonQuery(CommandType.Text, profileSql.SQL);

            return "Exc Success";
        } 
        #endregion

        #region 微信完善资料-酒店信息
        /// <summary>
        /// 微信完善资料-酒店信息
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string UpdateHotelInfo(dynamic requestData)
        {
            int? memberId = requestData.MemberId;
            string position = requestData.Position;
            if (memberId == null || memberId <= 0)
            {
                return "reuqest error";
            }

            try
            {
                var memberSql = SqlParamHelper.Builder
                           .Append("Update RegistMember Set MemberName = '" + requestData.MemberName + "',")
                           .Append(" Position = '" + requestData.Position + "',")
                           .Append(" PositionType = '" + requestData.PositionType + "',")
                           .Append(" ProvinceId = " + requestData.ProvinceId + ",")
                           .Append(" ProvinceName = '" + requestData.ProvinceName + "',")
                           .Append(" CityId = " + requestData.CityId + ",")
                           .Append(" CityName = '" + requestData.CityName + "',")
                           .Append(" AreaId = " + requestData.AreaId + ",")
                           .Append(" AreaName = '" + requestData.AreaName + "',")
                           .Append(" HotelName = '" + requestData.HotelName + "',")
                           .Append(" HotelAddress = '" + requestData.HotelAddress + "'")
                           .Append(" Where MemberId = " + memberId + "");

                dataContext.ExecuteNonQuery(CommandType.Text, memberSql.SQL);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }


            

            ////判断是否有记录
            string sql2 = string.Format(@"SELECT COUNT(1) Total FROM MemberProfile  WHERE MemberId = {0}", memberId);
            int count = Convert.ToInt16(dataContext.ExecuteScalar(CommandType.Text, sql2));
            if (count == 0)
            {
                sql2 = string.Format(@"INSERT INTO dbo.MemberProfile( MemberId) VALUES ({0})", memberId);
                dataContext.ExecuteNonQuery(CommandType.Text, sql2);
            }

            //往下单地址 新增 酒店地址
            sql2 = string.Format(@"SELECT  COUNT(1) Total FROM dbo.Address 
                                   WHERE MemberId = {0} AND MemberAddress = '{1}' ", memberId, requestData.HotelAddress);
            count = Convert.ToInt16(dataContext.ExecuteScalar(CommandType.Text, sql2));
            if (count == 0)
            {
                sql2 = string.Format(@"UPDATE dbo.Address SET IsDefault = 0 WHERE MemberId = {0} ;
                                       INSERT INTO dbo.Address
                                      ( MemberId ,FullName ,Mobile ,ProvinceId ,ProvinceName ,
                                        CityId ,CityName ,AreaId ,AreaName ,MemberAddress , IsDefault )
                                      select {0},'{1}',MemberTelePhone,{2},'{3}',
                                       {4},'{5}',{6},'{7}','{8}',1 from RegistMember where  MemberId = {0}", memberId, requestData.MemberName, requestData.ProvinceId, requestData.ProvinceName,
                                           requestData.CityId, requestData.CityName, requestData.AreaId, requestData.AreaName, requestData.HotelAddress);
                dataContext.ExecuteNonQuery(CommandType.Text, sql2);
            }


            //根据不同的职位然后选择不同的完善信息（甲方要求）
            if (position == "调味品供货商")
            {
                var sql = SqlParamHelper.Builder
                   .Append("Update MemberProfile Set WholesaleName = '" + requestData.WholesaleName + "',")
                   .Append(" ShopOperateSize = '" + requestData.ShopOperateSize + "'")
                   .Append(" Where MemberId = " + memberId + "");

                dataContext.ExecuteNonQuery(CommandType.Text, sql.SQL);
            }
            else if (position == "美食爱好者")
            {

                var sql = SqlParamHelper.Builder
                    .Append("Update MemberProfile Set IsAnyChild = '" + requestData.IsAnyChild + "',")
                    .Append(" HomeIncome = '" + requestData.HomeIncome + "',")
                    .Append(" CookRate = '" + requestData.CookRate + "'")
                    .Append(" Where MemberId = " + memberId + "");

                dataContext.ExecuteNonQuery(CommandType.Text, sql.SQL);
            }
            else
            {

                var sql = SqlParamHelper.Builder
                    .Append("Update MemberProfile Set OperationType = '" + requestData.OperationType + "',")
                    .Append(" PerConsumption = '" + requestData.PerConsumption + "',")
                    .Append(" IsUseShinho = '" + requestData.IsUseShinho + "',")
                    .Append(" Purchaser = '" + requestData.Purchaser + "'")
                    .Append(" Where MemberId = " + memberId + "");


                dataContext.ExecuteNonQuery(CommandType.Text, sql.SQL);
            }

            return "Exc Success";
        } 
        #endregion

        /// <summary>
        /// 后台设置认证信息
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string SetAuthInfo(dynamic requestData)
        {
            try
            {
                var profileSql = SqlParamHelper.Builder
                    .Append("Update MemberProfile Set Qualifications = '" + requestData.Qualifications + "',")
                    .Append(" MatchLv = '" + requestData.MatchLv + "',")
                    .Append(" MatchNum = '" + requestData.MatchNum + "',")
                    .Append(" MatchName = '" + requestData.MatchName + "',")
                    .Append(" DietitianLv = '" + requestData.DietitianLv + "',")
                    .Append(" CardId = '" + requestData.CardId + "',")
                    .Append(" Birthday = '" + requestData.Birthday + "',")
                    .Append(" Honor = '" + requestData.Honor + "',")
                    .Append(" HomeProvinceId = " + requestData.HomeProvinceId + ",")
                    .Append(" HomeProvinceName = '" + requestData.HomeProvinceName + "',")
                    .Append(" HomeCityId = " + requestData.HomeCityId + ",")
                    .Append(" HomeCityName = '" + requestData.HomeCityName + "',")
                    .Append(" HomeAreaId = " + requestData.HomeAreaId + ",")
                    .Append(" HomeAreaName = '" + requestData.HomeAreaName + "',")
                    .Append(" HomeAddress = '" + requestData.HomeAddress + "'")
                    .Append(" Where MemberId = " + requestData.MemberId + "");

                dataContext.ExecuteNonQuery(CommandType.Text, profileSql.SQL);
            }
            catch (Exception ex)
            {
                return ex.Message;
                
            }
            return "Exc Success";
        }

        /// <summary>
        /// 队员修改
        /// </summary>
        /// <param name="requestDatas"></param>
        /// <returns></returns>
        [HttpPost]
        public string UpdateBindMemberInfo(dynamic requestData)
        {
            int? memberId = requestData.MemberId;
            string position = requestData.Position;
            if (memberId == null || memberId <= 0)
            {
                return "reuqest error";
            }

            try
            {
                var memberSql = SqlParamHelper.Builder
                        .Append("Update RegistMember Set MemberName = '" + requestData.MemberName + "',")
                        .Append(" MemberTelePhone = '" + requestData.MemberTelePhone + "',")
                        .Append(" Position = '" + requestData.Position + "',")
                        .Append(" PositionType = '" + requestData.PositionType + "',")
                        .Append(" ProvinceId = " + requestData.ProvinceId + ",")
                        .Append(" ProvinceName = '" + requestData.ProvinceName + "',")
                        .Append(" CityId = " + requestData.CityId + ",")
                        .Append(" CityName = '" + requestData.CityName + "',")
                        .Append(" AreaId = " + requestData.AreaId + ",")
                        .Append(" AreaName = '" + requestData.AreaName + "',")
                        .Append(" MemberCode = '" + requestData.MemberCode + "',")
                        .Append(" MemberCodeTime = '" + requestData.MemberCodeTime + "',")
                        .Append(" HotelName = '" + requestData.HotelName + "',")
                        .Append(" Remark = '" + requestData.Remark + "',")
                        .Append(" HotelAddress = '" + requestData.HotelAddress + "'")
                        .Append(" Where MemberId = " + memberId + "");

                dataContext.ExecuteNonQuery(CommandType.Text, memberSql.SQL);

                //根据不同的职位然后选择不同的完善信息（甲方要求）
                if (position == "调味品供货商")
                {
                    var sql = SqlParamHelper.Builder
                       .Append("Update MemberProfile Set WholesaleName = '" + requestData.WholesaleName + "',")
                       .Append(" ShopOperateSize = '" + requestData.ShopOperateSize + "'")
                       .Append(" Where MemberId = " + memberId + "");

                    dataContext.ExecuteNonQuery(CommandType.Text, sql.SQL);
                }
                else if (position == "美食爱好者")
                {

                    var sql = SqlParamHelper.Builder
                        .Append("Update MemberProfile Set IsAnyChild = '" + requestData.IsAnyChild + "',")
                        .Append(" HomeIncome = '" + requestData.HomeIncome + "',")
                        .Append(" CookRate = '" + requestData.CookRate + "'")
                        .Append(" Where MemberId = " + memberId + "");

                    dataContext.ExecuteNonQuery(CommandType.Text, sql.SQL);
                }
                else
                {

                    var sql = SqlParamHelper.Builder
                        .Append("Update MemberProfile Set OperationType = '" + requestData.OperationType + "',")
                        .Append(" PerConsumption = '" + requestData.PerConsumption + "',")
                        .Append(" IsUseShinho = '" + requestData.IsUseShinho + "',")
                        .Append(" Purchaser = '" + requestData.Purchaser + "'")
                        .Append(" Where MemberId = " + memberId + "");

                    dataContext.ExecuteNonQuery(CommandType.Text, sql.SQL);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "Exc Success";
        }

    }
}

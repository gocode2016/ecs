using CommonAPI.Common;
using CommonAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CommonAPI.Controllers
{
    /// <summary>
    /// 地址管理API
    /// </summary>
    public class AddressController : ApiController
    {
        private SqlHelper dataContext = new SqlHelper();
        private CreditContext db = new CreditContext();

        /// <summary>
        /// 添加地址
        /// </summary>
        /// <param name="requestData">地址信息</param>
        /// <returns></returns>
        [HttpPost]
        public string SaveAddress(dynamic requestData)
        {
            try
            {
                string query = JsonConvert.SerializeObject(requestData);
                Address model = JsonConvert.DeserializeObject<Address>(query);

                db.Address.Add(model);
                db.SaveChanges();
                return model.AddressId.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <param name="memberId">会员id</param>
        /// <param name="isDefault">是否默认地址</param>
        /// <param name="AddressId">地址ID</param>
        /// <returns></returns>
        [HttpPost]
        public string GetMemberAddressList(int memberId = 0, int isDefault = 0, int AddressId = 0)
        {
            var sql = SqlParamHelper.Builder
                    .Append("select * from [Address] ");

            sql.Where(" MemberId = '"+ memberId +"'");

            if (isDefault > 1)
            {
                sql.Where(" IsDefault = 1");
            }
            else if (AddressId > 0)
            {
                sql.Where(" AddressId = "+ AddressId +"");
            }
            sql.OrderBy(" AddressId desc");
            var q = dataContext.ExecuteDataTable(CommandType.Text,sql.SQL);
            return JsonConvert.SerializeObject(q);
        }

        /// <summary>
        /// 修改会员地址
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public int UpdateAddress(dynamic requestData)
        {
            string query = JsonConvert.SerializeObject(requestData);
            Address model = JsonConvert.DeserializeObject<Address>(query);

            if (model.IsDefault == 1)
            {
                string sql = string.Format("Update [Address] Set IsDefault = 0 Where MemberId = {0}", model.MemberId);
                dataContext.ExecuteNonQuery(CommandType.Text, sql);
            }

            EntityState statebefore = db.Entry(model).State;
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();

            return model.AddressId;
        }


    }
}

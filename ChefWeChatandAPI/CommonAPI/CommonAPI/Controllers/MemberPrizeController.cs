using CommonAPI.Common;
using CommonAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace CommonAPI.Controllers
{
    public class MemberPrizeController : ApiController
    {
        private CreditContext db = new CreditContext();
        private SqlHelper dataContext = new SqlHelper();

        /// <summary>
        /// 批量保存中奖人数
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddMemberPrize(dynamic requestData)
        {
            try
            {
                string query = JsonConvert.SerializeObject(requestData);
                List<MemberPrize> memberList = JsonConvert.DeserializeObject<List<MemberPrize>>(query);

                db.MemberPrize.AddRange(memberList);
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "Exc Success";
        }

        /// <summary>
        /// 根据奖项获取中奖人员
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetMemberByDrawPrizeId(int drawPrizeId)
        {
            if (drawPrizeId <= 0)
            {
                return "Exc Error";
            }

            var sql = SqlParamHelper.Builder
                    .Append("select m.MemberId, r.MemberTelePhone, o.Nickname, o.OpenId, o.HeadImgUrl, d.DrawName, d.PrizeName, d.PrizeImg from MemberPrize as m")
                    .Append("left join OpenIdAssociated as o on m.MemberId = o.UserId")
                    .Append("left join RegistMember as r on m.MemberId = r.MemberId")
                    .Append("left join DrawPrize as d on d.DrawPrizeId = m.DrawPrizeId")
                    .Append("where m.DrawPrizeId = "+ drawPrizeId +" and o.UserType = 2");

            var q = dataContext.ExecuteDataTable(CommandType.Text, sql.SQL);

            return JsonConvert.SerializeObject(q); ;
        }

        /// <summary>
        /// 查看是否中奖
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        [HttpPost]
        public string CheckIsPrize(int memberId)
        {
            string sql = string.Format("Select d.DrawName, d.PrizeName, d.PrizeImg from MemberPrize as m left join DrawPrize as d on d.DrawPrizeId = m.DrawPrizeId where MemberId = {0}", memberId);

            var q = dataContext.ExecuteDataTable(CommandType.Text, sql);

            return JsonConvert.SerializeObject(q);
        }

        /// <summary>
        /// 查看是否中奖
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        [HttpPost]
        public string PrizeByTelephone(string telephone)
        {
            string sql = string.Format("Select top 1 m.[MemberPrizeId], d.DrawName, d.PrizeName, d.PrizeImg, m.IsEnable from MemberPrize as m left join DrawPrize as d on d.DrawPrizeId = m.DrawPrizeId left join RegistMember as r on r.MemberId = m.MemberId where r.MemberTelePhone = '{0}'", telephone);

            var q = dataContext.ExecuteDataTable(CommandType.Text, sql);

            return JsonConvert.SerializeObject(q);
        }

        /// <summary>
        /// 领奖
        /// </summary>
        /// <param name="memberPrizeId"></param>
        /// <returns></returns>
        [HttpPost]
        public string HavePrize(int memberPrizeId) 
        {
            string sql = string.Format("Update [MemberPrize] Set IsEnable = 1 where [MemberPrizeId] = {0}", memberPrizeId);
            dataContext.ExecuteNonQuery(CommandType.Text, sql);
            return "Exc Success";
        }

        /// <summary>
        /// 获取厨师节总人数
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetMemberByCookeday() 
        {
            string sql = string.Format("select Count(*) from RegistMember where Remark like '%厨师节%'");
            var q = dataContext.ExecuteScalar(CommandType.Text, sql);
            return q.ToString(); ;
        }


    }
}

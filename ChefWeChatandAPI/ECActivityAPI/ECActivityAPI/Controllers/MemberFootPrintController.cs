using ECActivityAPI.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ECActivityAPI.Controllers
{
    /// <summary>
    /// 会员，队员足迹控制器
    /// </summary>
    public class MemberFootPrintController : ApiController
    {
        private CreditContext db = new CreditContext();
        private SqlHelper dataContext = new SqlHelper();
        private ImgHandle imgHandle = new ImgHandle();

        #region 获取会员，队员足迹--wx
        /// <summary>
        /// 获取会员，队员足迹--wx
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string FootPirntMember(dynamic requestData)
        {

            try
            {
                string openId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.OpenId));
                int pageIndex = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.PageIndex));
                int userType = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.UserType));
                string name = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.Name));
                string sql = "";
                //会员
                if (userType == 2)
                {
                    sql = string.Format(@"select  ECD.PageShort,ECD.PageDetail,ECD.CreateTime,ECD.OpenId,RM.MemberName,RM.MemberId from ECBrowseDetails  as ECD 
left join OpenIdAssociated as OI on  ECD.OpenId=OI.OpenId
left join RegistMember as RM on RM.MemberId=OI.UserId
where ECD.PageType=1 and ECD.OpenId='{0}' 

", openId);
                }
                //队员
                else
                {
                    sql = string.Format(@"select  ECD.PageShort,ECD.PageDetail,ECD.CreateTime,ECD.OpenId,RM.MemberName,RM.MemberId from ECBrowseDetails  as ECD 
left join OpenIdAssociated as OI on  ECD.OpenId=OI.OpenId
left join RegistMember as RM on RM.MemberId=OI.UserId
where ECD.PageType=1 
and ECD.OpenId in ( select OpenIdAssociated.OpenId from OpenIdAssociated  join ( select  MemberId from OpenIdAssociated
left join RegistCode on OpenIdAssociated.UserId=RegistCode.SalemanId
where OpenIdAssociated.OpenId='{0}')A on OpenIdAssociated.UserId=A.MemberId)


", openId);
                }
                //   order by ECD.CreateTime desc
                if (!string.IsNullOrEmpty(name))
                {
                    //会员
                    if (userType == 2)
                        sql = sql + "and ( ECD.PageShort like '%" + name + "%' or ECD.PageDetail like '%" + name + "%'   )  order by ECD.CreateTime desc";

                   //队员
                    if (userType == 1)
                        sql = sql + "and ( ECD.PageShort like '%" + name + "%' or ECD.PageDetail like '%" + name + "%'  or RM.MemberName  like  '%" + name + "%' )  order by ECD.CreateTime desc"; 
                 }
                else
                    sql += "    order by ECD.CreateTime desc";
                var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
                var lastq = q;
                if (pageIndex > 0)
                    lastq = imgHandle.GetPagedTable(q, pageIndex, 10);

                return JsonConvert.SerializeObject(lastq, DateTimeConvent.DateTimehh());
            }
            catch (Exception)
            {
                return "No";
            }
        } 
        #endregion
    }
}

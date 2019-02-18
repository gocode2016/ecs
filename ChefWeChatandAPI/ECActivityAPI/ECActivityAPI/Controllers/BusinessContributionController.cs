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
    /// 生意贡献度
    /// </summary>
    public class BusinessContributionController : ApiController
    {
        private SqlHelper dataContext = new SqlHelper();


        #region 生意贡献度列表--生意贡献度--wx
        /// <summary>
        /// 生意贡献度列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetBCList(dynamic requestData)
        {

            try
            {
                int sId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.SalemanId));
                int memberId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.MemberId));

                string m = "";
                if (memberId > 0)
                    m = " and MemberId=" + memberId;


                DateTime dT = JsonConvert.DeserializeObject<DateTime>(JsonConvert.SerializeObject(requestData.Day));

                DateTime qian = dT.AddDays(1 - dT.Day);
                DateTime hou = dT.AddDays(1 - dT.Day).AddMonths(1).AddDays(0);
                string sql = "";
//                 sql = string.Format(@"select A.GoodsName,A.total,BC.Unit,BC.Price,A.total*BC.Price as zongjia 
// from 
//    ( select replace( case when CHARINDEX('-',Remark)>0 then reverse(substring(reverse(Remark),1,charindex('-',reverse(Remark)) - 1)) else Remark end ,' ','')  as GoodsName,COUNT(Remark) as total 
//    from [MemberIntegralDetail] as MD
//    where  1=1
//    and MD.CreatDate>='{2}'  and MD.CreatDate<'{3}'
//    and (MD.IntegralSource='扫描活动商品' or MD.[IntegralSource]='扫描普通商品')
//    and MD.MemberId in (SELECT MemberId
//                  FROM  [RegistCode]
//                  where SalemanId={0}
//                  and [RegistCodeState]=2 {1} ) 
//    group by MD.Remark ) A 
// left join BusinessContribution as BC on A.GoodsName=BC.GoodsName
// where BC.GoodsName is not null
// order by A.total desc", sId, m, qian, hou);
                sql = string.Format(@"select B.GoodsName,B.total,BC.Unit,BC.Price,B.total*BC.Price as zongjia 
 from ( select A.GoodsName as GoodsName ,Count(GoodsName) as total  from 
    ( select replace( case when CHARINDEX('-',Remark)>0 then reverse(substring(reverse(Remark),1,charindex('-',reverse(Remark)) - 1)) else Remark end ,' ','')  as GoodsName 
    from [MemberIntegralDetail] as MD
    where  1=1
    and MD.CreatDate>='{2}'  and MD.CreatDate<'{3}'
    and (MD.IntegralSource='扫描活动商品' or MD.[IntegralSource]='扫描普通商品')
    and MD.MemberId in (SELECT MemberId
                  FROM  [RegistCode]
                  where SalemanId={0}
                  and [RegistCodeState]=2 {1}) 
     ) A  group by A.GoodsName) B
 left join BusinessContribution as BC on B.GoodsName=BC.GoodsName
 where BC.GoodsName is not null
 order by B.total desc", sId, m, qian, hou);

                var lastq = dataContext.ExecuteDataTable(CommandType.Text, sql);
                return JsonConvert.SerializeObject(lastq);
            }
            catch (Exception)
            {
                return "No";
            }
        } 
        #endregion
    }
}

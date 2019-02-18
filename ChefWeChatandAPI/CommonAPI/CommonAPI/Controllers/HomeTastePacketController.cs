using CommonAPI.Common;
using CommonAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace CommonAPI.Controllers
{
    /// <summary>
    /// 201802月随机红包API
    /// </summary>
    public class HomeTastePacketController : ApiController
    {
        static private object olock = new object();
        private SqlHelper dataContext = new SqlHelper();
        private CreditContext db = new CreditContext();
        static private Random rand = new Random();

        private DateTime beginTime = DateTime.Parse("2018-01-30 10:00");
        private DateTime EndTime = DateTime.Parse("2018-02-10 23:59");

        #region 抽取随机红包
        /// <summary>
        /// 抽取随机红包
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddRedPacket(dynamic requestData)
        {
            try
            {
                // 随机红包数
                int red = 0;
                //随机数范围
                int randomMax = 1001;
                int randomMin = 100;
                //总红包数
                int totalRed = 200000;
                

                //DateTime beginTime = DateTime.Parse("2018-02-07 10:00");
                //DateTime EndTime = DateTime.Parse("2018-02-07 23:59");
                DateTime dayNow = DateTime.Now;
                if (dayNow < beginTime || dayNow >= EndTime)
                    return "0";



                var sql = "  select sum(RedPacket) as sumRedPacket from [dbo].[HomeTastePacket]";
                var dataTable = dataContext.ExecuteDataTable(CommandType.Text, sql);

                Thread.Sleep(400);
                //总积分
                int CountRed = 0;
                var dd = JsonConvert.SerializeObject(dataTable.Rows[0][0]);
                if (dd != "null")
                    CountRed = Convert.ToInt32(dd);

                if (CountRed - randomMin > totalRed)
                    return "-1";

                lock (olock)
                {

                    red = rand.Next(randomMin, randomMax);

                    //防止数据大于总红包
                    if (CountRed + red >= totalRed)
                        red = totalRed - CountRed;


                    HomeTastePacket redPacket = JsonConvert.DeserializeObject<HomeTastePacket>(JsonConvert.SerializeObject(requestData));

                    //首先判断是否抽过随机红包
                    var q = (from v in db.HomeTastePacket
                             where v.OpenId == redPacket.OpenId && v.MemberId == redPacket.MemberId
                             select v).FirstOrDefault();

                    Thread.Sleep(400);
                    if (q == null)
                    {
                        redPacket.RedPacket = red;
                        db.HomeTastePacket.Add(redPacket);
                        db.SaveChanges();
                        return red.ToString();
                    }
                    return "-2";
                }
            }
            catch (Exception)
            {
                return "-3";
            }
        } 
        #endregion

        #region 获取获奖列表，前500条
        /// <summary>
        /// 获取获奖列表，前500条
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string ListRedPacket()
        {
            var sql = @"select distinct  OI.HeadImgUrl,RM.MemberName,H.RedPacket,H.CreateTime from HomeTastePacket as H 
left join OpenIdAssociated as OI on H.OpenId=OI.OpenId
left join RegistMember as RM on H.MemberId=RM.MemberId
order by H.CreateTime 
";
          var d=  dataContext.ExecuteDataTable(CommandType.Text, sql);
          var iso = new IsoDateTimeConverter();
          iso.DateTimeFormat = "M月dd日  HH:mm";
            return JsonConvert.SerializeObject(d,iso);
        }
        #endregion

        #region 是否能抽积分
        /// <summary>
        /// 是否能抽积分
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string IsDrawRed(dynamic requestData)
        {
            try
            {
                //DateTime beginTime = DateTime.Parse("2018-02-07 10:00");
                //DateTime EndTime = DateTime.Parse("2018-02-07 23:59");
                DateTime dayNow = DateTime.Now;
                if (dayNow < beginTime || dayNow >= EndTime)
                    return "No";
                HomeTastePacket redPacket = JsonConvert.DeserializeObject<HomeTastePacket>(JsonConvert.SerializeObject(requestData));

                var q = (from v in db.HomeTastePacket
                         where v.OpenId == redPacket.OpenId
                         select v).FirstOrDefault();
                if (q == null)
                    return "OK";
                else
                    return "No";

            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 春节积分红包是否转发朋友圈（仅限朋友圈）
        /// <summary>
        /// 春节积分红包是否转发朋友圈（仅限朋友圈）
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string IsWXTranspond(dynamic requestData)
        {
            try
            {
                DateTime dayNow = DateTime.Now;
                if (dayNow < beginTime || dayNow >= EndTime)
                    return "No";

                ECWXTranspondDetails wx = JsonConvert.DeserializeObject<ECWXTranspondDetails>(JsonConvert.SerializeObject(requestData));
                if (wx.TranspondType == 1)
                    return "No";

                var q = (from v in db.ECWXTranspondDetails
                         where v.ECBrowseURL == wx.ECBrowseURL && v.OpenId == wx.OpenId &&v.TranspondType==2
                         select v).FirstOrDefault();
                if (q == null)
                    return "OK";
                else
                    return "No";
            }
            catch (Exception)
            {
                return "No";
            }
        }



        #endregion

        #region 是否转发朋友圏（仅限朋友圈）
        /// <summary>
        /// 是否转发朋友圏（仅限朋友圈）
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string IsCommTranspond(dynamic requestData)
        {
            try
            {
                ECWXTranspondDetails wx = JsonConvert.DeserializeObject<ECWXTranspondDetails>(JsonConvert.SerializeObject(requestData));

                if (wx.TranspondType == 1)
                    return "No";

                var q = (from v in db.ECWXTranspondDetails
                         where v.ECBrowseURL == wx.ECBrowseURL && v.OpenId == wx.OpenId && v.TranspondType == 2
                         select v).FirstOrDefault();
                if (q == null)
                    return "OK";
                else
                    return "No";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

    }
}

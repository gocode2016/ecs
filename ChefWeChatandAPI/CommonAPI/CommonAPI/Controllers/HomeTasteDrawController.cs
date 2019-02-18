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
    /// 欣春赢家味抽奖API
    /// </summary>
    public class HomeTasteDrawController : ApiController
    {
        static private object olock = new object();

        private SqlHelper dataContext = new SqlHelper();
        private CreditContext db = new CreditContext();
        static private Random rand = new Random();
        #region 增加未注册用户信息--欣春赢家味抽奖--wx
        /// <summary>
        /// 增加未注册用户信息--欣春赢家味抽奖--wx
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string AddPrizeHTD(dynamic requestData)
        {
            try
            {
                var homeOpenId = JsonConvert.DeserializeObject<PrizeOpenIdHomeTasteDraw>(JsonConvert.SerializeObject(requestData));
                db.PrizeOpenIdHomeTasteDraw.Add(homeOpenId);
                db.SaveChanges();
                return "OK";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 中奖--欣春赢家味抽奖--wx
        /// <summary>
        ///中奖--欣春赢家味抽奖--wx
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string PrizeProbability(dynamic requestData)
        {
            try
            {
                Random random = rand;
                PrizeInfo dd = new PrizeInfo();
                int memberId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.MemberId));
                DateTime beginTime = DateTime.Parse("2018-01-22 10:00");
                DateTime EndTime = DateTime.Parse("2018-10-30 23:59");
                DateTime dayNow = DateTime.Now;
                //活动结束
                if (dayNow < beginTime || dayNow >= EndTime)
                    return "-3";
                var sqlCount = " select sum(Count) as sumRedPacket from [PrizeInfo]";
                var dataTable = dataContext.ExecuteDataTable(CommandType.Text, sqlCount);
                var count = Convert.ToInt32(dataTable.Rows[0][0]);
                if (count <= 0)
                    return "-3";


                lock (olock)
                {
                    //整个活动中是否中过奖
                    var IsPrize = (from v in db.PrizeInfoWinner
                                   where v.MemberId == memberId && v.PrizeRank > 0
                                   select v).FirstOrDefault();
                    Thread.Sleep(50);

                    #region 整个活动中没有中奖
                    if (IsPrize == null)
                    {

                        int max = 200;
                        int adjust = 7;
                        var prizeProbability = random.Next(1, max+1);
                        //prizeRank
                        int prizeRank = 0;
                        if (prizeProbability <= max * 5 / 100 - adjust)
                            prizeRank = 1;
                        if (prizeProbability > max * 5 / 100  && prizeProbability <= max * 13 / 100 - adjust)
                            prizeRank = 2;
                        if (prizeProbability > max * 13 / 100 && prizeProbability <= max * 23 / 100)
                            prizeRank = 3;
                        if (prizeProbability > max * 23 / 100 && prizeProbability <= max * 38 / 100)
                            prizeRank = 4;
                        if (prizeProbability > max * 38 / 100 && prizeProbability <= max * 58 / 100)
                            prizeRank = 5;
                        DateTime today = DateTime.Today;

                        //当天是否抽奖
                        var IsToday = (from v in db.PrizeInfoWinner
                                       where v.MemberId == memberId && v.CreateDay == today
                                       select v).FirstOrDefault();


                        #region 表明当天没有抽奖

                        if (IsToday == null)
                        {

                            #region 表明没有抽中奖

                            if (prizeRank == 0)
                            {
                                PrizeInfoWinner pw = new PrizeInfoWinner();
                                pw.MemberId = memberId;
                                pw.PrizeRank = 0;
                                pw.PrizeProbabilityNum = prizeProbability;
                                pw.CreateDay = today;
                                db.PrizeInfoWinner.Add(pw);
                                db.SaveChanges();
                            }
                            #endregion

                            #region 表明抽中奖了
                            else
                            {
                                //看看奖品信息
                                var IsCount = (from v in db.PrizeInfo
                                               where v.PrizeRank == prizeRank
                                               select v).FirstOrDefault();

                                //如果不是5等奖，看看奖品数量是否充足
                                if (prizeRank != 5)
                                {
                                    //前4个奖品数量不足,让其转为第5名
                                    if (IsCount.Count < 1)
                                        prizeRank = 5;
                                }
                                #region 更新奖品数量,并把抽奖信息放入到抽奖单元里面

                                var sql = string.Format("Update  PrizeInfo set Count=Count-1 where PrizeRank={0}", prizeRank);

                                int updateCount = dataContext.ExecuteNonQuery(CommandType.Text, sql);

                                if (updateCount > 0)
                                {
                                    PrizeInfoWinner pw = new PrizeInfoWinner();
                                    pw.MemberId = memberId;
                                    pw.PrizeRank = prizeRank;
                                    pw.PrizeProbabilityNum = prizeProbability;
                                    pw.CreateDay = today;
                                    db.PrizeInfoWinner.Add(pw);
                                    db.SaveChanges();
                                }

                                #endregion

                            }
                            #endregion

                        }
                        #endregion

                        //表明当天抽过奖了
                        else
                        {
                            prizeRank = -1; 
                            PrizeInfoWinner pw = new PrizeInfoWinner();
                            pw.MemberId = memberId;
                            pw.PrizeRank = prizeRank;
                            pw.PrizeProbabilityNum = 0;
                            pw.CreateDay = today;
                            db.PrizeInfoWinner.Add(pw);
                            db.SaveChanges();
                        }

                        Thread.Sleep(500);
                        return prizeRank.ToString();
                    }
                    #endregion
                    else
                    {
                        PrizeInfoWinner pw = new PrizeInfoWinner();
                        pw.MemberId = memberId;
                        pw.PrizeRank = -2;
                        pw.PrizeProbabilityNum = 0;
                        pw.CreateDay = DateTime.Today;
                        db.PrizeInfoWinner.Add(pw);
                        db.SaveChanges();
                        return "-2"; 
                    }
                }
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 当天是否抽奖--欣春赢家味抽奖--wx
        /// <summary>
        /// 当天是否抽奖--欣春赢家味抽奖--wx
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string IsTodyPrize(dynamic requestData)
        {
            try
            {
                int memberId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.MemberId));
                DateTime today = DateTime.Today;
               
                DateTime beginTime = DateTime.Parse("2018-01-22 10:00");
                DateTime EndTime = DateTime.Parse("2018-10-09 23:59");
                DateTime dayNow = DateTime.Now;
                //活动结束
                if (dayNow < beginTime || dayNow >= EndTime)
                    return "0";

                var sqlCount = " select sum(Count) as sumRedPacket from [PrizeInfo]";
                var dataTable = dataContext.ExecuteDataTable(CommandType.Text, sqlCount);
                var count = Convert.ToInt32(dataTable.Rows[0][0]);
                if (count <= 0)
                    return "0";


                var IsToday = (from v in db.PrizeInfoWinner
                               where v.MemberId == memberId && v.CreateDay == today
                               select v).FirstOrDefault();

                if (IsToday == null)
                    return "1";
                else
                    return "0";
            }
            catch (Exception)
            {
                return "0";
            }

        }
        #endregion

        #region 中奖名单--欣春赢家味抽奖--wx
        /// <summary>
        ///  中奖名单--欣春赢家味抽奖--wx
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetWinning()
        {
            var iso = new IsoDateTimeConverter();
            iso.DateTimeFormat = " M/dd HH:mm";
          
           string sql = @"select top 500 MemberId,
case   PrizeRank
 when '1' then '一等奖'  
 when '2' then '二等奖' 
 when '3' then '三等奖' 
 when '4' then '四等奖' 
 when '5' then '五等奖' 
 end  as PrizeRank,CreateTime from [CreditDB].[dbo].[PrizeInfoWinner] where PrizeRank>0  order by CreateTime desc";
          var q =dataContext.ExecuteDataTable(CommandType.Text,sql);
          return JsonConvert.SerializeObject(q, iso);
        }
        #endregion
    }
}

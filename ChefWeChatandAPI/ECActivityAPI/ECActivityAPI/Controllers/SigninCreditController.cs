using ECActivityAPI.Common;
using ECActivityAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ECActivityAPI.Controllers
{
   /// <summary>
   /// 每天签到获取积分
   /// </summary>
    public class SigninCreditController : ApiController
    {
        private CreditContext db = new CreditContext();
        private SqlHelper dataContext = new SqlHelper();

        #region 新增抽奖积分--签到--wx
        /// <summary>
        /// 新增抽奖积分--签到--wx
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddSigninCredit(dynamic requestData)
        {
            try
            {
                int MemberId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.MemberId));

                var datatime = DateTime.Today;

                var q = (from v in db.SigninCreditLog
                         where v.MemberId == MemberId && v.SigninData == datatime
                         select v
                           ).FirstOrDefault();
                if (q == null)
                {
                    SigninCreditLog s = new SigninCreditLog();
                    s.SigninData = datatime;
                    s.MemberId = MemberId;
                    db.SigninCreditLog.Add(s);
                  
                    SigninCredit sC= new SigninCredit();
                    var sccun = (from v in db.SigninCredit
                                       where v.MemberId==MemberId
                                 select v
                                   ).FirstOrDefault();
                    if (sccun == null)
                    {
                        sC.MemberId = MemberId;
                        sC.CreditsCount = 1;
                        sC.CreateTime = DateTime.Now;
                        sC.UpdateTime = DateTime.Now;
                        db.SigninCredit.Add(sC);
                    }
                    else
                    {
                        sccun.CreditsCount = sccun.CreditsCount + 1;
                        sccun.UpdateTime = DateTime.Now;
                        db.Entry<SigninCredit>(sccun).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    return "OK";
                }
                else
                    return "2";
            }
            catch (Exception)
            {
                return "No";
            }
          
        }
        
        #endregion

        #region 返回当天是否签到--签到--wx
        /// <summary>
        /// 返回当天是否签到--签到--wx
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string IsDraw(dynamic requestData)
        {
            try
            {
                int MemberId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.MemberId));

                var datatime = DateTime.Today;
                var q = (from v in db.SigninCreditLog
                         where v.MemberId == MemberId && v.SigninData == datatime
                         select v
                               ).FirstOrDefault();
                if (q == null)
                {
                    return "0";
                }
                else
                    return "1";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 标记哪天抽奖，新增--签到--wx
        /// <summary>
        /// 标记哪天抽奖，新增--签到--wx
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
       [HttpPost]
        public string FlagDraw(dynamic requestData)
        {
            try
            {
                int MemberId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.MemberId));

                var datatime = DateTime.Today;

                var q = (from v in db.SigninCreditLog
                         where v.MemberId == MemberId && v.SigninData == datatime
                         select v
                           ).FirstOrDefault();
                q.IsDraw = 1;
                db.Entry<SigninCreditLog>(q).State = EntityState.Modified;
                db.SaveChanges();
                return "OK";
            }
            catch (Exception)
            {
                return "No";
            }

        }
        #endregion

       #region 返回最近七天签到，当天是否签到--签到--wx
       /// <summary>
       /// 返回最近七天签到，当天是否签到--签到--wx
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
       public string GetSignin(dynamic requestData)
       {
           #region 最近7天签到情况
           int MemberId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.MemberId));
           var datatime = DateTime.Today;
            List<SigninCreditLog>listSCLog=new List<SigninCreditLog>();
            for (int i = 0; i < 7; i++)
            {
                var list = (from v in db.SigninCreditLog
                         where v.MemberId == MemberId && v.SigninData==datatime
                         select v).FirstOrDefault();
                if (list == null)
                {
                    SigninCreditLog sclog = new SigninCreditLog();
                    sclog.Id = 0;
                    sclog.IsDraw = 2;
                    sclog.SigninData = datatime;
                    sclog.MemberId = MemberId;
                    listSCLog.Add(sclog);
                }
                else
                    listSCLog.Add(list);
                datatime=datatime.AddDays(-1);
            }
            var d = (from v in listSCLog
                    select new
                    {
                        MemberId = v.MemberId,
                        SigninData = v.SigninData,
                        IsDraw = v.IsDraw
                    }); 
           #endregion


            #region 计算连续签到天数

            int continuousDay = 0;

            for (int i = 0; i < 7; i++)
            {
                if (i == 0 && listSCLog[i].SigninData == DateTime.Today && listSCLog[i].IsDraw == 2)
                    continue;
                if (listSCLog[i].IsDraw == 0)
                    continuousDay = continuousDay + 1;
                else
                    break;
            }





            #endregion







                #region 当天是否签到

                datatime = DateTime.Today;
           var isSigninData = "f";
           var scl = (from v in db.SigninCreditLog
                    where v.MemberId == MemberId && v.SigninData == datatime
                    select v ).FirstOrDefault();
           if (scl == null)
           {
               isSigninData = "f";
           }
           else {
               isSigninData = "t";
           }
           #endregion
           //return JsonConvert.SerializeObject(d, DateTimeConvent.DateTimedd());
           return "{\"SigninCredit\":" + JsonConvert.SerializeObject(d, DateTimeConvent.DateTimedd()) + ",\"IsSiginCredit\":\"" + isSigninData + "\",\"ContinuousDay\":" + continuousDay + "}";
       }
        #endregion
    }
}

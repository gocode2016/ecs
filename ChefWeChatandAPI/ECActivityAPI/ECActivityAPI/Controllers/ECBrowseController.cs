using ECActivityAPI.Common;
using ECActivityAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace ECActivityAPI.Controllers
{
    public class ECBrowseController : ApiController
    {
        private CreditContext db = new CreditContext();
        private SqlHelper dataContext = new SqlHelper();
        private object locker = new object();
        private static object Lock = new object();

        #region 增加页面访问--页面访问--wx
        /// <summary>
        /// 增加页面访问--页面访问--wx
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddECBrowseDetails(dynamic requestData)
        {
            try
            {
                string BrowseDetails = JsonConvert.SerializeObject(requestData);
                var q = JsonConvert.DeserializeObject<ECBrowseDetails>(BrowseDetails);
                lock (Lock)
                {
                    //详情页是否包含
                    var IsPV = (from v in db.ECBrowseDetails
                                where v.ECURL == q.ECURL
                                select v).FirstOrDefault();

                    //如果详情里面没有这
                    if (IsPV == null)
                    {
                        ECBrowse newqECBrowse = new ECBrowse();
                        newqECBrowse.ECURL = q.ECURL;
                        newqECBrowse.PV = 1;
                        newqECBrowse.UV = 1;
                        //db.ECBrowse.Add(newqECBrowse);
                        var sql = string.Format("INSERT INTO [dbo].[ECBrowse]([ECURL],[PV],[UV],[Transpond]) VALUES('{0}',{1},{2},0)", newqECBrowse.ECURL, newqECBrowse.PV, newqECBrowse.UV);
                        dataContext.ExecuteNonQuery(CommandType.Text, sql);
                    }
                    else
                    {
                        //查找EC总的是否有
                        var qECBrowse = (from v in db.ECBrowse
                                         where v.ECURL == q.ECURL
                                         select v).FirstOrDefault();

                        if (qECBrowse != null)
                        {
                            qECBrowse.PV = qECBrowse.PV + 1;

                            var IsUV = (from v in db.ECBrowseDetails
                                        where v.ECURL == q.ECURL && v.OpenId == q.OpenId
                                        select v).FirstOrDefault();
                            Thread.Sleep(500);
                            if (IsUV == null)
                            {
                                qECBrowse.UV = qECBrowse.UV + 1;
                            }
                            
                            var sqlPVUV = string.Format("UPDATE  [dbo].[ECBrowse]  SET  [PV] = {0},[UV] = {1}  WHERE Id={2}", qECBrowse.PV, qECBrowse.UV, qECBrowse.Id);
                            dataContext.ExecuteNonQuery(CommandType.Text, sqlPVUV);
                        }
                    }
                }
                    db.ECBrowseDetails.Add(q);
                    db.SaveChanges();
               
                return "OK";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 增加转发页面--页面访问--wx
        /// <summary>
        ///  增加转发页面--页面访问--wx
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddECWXTranspondDetails(dynamic requestData)
        {
            try
            {
                string BrowseDetails = JsonConvert.SerializeObject(requestData);
                var q = JsonConvert.DeserializeObject<ECWXTranspondDetails>(BrowseDetails);
                //是否包含
                var IsUV = (from v in db.ECWXTranspondDetails
                            where v.ECBrowseURL==q.ECBrowseURL && v.OpenId == q.OpenId
                            select v).FirstOrDefault();

                //查找EC总的是否有
                var qECBrowse = (from v in db.ECBrowse
                                 where v.ECURL == q.ECBrowseURL
                                 select v).FirstOrDefault();
                if (IsUV == null)
                {
                    if (qECBrowse == null)
                    {
                        ECBrowse newqECBrowse = new ECBrowse();
                        newqECBrowse.ECURL = q.ECBrowseURL;
                        newqECBrowse.Transpond = 1;
                        db.ECBrowse.Add(newqECBrowse);
                    }
                    else
                    {
                        qECBrowse.Transpond = qECBrowse.Transpond + 1;
                        db.Entry<ECBrowse>(qECBrowse).State = EntityState.Modified;
                    }
                }
                else
                {
                    qECBrowse.Transpond = qECBrowse.Transpond + 1;
                    db.Entry<ECBrowse>(qECBrowse).State = EntityState.Modified;
                }
                db.ECWXTranspondDetails.Add(q);
                db.SaveChanges();
                return "OK";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion
    }
}

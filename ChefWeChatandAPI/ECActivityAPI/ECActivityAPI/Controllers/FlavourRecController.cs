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
using System.Web.Http;

namespace ECActivityAPI.Controllers
{
    public class FlavourRecController : ApiController
    {
        private CreditContext db = new CreditContext();
        private SqlHelper dataContext = new SqlHelper();

        #region 新增活动调料--调料新增--web
        /// <summary>
        /// 新增活动调料--调料新增--web
        /// </summary>
        /// <param name="requestData">调料Json</param>
        /// <returns></returns>
        [HttpPost]
        public string AddFlavourRec(dynamic requestData)
        {
            try
            {
                string updateSingle = JsonConvert.SerializeObject(requestData);
                var q = JsonConvert.DeserializeObject<FlavourRec>(updateSingle);

                var qFlavourRec = (from v in db.FlavourRec
                                   where v.FlavourName == q.FlavourName && v.ChefActivityId == q.ChefActivityId &&v.FlavourType==q.FlavourType
                                   select v
                                     ).FirstOrDefault();
                if (qFlavourRec != null)
                    return "名称重复";
                q.CreateTime = DateTime.Now;
                q.UpdateTime = DateTime.Now;
                db.FlavourRec.Add(q);
                db.SaveChanges();
                return "OK";
            }
            catch (Exception)
            {
                return "No";
            }

        }
        #endregion


        #region 获取单一调料--调料编辑--web
        /// <summary>
        /// 获取单一调料--调料编辑--web
        /// </summary>
        /// <param name="fr">调料Id</param>
        /// <returns></returns>
        public string GetSingeByFR(int fr)
        {
            var q = (from v in db.FlavourRec
                     where v.FlavourRecId == fr
                     select new { 
                         FlavourRecId=v.FlavourRecId,
                         FlavourName=v.FlavourName,
                         FlavourPicUrl=v.FlavourPicUrl,
                         FlavourType = v.FlavourType
                     }).FirstOrDefault();
            return JsonConvert.SerializeObject(q);
        }

        #endregion


        #region 编辑调料--调料编辑--web
        /// <summary>
        /// 编辑调料--调料编辑--web
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string UpdateFlavour(dynamic requestData)
        {
            try
            {
                string updateSingle = JsonConvert.SerializeObject(requestData);
                var q = JsonConvert.DeserializeObject<FlavourRec>(updateSingle);


                var sql = string.Format("Update FlavourRec set FlavourName='{0}',FlavourPicUrl='{1}',FlavourType={2},UpdateTime='{4}' where FlavourRecId={3}", q.FlavourName, q.FlavourPicUrl, q.FlavourType, q.FlavourRecId,DateTime.Now.ToString());
                dataContext.ExecuteNonQuery(CommandType.Text,sql);
                return "OK";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion


        #region 获取调料（分页）列表--调料列表--web
        /// <summary>
        /// 获取调料（分页）列表--调料列表--web
        /// </summary>
        /// <param name="ca">活动Id</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="type">类别，1，导师，2，厨师  3，星厨</param>
        /// <param name="name">调料名称</param>
        /// <returns></returns>
        [HttpGet]
        public string GetSingeByCA(int ca,int pageIndex,int type,string name)
        {
            var q = (from v in db.FlavourRec
                     where v.ChefActivityId == ca
                     select new
                     {
                         FlavourRecId = v.FlavourRecId,
                         ChefActivityId = v.ChefActivityId,
                         FlavourName = v.FlavourName,
                         FlavourPicUrl = v.FlavourPicUrl,
                         FlavourType = v.FlavourType,
                     });
            if (type == 1 || type == 2 || type == 3)
                q = q.Where(v => v.FlavourType == type);
            if (!string.IsNullOrWhiteSpace(name))
                q = q.Where(v => v.FlavourName .Contains(name));
            int count = q.Count();
            var lastq = q.OrderBy(v => v.FlavourRecId).Skip((pageIndex - 1) * 10).Take(10);
            return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(lastq) + "}"; 
        }

        #endregion

        #region 获取添加调料时的全部--调料列表--web
        /// <summary>
        /// 获取添加调料时的全部--调料列表--web
        /// </summary>
        /// <param name="ca">活动Id</param>
        /// <param name="fType">调料类型 1，导师，2，厨师  3，星厨</param>
        /// <returns></returns>
        public string GetFRBycaType(int caId, int fType)
        {
            var q = (from v in db.FlavourRec
                     where v.ChefActivityId == caId && v.FlavourType == fType
                     select new {
                         FlavourRecId=v.FlavourRecId,
                         ChefActivityId=v.ChefActivityId,
                         FlavourName=v.FlavourName,
                         FlavourPicUrl=v.FlavourPicUrl,
                         FlavourType = v.FlavourType
                     });
            return JsonConvert.SerializeObject(q);
        }
        #endregion
    }
}

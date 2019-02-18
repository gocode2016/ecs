using CommonAPI.Common;
using CommonAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CommonAPI.Controllers
{
    public class DrawPrizeController : ApiController
    {
        private CreditContext db = new CreditContext();
        private SqlHelper dataContext = new SqlHelper();

        /// <summary>
        /// 新增奖品
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string SaveDrawPrize(dynamic requestData)
        {
            try
            {
                string query = JsonConvert.SerializeObject(requestData);
                DrawPrize model = JsonConvert.DeserializeObject<DrawPrize>(query);
                model.CreateDate = DateTime.Now;
                db.DrawPrize.Add(model);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "Exc Success";
        }

        /// <summary>
        /// 修改奖品
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string UpdateDrawPrize(dynamic requestData)
        {
            string query = JsonConvert.SerializeObject(requestData);
            DrawPrize model = JsonConvert.DeserializeObject<DrawPrize>(query);
            EntityState statebefore = db.Entry(model).State;
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();

            return "Excute Success";
        }

        /// <summary>
        /// 查询奖品列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetPrizeList()
        {
            var q = (from o in db.DrawPrize
                     where o.IsDelete == 0 && o.PrizeState == 0
                     select o).ToList();

            return JsonConvert.SerializeObject(q); ;
        }

        /// <summary>
        /// 查询奖品列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetAllPrizeList()
        {
            var q = (from o in db.DrawPrize
                     where o.IsDelete == 0
                     select o).ToList();

            return JsonConvert.SerializeObject(q); ;
        }

    }
}

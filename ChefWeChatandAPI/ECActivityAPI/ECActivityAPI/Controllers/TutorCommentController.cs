using ECActivityAPI.Common;
using ECActivityAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ECActivityAPI.Controllers
{
    /// <summary>
    /// 导师留言
    /// </summary>
    public class TutorCommentController : ApiController
    {
        private CreditContext db = new CreditContext();
        private SqlHelper dataContext = new SqlHelper();

        #region 新增导师留言，刷新留言信息--导师介绍--wx
        /// <summary>
        /// 新增导师留言--导师介绍--wx
        /// </summary>
        /// <param name="requestData">留言Json</param>
        /// <returns></returns>
        [HttpPost]
        public string AddTutorComment(dynamic requestData)
        {
            try
            {
                string addTutor = JsonConvert.SerializeObject(requestData);
                TutorComment model = JsonConvert.DeserializeObject<TutorComment>(addTutor);
                model.CreateTime = DateTime.Now;
                db.TutorComment.Add(model);
                db.SaveChanges();


                var q = (from v in db.TutorComment
                         where v.TutorId == model.TutorId
                         orderby v.CreateTime descending
                         select new
                         {
                             TutorCommentId = v.TutorCommentId,
                             MemberName = v.MemberName,
                             HeadPicUrl = v.HeadPicUrl,
                             Commemt = v.Comment,
                             CreateTime = v.CreateTime
                         });
                int count = q.Count();
                var lastq = q.OrderByDescending(v => v.CreateTime).Skip(0).Take(3);
                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd hh:mm";

                return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(lastq, iso) + "}"; 
            }
            catch (Exception e)
            {
                return "No";
            }
        } 
        #endregion

        #region 导师留言分页查询--导师介绍--wx，web
        /// <summary>
        /// 导师留言分页查询--导师介绍--wx，web
        /// </summary>
        /// <param name="tutorId">导师ID</param>
        /// <param name="pageIndex">页数</param>
        /// <returns></returns>
        [HttpGet]
        public string GetTutorCommentAllByTutorId(int tutorId,int pageIndex)
        {
            try
            {
                var q = (from v in db.TutorComment
                         where v.TutorId == tutorId
                         orderby v.CreateTime descending
                         select new
                         {
                             TutorCommentId=v.TutorCommentId,
                             MemberName = v.MemberName,
                             HeadPicUrl = v.HeadPicUrl,
                             Commemt = v.Comment,
                             CreateTime = v.CreateTime
                         });
                int count = q.Count();
                var lastq = q.OrderByDescending(v=>v.CreateTime).Skip((pageIndex - 1) *3).Take(3);
                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd hh:mm";

                return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(lastq, iso) + "}"; 
            }
            catch (Exception)
            {
                return "{\"count\":0,\"data\":[{\"message\":\"发生错误\"}]}";
            }
        }
        #endregion

        #region 导师留言删除--后台预留--web
        /// <summary>
        /// 导师留言删除--后台预留--web
        /// </summary>
        /// <param name="tutorCommentId"></param>
        [HttpPost]
        public void DelTutorComment([FromBody]int tutorCommentId)
        {
            var sql = string.Format("delete TutorComment where TutorCommentId={0}", tutorCommentId);
            
            dataContext.ExecuteNonQuery(CommandType.Text, sql);
        }
        #endregion

        #region 新增菜品留言,刷新留言信息--导师菜品详情--wx
        /// <summary>
        /// 新增菜品留言,刷新留言信息--导师介绍--wx
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddDishComment(dynamic requestData)
        {
            try
            {
                string addDishComment = JsonConvert.SerializeObject(requestData);
                 DishComment model = JsonConvert.DeserializeObject<DishComment>(addDishComment);
                db.DishComment.Add(model);
                db.SaveChanges();

                var q = (from v in db.DishComment
                         where v.DishId == model.DishId
                         orderby v.CreateTime descending
                         select v);
                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd hh:mm";
                int count = q.Count();
                var lastq = q.OrderByDescending(v => v.CreateTime).Skip(0).Take(3);

                var lastResult = (from v in lastq
                                  join p in db.OpenIdAssociated on v.OpenId equals p.OpenId
                                  join n in db.RegistMember on p.UserId equals n.MemberId
                                  select new {
                                      HeadPicUrl = v.HeadPicUrl,
                                      MemebName=v.MemberName,
                                      Comment = v.Comment,
                                      CreateTime = v.CreateTime,
                                      HotelName = n.HotelName
                                  });


                return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(lastResult, iso) + "}";
            }
            catch (Exception e)
            {
                return "No";
            }
        }
        #endregion

        #region 菜品留言分页查询--导师菜品详情--wx，web
        /// <summary>
        ///  菜品留言分页查询--导师菜品详情--wx，web
        /// </summary>
        /// <param name="dishId">菜品Id</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        [HttpGet]
        public string GetDishCommentByDishId(int dishId, int pageIndex, int pageSize)
        {
            try
            {
                var q = (from v in db.DishComment
                         where v.DishId == dishId
                         orderby v.CreateTime descending
                         select v);
                int count = q.Count();
                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd hh:mm";
                var lastq = q.OrderByDescending(v => v.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize);


                //查找HotelName
                var lastResult = (from v in lastq
                                  join p in db.OpenIdAssociated on v.OpenId equals p.OpenId
                                  join n in db.RegistMember on p.UserId equals n.MemberId
                                  select new
                                  {
                                      HeadPicUrl = v.HeadPicUrl,
                                      MemebName = v.MemberName,
                                      Comment = v.Comment,
                                      CreateTime = v.CreateTime,
                                      HotelName = n.HotelName
                                  });


                return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(lastResult, iso) + "}";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 菜品留言删除根据Id--后台预留--web
        /// <summary>
        /// 菜品留言删除根据Id--后台预留--web
        /// </summary>
        /// <param name="Id">菜品ID</param>
        /// <returns></returns>
        [HttpPost]
        public string DelDishCommentById(dynamic Id)
        {
            try
            {
                var sql = string.Format("delete DishComment where Id={0}", Id.Id);
                dataContext.ExecuteNonQuery(CommandType.Text, sql);
                return "OK";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 增加导师菜品点赞--导师菜品详情--wx
        /// <summary>
        ///  增加导师菜品点赞--导师菜品详情--wx
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddDishPrasieLog(dynamic requestData)
        {
            try
            {
                // 先判断当前用户是否点赞过本菜品，如果点赞了，不在增加，如果没有增加
                //都返回当前菜品的ID号和点赞数

                var q = JsonConvert.SerializeObject(requestData);
                DishPrasieLog m = JsonConvert.DeserializeObject<DishPrasieLog>(q);

                var qDishPrasieLog = (from v in db.DishPrasieLog
                                      where v.DishId == m.DishId&&v.OpenId==m.OpenId&&v.DishType==m.DishType
                                      select new
                                      {
                                          DishId = v.DishId
                                      }).FirstOrDefault();
              
                if (qDishPrasieLog == null)
                {
                    db.DishPrasieLog.Add(m);
                    db.SaveChanges();
                    var sql = string.Format(@"update DishTutor set PrasieCount=PrasieCount+1 where TutorDishId={0}", m.DishId);
                    dataContext.ExecuteNonQuery(CommandType.Text, sql);
                    var qTutorDishId = (from v in db.DishTutor
                                        where v.TutorDishId == m.DishId
                                        select new
                                        {
                                            TutorDishId = v.TutorDishId,
                                            PrasieCount = v.PrasieCount
                                        }).FirstOrDefault();
                    return JsonConvert.SerializeObject(qTutorDishId);
                }
               

                else
                {
                    var qTutorDishId = (from v in db.DishTutor
                                        where v.TutorDishId == m.DishId
                                        select new
                                        {
                                            TutorDishId = v.TutorDishId,
                                            PrasieCount = v.PrasieCount
                                        }).FirstOrDefault();
                    return JsonConvert.SerializeObject(qTutorDishId);
                }
            }
            catch (Exception)
            {
                return "No";
            }
          
        }

        #endregion

    }
}

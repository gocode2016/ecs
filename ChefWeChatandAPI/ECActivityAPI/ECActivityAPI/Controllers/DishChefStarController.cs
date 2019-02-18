using ECActivityAPI.Common;
using ECActivityAPI.Models;
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
    /// 星厨菜品
    /// </summary>
    public class DishChefStarController : ApiController
    {
        private CreditContext db = new CreditContext();
        private SqlHelper dataContext = new SqlHelper();
        private ImgHandle imgHandle = new ImgHandle();



        #region 星厨.家厨师列表，城市，搜索框--星厨家--wx
        /// <summary>
        ///星厨.家厨师列表，城市，搜索框--星厨家--wx
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetChefStarAll(dynamic requestData)
        {

            try
            {
                //var d = ImgHandle.DNS;
                string name = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.Name));
                string city = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.City));

                int pageIndex = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.pageIndex));

                int chefActivityId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.ChefActivityId));

                //获取当前活动ID
                var qcaID = (from v in db.ChefActivity
                             where v.IsEnable == 1
                             select v
                                   ).FirstOrDefault();

                chefActivityId = qcaID.ChefActivityId;


                //获取赛区下城市列表
                var sql = string.Format("select AreaName from MatchRegion where ChefActivityId={0} group by AreaName",chefActivityId);
                var CityNameList = dataContext.ExecuteDataTable(CommandType.Text, sql);



                if (pageIndex == null || pageIndex <= 0)
                {
                    pageIndex = 1;
                }
                var q = (from v in db.ChefStar
                         where v.ChefActivityId == chefActivityId
                         orderby v.PriorityId
                         select new
                         {
                             ChefStarId = v.ChefStarId,
                             HeadPicUrl = v.HeadPicUrl,
                             ChefStarName = v.ChefStarName,
                             ChefStarPosition = v.ChefStarPosition,
                             ChefActivityId = v.ChefActivityId,
                             HotelName = v.HotelName,
                             CityName = v.CityName
                         });

                if (name != null && !string.IsNullOrWhiteSpace(name))
                {
                    q = q.Where(v => v.ChefStarName.Contains(name)|| v.HotelName.Contains(name));
                    //q = q.Where(v => v.HotelName.Contains(name));
                }
                if (city != null && !string.IsNullOrWhiteSpace(city))
                {
                    q = q.Where(v => v.CityName == city);
                }

                var count = q.Count();

                q = q.Skip((pageIndex - 1) * 6).Take(6);

                return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(q, DateTimeConvent.DateTimehh()) + ",\"CityNameList\":" +JsonConvert.SerializeObject(CityNameList) + "}";
            }
            catch (Exception)
            {
               return "No";
            }
        }


        #endregion

        #region 星厨介绍,代表菜品--星厨详情---wx
        /// <summary>
        /// 星厨介绍,代表菜品--星厨详情---wx
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetChefStarDetail(dynamic requestData)
        {
            try
            {
                int chefStarId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.ChefStarId));

                var qDetail = (from v in db.ChefStar
                               where v.ChefStarId == chefStarId
                               select new
                               {
                                   ChefStarId = v.ChefStarId,
                                   PicUrl = v.PicUrl,
                                   Introduce = v.Introduce,
                                   ChefStarName=v.ChefStarName,
                                   ChefStarPosition=v.ChefStarPosition,
                                   HotelName=v.HotelName,
                                  CityName= v.CityName
                               }).FirstOrDefault();
                var chefDish = (from v in db.DishChefStar
                                where v.ChefStarId == chefStarId
                                select new
                                {
                                    DishChefStarId = v.DishChefStarId,
                                    DishPicUrl = v.DishPicUrl,
                                    DishName = v.DishName,
                                    DishType=v.DishType
                                });
                return "{\"ChefStar\":" + JsonConvert.SerializeObject(qDetail) + ",\"chefDish\":" + JsonConvert.SerializeObject(chefDish) + "}";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 增加星厨留言--星厨详情--wx
        /// <summary>
        /// 增加星厨留言--星厨详情--wx
        /// </summary>
        /// <param name="requsetData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddChefStarComment(dynamic requestData)
        {
            try
            {
                //增加留言
                string addDishComment = JsonConvert.SerializeObject(requestData);
                ChefStarComment model = JsonConvert.DeserializeObject<ChefStarComment>(addDishComment);
                db.ChefStarComment.Add(model);
                db.SaveChanges();


                var q = (from v in db.ChefStarComment
                         where v.ChefStarId == model.ChefStarId
                         select v);
                
                var lastq = q.OrderByDescending(v => v.CreateTime).Skip(0).Take(3);
                //查找HotelName
                var lastResult = (from v in q
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
                var count = lastResult.Count();
                return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(lastResult, DateTimeConvent.DateTimehh()) + "}";
            }
            catch (Exception)
            {

                return "{\"Count\":0}";
            }


        }
        #endregion

        #region 星厨留言信息--星厨详情--wx
        /// <summary>
        /// 星厨留言信息--星厨详情--wx
        /// </summary>
        /// <param name="dishId">菜品ID</param>
        /// <returns></returns>
        [HttpGet]
        public string GetChefStarComment(int chefStarId, int index)
        {
            var q = (from v in db.ChefStarComment
                     where v.ChefStarId == chefStarId
                     select v);
            var count = q.Count();
            var lastq = q.OrderByDescending(v => v.CreateTime).Skip((index - 1) * 3).Take(3);
            //查找HotelName
            var lastResult = (from v in q
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
            return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(lastResult, DateTimeConvent.DateTimehh()) + "}";
        }
        #endregion

        #region  增加看过信息，获取星厨菜品信息，推荐调料，是否点赞过--星厨菜品详情--wx
        /// <summary>
        /// 增加看过信息，获取星厨菜品信息，推荐调料，是否点赞--星厨菜品详情--wx
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetDishChefStar(dynamic requestData)
        {
            try
            {
                //首先增加看过统计
                var q = JsonConvert.SerializeObject(requestData);
                DishVisitLog m = JsonConvert.DeserializeObject<DishVisitLog>(q);
                db.DishVisitLog.Add(m);
                db.SaveChanges();
                var sql = string.Format(@"update DishChefStar set VisitCount=VisitCount+1 where DishChefStarId={0}", m.DishId);
                dataContext.ExecuteNonQuery(CommandType.Text, sql);

                //菜品基本信息
                var dishType = (from v in db.DishChefStar
                                where v.DishChefStarId == m.DishId
                                select new
                                {
                                    DishChefStarId = v.DishChefStarId,
                                    DishTyPe = v.DishType,
                                    ChefStarId = v.ChefStarId,
                                    DishPicUrl = v.DishPicUrl,
                                    DishName = v.DishName,
                                    //点赞数
                                    PrasieCount = v.PrasieCount,
                                    VisitCount = v.VisitCount,
                                    
                                    DishStory = v.DishStory

                                }).FirstOrDefault();
                //菜品调料信息
                var dishType1DishMateria = (from v in db.DishMaterial
                                            where v.DishId == dishType.DishChefStarId
                                            select new
                                            {
                                                DishId = v.DishId,
                                                Material = v.Material,
                                                Unit = v.Unit,
                                                MaterialType = v.MaterialType
                                            });

                //菜品步骤信息
                var dishType1DishStep = (from v in db.DishStep
                                         where v.DishId == dishType.DishChefStarId
                                         select new
                                         {
                                             DishId = v.DishId,
                                             StepId = v.StepId,
                                             StepName = v.StepName
                                         });

                //菜品推荐调料信息
                var sqlSelect = string.Format(@"select FR.FlavourName,FR.FlavourPicUrl,FR.FlavourType,FR.FlavourRecId  from FlavourRec as FR join FlavourRecRole FRR on FR.FlavourRecId=FRR.FlavourRecId where FRR.RoleId={0} and FRR.DishId={1}", dishType.ChefStarId, dishType.DishChefStarId);

                var select = dataContext.ExecuteDataTable(CommandType.Text, sqlSelect);

                //是否点赞过
                var Parse = (from v in db.DishPrasieLog
                             where v.OpenId == m.OpenId && v.DishId == dishType.DishChefStarId
                             select v).FirstOrDefault();
                var IsPrasie = "f";
                if (Parse != null)
                    IsPrasie = "t";


                ////是否投票
                //var Selected = (from v in db.DishSelectedLog
                //                where v.OpenId == m.OpenId && v.DishId == m.DishId
                //                select v).FirstOrDefault();
                //var IsSelected = "f";
                //if (Selected != null)
                //    IsSelected = "t";



                //菜品是否收藏
                var isDishChefColle = (from v in db.DishCollectLog
                                       where v.DishId == m.DishId && v.OpenId == m.OpenId
                                       select v).FirstOrDefault();
                var isCollect = "f";
                if (isDishChefColle != null)
                    isCollect = "t";





                return "{\"dishInfo\":" + JsonConvert.SerializeObject(dishType) + ",\"dishType1DishMateria\":" + JsonConvert.SerializeObject(dishType1DishMateria) + ",\"dishType1DishStep\":" + JsonConvert.SerializeObject(dishType1DishStep) + ",\"select\":" + JsonConvert.SerializeObject(select) + ",\"IsPrasie\":\"" + IsPrasie + "\",\"IsCollect\":\"" + isCollect + "\"}";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 增加星厨菜品收藏--星厨菜品详情--wx
        public string AddDishChefStarIsColle(dynamic requestData)
        {
            return null;
        }
        #endregion




        #region 增加星厨菜品留言信息--星厨菜品详情--wx
        /// <summary>
        /// 增加星厨菜品留言信息--星厨菜品详情--wx
        /// </summary>
        /// <param name="requsetData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddDishComment(dynamic requestData)
        {
            try
            {
                //增加留言
                string addDishComment = JsonConvert.SerializeObject(requestData);
                DishComment model = JsonConvert.DeserializeObject<DishComment>(addDishComment);
                db.DishComment.Add(model);
                db.SaveChanges();


                var q = (from v in db.DishComment
                         where v.DishId == model.DishId
                         select v);
                var count = q.Count();
                var lastq = q.OrderByDescending(v => v.CreateTime).Skip(0).Take(3);
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


                return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(lastResult, DateTimeConvent.DateTimehh()) + "}";
            }
            catch (Exception)
            {

                return "{\"Count\":0}";
            }


        }
        #endregion

        #region 查询星厨菜品留言--星厨菜品菜品详情--wx
        /// <summary>
        /// 查询星厨菜品留言--星厨菜品菜品详情--wx
        /// </summary>
        /// <param name="dishId">菜品ID</param>
        /// <returns></returns>
        [HttpGet]
        public string GetDishChefComment(int dishId, int index, int pageSize)
        {
            var q = (from v in db.DishComment
                     where v.DishId == dishId
                     select v);
            var count = q.Count();
            var lastq = q.OrderByDescending(v => v.CreateTime).Skip((index - 1) * pageSize).Take(pageSize);
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
            count = lastResult.Count();
            return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(lastResult, DateTimeConvent.DateTimehh()) + "}";
        }
        #endregion

        #region 增加星厨菜品点赞--星厨菜品详情--wx
        /// <summary>
        ///  增加星厨菜品点赞--星厨菜品详情--wx
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
                                      where v.DishId == m.DishId && v.OpenId == m.OpenId && v.DishType == m.DishType
                                      select new
                                      {
                                          DishId = v.DishId
                                      }).FirstOrDefault();

                if (qDishPrasieLog == null)
                {
                    db.DishPrasieLog.Add(m);
                    db.SaveChanges();
                    var sql = string.Format(@"update DishChefStar set PrasieCount=PrasieCount+1 where DishChefStarId={0}", m.DishId);
                    dataContext.ExecuteNonQuery(CommandType.Text, sql);
                    var q1 = (from v in db.DishChefStar
                                        where v.DishChefStarId == m.DishId
                                        select new
                                        {
                                            ChefStarId = v.ChefStarId,
                                            PrasieCount = v.PrasieCount
                                        }).FirstOrDefault();
                    return JsonConvert.SerializeObject(q1);
                }

                else
                {
                    return "No";
                }
            }
            catch (Exception)
            {
                return "No";
            }

        }

        #endregion



        #region 多余

        //#region 增加星厨菜品留言信息--星厨菜品详情--wx
        ///// <summary>
        ///// 增加星厨菜品留言信息--星厨菜品详情--wx
        ///// </summary>
        ///// <param name="requsetData"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public string AddDishCommentChefStar(dynamic requestData)
        //{
        //    try
        //    {
        //        //增加留言
        //        string addDishComment = JsonConvert.SerializeObject(requestData);
        //        DishComment model = JsonConvert.DeserializeObject<DishComment>(addDishComment);
        //        db.DishComment.Add(model);
        //        db.SaveChanges();


        //        var q = (from v in db.DishComment
        //                 where v.DishId == model.DishId
        //                 select v);
        //        var count = q.Count();
        //        var lastq = q.OrderByDescending(v => v.CreateTime).Skip(0).Take(3);
        //        //查找HotelName
        //        var lastResult = (from v in q
        //                          join p in db.OpenIdAssociated on v.OpenId equals p.OpenId
        //                          join n in db.RegistMember on p.UserId equals n.MemberId
        //                          select new
        //                          {
        //                              HeadPicUrl = v.HeadPicUrl,
        //                              MemebName = v.MemberName,
        //                              Comment = v.Comment,
        //                              CreateTime = v.CreateTime,
        //                              HotelName = n.HotelName
        //                          });


        //        return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(lastResult, DateTimeConvent.DateTimehh()) + "}";
        //    }
        //    catch (Exception)
        //    {

        //        return "{\"Count\":0}";
        //    }


        //}
        //#endregion

        //#region 星厨菜品留言信息--星厨菜品详情--wx
        ///// <summary>
        ///// 星厨菜品留言信息--星厨菜品详情--wx
        ///// </summary>
        ///// <param name="dishId">菜品ID</param>
        ///// <returns></returns>
        //[HttpGet]
        //public string GetDishChefComment(int dishId, int index, int pageSize)
        //{
        //    var q = (from v in db.DishComment
        //             where v.DishId == dishId
        //             select v);
        //    var count = q.Count();
        //    var lastq = q.OrderByDescending(v => v.CreateTime).Skip((index - 1) * pageSize).Take(pageSize);
        //    //查找HotelName
        //    var lastResult = (from v in q
        //                      join p in db.OpenIdAssociated on v.OpenId equals p.OpenId
        //                      join n in db.RegistMember on p.UserId equals n.MemberId
        //                      select new
        //                      {
        //                          HeadPicUrl = v.HeadPicUrl,
        //                          MemebName = v.MemberName,
        //                          Comment = v.Comment,
        //                          CreateTime = v.CreateTime,
        //                          HotelName = n.HotelName
        //                      });
        //    return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(lastResult, DateTimeConvent.DateTimehh()) + "}";
        //}
        //#endregion
        
        #endregion

    }
}

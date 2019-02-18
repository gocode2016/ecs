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
    /// <summary>
    /// 培训交流
    /// </summary>
    public class LecturerController : ApiController
    {
        private CreditContext db = new CreditContext();
        private SqlHelper dataContext = new SqlHelper();
        private ImgHandle imgHandel = new ImgHandle();

        #region web

        #region 新增培训交流，新增讲师--培训交流新增--web
        /// <summary>
        ///  新增培训交流，新增讲师--培训交流新增--web
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddCulLecturerSingle(dynamic requestData)
        {
            try
            {
                CultivateInterflow ci = JsonConvert.DeserializeObject<CultivateInterflow>(JsonConvert.SerializeObject(requestData.Cultivate));

                List<Lecturer> l = JsonConvert.DeserializeObject<List<Lecturer>>(JsonConvert.SerializeObject(requestData.Lecturer));

                ci.UpdateTime = DateTime.Now;
                ci.UpdatePerson = ci.CreatePerson;

                if (ci.Introduce.IndexOf("base64") > 0)
                {
                    string d = ci.Introduce;

                    foreach (string a in imgHandel.GetHtmlImageUrlList(d))
                    {
                        if (a.IndexOf(ImgHandle.DNS) < 0)
                        {
                            string[] asplit = a.Split(',');
                            string imgtype = asplit[0].Substring(11, asplit[0].Length - 18);
                            string filepath = imgHandel.Base64StringToImage(asplit[1], imgtype, "/pic/Lecturer");
                            d = d.Replace(a, filepath);
                        }
                    }
                    ci.Introduce = d;
                }


                db.CultivateInterflow.Add(ci);

                db.SaveChanges();

                foreach (Lecturer i in l)
                {
                    i.CuInterId = ci.CuInterId;
                    db.Lecturer.Add(i);
                }
                db.SaveChanges();

                return "OK";
            }
            catch (Exception)
            {
                return "No";
            }
        }


        #endregion

        #region 获取培训交流活动，讲师信息--培训交流编辑--web
        /// <summary>
        /// 获取培训交流活动，讲师信息--培训交流编辑--web
        /// </summary>
        /// <param name="cuId">培训交流活动Id</param>
        /// <returns></returns>
        [HttpGet]
        public string GetLecturerByCuInterId(int cuId)
        {
            try
            {
                var cultivate = (from v in db.CultivateInterflow
                                 where v.CuInterId == cuId
                                 select new
                                 {
                                     CuInterId = v.CuInterId,
                                     CurriculumName = v.CurriculumName,
                                     CurriculumType = v.CurriculumType,
                                     Url = v.Url,
                                     Priority = v.Priority,
                                     IsDisplay = v.IsDisplay,
                                     Introduce = v.Introduce
                                 }).FirstOrDefault();
                var lecturer = (from v in db.Lecturer
                                where v.CuInterId == cuId
                                select new
                                {
                                    LecturerId = v.LecturerId,
                                    CuInterId = v.CuInterId,
                                    HeadPicUrl = v.HeadPicUrl,
                                    LecturerName = v.LecturerName,
                                    Post = v.Post,
                                    HotelName = v.HotelName
                                });

                return "{\"Cultivate\":" + JsonConvert.SerializeObject(cultivate) + ",\"Lecturer\":" + JsonConvert.SerializeObject(lecturer) + "}";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 获取讲师信息--培训交流编辑--web
        /// <summary>
        /// 获取讲师信息--培训交流编辑--web
        /// </summary>
        /// <param name="lecId">讲师ID</param>
        /// <returns></returns>
        [HttpGet]
        public string GetLecturer(int lecId)
        {
            var lecturer = (from v in db.Lecturer
                            where v.LecturerId == lecId
                            select new
                            {
                                LecturerId = v.LecturerId,
                                CuInterId = v.CuInterId,
                                HeadPicUrl = v.HeadPicUrl,
                                LecturerName = v.LecturerName,
                                Post = v.Post,
                                HotelName = v.HotelName
                            }).FirstOrDefault();
            return "{\"Lecturer\":" + JsonConvert.SerializeObject(lecturer) + "}";
        }
        #endregion

        #region 获取培训交流活动--培训交流编辑--web
        /// <summary>
        /// 获取培训交流活动--培训交流编辑--web
        /// </summary>
        /// <param name="cu">培训交流Id</param>
        /// <returns></returns>
        [HttpGet]
        public string GetCultivate(int cu)
        {
            try
            {
                var q = (from v in db.CultivateInterflow
                         where v.CuInterId == cu
                         select new
                         {
                             CuInterId = v.CuInterId,
                             CurriculumName = v.CurriculumName,
                             CurriculumType = v.CurriculumType,
                             Url = v.Url,
                             Priority = v.Priority,
                             IsDisplay = v.IsDisplay,
                             Introduce = v.Introduce
                         }).FirstOrDefault();
                return JsonConvert.SerializeObject(q);
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 新增单一讲师--培训交流编辑--web
        /// <summary>
        ///  新增单一讲师--培训交流编辑--web
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string AddLecturer(Lecturer le)
        {
            try
            {
                db.Lecturer.Add(le);
                db.SaveChanges();
                return le.LecturerId.ToString();
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 编辑培训交流活动--培训交流编辑--web
        /// <summary>
        /// 编辑培训交流活动--培训交流编辑--web
        /// </summary>
        /// <param name="cu"></param>
        /// <returns></returns>
        [HttpPost]
        public string UpdateCultivate(CultivateInterflow cu)
        {
            var q = (from v in db.CultivateInterflow
                     where v.CuInterId == cu.CuInterId
                     select v).FirstOrDefault();
            cu.CuInterId = 0;

            q.CurriculumName = cu.CurriculumName;
            q.CurriculumType = cu.CurriculumType;
            q.Url = cu.Url;
            q.Priority = cu.Priority;
            q.IsDisplay = cu.IsDisplay;
            q.Introduce = cu.Introduce;

            if (q.Introduce.IndexOf("base64") > 0)
            {
                string d = q.Introduce;

                foreach (string a in imgHandel.GetHtmlImageUrlList(d))
                {
                    if (a.IndexOf(ImgHandle.DNS) < 0)
                    {
                        string[] asplit = a.Split(',');
                        string imgtype = asplit[0].Substring(11, asplit[0].Length - 18);
                        string filepath = imgHandel.Base64StringToImage(asplit[1], imgtype, "/pic/Lecturer");
                        d = d.Replace(a, filepath);
                    }
                }
                q.Introduce = d;
            }

            q.UpdatePerson = cu.UpdatePerson;
            q.UpdateTime = DateTime.Now;
            db.Entry<CultivateInterflow>(q).State = EntityState.Modified;
            db.SaveChanges();



            return null;
        }
        #endregion

        #region 删除单一讲师--培训交流编辑--web
        /// <summary>
        ///  删除单一讲师--培训交流编辑--web
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string DelLecturer(dynamic requestData)
        {
            try
            {
                int lecturerId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.LecturerId));
                int cuInterId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.CuInterId));
                string sql = string.Format(@"Delete Lecturer where LecturerId={0} and CuInterId={1}", lecturerId, cuInterId);
                dataContext.ExecuteNonQuery(CommandType.Text, sql);
                return "OK";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 编辑培训交流活动，编辑讲师，包含新增--培训交流编辑--web
        /// <summary>
        /// 编辑培训交流活动，编辑讲师，包含新增--培训交流编辑--web
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string UpdateCultivateLecturer(dynamic requestData)
        {
            try
            {
                CultivateInterflow cu = JsonConvert.DeserializeObject<CultivateInterflow>(JsonConvert.SerializeObject(requestData.Cultivate));
                List<Lecturer> lecturer = JsonConvert.DeserializeObject<List<Lecturer>>(JsonConvert.SerializeObject(requestData.Lecturers));

                #region 修改培训交流活动基本信息
                var q = (from v in db.CultivateInterflow
                         where v.CuInterId == cu.CuInterId
                         select v).FirstOrDefault();
                cu.CuInterId = 0;

                q.CurriculumName = cu.CurriculumName;
                q.CurriculumType = cu.CurriculumType;
                q.Url = cu.Url;
                q.Priority = cu.Priority;
                q.IsDisplay = cu.IsDisplay;
                q.Introduce = cu.Introduce;

                if (q.Introduce.IndexOf("base64") > 0)
                {
                    string d = q.Introduce;

                    foreach (string a in imgHandel.GetHtmlImageUrlList(d))
                    {
                        if (a.IndexOf(ImgHandle.DNS) < 0)
                        {
                            string[] asplit = a.Split(',');
                            string imgtype = asplit[0].Substring(11, asplit[0].Length - 18);
                            string filepath = imgHandel.Base64StringToImage(asplit[1], imgtype, "/pic/Lecturer");
                            d = d.Replace(a, filepath);
                        }
                    }
                    q.Introduce = d;
                }

                q.UpdatePerson = cu.UpdatePerson;
                q.UpdateTime = DateTime.Now;
                db.Entry<CultivateInterflow>(q).State = EntityState.Modified;
                db.SaveChanges();
                #endregion


                #region 修改讲师，包含新增

                foreach (Lecturer i in lecturer)
                {
                    if (i.LecturerId == 0)
                    {
                        db.Lecturer.Add(i);
                        db.SaveChanges();
                    }
                    else
                    {
                        var qd = (from v in db.Lecturer
                                  where v.LecturerId == i.LecturerId
                                  select v).FirstOrDefault();
                        i.LecturerId = -1;

                        qd.CuInterId = i.CuInterId;
                        qd.HeadPicUrl = i.HeadPicUrl;
                        qd.HotelName = i.HotelName;
                        qd.LecturerName = i.LecturerName;
                        qd.Post = i.Post;
                        db.Entry<Lecturer>(qd).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }



                #endregion

                return "OK";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 获取培训交流列表--培训交流--web
        /// <summary>
        /// 获取培训交流列表--培训交流--web
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetCultivateList(dynamic requestData)
        {
            try
            {
                string sql = "select * from (select c.CuInterId,c.CurriculumName,c.CreateTime, STUFF((SELECT ' '+LecturerName FROM  Lecturer where CuInterId=c.CuInterId  FOR XML PATH('')), 1, 1, '') lecturer,c.CurriculumType,c.IsDisplay,c.Priority  from CultivateInterflow c  ) as d where 1=1";

                var StarTime = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.StarTime));


                var EndTime = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.EndTime));

                var Name = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.Name));



                var pageIndex = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.PageIndex));


                if (!string.IsNullOrEmpty(StarTime))
                {
                    sql += " and d.CreateTime>'" + StarTime + "'";
                }
                if (!string.IsNullOrEmpty(EndTime))
                {
                    sql += " and d.CreateTime<'" + EndTime + "'";
                }
                if (!string.IsNullOrEmpty(Name))
                {
                    sql += " and (d.CurriculumName like '%" + Name + "%' or  d.lecturer like '%" + Name + "%')";
                }
                sql = sql + "  Order by d.Priority";
                var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
                var count = q.Rows.Count;

                if (pageIndex <= 0)
                    pageIndex = 1;
                var lastq = imgHandel.GetPagedTable(q, pageIndex, 10);

                return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(lastq, DateTimeConvent.DateTimedd()) + "}";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #endregion

        #region wx

        #region 获取培训交流列表--培训交流列表--wx
        /// <summary>
        /// 获取培训交流列表--培训交流列表--wx
        /// </summary>
        /// <param name="requestDate"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetCultivateInterflowList(dynamic requestData)
        {

            try
            {
                // var orderBy = requestData.OrderBy;

                string orderBy = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.OrderBy));

                if (orderBy != "time" && orderBy != "count")
                    orderBy = "time";

                string isDesc = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.IsDesc));

                if (isDesc != "f" && isDesc != "t")
                    isDesc = "f";

                string openId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.OpenId));

                int pageIndex = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.PageIndex));
                if (pageIndex <= 0)
                    pageIndex = 1;
                //            var sql = string.Format(@"select C.CuInterId,C.CurriculumType,C.Url,C.CurriculumName,C.VisitCount,C.PraiseCount,C.CreateTime,case when CP.Id is null then 'f' else 't' end  as isPraise from  CultivateInterflow  C left join( select * from  CultivatePraise where OpenId={0}) as  CP  on C.CuInterId=CP.CuInterId
                //where C.IsDisplay=1
                //order By C.Priority ",openId);


                var qtest = (from v in db.CultivatePraise
                             where v.OpenId == openId
                             select v);


                var q = (from v in db.CultivateInterflow
                         join p in qtest on v.CuInterId equals p.CuInterId into temp
                         from tt in temp.DefaultIfEmpty()
                         where v.IsDisplay==1
                         select new
                         {
                             CuInterId = v.CuInterId,
                             CurriculumType = v.CurriculumType,
                             Url = v.Url,
                             CurriculumName = v.CurriculumName,
                             PraiseCount = v.PraiseCount,
                             VisitCount = v.VisitCount,
                             CreateTime = v.CreateTime,
                             IsPraise = tt.Id == null ? "f" : "t"
                         });

                var count = q.Count();



                //if (isDesc == "f")
                //{
                //    var lastq = q.OrderBy(v => orderBy).Skip((pageIndex - 1) * 4).Take(4);
                //    return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(lastq) + "}";
                //}
                //else
                //{
                //    var lastq = q.OrderByDescending(v => orderBy).Skip((pageIndex - 1) * 4).Take(4);
                //    return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(lastq) + "}";
                //}

                if (orderBy == "time")
                {
                    if (isDesc == "t")
                        q = q.OrderByDescending(v => v.CreateTime);
                    if (isDesc == "f")
                        q = q.OrderBy(v => v.CreateTime);
                }
                if (orderBy == "count")
                {
                    if (isDesc == "t")
                        q = q.OrderByDescending(v => v.VisitCount);
                    if (isDesc == "f")
                        q = q.OrderBy(v => v.VisitCount);
                }

                q = q.Skip((pageIndex - 1) * 4).Take(4);


                 return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(q,DateTimeConvent.DateTimedd()) + "}";
            }
            catch (Exception)
            {
                return "No";
            }

        }

        #endregion

        #region 读取培训交流详情,增加看过信息--培训详情--wx
        /// <summary>
        /// 读取培训交流详情，增加看过信息--培训详情--wx
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetCultivateDetail(dynamic requestData)
        {
            CultivateVisit cuVisit = JsonConvert.DeserializeObject<CultivateVisit>(JsonConvert.SerializeObject(requestData));
            //先增加看过数量

            db.CultivateVisit.Add(cuVisit);

            var sql = string.Format(@"Update CultivateInterflow set VisitCount=VisitCount+1 where CuInterId={0}",cuVisit.CuInterId);

            dataContext.ExecuteNonQuery(CommandType.Text,sql);




            //查询活动基本信息

            var qtest = (from v in db.CultivatePraise
                         where v.OpenId == cuVisit.OpenId
                         select v);
            
            var q = (from v in db.CultivateInterflow
                     join p in qtest on v.CuInterId equals p.CuInterId into temp
                     from tt in temp.DefaultIfEmpty()
                     where v.CuInterId==cuVisit.CuInterId
                     select new
                     {
                         CuInterId = v.CuInterId,
                         CurriculumType = v.CurriculumType,
                         Url = v.Url,
                         Introduce=v.Introduce,
                         CurriculumName = v.CurriculumName,
                         PraiseCount = v.PraiseCount,
                         VisitCount = v.VisitCount,
                         CreateTime = v.CreateTime,
                         IsPraise = tt.Id == null ? "f" : "t"
                     }).FirstOrDefault();


            //活动下面带的讲师
            var qLecturer = (from v in db.Lecturer
                             where v.CuInterId == cuVisit.CuInterId
                             select new {
                                 LecturerId=v.LecturerId,
                                 HeadPicUrl=v.HeadPicUrl,
                                 LecturerName=v.LecturerName,
                                 Post=v.Post,
                                 HotelName=v.HotelName
                             });


            return "{\"Cultivate\":" + JsonConvert.SerializeObject(q, DateTimeConvent.DateTimedd()) + ",\"Lecturer\":" + JsonConvert.SerializeObject(qLecturer) + "}";
        }


        #endregion

        #region 增加点赞--培训交流详情--wx
        /// <summary>
        ///  增加点赞--培训交流详情--wx
        /// </summary>
        /// <param name="requsetData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddCultivatePraise(dynamic requestData)
        {
            try
            {
                CultivatePraise cuVisit = JsonConvert.DeserializeObject<CultivatePraise>(JsonConvert.SerializeObject(requestData));
                var q = (from v in db.CultivatePraise
                         where v.CuInterId == cuVisit.CuInterId && v.OpenId == cuVisit.OpenId
                         select v).FirstOrDefault();

                if (q == null)
                {
                    db.CultivatePraise.Add(cuVisit);
                    db.SaveChanges();
                    var sql = string.Format(@"Update CultivateInterflow set PraiseCount=PraiseCount+1 where CuInterId={0}", cuVisit.CuInterId);

                    dataContext.ExecuteNonQuery(CommandType.Text, sql);

                }
                var count = (from v in db.CultivateInterflow
                             where v.CuInterId == cuVisit.CuInterId
                             select new
                             {
                                 PraiseCount = v.PraiseCount
                             }).FirstOrDefault();

                return JsonConvert.SerializeObject(count);
            }
            catch (Exception)
            {
                return "No";
            }
        }

        #endregion

        #region 增加评论--培训交流详情--wx
        /// <summary>
        ///  增加评论--培训交流详情--wx
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddCultivateComment(dynamic requestData)
        {
            try
            {
                CultivateComment cuCom = JsonConvert.DeserializeObject<CultivateComment>(JsonConvert.SerializeObject(requestData));
               
                //db.CultivateComment.Add(cuCom);
                //db.SaveChanges();
                string sql = string.Format(@"INSERT INTO [dbo].[CultivateComment] ([CuInterId] ,[OpenId] ,[HeadPicUrl] ,[NickName],[Comment],[CreateTime]) VALUES ({0} ,'{1}' ,'{2}'  ,'{3}' ,'{4}','{5}')",cuCom.CuInterId,cuCom.OpenId,cuCom.HeadPicUrl,cuCom.NickName,cuCom.Comment,cuCom.CreateTime);

                dataContext.ExecuteNonQuery(CommandType.Text, sql);
                return "Ok";
            }
            catch (Exception)
            {
                return "No";
            }
        }


        #endregion

        #region 获取培训交流评论--培训交流详情--wx
        /// <summary>
        /// 获取培训交流评论--培训交流详情--wx
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetCultivateComment(dynamic requestData)
        {
            try
            {
                int cuId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.CuInterId));
                int pageIndex = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.PageIndex));
                if (pageIndex <= 0)
                    pageIndex = 1;

                var q = (from v in db.CultivateComment
                         where v.CuInterId == cuId
                         select new
                         {
                             HeadPicUrl = v.HeadPicUrl,
                             NickName = v.NickName,
                             Comment = v.Comment,
                             CreateTime = v.CreateTime
                         });
                var count = q.Count();
                q = q.OrderByDescending(v => v.CreateTime).Skip((pageIndex - 1) * 3).Take(3);
                return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(q, DateTimeConvent.DateTimehh()) + "}";
            }
            catch (Exception)
            {
                return "No";
            }
        }

        #endregion

        #endregion

    }
}

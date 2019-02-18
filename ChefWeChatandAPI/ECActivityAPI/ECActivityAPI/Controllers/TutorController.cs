using ECActivityAPI.Common;
using ECActivityAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace ECActivityAPI.Controllers
{
    public class TutorController : ApiController
    {
        private CreditContext db = new CreditContext();
        private SqlHelper dataContext = new SqlHelper();
        private ImgHandle imgHandle = new ImgHandle();
        
        #region web

        #region 增加导师--导师详情新增--web
        /// <summary>
        /// 增加导师--导师详情新增--web
        /// </summary>
        /// <param name="requestDate">导师JSon</param>
        /// <returns>OK,成功，No，失败</returns>
        [HttpPost]
        public string AddTutor(dynamic requestData)
        {
            try
            {
                string addTutor = JsonConvert.SerializeObject(requestData);
                Tutor model = JsonConvert.DeserializeObject<Tutor>(addTutor);

                if (model.Introduce.IndexOf("base64") > 0)
                {
                    string d = model.Introduce;
                    GetHtmlImageUrlList(d);
                    foreach (string a in GetHtmlImageUrlList(d))
                    {
                        if (a.IndexOf(ImgHandle.DNS) < 0)
                        {
                            string[] asplit = a.Split(',');
                            string imgtype = asplit[0].Substring(11, asplit[0].Length - 18);
                            string filepath = Base64StringToImage(asplit[1], imgtype);
                            d = d.Replace(a, filepath);
                        }
                    }
                    model.Introduce = d;
                }
                model.CreateTime = DateTime.Now;
                model.UpdateTime = DateTime.Now;

                model.IsDel = 1;
                db.Tutor.Add(model);
                db.SaveChanges();
                return model.TutorId.ToString();
            }
            catch (Exception e)
            {
                return "No";
            }
        }
        #endregion

        #region 修改导师对象信息--导师详情新增--web
        /// <summary>
        /// 修改导师对象信息--导师详情新增--web
        /// </summary>
        /// <param name="bannerDate">导师对象</param>
        /// <returns>OK，修改成功，No，失败</returns>
        [HttpPost]
        public string UpdateSingle(dynamic bannerDate)
        {
            try
            {
                string updateSingle = JsonConvert.SerializeObject(bannerDate);
                var model = JsonConvert.DeserializeObject<Tutor>(updateSingle);
                if (model.Introduce.IndexOf("base64") > 0)
                {
                    string d = model.Introduce;
                    GetHtmlImageUrlList(d);
                    foreach (string a in GetHtmlImageUrlList(d))
                    {
                        if (a.IndexOf(ImgHandle.DNS) < 0)
                        {
                            string[] asplit = a.Split(',');
                            string imgtype = asplit[0].Substring(11, asplit[0].Length - 18);
                            string filepath = Base64StringToImage(asplit[1], imgtype);
                            d = d.Replace(a, filepath);
                        }
                    }
                    model.Introduce = d;
                }
                model.UpdateTime = DateTime.Now;
                db.Entry<Tutor>(model).State = EntityState.Modified;
                db.Entry<Tutor>(model).Property<DateTime>(v => v.CreateTime).IsModified = false;
                db.Entry<Tutor>(model).Property<string>(v => v.CreatePerson).IsModified = false;
                db.Entry<Tutor>(model).Property<int>(v => v.IsDel).IsModified = false;
                db.Entry<Tutor>(model).Property<string>(v => v.TutorComment).IsModified = false;
                db.SaveChanges();
                return "OK";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 获取全部导师信息，给后台列表看--导师列表--web
        /// <summary>
        /// 获取全部导师信息，给后台列表看--导师列表--web
        /// </summary>
        /// <param name="chefActivityId">活动Id</param>
        /// <param name="PageIndex">页数</param>
        /// <param name="PageSize">页大小</param>
        /// <returns>全部导师信息</returns>
        [HttpPost]
        public string GetAllTutor(dynamic requestData)
        {
            try
            {
                string AreaName = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.AreaName));
                string TutorName = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.TutorName));

                int PageIndex = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.PageIndex));

                int chefActivityId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.ChefActivityId));

                int PageSize = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.PageSize));


                //string AreaName = "";

                //string TutorName = "";

                string sql = string.Format(@"select T.TutorId,T.ChefActivityId, T.TutorName,T.AreaName,   T.TutorPosition as  TutorComment ,TD.DishOne,TD.DishTow from Tutor as T left   join(select case when tab1.TutorId is null then tab2.TutorId else tab1.TutorId end   TutorId,tab1.菜品1 as DishOne,tab2.菜品2 as DishTow from 
(select TutorId,DishName as '菜品1' from DishTutor where DishType = 1) tab1 
full join 
(select TutorId,DishName as '菜品2' from DishTutor where DishType = 2) tab2 
on tab1.TutorId = tab2.TutorId) as TD on T.TutorId=TD.TutorId  where T.ChefActivityId={0} 
", chefActivityId);

                if (!string.IsNullOrEmpty(AreaName) && AreaName!="全部")
                    sql = sql + "  and T.AreaName like '%" + AreaName + "%' ";
                if (!string.IsNullOrEmpty(TutorName) && TutorName != "全部")
                    sql = sql + " and T.TutorName like '%" + TutorName + "%'";


                var q = dataContext.ExecuteDataTable(CommandType.Text, sql);

                var count = q.Rows.Count;
                if (PageIndex <= 0)
                    PageIndex = 1;
                var lastq = imgHandle.GetPagedTable(q, PageIndex, PageSize);

                return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(lastq) + "}";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion 

        #region 获取导师基本资料，活动信息，赛区--导师编辑页面--web
        /// <summary>
        /// 获取导师基本资料，活动信息，赛区--导师编辑页面--web
        /// </summary>
        /// <param name="tutorId">导师ID</param>
        /// <returns></returns>
        public string GetTutorcaMRBy(int tutorId)
        {
            var tutor = (from v in db.Tutor
                         join p in db.ChefActivity on v.ChefActivityId equals p.ChefActivityId
                         where v.TutorId == tutorId
                         select new
                         {
                             TutorId = v.TutorId,
                             ChefActivityId = v.ChefActivityId,
                             ChefActivityName = p.ChefActivityName,
                             Introduce = v.Introduce,
                             PicUrl = v.PicUrl,
                             TutorName = v.TutorName,
                             HeadPicUrl = v.HeadPicUrl,
                             AreaName = v.AreaName,
                             TutorPosition = v.TutorPosition,
                             TutorHotel = v.TutorHotel,
                             PriorityId = v.PriorityId,
                             IsDisplay = v.IsDisplay,
                             UpdatePerson = v.UpdatePerson
                         }).FirstOrDefault();
            var iso = new IsoDateTimeConverter();
            iso.DateTimeFormat = "yyyy-MM-dd HH:mm";
            var qca = (from v in db.ChefActivity
                       where v.ChefActivityId == tutor.ChefActivityId
                       select new
                       {
                           ChefActivityId = v.ChefActivityId,
                           ChefActivityName = v.ChefActivityName,
                           IsEnable = v.IsEnable
                       }).FirstOrDefault();
            var qMR = (from v in db.MatchRegion
                       where v.ChefActivityId == tutor.ChefActivityId
                       select new
                       {
                           MatchRegionId = v.MatchRegionId,
                           AreaName = v.AreaName,
                           ProvinceName = v.ProvinceName
                       });
            return "{\"tutor\":" + JsonConvert.SerializeObject(tutor) + ",\"ca\":" + JsonConvert.SerializeObject(qca) + ",\"MR\":" + JsonConvert.SerializeObject(qMR) + "}";
        }

        #endregion

        #region 删除单个导师信息--导师列表--web
        /// <summary>
        /// 删除单个导师信息--导师列表--web
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string DeleteSingleTutor(int tutorId)
        {
            try
            {
                var sql = string.Format(@"delete DishMaterial where DishId in (select TutorDishId from DishTutor where TutorId={0})
delete DishStep  where DishId in (select TutorDishId from DishTutor where TutorId={0})
delete FlavourRecRole where DishId in (select TutorDishId from DishTutor where TutorId={0})

delete DishTutor where TutorId={0}
delete [Tutor]  where TutorId={0}",tutorId);
                dataContext.ExecuteNonQuery(CommandType.Text, sql);
                return "OK";
            }
            catch (Exception)
            {
                return "No";
            }
        }



        #endregion




        #endregion

        #region wx

        #region 根据列表页获取导师信息--导师列表--wx
        /// <summary>
        /// 根据列表页获取导师信息--导师列表--wx
        /// </summary>
        /// <param name="chefActivityId">活动Id</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>活动全部信息</returns>
        [HttpGet]
        public string GetTutorListByChefActivityId(int chefActivityId, int pageIndex, int pageSize)
        {

            //获取当前活动ID
            var qcaID = (from v in db.ChefActivity
                         where v.IsEnable == 1
                         select v
                               ).FirstOrDefault();

            chefActivityId = qcaID.ChefActivityId;

            var q = (from v in db.Tutor
                     where v.ChefActivityId == chefActivityId && v.IsDel == 1
                     select new
                     {
                         TutorId = v.TutorId,
                         ChefActivityId = v.ChefActivityId,
                         //Introduce = v.Introduce,
                         HeadPicUrl = v.HeadPicUrl,
                         TutorName = v.TutorName,
                         //TutorComment = v.TutorComment,
                         AreaName = v.AreaName,
                         TutorHotel = v.TutorHotel,
                         TutorPosition=v.TutorPosition,
                         PriorityId = v.PriorityId
                     });
            int count = q.Count();
            var lastq = q.OrderBy(v => v.PriorityId).Skip(pageSize * (pageIndex - 1)).Take(pageSize);


            return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(lastq) + "}";
        }
        #endregion 

        #region 获取导师基本资料，代表菜品--导师介绍--wx
        /// <summary>
        /// 获取导师基本资料，代表菜品--导师介绍--wx
        /// </summary>
        /// <param name="tutorid">导师ID</param>
        /// <returns></returns>
        [HttpGet]
        public string GetTutorDTByTutorId(int tutorid)
        {
            try
            {
                var q = (from v in db.Tutor
                         where v.TutorId == tutorid
                         select new
                         {
                             TutorId = v.TutorId,
                             ChefActivityId = v.ChefActivityId,
                             Introduce = v.Introduce,
                             AreaName=v.AreaName,
                             TutorHotel=v.TutorHotel,
                             PicUrl = v.PicUrl,
                             TutorName = v.TutorName,
                             TutorPosition = v.TutorPosition
                         }).FirstOrDefault();
                var dishtutor = (from v in db.DishTutor
                                 where v.TutorId == tutorid
                                 select new
                                 {
                                     TutorDishId = v.TutorDishId,
                                     DishPicUrl = v.DishPicUrl,
                                     DishName = v.DishName,
                                     DishType = v.DishType
                                 });
                return "{\"tutorid\":" + JsonConvert.SerializeObject(q) + ",\"dishtutor\":" + JsonConvert.SerializeObject(dishtutor) + "}";
            }
            catch (Exception)
            {
                return "No";
            }
           
        }
        #endregion

        #region 增加导师看过数量,一次性读取菜品信息--导师菜品详情页面--wx
        /// <summary>
        /// 增加导师看过数量,一次性读取菜品信息--导师菜品详情页面--wx
        /// </summary>
        /// <param name="requestData">菜品看过次数</param>
        /// <returns></returns>
        [HttpPost]
        public string GetDishTutorDetail(dynamic requestData)
        {
            try
            {
                #region  增加导师看过数量
                var q = JsonConvert.SerializeObject(requestData);
                DishVisitLog m = JsonConvert.DeserializeObject<DishVisitLog>(q);
                db.DishVisitLog.Add(m);
                db.SaveChanges();
                var sql = string.Format(@"update DishTutor set VisitCount=VisitCount+1 where TutorDishId={0}", m.DishId);
                dataContext.ExecuteNonQuery(CommandType.Text, sql); 
                #endregion

                //导师基本信息
                var qdishTutor = (from v in db.DishTutor
                                  where v.TutorDishId == m.DishId
                                  select new
                                  {
                                      TutorDishId=v.TutorDishId,
                                      TutorId=v.TutorId,
                                      DishStory=v.DishStory,
                                      DishPic=v.DishPicUrl,
                                      DishName=v.DishName,
                                      
                                      VisitCount=v.VisitCount,
                                      PrasieCount=v.PrasieCount
                                  }).FirstOrDefault();
                //菜品调料
                var qdishMaterial = (from v in db.DishMaterial
                                     where v.DishId == m.DishId
                                     select new { 
                                         Material=v.Material,
                                         Unit=v.Unit,
                                         MaterialType = v.MaterialType 
                                     });
                var qdishStep = (from v in db.DishStep
                                 where v.DishId == m.DishId
                                 select new
                                 {
                                     Step = v.StepId,
                                     StepName = v.StepName
                                 });
                var sqlflavourRec = string.Format(@"select FlavourName,FlavourPicUrl from FlavourRec where FlavourRecId in(select FlavourRecId from FlavourRecRole where DishId={0})", m.DishId);
                var qflavourRec = dataContext.ExecuteDataTable(CommandType.Text, sqlflavourRec);


                var sqlDishPrasieLog = string.Format(@"select count(Id) as count from DishPrasieLog where DishId={0} and OpenId='{1}' and DishType={2}", m.DishId, m.OpenId, m.DishType);
              
                var qDishPrasieLog = dataContext.ExecuteDataTable(CommandType.Text, sqlDishPrasieLog);
                //var d = qDishPrasieLog.Rows[0]["count"];

                //是否点赞
                string flagPrasie ="f";
                if (Convert.ToInt32(qDishPrasieLog.Rows[0]["count"]) > 0)
                {
                    flagPrasie = "t";
                }

                //菜品是否收藏
                var isDishChefColle = (from v in db.DishCollectLog
                                       where v.DishId == m.DishId && v.OpenId == m.OpenId
                                       select v).FirstOrDefault();
                var isCollect = "f";
                if (isDishChefColle != null)
                    isCollect = "t";

                return "{\"dishTutor\":" + JsonConvert.SerializeObject(qdishTutor) + ",\"dishMaterial\":" + JsonConvert.SerializeObject(qdishMaterial) + ",\"dishStep\":" + JsonConvert.SerializeObject(qdishStep) + ",\"flavourRec\":" + JsonConvert.SerializeObject(qflavourRec) + ",\"flagPrasie\":\"" + flagPrasie + "\",\"IsCollect\":\"" + isCollect + "\"}";
            }
            catch (Exception)
            {
                return "No";
            }
        }


        #endregion

        #region 获取单一导师信息--导师详情页--wx
        /// <summary>
        /// 获取单一导师信息--导师详情页--wx
        /// </summary>
        /// <param name="tutorId">导师编号</param>
        /// <returns>导师信息</returns>
        public string GetTutorByTutorId(int  tutorId)
        {
            var tutor = (from v in db.Tutor
                         join p in db.ChefActivity on v.ChefActivityId equals p.ChefActivityId
                         where v.TutorId == tutorId
                         select new {
                             TutorId=v.TutorId,
                             ChefActivityId=v.ChefActivityId,
                             ChefActivityName=p.ChefActivityName,
                             Introduce=v.Introduce,
                             PicUrl = v.PicUrl,
                             TutorName = v.TutorName,
                             HeadPicUrl = v.HeadPicUrl,
                             AreaName = v.AreaName,
                             TutorPosition = v.TutorPosition,
                             TutorHotel = v.TutorHotel,
                             PriorityId = v.PriorityId,
                             IsDisplay = v.IsDisplay,
                             UpdatePerson = v.UpdatePerson
                         }).FirstOrDefault();
            var iso = new IsoDateTimeConverter();
            iso.DateTimeFormat = "yyyy-MM-dd HH:mm";
            
            return JsonConvert.SerializeObject(tutor, iso);
        }
        
        #endregion
        #endregion

      

        #region base64转为图片
        private string Base64StringToImage(string baseString, string fileType)
        {
            try
            {
                byte[] arr = Convert.FromBase64String(baseString);
                MemoryStream ms = new MemoryStream(arr);
                Bitmap bmp = new Bitmap(ms);

                var filename = DateTime.Now.ToString("yyMMddhhmmssff") + "." + fileType;
                string uploadPath = System.Web.HttpContext.Current.Server.MapPath(@"/pic/tutor");
                
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                FileStream fs1 = new FileStream(uploadPath + "/" + filename, FileMode.Create, FileAccess.Write);//创建写入文件 
                fs1.Close();
                bmp.Save(uploadPath + "/" + filename);
                ms.Close();
                return ImgHandle.DNS + "/pic/tutor/" + filename;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region 取出<img>中Src的信息
        /// <summary>   
        /// 取得HTML中所有图片的 URL。   
        /// </summary>   
        /// <param name="sHtmlText">HTML代码</param>   
        /// <returns>图片的URL列表</returns>   
        public static string[] GetHtmlImageUrlList(string sHtmlText)
        {
            // 定义正则表达式用来匹配 img 标签   
            //Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[''']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n'''<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

            // 搜索匹配的字符串   
            MatchCollection matches = regImg.Matches(sHtmlText);
            int i = 0;
            string[] sUrlList = new string[matches.Count];

            // 取得匹配项列表   
            foreach (Match match in matches)
                sUrlList[i++] = match.Groups["imgUrl"].Value;
            return sUrlList;
        }

        #endregion
    }
}

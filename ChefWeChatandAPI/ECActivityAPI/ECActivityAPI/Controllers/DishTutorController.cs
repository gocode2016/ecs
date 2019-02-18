using ECActivityAPI.Common;
using ECActivityAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
    /// 导师菜品
    /// </summary>
    public class DishTutorController : ApiController
    {
        private CreditContext db = new CreditContext();
        private SqlHelper dataContext = new SqlHelper();
        private ImgHandle imgHandel = new ImgHandle();

       

        #region 新增（修改）导师菜品--导师新增--web
        /// <summary>
        /// 新增（修改）导师菜品--导师新增--web
        /// </summary>
        /// <param name="requetData">导师菜品Json</param>
        /// <returns>OK，成功，No失败</returns>
        [HttpPost]
        public string AddDishTutor(dynamic requestData)
        {
            try
            {

                string addDishTutor = JsonConvert.SerializeObject(requestData);
                DishTutor model = JsonConvert.DeserializeObject<DishTutor>(addDishTutor);
                var IsExist = (from v in db.DishTutor
                               where v.TutorId == model.TutorId && v.DishType == model.DishType
                               select v).FirstOrDefault();
                //等于空就增加
                if (IsExist == null)
                {

                    if (model.DishStory.IndexOf("base64") > 0)
                    {
                        string d = model.DishStory;
                       
                        foreach (string a in imgHandel.GetHtmlImageUrlList(d))
                        {
                            if (a.IndexOf(ImgHandle.DNS) < 0)
                            {
                                string[] asplit = a.Split(',');
                                string imgtype = asplit[0].Substring(11, asplit[0].Length - 18);
                                string filepath = imgHandel.Base64StringToImage(asplit[1], imgtype,"/pic/Dishtutor");
                                d = d.Replace(a, filepath);
                            }
                        }
                        model.DishStory = d;
                    }
                    model.CreateTime = DateTime.Now;
                    model.UpdateTime = DateTime.Now;
                    model.VisitCount = 0;
                    model.PrasieCount = 0;
                    model.ShareCount = 0;
                    db.DishTutor.Add(model);
                    db.SaveChanges();
                    return  model.TutorDishId.ToString();
                }
                //不为空就是修改
                else
                {
                    IsExist.UpdateTime = DateTime.Now;
                    IsExist.DishName = model.DishName;
                    IsExist.DishStory = model.DishStory;

                    if (IsExist.DishStory.IndexOf("base64") > 0)
                    {
                        string d = IsExist.DishStory;

                        foreach (string a in imgHandel.GetHtmlImageUrlList(d))
                        {
                            if (a.IndexOf(ImgHandle.DNS) < 0)
                            {
                                string[] asplit = a.Split(',');
                                string imgtype = asplit[0].Substring(11, asplit[0].Length - 18);
                                string filepath = imgHandel.Base64StringToImage(asplit[1], imgtype, "/pic/Dishtutor");
                                d = d.Replace(a, filepath);
                            }
                        }
                        IsExist.DishStory = d;
                    }

                    IsExist.DishPicUrl = model.DishPicUrl;
                    IsExist.UpdatePerson = model.UpdatePerson;
                    db.Entry<DishTutor>(IsExist).State = EntityState.Modified;
                    db.SaveChanges();
                    return IsExist.TutorDishId.ToString();
                }
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 新增(修改)调料--导师新增--web
        /// <summary>
        /// 新增(修改)导师调料--导师新增--web
        /// </summary>
        /// <param name="requestData">调料数组</param>
        /// <returns>OK，No</returns>
        [HttpPost]
        public string AddDishMaterial(List<DishMaterial> requestData)
        {
            try
            {
                //首先删除已存在菜品Id的调料
                var sql = string.Format(@"Delete DishMaterial where DishId={0}", requestData[0].DishId);
                dataContext.ExecuteNonQuery(CommandType.Text, sql);

                //保存调料信息
                foreach (DishMaterial dm in requestData)
                {
                    db.DishMaterial.Add(dm);
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

        #region 新增(修改)菜品步骤--导师新增--web
        /// <summary>
        /// 新增(修改)菜品步骤--导师新增--web
        /// </summary>
        /// <param name="requestData">菜品Json</param>
        /// <returns></returns>
        [HttpPost]
        public string AddDishStep(List<DishStep> requestData)
        {
            try
            {
                //先删除已有调料
                var sql = string.Format(@"Delete DishStep where DishId={0}", requestData[0].DishId);
                dataContext.ExecuteNonQuery(CommandType.Text, sql);
              
                //保存
                int t = 1;
                foreach (DishStep i in requestData)
                {
                    i.StepId = t;
                    db.DishStep.Add(i);
                    t++;
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

        #region 新增(修改)调料推荐--导师新增--web
        /// <summary>
        /// 新增(修改)调料推荐--导师新增--web
        /// </summary>
        /// <param name="requestData">调料推荐Json</param>
        /// <returns></returns>
        public string AddFR(dynamic requestData)
        {
            try
            {
                string addMatchRegion = JsonConvert.SerializeObject(requestData);
                List<FlavourRecRole> model = JsonConvert.DeserializeObject<List<FlavourRecRole>>(addMatchRegion);
                var sql = string.Format(@"Delete FlavourRecRole where RoleId={0} and DishId={1}", model[0].RoleId,model[0].DishId);
                dataContext.ExecuteNonQuery(CommandType.Text, sql);
                foreach (FlavourRecRole m in model)
                {
                    m.CreateTime = DateTime.Now;
                    m.UpdateTime = DateTime.Now;
                    db.FlavourRecRole.Add(m);
                    db.SaveChanges();
                }
                return "OK";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 获取导师菜品信息(1个菜品)--导师菜品修改页面--web
        /// <summary>
        /// 获取导师菜品信息(1个菜品)--导师菜品修改页面--web
        /// </summary>
        /// <param name="caId">活动ID</param>
        /// <param name="dishType">菜品类型</param>
        /// <param name="tutorId">导师ID</param>
        /// <returns></returns>
        [HttpGet]
        public string GetToutDishSingle(int caId,int dishType, int tutorId)
        {
            try
            {
                #region 菜品的信息

                //菜品基本信息
                var dishType1 = (from v in db.DishTutor
                                 where v.TutorId == tutorId && v.DishType == dishType
                                 select v).FirstOrDefault();
                //菜品调料信息
                var dishType1DishMaterial = (from v in db.DishMaterial
                                             where v.DishId == dishType1.TutorDishId
                                             select new
                                             {
                                                 DishId = v.DishId,
                                                 Material = v.Material,
                                                 Unit = v.Unit,
                                                 MaterialType = v.MaterialType
                                             });

                //菜品步骤信息
                var dishType1DishStep = (from v in db.DishStep
                                         where v.DishId == dishType1.TutorDishId
                                         select new
                                         {
                                             DishId = v.DishId,
                                             //StepId = v.StepId,
                                             StepName = v.StepName
                                         });
                //菜品推荐调料信息
                var sqlSelect = string.Format(@"select FR.*  from FlavourRec as FR join FlavourRecRole FRR on FR.FlavourRecId=FRR.FlavourRecId where FRR.RoleId={0} and FRR.DishId={1}  and FR.FlavourType=1", tutorId, dishType1.TutorDishId);

                var select = dataContext.ExecuteDataTable(CommandType.Text, sqlSelect);


                ////菜品未被推荐的调料信息
                //var sqlNotSelect = string.Format(@"select * from FlavourRec where FlavourRecId not in (select FR.FlavourRecId from FlavourRec as FR join FlavourRecRole FRR on FR.FlavourRecId=FRR.FlavourRecId where FRR.RoleId={0} and FRR.DishId={1}) and ChefActivityId={2} and FlavourType=1", tutorId, dishType1.TutorDishId, caId);
                //var notSelect = dataContext.ExecuteDataTable(CommandType.Text, sqlNotSelect);

                //所用调料信息
                var sqlselectAll = string.Format(@"select FlavourRecId,ChefActivityId,FlavourName,FlavourPicUrl,FlavourType from FlavourRec where ChefActivityId={0} and FlavourType=1", caId);
                var selectAll = dataContext.ExecuteDataTable(CommandType.Text, sqlselectAll);

                #endregion


                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd hh:mm";

                return "{\"dish\":{\"Basic\":" + JsonConvert.SerializeObject(dishType1, iso) + ",\"DishMaterial\":" + JsonConvert.SerializeObject(dishType1DishMaterial, iso) + ",\"DishStep\":" + JsonConvert.SerializeObject(dishType1DishStep, iso) + ",\"Select\":" + JsonConvert.SerializeObject(select) + ",\"AllSelect\":" + JsonConvert.SerializeObject(selectAll) + "}}";
            }
            catch (Exception)
            {
                return "No";
            }
        }

        #endregion       

        #region 获取（1个活动）活动名称，赛区，所用导师调料--新增导师页面--web
        /// <summary>
        /// 获取（1个活动）活动名称，赛区，所用导师调料--新增导师页面--web
        /// </summary>
        /// <param name="caid">活动Id</param>
        /// <returns></returns>
        [HttpGet]
        public string GetcaNameMRFRBycaId(int caId)
        {
            var qca = (from v in db.ChefActivity
                       where v.ChefActivityId == caId
                       select new
                       {
                           ChefActivityId = v.ChefActivityId,
                           ChefActivityName = v.ChefActivityName,
                           IsEnable = v.IsEnable
                       }).FirstOrDefault();
            var qMR = (from v in db.MatchRegion
                       where v.ChefActivityId == caId
                       select new
                       {
                           MatchRegionId = v.MatchRegionId,
                           AreaName = v.AreaName,
                           ProvinceName = v.ProvinceName
                       });
            var qFR = (from v in db.FlavourRec
                       where v.ChefActivityId == caId && v.FlavourType==1
                       select new
                       {
                           FlavourRecId = v.FlavourRecId,
                           ChefActivityId = v.ChefActivityId,
                           FlavourName = v.FlavourName,
                           FlavourPicUrl = v.FlavourPicUrl,
                           FlavourType = v.FlavourType
                       });


            return "{\"ca\":" + JsonConvert.SerializeObject(qca) + ",\"MR\":" + JsonConvert.SerializeObject(qMR) + ",\"FR\":" + JsonConvert.SerializeObject(qFR) + "}";
        }

        #endregion

        #region 修改导师菜品信息，1个接口--导师菜品修改页面--web
        /// <summary>
        /// 修改导师菜品信息，一个接口--导师菜品修改页面--web
        /// </summary>
        /// <param name="re">菜品的所有信息</param>
        /// <returns>OK，No</returns>
        [HttpPost]
        public string UpdateDishTutor(dynamic requestData)
        {
            try
            {

                #region 修改菜品基本信息
                DishTutor model = JsonConvert.DeserializeObject<DishTutor>(JsonConvert.SerializeObject(requestData.dishTutor));
                var IsExist = (from v in db.DishTutor
                               where v.TutorId == model.TutorId && v.DishType == model.DishType
                               select v).FirstOrDefault();
                
                //等于空就增加
                if (IsExist == null)
                {

                    if (model.DishStory.IndexOf("base64") > 0)
                    {
                        string d = model.DishStory;

                        foreach (string a in imgHandel.GetHtmlImageUrlList(d))
                        {
                            if (a.IndexOf(ImgHandle.DNS) < 0)
                            {
                                string[] asplit = a.Split(',');
                                string imgtype = asplit[0].Substring(11, asplit[0].Length - 18);
                                string filepath = imgHandel.Base64StringToImage(asplit[1], imgtype, "/pic/Dishtutor");
                                d = d.Replace(a, filepath);
                            }
                        }
                        model.DishStory = d;
                    }
                    model.CreateTime = DateTime.Now;
                    model.UpdateTime = DateTime.Now;
                    model.VisitCount = 0;
                    model.PrasieCount = 0;
                    model.ShareCount = 0;
                    db.DishTutor.Add(model);
                    db.SaveChanges();
                }

                else
                {
                    IsExist.UpdateTime = DateTime.Now;
                    IsExist.DishName = model.DishName;
                    IsExist.DishStory = model.DishStory;
                    string dd = IsExist.DishStory;
                    if (IsExist.DishStory.IndexOf("base64") > 0)
                    {
                        foreach (string a in imgHandel.GetHtmlImageUrlList(dd))
                        {
                            if (a.IndexOf(ImgHandle.DNS) < 0)
                            {
                                string[] asplit = a.Split(',');
                                string imgtype = asplit[0].Substring(11, asplit[0].Length - 18);
                                string filepath = imgHandel.Base64StringToImage(asplit[1], imgtype, "/pic/Dishtutor");
                                dd = dd.Replace(a, filepath);
                            }
                        }
                    }
                    IsExist.DishStory = dd;
                    IsExist.DishPicUrl = model.DishPicUrl;
                    IsExist.UpdatePerson = model.UpdatePerson;
                    db.Entry<DishTutor>(IsExist).State = EntityState.Modified;
                    db.SaveChanges();
                }
                #endregion

                #region 修改菜品调料

                var middledishMaterial = requestData.dishMaterial;
                //首先删除已存在菜品Id的调料
                List<DishMaterial> qdishMaterial = new List<DishMaterial>();

                for (int i = 0; i < middledishMaterial.Count; i++)
                {
                    qdishMaterial.Add(JsonConvert.DeserializeObject<DishMaterial>(JsonConvert.SerializeObject(middledishMaterial[i])));
                }
                if (IsExist != null)
                {
                    var sql = string.Format(@"Delete DishMaterial where DishId={0}", qdishMaterial[0].DishId);
                    dataContext.ExecuteNonQuery(CommandType.Text, sql);
                }
                //保存调料信息
                foreach (DishMaterial dm in qdishMaterial)
                {
                    if (IsExist == null)
                        dm.DishId = model.TutorDishId;
                    db.DishMaterial.Add(dm);
                }
                db.SaveChanges();

                #endregion

                #region 修改菜品步骤

                var middleDishStep = requestData.dishStep;
                //先删除已有调料


                List<DishStep> qDishStep = new List<DishStep>();

                for (int i = 0; i < middleDishStep.Count; i++)
                {
                    qDishStep.Add(JsonConvert.DeserializeObject<DishStep>(JsonConvert.SerializeObject(middleDishStep[i])));
                }
                if (IsExist != null)
                {
                    string sqlDishStep = string.Format(@"Delete DishStep where DishId={0}", qDishStep[0].DishId);
                    dataContext.ExecuteNonQuery(CommandType.Text, sqlDishStep);
                }
                //保存
                int t = 1;
                foreach (DishStep i in qDishStep)
                {
                    if (IsExist == null)
                    {
                        i.DishId = model.TutorDishId;
                    }
                    i.StepId = t;
                    db.DishStep.Add(i);
                    t++;
                }
                db.SaveChanges();

                #endregion

                #region 修改推荐调料

                var middleflavourRecRole = requestData.flavourRecRole;

                List<FlavourRecRole> modelflavourRecRole = new List<FlavourRecRole>();
                if (middleflavourRecRole.Count > 0)
                {
                    for (int i = 0; i < middleflavourRecRole.Count; i++)
                    {
                        var test1 = JsonConvert.SerializeObject(middleflavourRecRole[i]);
                        FlavourRecRole test2 = JsonConvert.DeserializeObject<FlavourRecRole>(test1);
                        if (IsExist == null)
                        {
                            test2.DishId = model.TutorDishId;
                            test2.RoleId = model.TutorId;
                        }
                        modelflavourRecRole.Add(test2);
                    }


                    var sqlflavourRecRole = string.Format(@"Delete FlavourRecRole where RoleId={0} and DishId={1}", modelflavourRecRole[0].RoleId, modelflavourRecRole[0].DishId);
                    dataContext.ExecuteNonQuery(CommandType.Text, sqlflavourRecRole);
                    foreach (FlavourRecRole m in modelflavourRecRole)
                    {
                        m.CreateTime = DateTime.Now;
                        m.UpdateTime = DateTime.Now;
                        db.FlavourRecRole.Add(m);
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
       


    }
}
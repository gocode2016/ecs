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
    /// 星厨API
    /// </summary>
    public class ChefStarController : ApiController
    {
        private CreditContext db = new CreditContext();
        private SqlHelper dataContext = new SqlHelper();
        private ImgHandle imgHandel = new ImgHandle();


        #region 获取当前放开赛区星厨的调料--星厨详情新增--web
        /// <summary>
        /// 获取当前放开赛区星厨的调料--星厨详情新增--web
        /// </summary>
        /// <param name="caId">活动ID</param>
        /// <returns></returns>
        [HttpGet]
        public string GetChefStarFlavourRec(int caId)
        {
            try
            {
                var q = (from v in db.FlavourRec
                         where v.FlavourType == 3 && v.ChefActivityId == caId
                         select new
                         {
                             FlavourName = v.FlavourName,
                             FlavourRecId = v.FlavourRecId,
                             FlavourPicUrl = v.FlavourPicUrl
                         });

                return JsonConvert.SerializeObject(q);
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 新增星厨--星厨详情新增--web
        /// <summary>
        /// 新增星厨--星厨详情新增--web
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddChefStar(dynamic requestData)
        {
            try
            {
                string addChefStar = JsonConvert.SerializeObject(requestData);
                ChefStar model = JsonConvert.DeserializeObject<ChefStar>(addChefStar);


                if (model.Introduce.IndexOf("base64") > 0)
                {
                    string d = model.Introduce;

                    foreach (string a in imgHandel.GetHtmlImageUrlList(d))
                    {
                        if (a.IndexOf(ImgHandle.DNS) < 0)
                        {
                            string[] asplit = a.Split(',');
                            string imgtype = asplit[0].Substring(11, asplit[0].Length - 18);
                            string filepath =imgHandel.Base64StringToImage(asplit[1], imgtype,"/pic/Dishtutor");
                            d = d.Replace(a, filepath);
                        }
                    }
                    model.Introduce = d;
                }
                model.UpdateTime = DateTime.Now;
                model.UpdatePerson = model.CreatePerson;
                db.ChefStar.Add(model);
                db.SaveChanges();
                return model.ChefStarId.ToString();
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 新增（修改）菜品(基本信息)--星厨详情新增--web
        /// <summary>
        /// 新增（修改）菜品(基本信息)--星厨详情新增--web
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddDishChefStar(dynamic requestData)
        {
            try
            {

                string addDishTutor = JsonConvert.SerializeObject(requestData);
                DishChefStar model = JsonConvert.DeserializeObject<DishChefStar>(addDishTutor);
                var IsExist = (from v in db.DishChefStar
                               where v.ChefStarId == model.ChefStarId && v.DishType == model.DishType
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
                    db.DishChefStar.Add(model);
                    db.SaveChanges();
                    return model.DishChefStarId.ToString();
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
                    db.Entry<DishChefStar>(IsExist).State = EntityState.Modified;
                    db.SaveChanges();
                    return IsExist.DishChefStarId.ToString();
                }
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 新增（修改）星厨调料--星厨详情新增--web
        /// <summary>
        /// 新增（修改）星厨调料--星厨详情新增--web
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
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

        #region 新增（修改）菜品步骤--星厨详情新增--web
        /// <summary>
        /// 新增（修改）菜品步骤--星厨详情新增--web
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

        #region 新增(修改)调料推荐--星厨详情新增--web
        /// <summary>
        /// 新增(修改)调料推荐--星厨详情新增--web
        /// </summary>
        /// <param name="requestData">调料推荐Json</param>
        /// <returns></returns>
        [HttpPost]
        public string AddFR(dynamic requestData)
        {
            try
            {
                string addMatchRegion = JsonConvert.SerializeObject(requestData);
                List<FlavourRecRole> model = JsonConvert.DeserializeObject<List<FlavourRecRole>>(addMatchRegion);
                var sql = string.Format(@"Delete FlavourRecRole where RoleId={0} and DishId={1}", model[0].RoleId, model[0].DishId);
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

        #region 查询星厨列表（分页）--星厨列表--web
        /// <summary>
        /// 查询星厨列表（分页）--星厨列表--web
        /// </summary>
        /// <param name="caId">活动Id</param>
        /// <param name="pageIndex">页数</param>
        /// <returns></returns>
        [HttpPost]
        public string GetChefStarList(dynamic requestData)
        {
            try
            {
                //姓名
                string name = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.Name));
                string city = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.City));

                int pageIndex = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.pageIndex));

                int caId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.ChefActivityId));


                var sql = string.Format(@"select  CS.ChefStarId,  CS.ChefStarName, CS.ChefStarPosition,CS.CityName, CS.HotelName,CS.ChefActivityId,CS.IsDisplay,CS.PriorityId,  TD.DishOne,TD.DishTow 
from ChefStar as CS Left join(
select case when tab1.ChefStarId is null then tab2.ChefStarId else 
tab1.ChefStarId end  ChefStarId,tab1.菜品1 as DishOne,tab2.菜品2 as DishTow from 
(select ChefStarId,DishName as '菜品1' from DishChefStar where DishType=1) tab1
full join 
(select ChefStarId,DishName as '菜品2'  from DishChefStar where DishType=2) tab2
on tab1.ChefStarId=tab2.ChefStarId) as TD on CS.ChefStarId=TD.ChefStarId where CS.ChefActivityId={0}", caId);
                bool bName = name != null && !string.IsNullOrEmpty(name);
                bool bCity = city != null && !string.IsNullOrEmpty(city);

               if (bName)
                        sql = sql + "  and CS.ChefStarName like '%" + name + "%' ";
                if(bCity)
                {
                    if (city != "全部")
                        sql = sql + " and CS.CityName='" + city + "'   ";
                }
                sql = sql + "  order by CS.PriorityId";
                var q = dataContext.ExecuteDataTable(CommandType.Text, sql);

                var count = q.Rows.Count;
                if (pageIndex <= 0)
                    pageIndex = 1;
                var lastq = imgHandel.GetPagedTable(q, pageIndex, 10);

                return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(lastq) + "}";
            }
            catch (Exception)
            {
                return "{ \"Count\":\"" + 0 + "\",\"data\":[]}";
            }
        }
        #endregion

        #region 获取星厨详情--星厨详情，星厨详情编辑--web
        /// <summary>
        /// 获取星厨详情--星厨详情，星厨详情编辑--web
        /// </summary>
        /// <param name="csId">星厨Id</param>
        /// <returns></returns>
        [HttpGet]
        public string GetChefStarByCSId(int csId)
        {
            try
            {
                var q = (from v in db.ChefStar
                         where v.ChefStarId == csId
                         select new
                         {
                             ChefStarId = v.ChefStarId,
                             ChefStarName = v.ChefStarName,
                             ChefStarPosition = v.ChefStarPosition,
                             HotelName = v.HotelName,
                             CityName = v.CityName,
                             IsDisplay = v.IsDisplay,
                             HeadPicUrl = v.HeadPicUrl,
                             PicUrl = v.PicUrl,
                             Introduce = v.Introduce,
                             PriorityId=v.PriorityId
                         }).FirstOrDefault();

                return JsonConvert.SerializeObject(q);
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 编辑星厨信息--星厨详情编辑--web
        /// <summary>
        /// 编辑星厨信息--星厨详情编辑--web
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string UpdateChefStar(dynamic requestData)
        {
            try
            {
                string chefStar = JsonConvert.SerializeObject(requestData);
                ChefStar model = JsonConvert.DeserializeObject<ChefStar>(chefStar);

                
                var q = (from v in db.ChefStar
                         where v.ChefStarId == model.ChefStarId
                         select v).FirstOrDefault();

                q.ChefStarName = model.ChefStarName;
                q.ChefStarPosition = model.ChefStarPosition;
                q.CityName=model.CityName;
                q.HeadPicUrl=model.HeadPicUrl;
                q.HotelName=model.HotelName;

                if (model.Introduce.IndexOf("base64") > 0)
                {
                    string d = model.Introduce;

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
                    model.Introduce = d;
                }

                q.Introduce=model.Introduce;

                q.UpdateTime = DateTime.Now;
                q.UpdatePerson = model.UpdatePerson;
                q.IsDisplay = model.IsDisplay;
                q.PriorityId = model.PriorityId;
                q.PicUrl = model.PicUrl;
                model.ChefStarId = 0;

                db.Entry<ChefStar>(q).State = EntityState.Modified;
                db.SaveChanges();
                return "OK";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion


        #region 读取菜品（1个菜）信息--星厨详情，星厨详情编辑--web
        /// <summary>
        /// 读取菜品（1个菜）信息--星厨详情，星厨详情编辑--web
        /// </summary>
        /// <param name="caId">活动Id</param>
        /// <param name="dishType">菜品类型</param>
        /// <param name="chefStarId">星厨Id</param>
        /// <returns></returns>
        [HttpGet]
        public string GetDishChefStar(int caId, int dishType, int chefStarId)
        {
            try
            {
                #region 菜品的信息

                //菜品基本信息
                var dishType1 = (from v in db.DishChefStar
                                 where v.ChefStarId == chefStarId && v.DishType == dishType 
                                 select v).FirstOrDefault();
                //菜品调料信息
                var dishType1DishMaterial = (from v in db.DishMaterial
                                             where v.DishId == dishType1.DishChefStarId
                                             select new
                                             {
                                                 DishId = v.DishId,
                                                 Material = v.Material,
                                                 Unit = v.Unit,
                                                 MaterialType = v.MaterialType
                                             });

                //菜品步骤信息
                var dishType1DishStep = (from v in db.DishStep
                                         where v.DishId == dishType1.DishChefStarId
                                         select new
                                         {
                                             DishId = v.DishId,
                                             //StepId = v.StepId,
                                             StepName = v.StepName
                                         });
                //菜品推荐调料信息
                var sqlSelect = string.Format(@"select FR.*  from FlavourRec as FR join FlavourRecRole FRR on FR.FlavourRecId=FRR.FlavourRecId where FRR.RoleId={0} and FRR.DishId={1} and FR.FlavourType=3", chefStarId, dishType1.DishChefStarId);

                var select = dataContext.ExecuteDataTable(CommandType.Text, sqlSelect);


                

                //所用调料信息
                var sqlselectAll = string.Format(@"select FlavourRecId,ChefActivityId,FlavourName,FlavourPicUrl,FlavourType from FlavourRec where ChefActivityId={0} and FlavourType=3", caId);
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

        #region 修改菜品信息，1个接口--星厨详情，星厨详情编辑--web
        /// <summary>
        /// 修改菜品信息，1个接口--星厨详情，星厨详情编辑--web
        /// </summary>
        /// <param name="re">菜品的所有信息</param>
        /// <returns>OK，No</returns>
        [HttpPost]
        public string UpdateDishChefStar(dynamic requestData)
        {
            try
            {

                #region 修改菜品基本信息
                DishChefStar model = JsonConvert.DeserializeObject<DishChefStar>(JsonConvert.SerializeObject(requestData.dishChefStar));
                var IsExist = (from v in db.DishChefStar
                               where v.ChefStarId == model.ChefStarId && v.DishType == model.DishType
                               select v).FirstOrDefault();

                var dishChefDishId = 0;


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
                    model.CreatePerson = model.UpdatePerson;
                    model.VisitCount = 0;
                    model.PrasieCount = 0;
                    model.ShareCount = 0;
                    db.DishChefStar.Add(model);
                    db.SaveChanges();
                    dishChefDishId = model.DishChefStarId;
                }
                else
                {
                    IsExist.UpdateTime = DateTime.Now;
                    IsExist.DishName = model.DishName;
                    IsExist.DishStory = model.DishStory;
                    string d = IsExist.DishStory;
                    if (IsExist.DishStory.IndexOf("base64") > 0)
                    {
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
                    }
                    IsExist.DishStory = d;
                    IsExist.DishPicUrl = model.DishPicUrl;
                    IsExist.UpdatePerson = model.UpdatePerson;
                    model.DishChefStarId = 0;
                    db.Entry<DishChefStar>(IsExist).State = EntityState.Modified;
                    db.SaveChanges();
                    dishChefDishId = IsExist.DishChefStarId;
                }
                
                #endregion

                try
                {
                    #region 修改菜品调料

                    var middledishMaterial = requestData.dishMaterial;
                    //首先删除已存在菜品Id的调料
                    List<DishMaterial> qdishMaterial = new List<DishMaterial>();

                    for (int i = 0; i < middledishMaterial.Count; i++)
                    {
                        qdishMaterial.Add(JsonConvert.DeserializeObject<DishMaterial>(JsonConvert.SerializeObject(middledishMaterial[i])));
                    }

                    var sql = string.Format(@"Delete DishMaterial where DishId={0}", dishChefDishId);
                    dataContext.ExecuteNonQuery(CommandType.Text, sql);

                    //保存调料信息
                    foreach (DishMaterial dm in qdishMaterial)
                    {
                        dm.DishId = dishChefDishId;
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
                    string sqlDishStep = string.Format(@"Delete DishStep where DishId={0}", dishChefDishId);
                    dataContext.ExecuteNonQuery(CommandType.Text, sqlDishStep);
                    //保存
                    int t = 1;
                    foreach (DishStep i in qDishStep)
                    {
                        i.DishId = dishChefDishId;
                        i.StepId = t;
                        db.DishStep.Add(i);
                        t++;
                    }
                    db.SaveChanges();

                    #endregion

                    #region 修改推荐调料

                    var middleflavourRecRole = requestData.flavourRecRole;

                    List<FlavourRecRole> modelflavourRecRole = new List<FlavourRecRole>();
                    for (int i = 0; i < middleflavourRecRole.Count; i++)
                    {
                        var test1 = JsonConvert.SerializeObject(middleflavourRecRole[i]);
                        var test2 = JsonConvert.DeserializeObject<FlavourRecRole>(test1);
                        modelflavourRecRole.Add(test2);
                    }


                    var sqlflavourRecRole = string.Format(@"Delete FlavourRecRole where RoleId={0} and DishId={1}", modelflavourRecRole[0].RoleId, dishChefDishId);
                    dataContext.ExecuteNonQuery(CommandType.Text, sqlflavourRecRole);
                    foreach (FlavourRecRole m in modelflavourRecRole)
                    {
                        m.DishId = dishChefDishId;
                        m.CreateTime = DateTime.Now;
                        m.UpdateTime = DateTime.Now;
                        db.FlavourRecRole.Add(m);
                        db.SaveChanges();
                    }
                    #endregion
                }
                catch (Exception)
                {
                    return "OK";
                }

                return "OK";
            }
            catch (Exception)
            {
                return "No";
            }
        }


        #endregion

        #region 删除单个星厨信息--星厨列表--web
        /// <summary>
        /// 删除单个星厨信息--星厨列表--web
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string DeleteSingleChefStar(int chefStarId)
        {
            try
            {
                var sql = string.Format(@"delete DishMaterial where DishId in (select DishChefStarId from DishChefStar where ChefStarId={0})
delete DishStep  where DishId in (select DishChefStarId from DishChefStar where ChefStarId={0})
delete FlavourRecRole where DishId in (select DishChefStarId from DishChefStar where ChefStarId={0})

delete DishChefStar where ChefStarId={0}
delete [ChefStar]  where  ChefStarId={0}", chefStarId);
                dataContext.ExecuteNonQuery(CommandType.Text, sql);
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

using ECActivityAPI.Common;
using ECActivityAPI.Models;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace ECActivityAPI.Controllers
{
    public class BannerController : ApiController
    {
        private CreditContext db = new CreditContext();
        private SqlHelper dataContext = new SqlHelper();
        private ImgHandle imgHandle = new ImgHandle();

        #region 增加轮播图--轮播图列表--web
        /// <summary>
        /// 增加轮播图--轮播图列表--web
        /// </summary>
        /// <param name="requestData">增加轮播图 数组</param>
        /// <returns>OK,成功，No，失败</returns>
       [HttpPost]
        public string AddBanner(dynamic requestData)
        {
            try
            {
                string addBanner = JsonConvert.SerializeObject(requestData);
                Banner model = JsonConvert.DeserializeObject<Banner>(addBanner);

                model.UpdateTime = DateTime.Now;
                model.CreateTime = DateTime.Now;
                model.UpdatePerson = model.CreatePerson;
                db.Banner.Add(model);
               
                db.SaveChanges();
                return "OK";
            }
            catch (Exception)
            {
                return "No";
            }
            
        } 
        #endregion

        #region 修改信息--轮播图列表--web
       /// <summary>
       /// 修改单个信息--轮播图列表--web
        /// </summary>
        /// <param name="bannerDate">轮播图对象</param>
        /// <returns>OK，修改成功，No，失败</returns>
        [HttpPost]
        public string UpdateSingle(dynamic bannerDate)
        {
            try
            {
                string updateSingle = JsonConvert.SerializeObject(bannerDate);
                Banner q = JsonConvert.DeserializeObject<Banner>(updateSingle);

                var d = (from v in db.Banner
                         where v.BannerId == q.BannerId
                         select v).FirstOrDefault();
                q.BannerId = 0;

                d.PicUrl = q.PicUrl;
                d.Link = q.Link;
                d.PriorityId = q.PriorityId;
                d.IsDisplay = q.IsDisplay;
                d.BannerType = q.BannerType;
                d.UpdatePerson = q.UpdatePerson;
                d.UpdateTime = DateTime.Now;
                
                db.Entry<Banner>(d).State = EntityState.Modified;
                db.SaveChanges(); 
                return "OK";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 单个轮播图删除删除信息--轮播图列表--web
       /// <summary>
        ///  单个轮播图删除删除信息--轮播图列表--web
       /// </summary>
       /// <param name="requestData"></param>
       /// <returns></returns>
        [HttpPost]
        public string UpdateIsDel(dynamic requestData)
        {
            try
            {
                int bannerId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.BannerId));
                //先查询
                var banner = (from v in db.Banner
                              where v.BannerId == bannerId
                              select v).Single();
                db.Banner.Remove(banner);
                var count = db.SaveChanges();
                if (count > 0)
                {
                    var q = (from v in db.Banner
                             orderby v.BannerId
                             select new
                             {
                                 BannerId = v.BannerId,
                                 BannerType = v.BannerType,
                                 PicUrl = v.PicUrl,
                                 Link = v.Link,
                                 IsDisplay = v.IsDisplay == 1 ? "是" : "否",
                                 PriorityId = v.PriorityId
                             });
                    var count1 = q.Count();
                    var lastq = q.Skip(0).Take(10);
                    return "{ \"Count\":\"" + count1 + "\",\"data\":" + JsonConvert.SerializeObject(lastq, DateTimeConvent.DateTimehh()) + "}";
                }
                else
                    return "No";
            }
            catch (Exception)
            {
                return "No";
            }
        }

        #endregion

        #region 获取轮播图列表--轮播图列表--web
        /// <summary>
        /// 获取轮播图列表--轮播图列表--web
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetBannerList(int pageIndex,int pageCount)
        {
            try
            {
                if (pageIndex < 1)
                    pageIndex = 1;
                var q = (from v in db.Banner
                         orderby v.BannerId
                         select new
                         {
                             BannerId = v.BannerId,
                             BannerType = v.BannerType,
                             PicUrl = v.PicUrl,
                             Link = v.Link,
                             IsDisplay = v.IsDisplay == 1 ? "是" : "否",
                             PriorityId = v.PriorityId,
                         });
                var count = q.Count();
                var lastq = q.OrderBy(v=>v.BannerId).Skip((pageIndex - 1) * pageCount).Take(pageCount);
               return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(lastq, DateTimeConvent.DateTimehh()) + "}";
            }
            catch (Exception)
            {
                return "No";
            }
        }



        #endregion

        #region 批量删除轮播信息--轮播图列表--web
        /// <summary>
        ///  批量删除轮播信息--轮播图列表--web
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string DelBannerMore(dynamic requestData)
        {
            try
            {
                string bannerIds = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.BannerIds));
                string[] bannerIdstring = null;
                int count = 0;
                int bannerId = 0;
                if (bannerIds.Contains(","))
                    bannerIdstring = bannerIds.Split(',');
                else
                    bannerId = Convert.ToInt32(bannerIds);
                if (bannerId != 0)
                {
                    var banner = (from v in db.Banner
                                  where v.BannerId == bannerId
                                  select v).Single();
                    db.Banner.Remove(banner);
                    count = db.SaveChanges();
                }
                if (bannerId== 0)
                {
                    for (int i = 0; i < bannerIdstring.Count(); i++)
                    {

                        int p = int.Parse(bannerIdstring[i]);
                        var banner = (from v in db.Banner
                                      where v.BannerId == p
                                      select v).Single();
                        db.Banner.Remove(banner);
                    }
                    count = db.SaveChanges();
                }

                if (count > 0)
                {
                    //var q = (from v in db.Banner
                    //         orderby v.BannerId
                    //         select new
                    //         {
                    //             BannerId = v.BannerId,
                    //             BannerType = v.BannerType,
                    //             PicUrl = v.PicUrl,
                    //             Link = v.Link,
                    //             IsDisplay = v.IsDisplay == 1 ? "是" : "否",
                    //             PriorityId = v.PriorityId
                    //         });

                    //var count1 = q.Count();
                    //var lastq = q.Skip(0).Take(10);
                    return "OK";
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

        #region 获取单个轮播信息--轮播图列表--web
       /// <summary>
        /// 获取单个轮播信息--轮播图列表--web
       /// </summary>
       /// <param name="bannerId"></param>
       /// <returns></returns>
        [HttpGet]
        public string getSingleBanner(int bannerId)
        {
            var q = (from v in db.Banner
                     where v.BannerId==bannerId
                     select new
                     {
                         BannerId = v.BannerId,
                         BannerType = v.BannerType,
                         PicUrl = v.PicUrl,
                         Link = v.Link,
                         IsDisplay = v.IsDisplay ,
                         PriorityId = v.PriorityId
                     }).FirstOrDefault();

            return JsonConvert.SerializeObject(q);
        }
        #endregion

        #region 读取轮播图，活动，赛区信息--EC活动首页--wx
        /// <summary>
        /// 读取轮播图，活动，赛区信息--EC活动首页--wx
        /// </summary>
        /// <param name="chefActivityId">活动ID</param>
        /// <param name="matchRegionId">赛区ID</param>
        /// <returns>非空返回轮播图JSon，空返回No</returns>
        [HttpGet]
        public string GetBanner()
        {
            //获取当前活动信息
            var qChefActivity = (from v in db.ChefActivity
                                 where v.IsEnable == 1
                                 select new
                                 {
                                     ChefActivityId = v.ChefActivityId,
                                     ChefName = v.ChefActivityName
                                 }).FirstOrDefault();

            #region 获取当前活动下的比赛区域
            ////获取当前活动下的比赛区域
            // var qMatchRegion = (from v in db.MatchRegion
            //                     where v.ChefActivityId == qChefActivity.ChefActivityId && v.EndTime > DateTime.Now
            //                     select v).GroupBy(a => a.ProvinceName).ToList();
            ////对比赛区域做一个Json格式化
            // string json = "[";
            // for (int i = 0; i < qMatchRegion.Count; i++)
            // {
            //     var ddddd = qMatchRegion[i].Key;
            //     var sql = string.Format(@"select MatchRegionId,AreaName,ChefActivityId from MatchRegion where ChefActivityId={0} and  ProvinceName='{1}' and  EndTime>'{2}'", qChefActivity.ChefActivityId, qMatchRegion[i].Key, DateTime.Now);
            //     var qAreaName = dataContext.ExecuteDataTable(CommandType.Text, sql);

            //     if (qAreaName.Rows.Count > 0)
            //     {
            //         json += "{\"ProvinceName\":\"" + qMatchRegion[i].Key + "\"," + "\"AreaName\":" + JsonConvert.SerializeObject(qAreaName) + "}";
            //         if (i != (qMatchRegion.Count - 1))
            //             json += ",";
            //     }
            // }
            // json += "]"; 
            #endregion

            //0表示公共轮播图
            var q = (from v in db.Banner
                     where v.IsDisplay == 1 && v.BannerType==0
                     orderby v.PriorityId ascending
                     select new
                     {
                         BannerId= v.BannerId,
                         PicUrl = v.PicUrl,
                         Link = v.Link
                     });
            if (q != null)
                return "{\"ChefActivity\":" + JsonConvert.SerializeObject(qChefActivity) + ",\"Banner\":" + JsonConvert.SerializeObject(q) + "}";
            else
                return "No";
        }

        #endregion

        #region 读取公告信息--EC活动首页--wx
        /// <summary>
        /// 读取公告信息--EC活动首页--wx
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetNotice()
        {
            var sql = @"  select distinct top 10  RM.MemberName,MR.AreaName from Chef as c 
               join MatchRegion as MR on c.MatchRegionId=MR.MatchRegionId
			   join DishChef as DC  on c.ChefId=DC.ChefId
			   join RegistMember as RM on RM.MemberId=c.MemberId
			   where DC.IsApply=1 and c.ChefActivityId=(select ChefActivityId from ChefActivity where IsEnable=1) ";
            var d = dataContext.ExecuteDataTable(CommandType.Text, sql);
            return JsonConvert.SerializeObject(d);
        }
        #endregion

        #region 根据首页活动获取导师信息--EC活动首页--wx
        /// <summary>
        /// 根据首页活动获取导师信息--EC活动首页--wx
        /// </summary>
        /// <returns>活动信息</returns>
        [HttpGet]
        public string GetTutorByChefActivityId()
        {
            try
            {
                //获取当前活动信息
                var qChefActivity = (from v in db.ChefActivity
                                     where v.IsEnable == 1
                                     select new
                                     {
                                         ChefActivityId = v.ChefActivityId,
                                         ChefName = v.ChefActivityName
                                     }).FirstOrDefault();
                if (qChefActivity != null)
                {
                    var q = (from v in db.Tutor
                             where v.ChefActivityId == qChefActivity.ChefActivityId && v.IsDisplay == 1 && v.IsDel == 1
                             orderby v.PriorityId
                             select new { 
                                 TutorId=v.TutorId,
                                 HeadPicUrl=v.HeadPicUrl,
                                 TutorName=v.TutorName,
                                 ChefActivityId = v.ChefActivityId
                             });
                    return JsonConvert.SerializeObject(q);
                }
                else
                    return "[]";
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 参赛菜品，最新入选，人气枪手，星厨列表--EC活动首页--wx
        /// <summary>
        /// 参赛菜品，最新入选，人气枪手，星厨列表--EC活动首页--wx
        /// </summary>
        /// <param name="openId">OpenId</param>
        /// <returns></returns>
        [HttpGet]
        public string GetDishChefHomePage(string openId)
        {
            try
            {
                //获取当前活动信息
                var qChefActivity = (from v in db.ChefActivity
                                     where v.IsEnable == 1
                                     select new
                                     {
                                         ChefActivityId = v.ChefActivityId,
                                         ChefName = v.ChefActivityName
                                     }).FirstOrDefault();
                if (qChefActivity != null)
                {  //最新入选
                    var newSelectSql = string.Format(@"select b.*,case when  DCL.Id is null then 'false' else 'ture' end as IsCollect  from( select top 6 DC.DishChefId as DishChefId,DC.IsSelected,c.ChefId,DC.DishName,DC.DishPicUrl,DC.DishType,DC.VisitCount,RM.HotelName,RM.MemberName 
 from  Chef as c 
                join DishChef as DC on c.ChefId=DC.ChefId
				join RegistMember as RM on c.MemberId=RM.MemberId
where DC.IsApply=1 and c.ChefActivityId={0} 
order by DC.ApplyTime desc)as b 
        left join (select * from DishCollectLog where OpenId='{1}') as DCL on b.DishChefId=DCL.DishId", qChefActivity.ChefActivityId, openId);

                    var newSelectSqlJson = dataContext.ExecuteDataTable(CommandType.Text, newSelectSql);


                    //人气枪手
                    var newTopVisitCountSql = string.Format(@"select b.*,case when  DCL.Id is null then 'false' else 'ture' end as IsCollect  from( select top 6 DC.DishChefId as DishChefId,DC.IsSelected,c.ChefId,DC.DishName,DC.DishPicUrl,DC.DishType,DC.VisitCount,RM.HotelName,RM.MemberName 
 from  Chef as c 
                join DishChef as DC on c.ChefId=DC.ChefId
				join RegistMember as RM on c.MemberId=RM.MemberId
where DC.IsApply=1 and c.ChefActivityId={0} 
order by DC.VisitCount desc)as b 
        left join (select * from DishCollectLog where OpenId='{1}') as DCL on b.DishChefId=DCL.DishId", qChefActivity.ChefActivityId, openId);

                    var newTopVisitCountJson = dataContext.ExecuteDataTable(CommandType.Text, newTopVisitCountSql);



                    //星厨列表

                    var qChefStar = (from v in db.ChefStar
                                     from p in db.DishChefStar
                                     where v.ChefActivityId == qChefActivity.ChefActivityId && v.IsDisplay == 1 && v.ChefStarId==p.ChefStarId
                                     orderby v.PriorityId
                                     select new {
                                         ChefStarId=v.ChefStarId,
                                         HeadPicUrl=v.HeadPicUrl,
                                         ChefStarName=v.ChefStarName,
                                         ChefStarPosition=v.ChefStarPosition,
                                         ChefActivityId=v.ChefActivityId
                                     });
                    qChefStar = qChefStar.Distinct();



                    return "{\"newSelectSqlJson\":" + JsonConvert.SerializeObject(newSelectSqlJson) + ",\"newTopVisitCountJson\":" + JsonConvert.SerializeObject(newTopVisitCountJson) + ",\"newChefStar\":" + JsonConvert.SerializeObject(qChefStar) + "}";
                }
                else
                    return "[]";
            }
            catch (Exception)
            {
                return "No";
            }

        }



        #endregion

        #region 判断是否可以报名--EC活动首页--wx
        /// <summary>
        /// 判断是否可以报名--EC活动首页--wx
        /// </summary>
        /// <returns>-2 没有报名,-1没有任何菜品,0、待审核   1、审核通过   2、审核不通过、4、表示填写了一道菜品信息</returns>
        [HttpPost]
        public string IsApply(dynamic requestData)
        {

            try
            {
                //获取当前活动信息
                var qChefActivity = (from v in db.ChefActivity
                                     where v.IsEnable == 1
                                     select new
                                     {
                                         ChefActivityId = v.ChefActivityId,
                                         ChefName = v.ChefActivityName
                                     }).FirstOrDefault();

                int MemberId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.MemberId));


                string isApply = "-2";//-2 表示没有报名信息或者报名信息不全


                #region 首先判断厨师基本信息是否填写完毕

                var chef = (from v in db.Chef
                            where v.ChefActivityId == qChefActivity.ChefActivityId && v.MemberId == MemberId
                            select v).FirstOrDefault();

                if (chef == null)
                    return isApply;//厨师信息没有填
                //var isChefNull = "f";
                if (string.IsNullOrEmpty(chef.ApplyType) || chef.Birthday == Convert.ToDateTime("1900-01-01 00:00:00.000") || string.IsNullOrEmpty(chef.ChefPicUrl) || string.IsNullOrEmpty(chef.ChefHotelPicUrl))
                {
                    isApply = "-2";
                }
                if (chef.ApplyType != "自我推荐" && string.IsNullOrEmpty(chef.ReferrerName))
                {
                    isApply = "-2";
                }
                //表明基本信息完成，菜品还未填写
                isApply = "-1";
                #endregion



                #region 菜品信息是否保存完成


                var dishChef = (from v in db.DishChef
                                where v.ChefId == chef.ChefId
                                select v);
                if (dishChef == null)
                {
                    isApply = "-1";//-1表示没有填写任何菜品信息
                }
                else if (dishChef.Count() == 1)
                {  
                    isApply = "4";//4表示填只写了一道菜品并且信息不完整
                    var firstDishChef = dishChef.FirstOrDefault();
                    var zhu = (from v in db.DishMaterial
                               where v.DishId == firstDishChef.DishChefId && v.MaterialType == 1
                               select v);
                    var fu = (from v in db.DishMaterial
                              where v.DishId == firstDishChef.DishChefId && v.MaterialType == 2
                              select v);
                    var tiao = (from v in db.DishMaterial
                              where v.DishId == firstDishChef.DishChefId && v.MaterialType == 3
                              select v);
                    var step = (from v in db.DishStep
                                where v.DishId == firstDishChef.DishChefId 
                                select v);
                    if (string.IsNullOrEmpty(firstDishChef.DishName) || string.IsNullOrEmpty(firstDishChef.DishStory) || string.IsNullOrEmpty(firstDishChef.DishPicUrl)||zhu==null||fu==null||tiao==null||step==null)
                        isApply = "4";
                }
                else if (dishChef.Count() == 2)
                {
                    foreach (DishChef d in dishChef)
                    {
                        var zhu = (from v in db.DishMaterial
                                   where v.DishId == d.DishChefId && v.MaterialType == 1
                                   select v);
                        var fu = (from v in db.DishMaterial
                                  where v.DishId == d.DishChefId && v.MaterialType == 2
                                  select v);
                        var tiao = (from v in db.DishMaterial
                                    where v.DishId == d.DishChefId && v.MaterialType == 3
                                    select v);
                        var step = (from v in db.DishStep
                                    where v.DishId == d.DishChefId
                                    select v);
                        if (string.IsNullOrEmpty(d.DishName) || string.IsNullOrEmpty(d.DishStory) || string.IsNullOrEmpty(d.DishPicUrl) || zhu == null || fu == null || tiao == null || step == null)
                            isApply = "4";
                        else
                            isApply = d.IsApply.ToString();//0、待审核   1、审核通过   2、审核不通过 
                    }
                }
                
                
                #endregion

                return isApply;

            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion

        #region 目前有的我的足迹--我的足迹--wx
        /// <summary>
        /// 目前有的我的足迹--我的足迹--wx
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetMyTrack(dynamic requestData)
        {

            string openId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.OpenId));
            int memeberId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.MemberId));
            int pageIndex = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.pageIndex));

            string keyword = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(requestData.KeyWord));




            string sql = string.Format(@"select * from 
 (
 select '知味报名' as module,c.CreateTime as CreateTime,CA.ChefActivityName  as Name from Chef as c join ChefActivity as CA on c.ChefActivityId=CA.ChefActivityId  where MemberId={0}

 union all
 select '菜品投票' as   module ,DSL.CreateTime as CreateTime , CA.ChefActivityName as Name from  DishSelectedLog as DSL join ChefActivity as CA on DSL.ChefActivityId=CA.ChefActivityId where OpenId='{1}'

 ) as a where  1=1 ", memeberId, openId);
            if (!string.IsNullOrEmpty(keyword))
            {
                sql += "and a.module like '%" + keyword + "%' or a.Name like '%" + keyword + "%' ";
            }


            var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
            var count = q.Rows.Count;

            if (pageIndex <= 0 || string.IsNullOrEmpty(keyword))
                pageIndex = 1;

            var lastq = imgHandle.GetPagedTable(q, pageIndex, 10);

            return "{ \"Count\":\"" + count + "\",\"data\":" + JsonConvert.SerializeObject(lastq, DateTimeConvent.DateTimedd()) + "}";
           
        }
        #endregion

        #region 读取商城轮播图--商城--wx
        /// <summary>
        /// 读取商城轮播图--商城--wx
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetShopingBanner()
        {
            try
            {
                var q = (from v in db.Banner
                         where v.IsDisplay == 1 && v.BannerType == 1
                         orderby v.PriorityId ascending
                         select new
                         {
                             BannerId = v.BannerId,
                             PicUrl = v.PicUrl,
                             Link = v.Link
                         });
                return JsonConvert.SerializeObject(q);
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion


        #region 获取轮播基本信息--做成获取统一轮播图接口--wx
        /// <summary>
        ///  获取轮播基本信息--做成获取统一轮播图接口--wx
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetBanner(dynamic requestData)
        {
            try
            {
                int bannerType = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(requestData.BannerType));
                var q = (from v in db.Banner
                         where v.IsDisplay == 1 && v.BannerType == bannerType
                         orderby v.PriorityId ascending
                         select new
                         {
                             BannerId = v.BannerId,
                             PicUrl = v.PicUrl,
                             Link = v.Link
                         });
                return JsonConvert.SerializeObject(q);
            }
            catch (Exception)
            {
                return "No";
            }
        }
        #endregion
    }
}

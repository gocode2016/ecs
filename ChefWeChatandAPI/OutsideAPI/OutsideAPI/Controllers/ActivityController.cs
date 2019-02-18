using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using OutsideAPI.Common;
using OutsideAPI.Models;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace OutsideAPI.Controllers
{
    /// <summary>
    /// 第三方活动数据通过接口进入系统
    /// </summary>
    public class ActivityController : ApiController
    {
        private CreditContext db = new CreditContext();
        /// <summary>
        /// 获取所有省市区及城市级别
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetRegion()
        {
            //string sql = string.Format("select p.ProvinceId, p.ProvinceName, c.CityId, c.CityName, a.AreaId, a.AreaName, a.AreaLevel from Province as p left join City as c on p.ProvinceId = c.ProvinceId left join Area as a on c.CityId = a.CityId");

            var sql = string.Format("select ProvinceId, ProvinceName from Province");
            var pro = SqlHelper.ExecuteDataTable(sql);
            string province = JsonConvert.SerializeObject(pro);

            sql = string.Format("select ProvinceId, CityId, CityName from City");
            var cit = SqlHelper.ExecuteDataTable(sql);
            string city = JsonConvert.SerializeObject(cit);

            sql = string.Format("select CityId, AreaId, AreaName, AreaLevel from Area");
            var are = SqlHelper.ExecuteDataTable(sql);
            string area = JsonConvert.SerializeObject(are);

            return "{\"Province\":" + province + ",\"City\":" + city + ", \"Area\": " + area + " }";
        }
        [HttpGet]
        public string GetUserByOpenId(string openid = "")
        {
            string sql = string.Format(@"SELECT top 1 a.MemberName,a.MemberTelePhone,a.PositionType,a.Position,a.ProvinceId,a.ProvinceName,a.CityId,a.CityName,
                                        a.AreaId,a.AreaName ,a.HotelName,a.HotelAddress
                                        FROM dbo.RegistMember a JOIN dbo.OpenIdAssociated b ON a.MemberId=b.UserId AND b.UserType=2
                                        LEFT JOIN dbo.MemberProfile c ON c.MemberId = a.MemberId
                                        WHERE b.OpenId = '{0}'", openid);
            DataTable dt = SqlHelper.ExecuteDataTable(sql);
            return JsonConvert.SerializeObject(dt);
        }

        [HttpGet]
        public string TempActivityDataToEcs()
        {
            try
            {
                string sql = string.Format(@"SELECT Id,prize_name,name,job1,job2,mobile,hotel,province,province_id,city,
                        city_id,area,area_id,address,createtime,openid,nickname FROM dbo.TempActivityData
                        WHERE handle_status IS NULL OR handle_status <>'succ' ORDER BY Id");
                DataTable dt = SqlHelper.ExecuteDataTable(sql);
                int id = 0;
                System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
 
                foreach (DataRow dr in dt.Rows)
                {
                    id = Convert.ToInt32(dr["Id"]);
                    ActivityData data = new ActivityData();
                    data.PrizeName = dr["prize_name"].ToString();
                    data.Name = dr["name"].ToString();
                    data.PositionType = dr["job1"].ToString();
                    data.Position = dr["job2"].ToString();
                    data.Phone = dr["mobile"].ToString();
                    data.HotelOrMdName = dr["hotel"].ToString();
                    data.ProvinceName = dr["province"].ToString();
                    data.ProvinceId = Convert.ToInt32(dr["province_id"]);
                    data.CityName = dr["city"].ToString();
                    data.CityId = Convert.ToInt32(dr["city_id"]);
                    data.AreaName = dr["area"].ToString();
                    data.AreaId = Convert.ToInt32(dr["area_id"]);
                    data.Address = dr["address"].ToString();

                    data.AddDate = startTime.AddSeconds(Convert.ToDouble(dr["createtime"])).ToString("yyyy-MM-dd HH:mm:ss");     //时间戳转日期

                    data.OpenId = dr["openid"].ToString();
                    data.NickName = dr["nickname"].ToString();
                    string result = HandleAction(data);
                    if (result == "succ")
                    {
                        //更新状态为succ
                        sql = string.Format(@"UPDATE dbo.TempActivityData 
                                            SET handle_status = 'succ',handle_date= getdate(),handle_result_message = 'succ' 
                                            WHERE Id ={0} ",id);
                        SqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                    }
                    else
                    {
                        //更新状态为fail
                        sql = string.Format(@"UPDATE dbo.TempActivityData 
                                            SET handle_status = 'fail',handle_date= getdate(),handle_result_message = '{0}' 
                                            WHERE Id ={1} ",result,id);
                        SqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                    }
                }
                return "处理完毕";
            }
            catch (Exception ex)
            {
                Common.LogHelper.WriteLog(ex.ToString());
                return "fail";
            }
        }

        [HttpPost]
        public string AddInfo(dynamic requestData)
        {
            Common.LogHelper.WriteLog(requestData.ToString());

            try
            {
                string opendid = requestData.OpenId;
                string nowtime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                Random rd = new Random();
                string flag = nowtime + rd.Next(10000, 100000).ToString();

                string sql = string.Format(@"INSERT INTO dbo.ActivityData
                                              ( OpenId ,JsonString,Flag )
                                                VALUES  ( '{0}' , '{1}','{2}')", opendid, requestData.ToString(), flag);
                SqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                //redisClient.Select(3);
                //redisClient.Set("Ecs:ActivityInfo:" + opendid + nowtime, requestData.ToString());
                ActivityData data = JsonConvert.DeserializeObject<ActivityData>(requestData.ToString());
                string result = HandleAction(data);
                if (result == "succ")
                {
                    //更新状态为succ
                    sql = string.Format(@"UPDATE dbo.ActivityData 
                                             SET HandleStatus = '{0}',HandleResult = '{1}',HandleDate = GETDATE() ,IsHandle =1
                                             WHERE  Flag ='{2}'", "succ", "succ", flag);
                    SqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                    return "succ";
                }
                else
                {
                    //更新状态为fail
                    sql = string.Format(@"UPDATE dbo.ActivityData 
                                             SET HandleStatus = '{0}',HandleResult = '{1}',HandleDate = GETDATE(),IsHandle =1
                                             WHERE Flag ='{2}'", "fail", result.Length > 1000 ? result.Substring(0, 1000) : result, flag);
                    SqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                    return "fail";
                }
            }
            catch (Exception ex)
            {
                Common.LogHelper.WriteLog(ex.ToString());
                return "fail";
            }
        }
        /// <summary>
        /// 循环中间表数据创建到ecs会员 订单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string HandleActivityDataToEcs()
        {
            //Task task1 = new Task(() => MyMethod());
            //task1.Start();
            //return "后台异步执行中......";
            string jsonstring = "";
            int id = 0;
            string sql = string.Format(@"SELECT top 10 Id,OpenId,JsonString FROM dbo.ActivityData 
                                            WHERE IsHandle = 0 AND (HandleStatus IS NULL OR LEN(HandleStatus)=0) ORDER BY Id");
            DataTable dt = SqlHelper.ExecuteDataTable(sql);
            Common.LogHelper.WriteLog("总数" + dt.Rows.Count.ToString());
            foreach (DataRow dr in dt.Rows)
            {
                //先置为已处理
                sql = "UPDATE dbo.ActivityData SET IsHandle= 1 WHERE Id=" + dr["Id"].ToString();
                SqlHelper.ExecuteNonQuery(CommandType.Text, sql);

                id = Convert.ToInt32(dr["Id"]);
                jsonstring = dr["JsonString"].ToString(); //第三方传来的json字符串
                Common.LogHelper.WriteLog(jsonstring);

                ActivityData data = JsonConvert.DeserializeObject<ActivityData>(jsonstring);
                string result = HandleAction(data);
                if (result == "succ")
                {
                    //更新状态为succ
                    sql = string.Format(@"UPDATE dbo.ActivityData 
                                             SET HandleStatus = '{0}',HandleResult = '{1}',HandleDate = GETDATE() ,IsHandle =1
                                             WHERE Id ={2}", "succ", "succ", id);
                    SqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                }
                else
                {
                    //更新状态为fail
                    sql = string.Format(@"UPDATE dbo.ActivityData 
                                             SET HandleStatus = '{0}',HandleResult = '{1}',HandleDate = GETDATE() ,IsHandle =1
                                             WHERE Id ={2}", "fail", result.Length > 1000 ? result.Substring(0, 1000) : result, id);
                    SqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                }
            }
            return "succ";
        }
        public Task MyMethod()
        {
            while (true)
            {
                string jsonstring = "";
                int id = 0;
                string sql = string.Format(@"SELECT Id,OpenId,JsonString FROM dbo.ActivityData 
                                            WHERE IsHandle = 0 AND (HandleStatus IS NULL OR LEN(HandleStatus)=0)");
                DataTable dt = SqlHelper.ExecuteDataTable(sql);
                Common.LogHelper.WriteLog("总数" + dt.Rows.Count.ToString());
                foreach (DataRow dr in dt.Rows)
                {
                    //先置为已处理
                    sql = "UPDATE dbo.ActivityData SET IsHandle= 1 WHERE Id="+dr["Id"].ToString();
                    SqlHelper.ExecuteNonQuery(CommandType.Text, sql);

                    id = Convert.ToInt32(dr["Id"]);
                    jsonstring = dr["JsonString"].ToString(); //第三方传来的json字符串
                    Common.LogHelper.WriteLog(jsonstring);

                    ActivityData data = JsonConvert.DeserializeObject<ActivityData>(jsonstring);
                    string result = HandleAction(data);
                    if (result == "succ")
                    {
                        //更新状态为succ
                        sql = string.Format(@"UPDATE dbo.ActivityData 
                                             SET HandleStatus = '{0}',HandleResult = '{1}',HandleDate = GETDATE() ,IsHandle =1 
                                             WHERE Id ={2}", "succ", "succ", id);
                        SqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                    }
                    else
                    {
                        //更新状态为fail
                        sql = string.Format(@"UPDATE dbo.ActivityData 
                                             SET HandleStatus = '{0}',HandleResult = '{1}',HandleDate = GETDATE(),IsHandle =1 
                                             WHERE Id ={2}", "fail", result.Length > 1000 ? result.Substring(0, 1000) : result, id);
                        SqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                    }
                }
            }
        }

        /// <summary>
        /// 具体数据处理动作 将数据转为会员信息、订单信息
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        private string HandleAction(ActivityData requestData)
        {
            string OpenId = requestData.OpenId;
            string Phone = requestData.Phone;
            string Name = requestData.Name;
            string PositionType = requestData.PositionType;  //岗位类型 后厨主管
            string Position = requestData.Position;  //  岗位
            string HotelOrMdName = requestData.HotelOrMdName;
            string Address = requestData.Address;
            string NickName = requestData.NickName;
            string HeadImageUrl = requestData.HeadImageUrl;
            string PrizeName = requestData.PrizeName;
            int ProvinceId = requestData.ProvinceId;
            string ProvinceName = requestData.ProvinceName;
            int CityId = requestData.CityId;
            string CityName = requestData.CityName;
            int AreaId = requestData.AreaId;
            string AreaName = requestData.AreaName;
            string AddDate = requestData.AddDate;

            int MemberId = 0; //会员id 
            int AssociatedId = 0;  //


            int step = 0;
            try
            {

                #region 判断是否为老用户
                string sql = string.Format(@"SELECT a.MemberId,b.AssociatedId   FROM dbo.RegistMember a JOIN dbo.OpenIdAssociated b ON a.MemberId=b.UserId 
                                                WHERE b.OpenId = '{0}' AND  b.UserType=2", OpenId);
                DataTable dtMember = SqlHelper.ExecuteDataTable(sql);
                if (dtMember.Rows.Count > 0)
                {
                    MemberId = Convert.ToInt32(dtMember.Rows[0]["MemberId"]); //赋值老用户memberid
                    AssociatedId = Convert.ToInt32(dtMember.Rows[0]["AssociatedId"]); //赋值老用户微信表id
                }
                #endregion


                if (MemberId > 0) //更新
                {
                    if (string.IsNullOrEmpty(PositionType))  //留言登陆,之前已经有信息的跳过,直接返回
                    {
                        return "succ";
                    }
                    step = 1;
                    #region  step1更新基本信息
                    sql = string.Format(@"UPDATE dbo.RegistMember SET MemberName='{0}',MemberTelePhone='{1}',PositionType='{2}',Position='{3}',UpdateDate='{4}'
                                          WHERE MemberId = {5} ;", Name, Phone, PositionType, Position, AddDate, MemberId);
                    if (Position != "美食爱好者")
                    {
                        sql = sql + string.Format(@"UPDATE dbo.RegistMember SET ProvinceId={0},ProvinceName='{1}',CityId={2},CityName='{3}',
                                            AreaId={4},AreaName = '{5}' ,HotelAddress='{6}',HotelName='{7}'
                                         WHERE MemberId ={8}", ProvinceId, ProvinceName, CityId, CityName, AreaId, AreaName, Address, HotelOrMdName, MemberId);
                    }
                    SqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                    #endregion

                    step = 2;
                    #region  step2更新MemberProfile会员属性表
                    //
                    if (Position == "美食爱好者") //往更新家庭信息
                    {

                        sql = string.Format(@"UPDATE dbo.MemberProfile 
                                            SET HomeProvinceId ={0},HomeProvinceName='{1}',HomeCityId={2},HomeCityName='{3}',
                                                HomeAreaId={4},HomeAreaName='{5}',HomeAddress='{6}' WHERE MemberId={7}  
                                                ", ProvinceId, ProvinceName, CityId, CityName, AreaId, AreaName, Address, MemberId);
                        SqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                    }
                    #endregion
                }
                else  //新增
                {
                    step = 1;
                    #region  step1新增基本信息
                    RegistMember member = new RegistMember();
                    member.MemberId = MemberId;
                    member.MemberName = Name;
                    member.MemberTelePhone = Phone;
                    member.PositionType = PositionType;
                    member.Position = Position;
                    member.MemberState = 1;
                    member.RegistDate = Convert.ToDateTime(AddDate);
                    member.TotalIntegral = 0;
                    member.LeaveIntegral = 0;
                    member.IsEnable = 0;
                    member.Remark = "世界厨师日抽奖";
                    member.RecommendId = 0;
                    if (Position != "美食爱好者")
                    {
                        member.ProvinceId = ProvinceId;
                        member.ProvinceName = ProvinceName;
                        member.CityId = CityId;
                        member.CityName = CityName;
                        member.AreaId = AreaId;
                        member.AreaName = AreaName;
                        member.HotelName = HotelOrMdName;
                        member.HotelAddress = Address;
                    }
                    db.Member.Add(member);   //新增
                    db.SaveChanges();
                    MemberId = member.MemberId;
                    #endregion

                    step = 2;
                    #region  step2新增MemberProfile会员属性表
                    if (Position == "美食爱好者") //往MemberProfile新增带家庭信息记录
                    {
                        sql = string.Format(@"INSERT INTO MemberProfile (MemberId,HomeProvinceId,HomeProvinceName,HomeCityId,
                                            HomeCityName,HomeAreaId,HomeAreaName,HomeAddress)
                                            VALUES ({0},{1},'{2}',{3},'{4}',{5},'{6}','{7}')    
                                                ", MemberId, ProvinceId, ProvinceName, CityId,
                                            CityName, AreaId, AreaName, Address);
                        SqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                    }
                    else   //新增一条空记录
                    {
                        sql = string.Format(@"INSERT INTO MemberProfile (MemberId) VALUES ({0}) ", MemberId);
                        SqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                    }
                    #endregion

                }
                Common.LogHelper.WriteLog(OpenId + "-step" + step.ToString() + "succ");

                step = 3;
                #region 存用户OpenId
                OpenIdAssociated openbase = new OpenIdAssociated();
                openbase.AssociatedId = AssociatedId;
                openbase.OpenId = OpenId;
                openbase.UserId = MemberId;
                openbase.UserType = 2;
                openbase.Nickname = NickName;
                openbase.HeadImgUrl = HeadImageUrl;
                openbase.CreateDate = DateTime.Now;
                db.OpenIdAssociated.AddOrUpdate(openbase);
                db.SaveChanges();
                Common.LogHelper.WriteLog(OpenId + "-step" + step.ToString() + "succ");
                #endregion

                step = 4;
               
                if (!string.IsNullOrEmpty(PrizeName))
                {
                    #region 将抽奖奖品转为积分或者订单
                    if (PrizeName == "电子菜谱")
                    {
                        return "succ";
                    }
                    else if (PrizeName == "20积分")   //积分奖品
                    {
                        string result = AddJifenFromActivity(MemberId, AddDate);
                        if (result == "succ")
                        {
                            Common.LogHelper.WriteLog(OpenId + "-step" + step.ToString() + "增加积分succ");
                            return "succ";
                        }
                        else
                        {
                            Common.LogHelper.WriteLog(OpenId + "-step" + step.ToString() + "增加积分fail");
                            return "fail" + "-step" + step.ToString() + "增加积分fail";
                        }
                    }
                    else  //实物奖品 转为订单
                    {

                        //抽奖奖品名称转为商城物品名称
                        if (PrizeName == "iWatch")
                        {
                            PrizeName = "Apple Watch Series4智能手表组";
                        }
                        else if (PrizeName == "剃须刀")
                        {
                            PrizeName = "飞利浦男士电动剃须刀组";
                        }
                        else if (PrizeName == "牛仔围裙")
                        {
                            PrizeName = "牛仔围裙件";
                        }
                        else if (PrizeName == "酸汤酱")
                        {
                            PrizeName = "味达美酸汤酱瓶";
                        }

                        Address = ProvinceName + CityName + AreaName + Address;
                        string result = AddOrderFromActivity(MemberId, PrizeName, Name, Address, Phone,AddDate);

                        if (result == "succ")
                        {
                            Common.LogHelper.WriteLog(OpenId + "-step" + step.ToString() + "增加实物订单" + PrizeName + "succ");
                            return "succ";
                        }
                        else
                        {
                            Common.LogHelper.WriteLog(OpenId + "-step" + step.ToString() + "增加实物订单fail,没有" + PrizeName);
                            return "fail" + "-step" + step.ToString() + "增加实物订单fail,没有" + PrizeName;
                        }
                    }
                #endregion
                }
                else
                {
                    return "succ";
                }
               
            }
            catch (Exception ex)
            {
                Common.LogHelper.WriteLog(OpenId + "-step" + step.ToString() + "error:" + ex.ToString());
                return "error-" + ex.ToString();
            }
        }

        public string AddOrderFromActivity(int memberId, string skuName, string name, string address, string phone,string adddate)
        {
            string sql = string.Format(@"SELECT  s.SkuName,p.ProductName, s.SkuId,s.ProductId,s.Price,p.ProductType
                                    From dbo.SKU s
                                    JOIN dbo.Product p ON p.ProductId = s.ProductId
                                    WHERE s.SkuName='{0}' ", skuName);
            DataTable dt = SqlHelper.ExecuteDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                sql = string.Format(@"INSERT INTO dbo.MemberOrder
                                                ( MemberId ,
                                                    OrderAddress ,
                                                    OrderName ,
                                                    OrderTelephone ,
                                                    OrderState ,
                                                    OrderPrice ,
                                                    OrderType ,
                                                    InventedType ,
                                                    OrderFrom ,
                                                    AddDate
                                                )
                                        VALUES  ( '{0}' , -- MemberId - int
                                                    '{1}' , -- OrderAddress - varchar(500)
                                                    '{2}' , -- OrderName - varchar(50)
                                                    '{3}' , -- OrderTelephone - varchar(50)
                                                    '备货中' , -- OrderState - varchar(50)
                                                    0 , -- OrderPrice - int
                                                    {4} , -- OrderType - int
                                                    {5} , -- InventedType - int
                                                    '{6}' , -- OrderFrom - varchar(32)
                                                    '{7}' );
                                            SELECT SCOPE_IDENTITY() ",
                                        memberId, address, name, phone, dt.Rows[0]["ProductType"].ToString(),
                                        Convert.ToInt16(dt.Rows[0]["ProductType"]) == 1 ? 0 : 1, "世界厨师日",adddate);
                int orderid = Convert.ToInt32(SqlHelper.ExecuteScalar(CommandType.Text, sql));

                //orderitem 并减少库存
                sql = string.Format(@"Insert into OrderDetaile(OrderId,SkuId,SkuName,ProductId,Count)Values({0},{1},'{2}',{3},1);
                                      Update SKU Set Stock = Stock - 1 Where SkuId = {1}",
                                    orderid, dt.Rows[0]["SkuId"], skuName, dt.Rows[0]["ProductId"]);
                SqlHelper.ExecuteNonQuery(CommandType.Text, sql);

                Common.LogHelper.WriteLog(memberId.ToString() + "增加实物订单" + skuName + "succ");
                return "succ";
            }
            else
            {

                return "fail";
            }
        }

        #region 增加第三方抽奖所得20积分
        /// <summary>
        /// 增加第三方抽奖所得20积分
        /// </summary>
        /// <param name="memberid">会员id</param>
        /// <returns></returns>
        public string AddJifenFromActivity(int memberId, string adddDate)
        {
            string sql = string.Format("update RegistMember Set LeaveIntegral = LeaveIntegral + {0}, TotalIntegral = TotalIntegral + {0} where MemberId = {1}", 20, memberId);
            int effect = SqlHelper.ExecuteNonQuery(CommandType.Text, sql);
            if (effect == 1)
            {
                MemberIntegralDetail model = new MemberIntegralDetail();
                model.CreatDate = Convert.ToDateTime(adddDate);
                model.InvalidDate = Convert.ToDateTime(adddDate);
                model.IntegralNum = 20;
                model.IntegralSource = "世界厨师日";
                model.IntegralType = 1;
                model.Remark = "世界厨师日";
                model.MemberId = memberId;
                model.ScanHotelName = "";
                model.ScanMemberCode = "";

                db.MemberIntegralDetail.Add(model);
                db.SaveChanges();
                return "succ";
            }
            else
            {
                return "fail";
            }
        }
        #endregion



        /// <summary>
        /// 20180913餐饮积分系统用第三方举行抽奖活动，第三方将数据给我们，将用户创建到餐饮积分
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string CreateUserTemp()
        {

            int recommandid = 0;
            string sql = "";
            int ProvinceId = 0;
            string ProvinceName = "";
            int CityId = 0;
            string CityName = "";
            int AreaId = 0;
            string AreaName = "";

            //从数据库表查询要创建的用户
            sql = string.Format(@"SELECT  Id,OpenId,NickName,HeadImageUrl,Name,Phone,AddressType,
                                    Province,City,Area,Address ,CONVERT(DATETIME,RegistDate,121) RegistDate
                                    from dbo.TempUsers 
                                    WHERE Status IS NULL or LEN(Status) =0 order by Id ");
            DataTable dt = SqlHelper.ExecuteDataTable(sql);

            foreach (DataRow dr in dt.Rows)
            {
                var a = Convert.ToDateTime(dr["RegistDate"].ToString());
                try
                {
                    sql = string.Format(@"SELECT a.MemberId  FROM dbo.RegistMember a JOIN dbo.OpenIdAssociated b ON a.MemberId=b.UserId 
                                                WHERE b.OpenId = '{0}' AND  b.UserType=2", dr["OpenId"].ToString());
                    DataTable dt6 = SqlHelper.ExecuteDataTable(sql);

                    //int openidcount = Convert.ToInt32(dataContext.ExecuteScalar(CommandType.Text, sql));
                    if (dt6.Rows.Count == 0)
                    {
                        if (string.IsNullOrEmpty(dr["AddressType"].ToString()))
                        {

                            //基本信息
                            RegistMember member = new RegistMember();
                            member.MemberTelePhone = dr["Phone"].ToString();
                            member.MemberState = 1;
                            member.RegistDate = DateTime.Now;
                            member.TotalIntegral = 0;
                            member.LeaveIntegral = 0;
                            member.IsEnable = 0;
                            member.Remark = "匠心寻百味抽奖";
                            member.RecommendId = recommandid;
                            member.RegistDate = Convert.ToDateTime(dr["RegistDate"].ToString());
                            db.Member.Add(member);
                            db.SaveChanges();

                            #region 存入用户OpenId
                            OpenIdAssociated openbase = new OpenIdAssociated();

                            openbase.OpenId = dr["OpenId"].ToString();
                            openbase.UserId = member.MemberId;
                            openbase.UserType = 2;
                            openbase.Nickname = dr["NickName"].ToString();
                            openbase.HeadImgUrl = dr["HeadImageUrl"].ToString();
                            openbase.CreateDate = DateTime.Now;

                            db.OpenIdAssociated.Add(openbase);
                            db.SaveChanges();

                            #endregion
                            // 当新增完会员之后 在会员简历表里同步新增一条数据

                            sql = string.Format(@"INSERT INTO MemberProfile (MemberId) VALUES ({0});
                                                UPDATE dbo.TempUsers SET Status = 'succ' WHERE OpenId = '{1}'", member.MemberId, dr["OpenId"].ToString());
                            SqlHelper.ExecuteNonQuery(CommandType.Text, sql);

                            Common.LogHelper.WriteLog(dr["OpenId"].ToString() + "-" + dr["Phone"].ToString() + ":用户创建成功");

                        }
                        else
                        {
                            //查询地址信息,主要是省市区id
                            sql = string.Format(@"SELECT a.ProvinceId,a.ProvinceName,b.CityId,b.CityName,c.AreaId,c.AreaName FROM dbo.Province a 
                                                    JOIN dbo.City b ON b.ProvinceId = a.ProvinceId JOIN dbo.Area c ON c.CityId = b.CityId
                                                    WHERE a.ProvinceName like '%{0}%' AND b.CityName like '%{1}%' AND c.AreaName like '%{2}%'",
                                    dr["Province"].ToString(), dr["City"].ToString(), dr["Area"].ToString());
                            DataTable dt2 = SqlHelper.ExecuteDataTable(sql);



                            if (dt2.Rows.Count > 0)  //地址正确 创建用户
                            {
                                ProvinceId = Convert.ToInt32(dt2.Rows[0]["ProvinceId"]);
                                ProvinceName = dt2.Rows[0]["ProvinceName"].ToString();
                                CityId = Convert.ToInt32(dt2.Rows[0]["CityId"]);
                                CityName = dt2.Rows[0]["CityName"].ToString();
                                AreaId = Convert.ToInt32(dt2.Rows[0]["AreaId"]);
                                AreaName = dt2.Rows[0]["AreaName"].ToString();

                                //基本信息
                                RegistMember member = new RegistMember();
                                member.MemberTelePhone = dr["Phone"].ToString();
                                member.MemberState = 1;
                                member.RegistDate = DateTime.Now;
                                member.TotalIntegral = 0;
                                member.LeaveIntegral = 0;
                                member.IsEnable = 0;
                                member.Remark = "匠心寻百味抽奖";
                                member.RecommendId = recommandid;
                                if (dr["AddressType"].ToString() == "酒店地址")
                                {
                                    member.ProvinceId = ProvinceId;
                                    member.ProvinceName = ProvinceName;
                                    member.CityId = CityId;
                                    member.CityName = CityName;
                                    member.AreaId = AreaId;
                                    member.AreaName = AreaName;
                                    member.HotelAddress = dr["Address"].ToString();
                                }
                                db.Member.Add(member);
                                db.SaveChanges();

                                #region 存入用户OpenId
                                OpenIdAssociated openbase = new OpenIdAssociated();

                                openbase.OpenId = dr["OpenId"].ToString();
                                openbase.UserId = member.MemberId;
                                openbase.UserType = 2;
                                openbase.Nickname = dr["NickName"].ToString();
                                openbase.HeadImgUrl = dr["HeadImageUrl"].ToString();
                                openbase.CreateDate = DateTime.Now;

                                db.OpenIdAssociated.Add(openbase);
                                db.SaveChanges();

                                #endregion
                                // 当新增完会员之后 在会员简历表里同步新增一条数据
                                if (dr["AddressType"].ToString() == "家庭地址") //往MemberProfile 新增家庭信息
                                {
                                    sql = string.Format(@"INSERT INTO MemberProfile (MemberId,HomeProvinceId,HomeProvinceName,HomeCityId,
                                                HomeCityName,HomeAreaId,HomeAreaName,HomeAddress)
                                                VALUES ({0},{1},'{2}',{3},'{4}',{5},'{6}','{7}');
                                                UPDATE dbo.TempUsers SET Status = 'succ' WHERE OpenId = '{8}'                    
                                                    ", member.MemberId, ProvinceId, ProvinceName, CityId,
                                                     CityName, AreaId, AreaName, dr["Address"].ToString(), dr["OpenId"].ToString());
                                    SqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                                }
                                else  //新增一条记录
                                {
                                    sql = string.Format(@"INSERT INTO MemberProfile (MemberId) VALUES ({0});
                                                      UPDATE dbo.TempUsers SET Status = 'succ' WHERE OpenId = '{1}'", member.MemberId, dr["OpenId"].ToString());
                                    SqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                                }
                                Common.LogHelper.WriteLog(dr["OpenId"].ToString() + "-" + dr["Phone"].ToString() + ":用户创建成功");

                            }
                            else
                            {
                                sql = string.Format(@"UPDATE dbo.TempUsers SET Status = 'fail',Remark = '地址信息错误' WHERE Id = {0}", dr["Id"].ToString());
                                SqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                                Common.LogHelper.WriteLog(dr["OpenId"].ToString() + "-" + dr["Phone"].ToString() + ":用户创建失败,地址信息错误");
                            }
                        }
                    }
                    else
                    {
                        sql = string.Format(@"UPDATE dbo.TempUsers SET Status = 'fail',Remark='{0}' WHERE Id = {1}", dt6.Rows[0]["MemberId"], dr["Id"].ToString());
                        SqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                        Common.LogHelper.WriteLog(dr["OpenId"].ToString() + "已存在");
                    }
                }
                catch (Exception ex)
                {
                    Common.LogHelper.WriteLog(dr["OpenId"].ToString() + "-" + dr["Phone"].ToString() + "用户创建失败" + ex.Message);
                }
            }
            return "succ";
        }

        [HttpGet]
        public string AddJifenTemp()
        {
            //从数据库表查询要创建的用户
            string sql = string.Format(@"SELECT  a.Id,a.OpenId,c.MemberId FROM TempAddJiFen a 
                                        JOIN dbo.OpenIdAssociated b ON b.OpenId = a.OpenId AND b.UserType=2
                                        JOIN dbo.RegistMember c ON c.MemberId=b.UserId
                                        WHERE  (a.Status IS NULL or LEN(a.Status) =0) AND b.UserType=2 order by Id");
            DataTable dt = SqlHelper.ExecuteDataTable(sql);

            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    sql = string.Format("update RegistMember Set LeaveIntegral = LeaveIntegral + {0}, TotalIntegral = TotalIntegral + {0} where MemberId = {1}", 20, Convert.ToInt32(dr["MemberId"]));
                    int a = SqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                    if (a == 1)
                    {
                        MemberIntegralDetail model = new MemberIntegralDetail();
                        model.CreatDate = DateTime.Now;
                        model.InvalidDate = DateTime.Now;
                        model.IntegralNum = 20;
                        model.IntegralSource = "匠心寻百味抽奖";
                        model.IntegralType = 1;
                        model.Remark = "匠心寻百味抽奖";
                        model.MemberId = Convert.ToInt32(dr["MemberId"]);
                        model.ScanHotelName = "";
                        model.ScanMemberCode = "";

                        db.MemberIntegralDetail.Add(model);
                        db.SaveChanges();

                        sql = string.Format(@"UPDATE dbo.TempAddJiFen SET Status = 'succ',Remark='{0}' WHERE Id = {1}", "succ", dr["Id"].ToString());
                        SqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                        Common.LogHelper.WriteLog(dr["OpenId"].ToString() + "RegistMember增加20积分SUCC");
                    }
                    else
                    {
                        sql = string.Format(@"UPDATE dbo.TempAddJiFen SET Status = 'fail',Remark='{0}' WHERE Id = {1}", "RegistMember增加20积分失败", dr["Id"].ToString());
                        SqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                        Common.LogHelper.WriteLog(dr["OpenId"].ToString() + "RegistMember增加20积分失败");
                    }
                }
                catch (Exception ex)
                {
                    sql = string.Format(@"UPDATE dbo.TempAddJiFen SET Status = 'fail',Remark='{0}' WHERE Id = {1}", ex.ToString(), dr["Id"].ToString());
                    SqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                    Common.LogHelper.WriteLog(dr["OpenId"].ToString() + ex.ToString());
                }
            }
            return "执行完成";
        }
    }
}

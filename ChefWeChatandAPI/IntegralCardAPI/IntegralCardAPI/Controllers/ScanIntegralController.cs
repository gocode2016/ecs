using IntegralCardAPI.Common;
using IntegralCardAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace IntegralCardAPI.Controllers
{
    /// <summary>
    /// 积分卡片扫码逻辑部分
    /// </summary>
    public class ScanIntegralController : ApiController
    {
        private IntegralCardContext db = new IntegralCardContext();
        private SqlHelper dataContext = new SqlHelper();

        /// <summary>
        /// 积分卡片扫码部分
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        [HttpGet]
        public string ScanIntegral(string url, string openId)
        {
            if (string.IsNullOrEmpty(openId))
            {
                return "Exctue Error";
            }

            try
            {
                var result = "";
                var user = (from o in db.OpenIdAssociated
                           where o.OpenId == openId
                           select o).FirstOrDefault();

                if (user == null)
                {
                    return "产品积分扫码活动只针对已认证会员开展，点击菜单【我的】【我要注册】立即体验会员特权！";
                }
                if (user.UserType == 1)
                {
                    return "队员不可以扫码";
                }
                else
                {
                    var rawUrl = HttpUtility.UrlDecode(url).ToLower();
                    var SnIndexOf = rawUrl.IndexOf("?sn=");

                    if (SnIndexOf > 0)
                    {
                        var snBeginIndex = SnIndexOf + 4;
                        var sn = rawUrl.Substring(snBeginIndex);
                        if (sn.Length == 18)
                        {
                            var scanResult = ScanQrCode(sn, user.UserId, user.UserType);
                            result = scanResult;
                        }
                        else
                        {
                            result = "您好，系统不能识别该二维码！";
                        }
                    }
                    else
                    {
                        result = "您好，系统不能识别该二维码！";
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        /// <summary>
        /// 二维码扫描处理方法
        /// </summary>
        /// <param name="p">二维码所属包ID</param>
        /// <param name="sn">二维码的信息编码</param>
        /// <param name="userId">扫描人的用户ID</param>
        /// <param name="userType">扫描人的用户类型</param>
        /// <returns>二维码扫描结果</returns>
        public string ScanQrCode(string sn = "", int userId = 0, int userType = 0)
        {
            ScanQrCodeViewModel scanResult = new ScanQrCodeViewModel();
            scanResult.IsSuccess = false;
            scanResult.Msg = "扫描二维码无效，请换一瓶";
            if (string.IsNullOrEmpty(sn))
            {
                scanResult.ReturnUrl = "二维码不存在";
            }
            var totalIntegral = 0;
            var rowNumber = 0;

            //var memberInfo = _memberService.Find(userId);
            RegistMember memberInfo = (from o in db.RegistMember
                                      where o.MemberId == userId
                                      select o).FirstOrDefault();

            string isChance = "";   //标识，判断是否为活动概率规则

            if (!string.IsNullOrEmpty(sn))
            {
                var code = EncryptHelper.Confusion(sn);
                if (!string.IsNullOrEmpty(code) && code.Length == 18)
                {
                        
                    //获取二维码
                    IntegralGoodsQrc qrc = (from o in db.IntegralGoodsQrc
                                            where o.Code == code
                                            select o).FirstOrDefault();

                    if (qrc.State == 1)
                    {
                        var wrapCode = code.Substring(0, 2);
                        var categoryCode = code.Substring(2, 2);
                        var factoryCode = code.Substring(4, 2);
                        var monthCode = code.Substring(6, 2);
                        var productCode = code.Substring(8);

                        //获取积分规则
                        DateTime date = DateTime.Now;
                        var integralModel = (from o in db.IntegralRule
                                             join c in db.IntegralGoods on o.GoodsId equals c.GoodsId
                                             where o.RuleState != 100 && c.GoodsState != 100 && c.GoodsNo == categoryCode &&
                                             o.StartDate < date && o.EndDate > date
                                             orderby o.RuleType descending
                                             select o).FirstOrDefault();
                        
                        if (memberInfo.IsEnable == 0 && (memberInfo.RecommendId > 0 || integralModel.RuleId == 83))
                        {
                            if (integralModel != null)
                            {
                                int count = 0;
                                int scanIntegral = 0;
                                
                                //使用二维码;添加商品积分记录;增加积分
                                var flag = Scan(integralModel.GoodsId, userId, qrc.QrcId, integralModel.Integral, (int)integralModel.RuleType, code, memberInfo.MemberCode, memberInfo.HotelName, out count, out scanIntegral, out isChance);

                                if (flag)
                                {

                                    //操作成功
                                    scanResult.IsSuccess = true;

                                    if (integralModel.ScanLimit > 0)
                                    {
                                        if (count >= integralModel.ScanLimit)
                                        {
                                            //int score = integralModel.Integral / 2;
                                            scanResult.Msg = string.Format("恭喜您！成功增加{0}积分", scanIntegral);
                                        }
                                        else
                                        {
                                            scanResult.Msg = string.Format("恭喜您！成功增加{0}积分", scanIntegral);
                                        }
                                    }
                                    else
                                    {
                                        scanResult.Msg = string.Format("恭喜您！成功增加{0}积分", scanIntegral);
                                    }
                                }
                                else
                                {
                                    //操作失败
                                    scanResult.Msg = "增加积分失败，请换一瓶。";
                                }
                            }
                            else
                            {
                                scanResult.Msg = "该产品不存在或者还没有对应的积分规则。";
                            }
                        }
                        else
                        {
                            scanResult.Msg = "产品积分扫码活动只针对已认证会员开展，未认证会员暂时无法扫描积分卡片。";
                        }
                    }
                    else if (qrc.State == -1)
                    {
                        scanResult.Msg = string.Format("该二维码已失效。");
                    }
                    else
                    {
                        scanResult.Msg = string.Format("该二维码已被使用过。");
                    }
                }
            }

            scanResult.TotalIntegral = totalIntegral;
            scanResult.RowNumber = rowNumber;
            return scanResult.Msg + isChance.ToString();
        }

        /// <summary>
        /// 扫码操作部分
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="memberId"></param>
        /// <param name="codeId"></param>
        /// <param name="integral"></param>
        /// <param name="ruleType"></param>
        /// <param name="goodsCode"></param>
        /// <param name="memberCode"></param>
        /// <param name="hotelName"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool Scan(int goodsId, int memberId, int codeId, int integral, int ruleType, string goodsCode, string memberCode, string hotelName, out int count, out int scanIntegral, out string isChance)
        {
            string inegralSource = "扫描普通商品";
            var scanDesc = "扫描普通商品";
            count = 0;
            isChance = "";

            //使用二维码
            UseQrc(memberId, codeId);

            #region 获取活动规则
            var sql = SqlParamHelper.Builder
                    .Append("SELECT ir.*,ig.GoodsName FROM dbo.IntegralRule AS ir")
                    .Append("LEFT JOIN dbo.IntegralGoods AS ig ON ig.GoodsId = ir.GoodsId")
                    .Where("ir.RuleState <> 100 AND ir.GoodsId = " + goodsId + " AND GETDATE() BETWEEN ir.StartDate AND ir.EndDate")
                    .OrderBy("ir.RuleType DESC");

            var result = dataContext.ExecuteDataTable(CommandType.Text, sql.SQL);
            string query = JsonConvert.SerializeObject(result);
            List<IntegralRuleViewModel> rule = JsonConvert.DeserializeObject<List<IntegralRuleViewModel>>(query); 
            #endregion

            IntegralRuleViewModel ruleObj = rule[0];
            
            //活动规则
            if (ruleType == 2)
            {
                inegralSource = "扫描活动商品";
                //本次活动的扫描次数
                var activeAddCount = ActiveAddCount(memberId, ruleObj.GoodsName, ruleObj.StartDate, ruleObj.EndDate);
                count = activeAddCount;

                //查询出活动规则的概率部分
                List<IntegralRuleChance> chanceList = (from o in db.IntegralRuleChance
                                                       where o.RuleId == ruleObj.RuleId && o.PutNum > 0
                                                       orderby o.Chance ascending
                                                       select o).ToList();

                string minusSql = string.Empty;

                //活动规则扫码
                if (count < ruleObj.ScanLimit)
                {
                    Random rd = new Random();
                    int a = rd.Next(0, chanceList.Sum(p => p.Chance));

                    int num = 0;

                    for (int i = 0; i < 11; i++)
                    {
                        if (a <= chanceList[i].Chance + num && chanceList[i].PutNum > 0)
                        {
                            integral = chanceList[i].Integral;
                            scanDesc = "扫描活动商品-" + ruleObj.RuleName;
                            minusSql = string.Format("Update IntegralRuleChance Set PutNum = PutNum - 1 Where RuleChanceId = {0}", chanceList[i].RuleChanceId);
                            dataContext.ExecuteNonQuery(CommandType.Text, minusSql);
                            break;
                        }
                        num = num + chanceList[i].Chance;

                    }
                    isChance = "x";
                }
                else
                {
                    inegralSource = "扫描普通商品";
                }
            }
            scanDesc += "-" + ruleObj.GoodsName;
            scanIntegral = integral;
            if (integral > 0)
            {
                //添加商品积分记录
                AddIntegralForGoods(goodsId, integral, goodsCode);
                //增加积分            
                var flag = IntegralPlus(memberId, integral, inegralSource, scanDesc, memberCode, hotelName);
                return flag;

            }
            return false;
        }

        /// <summary>
        /// 使用二维码
        /// </summary>
        /// <returns></returns>
        public void UseQrc(int memberId, int qrcId)
        {
            string sqlQrc = string.Format("UPDATE dbo.IntegralGoodsQrc SET [State] = {0},UseDate=GETDATE() WHERE QrcId = {1}", 2, qrcId);
            dataContext.ExecuteNonQuery(CommandType.Text, sqlQrc);

            string sqlRelar = string.Format("INSERT dbo.MemberQrcRelar( MemberId, QrcId ) VALUES({0},{1})", memberId, qrcId);
            dataContext.ExecuteNonQuery(CommandType.Text, sqlRelar);
        }

        /// <summary>
        /// 查询活动扫描次数
        /// </summary>
        /// <returns></returns>
        public int ActiveAddCount(int memberId, string product, DateTime begin, DateTime end)
        {
            var sql = SqlParamHelper.Builder
                .Append("SELECT COUNT(*) FROM dbo.MemberIntegralDetail AS mid")
                .Where("IntegralSource like '扫描活动商品' AND MemberId = " + memberId + " AND CreatDate BETWEEN '" + begin + "' AND '" + end + "' AND Remark like '%" + product + "%'");
            var count = dataContext.ExecuteScalar(CommandType.Text, sql.SQL);
            return Convert.ToInt32(count);
        }

        /// <summary>
        /// 添加商品积分记录
        /// </summary>
        /// <param name="goodsId">商品ID</param>
        /// <param name="integral">积分</param>
        /// <param name="goodsCode">商品Code</param>
        public void AddIntegralForGoods(int goodsId, int integral, string goodsCode)
        {
            GoodsIntegral model = new GoodsIntegral();
            model.GoodsId = goodsId;
            model.Integral = integral;
            model.CreateDate = DateTime.Now;
            model.GoodsCode = goodsCode;

            db.GoodsIntegral.Add(model);
        }

        /// <summary>
        /// 扫码增加积分
        /// </summary>
        /// <param name="memberId">会员ID</param>
        /// <param name="integral">增加积分</param>
        /// <param name="opation">操作项</param>
        /// <param name="remark">增加积分备注</param>
        /// <param name="memberCode">会员编码</param>
        /// <param name="hotelName">酒店名称</param>
        /// <returns></returns>
        public bool IntegralPlus(int memberId, int integral, string opation, string remark = "", string memberCode = "", string hotelName = "")
        {

            var sql = string.Format("update RegistMember Set LeaveIntegral = LeaveIntegral + {0}, TotalIntegral = TotalIntegral + {0} where MemberId = {1}", integral, memberId);
            var count = dataContext.ExecuteNonQuery(CommandType.Text, sql);

            MemberIntegralDetail model = new MemberIntegralDetail();
            model.CreatDate = DateTime.Now;
            model.InvalidDate = DateTime.Now;
            model.IntegralNum = integral;
            model.IntegralSource = opation;
            model.IntegralType = 1;
            model.Remark = remark;
            model.MemberId = memberId;
            model.ScanMemberCode = memberCode;
            model.ScanHotelName = hotelName;

            db.MemberIntegralDetail.Add(model);
            db.SaveChanges();

            if ((int)count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取同城排名，根据总积分和最近活跃时间排序(最新修改为每月积分同城排行)
        /// </summary>
        /// <param name="memberID"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<RankingsViewModel> Top(int memberID, string date = "")
        {

            var sql = SqlParamHelper.Builder
                .Append("select b.*,c.HeadImgUrl,d.MemberTelePhone from (select a.MemberId,sum(a.IntegralNum) IntegralNum from MemberIntegralDetail a ")
                .Append("where a.IntegralNum>0 and a.IntegralType=1 and ");
            if (string.IsNullOrEmpty(date))
            {
                sql.Append("convert(varchar(7),a.CreatDate,120)= convert(varchar(7),GETDATE(),120) ");
            }
            else
            {
                sql.Append("convert(varchar(7),a.CreatDate,120)= convert(varchar(7)," + date + ",120)");
            }
            sql.Append(" group by a.MemberId) as b ")
            .Append("OUTER APPLY (select top 1 HeadImgUrl from OpenIdAssociated where UserId=b.MemberId ")
            .Append("and UserType=2  order by AssociatedId desc) c  ")
            .Append("left join RegistMember d on b.MemberId=d.MemberId")
            .Append("where d.MemberState<>100 and d.CityId=(select top 1 CityId from RegistMember where MemberId=" + memberID + ")")
            .Append("order by b.IntegralNum desc");

            var q = dataContext.ExecuteDataTable(CommandType.Text, sql.SQL);
            string result = JsonConvert.SerializeObject(q);
            List<RankingsViewModel> model = JsonConvert.DeserializeObject<List<RankingsViewModel>>(result);
            

            return model;
        }

    }
}

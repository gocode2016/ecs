using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WxScanAPI.Models;
using System.Text;
using WxScanAPI.Common;
using System.Configuration;
using System.Data;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace WxScanAPI.Controllers
{
    public class RedPackController : ApiController
    {
        private WxScanContext db = new WxScanContext();


        #region 扫码用户注册 
        /// <summary>
        /// 扫码用户注册  08-17
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
         [HttpPost]
        public string CreateVisitor(dynamic requestData)
        {
            RJson r = new RJson();
            string telephone = requestData.Phone;
            string openid = requestData.OpenId;
            string jobType = requestData.JobType;
            if (string.IsNullOrEmpty(telephone) || string.IsNullOrEmpty(openid) || string.IsNullOrEmpty(jobType))
            {
                r.message = "参数不能为空";
                return JsonConvert.SerializeObject(r); 
            }
            try
            {
                RegistMember member = new RegistMember();
                member.MemberTelePhone = telephone;
                member.MemberState = 1;
                member.RegistDate = DateTime.Now;
                member.TotalIntegral = 0;
                member.LeaveIntegral = 0;
                member.IsEnable = 0;
                member.Remark = requestData.Remark;

                db.RegistMember.Add(member);
                db.SaveChanges();

                #region 存入用户OpenId
                OpenIdAssociated openbase = new OpenIdAssociated();

                openbase.OpenId = requestData.OpenId;
                openbase.UserId = member.MemberId;
                openbase.UserType = 2;
                openbase.Nickname = requestData.Nickname;
                openbase.HeadImgUrl = requestData.HeadImgUrl;
                openbase.CreateDate = DateTime.Now;

                db.OpenIdAssociated.Add(openbase);
                db.SaveChanges();

                #endregion

                //当新增完会员之后 在会员简历表里同步新增一条数据
                var sql = string.Format(@"INSERT INTO MemberProfile (MemberId ,JobType) VALUES ({0},'{1}')
                                        ", member.MemberId,jobType);
                SqlHelper2.ExecuteNonQuery(CommandType.Text, sql);
                r.result_status = "succ";
                r.data = member.MemberId.ToString();
            }
            catch (Exception ex)
            {
                r.message = ex.Message.ToString();
            }
            return JsonConvert.SerializeObject(r); 
        } 
        #endregion

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="num">个数</param>
        /// <returns></returns>
        [HttpGet]
        public string Generate(int num)
        {
            Thread t = new Thread(new ParameterizedThreadStart(AsyncGenerate));
            t.Start(num);
            return "ok";
        }
        
        public void AsyncGenerate(object obj)
        {
           RedPack.CreateQrCodeByMD5(Convert.ToInt32(obj));
        }

        /// <summary>
        /// 提现操作
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string TiXian(dynamic requestData)
        {
            RJson r = new RJson();
            string openid = requestData.OpenId;
            try
            {
                string kaiguan = ConfigurationManager.AppSettings["kaiguan"];
                if (kaiguan == "ON")
                {
                    if (string.IsNullOrEmpty(openid))
                    {
                        r.message = "参数错误";
                    }
                    else
                    {
                        int isRegist = RedPack.GetIsRegist(openid);
                        if (isRegist == 0)
                        {
                            r.message = "您还未注册,注册后方可提现";
                        }
                        else if (isRegist == -2) //表示是岗位是调味品供货商 的会员
                        {
                            r.message = "调味品供货商";
                        }
                        else if (isRegist == -1) //表示是队员 返回
                        {
                            r.message = "队员不能参与活动";
                        }
                        else
                        {
                            r = RedPack.PayAction(openid);
                        }
                    }
                }
                else
                {
                    r.message = "活动现处于关闭状态,谢谢~";
                }
            }
            catch(Exception ex)
            {
                r.message = "有异常";
                RedPack.AddAlertLog(openid, ex.ToString(), "tixian-error");
            }
            return JsonConvert.SerializeObject(r);
        }



        /// <summary>
        /// 用户扫描提现记录
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetHistory(string openid)
        {

            RHistoryJson r = new RHistoryJson();
            try
            {
                if (string.IsNullOrEmpty(openid))
                {
                    r.message = "参数有误";
                }
                else
                {
                    string sql = string.Format(@"SELECT  * FROM  RedPackScanRecord WHERE OpenId = '{0}'", openid);
                    DataTable dt = SqlHelper2.ExecuteDataTable(sql);
                    r.haveTxMoney = Convert.ToDouble(dt.Compute("Sum(Money)", "IsPay=1").ToString() == "" ? 0 : dt.Compute("Sum(Money)", "IsPay=1")); //已提现金额
                    r.kTxMoney = Convert.ToDouble(dt.Compute("Sum(Money)", "IsPay=0").ToString() == "" ? 0 : dt.Compute("Sum(Money)", "IsPay=0")); //还没有进行提现的金额
                    //获取用户扫描但是未提现的金额总和
                    //                string sql = string.Format(@"SELECT  ISNULL(SUM(Money),0) Total 
                    //                                  FROM dbo.RedPackScanRecord WHERE OpenId = '{0}' AND IsPay = 0 ", openid);
                    //                r.kTxMoney = Convert.ToDouble(SqlHelper2.ExecuteScalar(CommandType.Text, sql));
                    if (r.kTxMoney > 1)
                    {
                        r.isKeTiXian = 1;
                    }
                    //获取用户扫描 提现记录
                    sql = string.Format(@"SELECT * FROM (
                                    SELECT Money Money,CONVERT(VARCHAR(30),ScanDate,121) CreateDate,'+' Action FROM dbo.RedPackScanRecord WHERE OpenId = '{0}'
                                    UNION 
                                    SELECT PayAmout Money,CONVERT(VARCHAR(30),PayDate,121) CreateDate,'-' Action FROM dbo.RedPackPayRecord WHERE OpenId = '{0}'
                                    ) t ORDER BY CreateDate", openid);
                    var q = SqlHelper2.ExecuteDataTable(sql);
                    r.data = q;   //用户扫描 提现记录
                    r.isRegist = RedPack.GetIsRegist(openid); //是否注册用户 0未注册 1已注册  
                    r.scanCount = dt.Rows.Count;
                    r.result_status = "succ"; //返回成功状态
                } 
            }
            catch(Exception ex)
            {
                r.message = "有异常";
                RedPack.AddAlertLog(openid, ex.ToString(), "GetHistory-error");
            }

            return JsonConvert.SerializeObject(r);
        }

        /// <summary>
        /// 微信扫码
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public  string  Scan(dynamic requestData)
        {
            RScanJson r = new RScanJson();

            string kaiGuan = ConfigurationManager.AppSettings["kaiguan"];
            string openid = requestData.OpenId;
            string scode = requestData.Scode;
            LogHelper.WriteMsg(openid + "扫码:" + scode);
            if (kaiGuan == "ON")
            {
                try
                {
                    if (string.IsNullOrEmpty(openid) || string.IsNullOrEmpty(scode))
                    {
                        r.message = "参数有误";
                    }
                    else
                    {
                        #region  1.判断是否注册，是否是队员
                        int isRegist = RedPack.GetIsRegist(openid);
                        if ( isRegist == 1)
                        {
                            r.isRegist = 1;
                        }
                        else if (isRegist == -1) //表示是队员 返回
                        {
                            r.isRegist = 0;
                            r.message = "队员不能扫码";
                            return JsonConvert.SerializeObject(r);
                        }
                        else if (isRegist == -2) //表示是岗位是调味品供货商 的会员
                        {
                            r.isRegist = 0;
                            r.message = "调味品供货商";
                            return JsonConvert.SerializeObject(r);
                        }
                        //else//表示是队员 返回
                        //{
                        //    r.isRegist = 0;
                        //    r.message = "还未注册,请注册";
                        //    return JsonConvert.SerializeObject(r);
                        //}

                        #endregion

                        #region 2.获取用户扫描但是未提现的金额总和
                        string sql = "";
                        sql = string.Format(@"SELECT  ISNULL(SUM(Money),0) Total 
                                  FROM dbo.RedPackScanRecord WHERE OpenId = '{0}' AND IsPay = 0 ", openid);

//                        sql = string.Format(@"SELECT ISNULL(SUM(Money),0) Total FROM (
//                                                    SELECT  MIN(Money) Money
//                                                    FROM dbo.RedPackScanRecord 
//                                                    WHERE OpenId = '{0}' AND IsPay = 0 GROUP BY ScanCode) t", openid);
                        r.kTxMoney = Convert.ToDouble(SqlHelper2.ExecuteScalar(CommandType.Text, sql));
                        if (r.kTxMoney > 1)
                        {
                            r.isKeTiXian = 1;
                        }
                        #endregion

                        //当天
                        string nowDay = DateTime.Now.ToString("yyyy-MM-dd"); 
                        //当月
                        string nowMonth = DateTime.Now.ToString("yyyy-MM");

                        //string a = CheckRule(openid, scode);

                        //验证二维码是否有效
                        sql = string.Format(@"SELECT a.IsScan,b.Money,convert(varchar(19),b.ScanDate,121) ScanDate
                                            FROM dbo.RedPackCode a LEFT JOIN dbo.RedPackScanRecord b ON a.Code=b.ScanCode
                                            WHERE a.Code = '{0}' ", scode);
                        DataTable dt = SqlHelper2.ExecuteDataTable(sql);
                        if (dt.Rows.Count == 0)
                        { 
                            r.code_status = "非活动码";
                            r.message = "此码不属于本次活动";
                            RedPack.AddAlertLog(openid, scode + ":" + r.message, "scan");
                        }
                        //配合压测 mark掉
                        else if (Convert.ToInt16(dt.Rows[0]["IsScan"]) == 1 || !string.IsNullOrEmpty(dt.Rows[0]["ScanDate"].ToString()))
                        {
                            r.code_status = "已被扫";
                            r.money = Convert.ToDouble(dt.Rows[0]["Money"]);
                            r.scan_date = dt.Rows[0]["ScanDate"].ToString();
                            r.message = "该二维码已被扫描";
                            RedPack.AddAlertLog(openid, scode + ":" + r.message, "scan");
                        }
                        else
                        {
                            //验证当天扫描是否超过10次
                            sql = string.Format("SELECT COUNT(1) FROM RedPackScanRecord WHERE   OpenId ='{0}' AND CONVERT(VARCHAR(10),ScanDate,121) = '{1}'  ", openid, nowDay);
                            int nowDayScan = SqlHelper2.GetCount(CommandType.Text, sql);
                            if (nowDayScan >= 10)
                            {
                                r.message = "当天扫描是已超过10次";
                                RedPack.AddAlertLog(openid, r.message, "scan");
                            }
                            else
                            {
                                sql = string.Format("SELECT COUNT(1) FROM RedPackScanRecord WHERE   OpenId ='{0}' ", openid);
                                int allScan = SqlHelper2.GetCount(CommandType.Text, sql);

                                //去抽红包
                                double money = RedPack.GetMoney(allScan, openid, nowMonth);
                                LogHelper.WriteMsg(openid + "扫码：" + scode + ",金额:" + money.ToString());

                                #region 增加扫描红包记录
                                sql = string.Format(@"INSERT INTO dbo.RedPackScanRecord
                                            ( OpenId ,Money ,ScanCode ,IsPay ,PayId,IsFirst,ScanMonth)
                                    VALUES  ( '{0}' , -- OpenId - varchar(50)
                                              {1} , -- Money - float
                                              '{2}' , -- ScanCode - varchar(50)
                                              0 , -- IsPay - int
                                              0 , -- PayId - int
                                              {4},'{3}') ;
                                 UPDATE dbo.RedPackCode SET IsScan =1,UseDate = GETDATE() WHERE Code = '{2}';
                                 UPDATE dbo.RedPackConfig SET LeiJiMoney = LeiJiMoney + {1} WHERE Month = '{3}'
                                    ", openid, money, scode, nowMonth, allScan == 0 ? 1 : 0);
                                LogHelper.WriteMsg("sacn增加扫描红包记录-openid:" + openid + ",sql:" + sql);
                                SqlHelper2.ExecuteNonQuery(CommandType.Text, sql);
                                #endregion

                                r.scan_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                r.money = money;  //返回本次中奖金额

                                //获取用户扫描但是未提现的金额总和
                                sql = string.Format(@"SELECT  ISNULL(SUM(Money),0) Total 
                                  FROM dbo.RedPackScanRecord WHERE OpenId = '{0}' AND IsPay = 0 ", openid);
//                                sql = string.Format(@"SELECT ISNULL(SUM(Money),0) Total FROM (
//                                                    SELECT  MIN(Money) Money
//                                                    FROM dbo.RedPackScanRecord 
//                                                    WHERE OpenId = '{0}' AND IsPay = 0 GROUP BY ScanCode) t", openid);
                                r.kTxMoney = Convert.ToDouble(SqlHelper2.ExecuteScalar(CommandType.Text, sql));
                                if (r.kTxMoney > 1)
                                {
                                    r.isKeTiXian = 1;
                                }

                                r.result_status = "succ"; //返回成功状态
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    r.message = "有异常";
                    LogHelper.WriteMsg("error:" + ex.ToString());
                    RedPack.AddAlertLog(openid, ex.ToString(), "scan-error");
                }
            }
            else
            {
                r.message = "活动现处于关闭状态,谢谢~";
            }
            return JsonConvert.SerializeObject(r);
        }




        [HttpPost]
        public async Task<string> ScanPressureTest(dynamic requestData)
        {
            RScanJson r = new RScanJson();

            string kaiGuan = ConfigurationManager.AppSettings["kaiguan"];
            //if (kaiGuan == "ON")
            //{
            string openid = requestData.OpenId;
            string scode = requestData.Scode;
            try
            {
                if (string.IsNullOrEmpty(openid) || string.IsNullOrEmpty(scode))
                {
                    r.message = "参数有误";
                }
                else
                {
                    #region  1.判断是否注册，是否是队员
                    int isRegist = RedPack.GetIsRegist(openid);
                    if (isRegist == 1)
                    {
                        r.isRegist = 1;
                    }
                    else if (isRegist == -1) //表示是队员 返回
                    {
                        r.isRegist = 0;
                        r.message = "队员不能扫码";
                        return JsonConvert.SerializeObject(r);
                    }
                    #endregion

                    #region 2.获取用户扫描但是未提现的金额总和
                    string sql = "";
                    sql = string.Format(@"SELECT  ISNULL(SUM(Money),0) Total 
                                  FROM dbo.RedPackScanRecord WHERE OpenId = '{0}' AND IsPay = 0 ", openid);
                    r.kTxMoney = Convert.ToDouble(SqlHelper2.ExecuteScalar(CommandType.Text, sql));
                    if (r.kTxMoney > 1)
                    {
                        r.isKeTiXian = 1;
                    }
                    #endregion

                    //当天
                    string nowDay = DateTime.Now.ToString("yyyy-MM-dd");
                    //当月
                    string nowMonth = DateTime.Now.ToString("yyyy-MM");

                    //string a = CheckRule(openid, scode);

                    //验证二维码是否有效
                    sql = string.Format(@"SELECT a.IsScan,b.Money,convert(varchar(19),b.ScanDate,121) ScanDate
                                            FROM dbo.RedPackCode a LEFT JOIN dbo.RedPackScanRecord b ON a.Code=b.ScanCode
                                            WHERE a.Code = '{0}' ", scode);
                    DataTable dt = SqlHelper2.ExecuteDataTable(sql);
                    if (dt.Rows.Count == 0)
                    {
                        r.code_status = "非活动码";
                        r.message = "此码不属于本次活动";
                        RedPack.AddAlertLog(openid, scode + ":" + r.message, "scan");
                    }
                    //配合压测 mark掉
                    //else if (Convert.ToInt16(dt.Rows[0]["IsScan"]) == 1)
                    //{
                    //    r.code_status = "已被扫";
                    //    r.money = Convert.ToDouble(dt.Rows[0]["Money"]);
                    //    r.scan_date = dt.Rows[0]["ScanDate"].ToString();
                    //    r.message = "该二维码已被扫描";
                    //    RedPack.AddAlertLog(openid, scode + ":" + r.message, "scan");
                    //}
                    else
                    {
                        //验证当天扫描是否超过10次
                        sql = string.Format("SELECT COUNT(1) FROM RedPackScanRecord WHERE   OpenId ='{0}' AND CONVERT(VARCHAR(10),ScanDate,121) = '{1}'  ", openid, nowDay);
                        int nowDayScan = SqlHelper2.GetCount(CommandType.Text, sql);
                        if (nowDayScan >= 10)
                        {
                            r.message = "当天扫描是已超过10次";
                            RedPack.AddAlertLog(openid, r.message, "scan");
                        }
                        else
                        {
                            sql = string.Format("SELECT COUNT(1) FROM RedPackScanRecord WHERE   OpenId ='{0}' ", openid);
                            int allScan = SqlHelper2.GetCount(CommandType.Text, sql);

                            //去抽红包
                            double money = RedPack.GetMoney(allScan, openid, nowMonth);

                            #region 增加扫描红包记录
                            sql = string.Format(@"INSERT INTO dbo.RedPackScanRecord
                                            ( OpenId ,Money ,ScanCode ,IsPay ,PayId,IsFirst,ScanMonth)
                                    VALUES  ( '{0}' , -- OpenId - varchar(50)
                                              {1} , -- Money - float
                                              '{2}' , -- ScanCode - varchar(50)
                                              0 , -- IsPay - int
                                              0 , -- PayId - int
                                              {4},'{3}') ;
                                 UPDATE dbo.RedPackCode SET IsScan =1,UseDate = GETDATE() WHERE Code = '{2}';
                                 UPDATE dbo.RedPackConfig SET LeiJiMoney = LeiJiMoney + {1} WHERE Month = '{3}'
                                    ", openid, money, scode, nowMonth, nowDayScan == 0 ? 1 : 0);
                            LogHelper.WriteMsg("sacn增加扫描红包记录-openid:" + openid + ",sql:" + sql);
                            SqlHelper2.ExecuteNonQuery(CommandType.Text, sql);
                            #endregion

                            r.scan_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            r.money = money;  //返回本次中奖金额

                            //获取用户扫描但是未提现的金额总和
                            sql = string.Format(@"SELECT  ISNULL(SUM(Money),0) Total 
                                  FROM dbo.RedPackScanRecord WHERE OpenId = '{0}' AND IsPay = 0 ", openid);
                            r.kTxMoney = Convert.ToDouble(SqlHelper2.ExecuteScalar(CommandType.Text, sql));
                            if (r.kTxMoney > 1)
                            {
                                r.isKeTiXian = 1;
                            }

                            r.result_status = "succ"; //返回成功状态
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                r.message = "有异常";
                RedPack.AddAlertLog(openid, ex.ToString(), "scan-error");
            }
            //}
            //else
            //{
            //    r.message = "活动现处于关闭状态,谢谢~";
            //}
            return JsonConvert.SerializeObject(r);
        }


        [HttpGet]
        public string TestGetRedPack(string openid = "0")
        {
            //RedPack.CreateRedPack(2,"2018-08") ;

            //var a = RedPack.GetMoneys();
            //var a = RedPack.GetMoneys2();
            string nowday = DateTime.Now.ToString("yyyy-MM-dd");
            string nowmonth = DateTime.Now.ToString("yyyy-MM");
            if (string.IsNullOrEmpty(openid))
            {
                for (int s = 51; s <= 52; s++)
                {
                    openid = "xjy" + s.ToString();
                    for (int i = 1; i <= 10; i++)
                    {
                        string sql = string.Format("SELECT COUNT(1) FROM RedPackScanRecord WHERE   OpenId ='{0}' AND CONVERT(VARCHAR(10),ScanDate,121) = '{1}'  ", openid, nowday);
                        int nowdayscan = SqlHelper2.GetCount(CommandType.Text, sql);
                        var money = RedPack.GetMoney(nowdayscan, openid, nowmonth);
                        //增加扫描红包记录
                        sql = string.Format(@"INSERT INTO dbo.RedPackScanRecord
                                            ( OpenId ,Money ,ScanCode ,IsPay ,PayId,IsFirst,ScanMonth)
                                    VALUES  ( '{0}' , -- OpenId - varchar(50)
                                              {1} , -- Money - float
                                              '{2}' , -- ScanCode - varchar(50)
                                              0 , -- IsPay - int
                                              0 , -- PayId - int
                                              {4},'{3}') ;
                                    ", openid, money, "", nowmonth, nowdayscan == 0 ? 1 : 0);
                        SqlHelper2.ExecuteNonQuery(CommandType.Text, sql);
                    }
                }
            }
            else
            {
                string sql = string.Format("SELECT COUNT(1) FROM RedPackScanRecord WHERE   OpenId ='{0}' AND CONVERT(VARCHAR(10),ScanDate,121) = '{1}'  ", openid, nowday);
                int nowdayscan = SqlHelper2.GetCount(CommandType.Text, sql);
                var money = RedPack.GetMoney(nowdayscan, openid, nowmonth);
                //增加扫描红包记录
                sql = string.Format(@"INSERT INTO dbo.RedPackScanRecord
                                            ( OpenId ,Money ,ScanCode ,IsPay ,PayId,IsFirst,ScanMonth)
                                    VALUES  ( '{0}' , -- OpenId - varchar(50)
                                              {1} , -- Money - float
                                              '{2}' , -- ScanCode - varchar(50)
                                              0 , -- IsPay - int
                                              0 , -- PayId - int
                                              {4},'{3}') ;
                                    ", openid, money, "", nowmonth, nowdayscan == 0 ? 1 : 0);
                SqlHelper2.ExecuteNonQuery(CommandType.Text, sql);
            }

            return "";
        }
        /// <summary>
        /// 验证抽奖规则
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="scode"></param>
        /// <returns></returns>
        private string CheckRule(string openid, string scode)
        {
            string msg = "fail,";
            string sql = "";
            //验证scode码是否可用
            sql = string.Format(@"SELECT State FROM dbo.RedPackCode WHERE Code = '{0}' ",scode);
            var a = SqlHelper2.ExecuteScalar(CommandType.Text, sql);

            string nowday = DateTime.Now.ToString("yyyy-MM-dd");
            //验证当天扫描是否超过10次
            sql = string.Format("SELECT COUNT(1) FROM dboRedPackScanRecord WHERE   OpenId ='{0}' AND ScanDate LIKE'%{1}%' ",openid,nowday);
            int nowdayscan = SqlHelper2.GetCount(CommandType.Text, sql);
            if (nowdayscan >= 10)
            {
                msg = "fail,当天扫描是已超过10次";
                return msg;
                //r.message = "当天扫描是已超过10次";
                //return JsonConvert.SerializeObject(r);
            }
            return "";
        }



    }
}

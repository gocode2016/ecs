using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using WxScanAPI.Models;
using System.Diagnostics;


namespace WxScanAPI.Common
{
    public static class RedPack
    {
        /// <summary>
        /// 生成红包算法
        /// </summary>
        /// <param name="nowdayscan">当日扫码次数</param>
        /// <param name="openid">微信id</param>
        /// <param name="nowmonth">当前月份</param>
        /// <returns></returns>
        public static double GetMoney(int allScan, string openid, string nowmonth)
        {
            double money = 0.08;
            try
            {
                if (allScan == 0)
                {
                    money = 1.08;  //第一次默认是1.08元,以后随机
                }
                else
                {
                    double limitMoney = Convert.ToDouble(ConfigurationManager.AppSettings[nowmonth + "LimitMoney"]); //每月限定金额
                    int firstScanCount = Convert.ToInt32(ConfigurationManager.AppSettings[nowmonth + "FirstScanCount"]);  //每月设定的参与人数,即第一次扫码的次数
                    int otherScanCount = Convert.ToInt32(ConfigurationManager.AppSettings[nowmonth + "OtherScanCount"]);  //每月设定的除去第一次扫码次数
                    string sql = "";
                    Random r = new Random();
                    Random r2 = new Random();
                    sql = string.Format(@"
                            SELECT  ISNULL(SUM(a.Money),0) MonthTotal , ISNULL(MIN(c.FirstScanCount),0) FirstScanCount ,
                            ISNULL(MIN(b.OtherScanCount),0) OtherScanCount,MIN(d.LargeMoneyCount) LargeMoneyCount
                            FROM  dbo.RedPackScanRecord a  
                            LEFT JOIN (SELECT COUNT(1)  OtherScanCount FROM  RedPackScanRecord WHERE   ScanMonth = '{1}' AND IsFirst = 0
                            ) b ON 1=1 
                            LEFT JOIN (SELECT COUNT(1)  FirstScanCount FROM  RedPackScanRecord WHERE   ScanMonth = '{1}' AND IsFirst = 1
                            ) c ON 1=1 
                            LEFT JOIN (SELECT COUNT(1) LargeMoneyCount FROM RedPackScanRecord WHERE Money >10 AND OpenId = '{0}') d ON 1=1
                            WHERE a.ScanMonth = '{1}' ", openid, nowmonth);
                    DataTable dt2 = SqlHelper2.ExecuteDataTable(sql);
                    double monthTotal = Convert.ToDouble(dt2.Rows[0]["MonthTotal"]);  //当月累计金额
                    int mFirstScanCount = Convert.ToInt32(dt2.Rows[0]["FirstScanCount"]);  //当月实际参与人数
                    int mOtherScanCount = Convert.ToInt32(dt2.Rows[0]["OtherScanCount"]);  //当月除去第一次扫描后的扫描次数
                    int largeMoneyCount = Convert.ToInt32(dt2.Rows[0]["LargeMoneyCount"]);   // 该openid是否有中大奖的记录
                    int aa = 0;
                    int bb = 0;

                    //不是第一次扫码的次数总和 大于设定的浏览次数
                    //或者当月扫码总金额大于累计金额
                    if (mOtherScanCount >= otherScanCount || monthTotal >= limitMoney)
                    {
                        money = 0.08;
                    }
                    else
                    {
                        if (largeMoneyCount == 0)  //没有中大奖的记录
                        {
                            aa = r.Next(0, 10001);
                            bb = 0;
                            if (aa > 9998)   //中大奖30-80元
                            {
                                bb = r2.Next(3000, 8801);
                                money = Convert.ToDouble(((float)bb / 100).ToString("0.00"));
                            }
                            else if (aa > 9980)//中大奖10-30元
                            {
                                bb = r2.Next(1000, 3000);
                                money = Convert.ToDouble(((float)bb / 100).ToString("0.00"));
                            }
                            else
                            {
                                double minMoney = Convert.ToDouble(ConfigurationManager.AppSettings["MinMoney"]);  //最大值
                                double maxMoney = Convert.ToDouble(ConfigurationManager.AppSettings["MaxMoney"]); //最小值

                                //当月剩余金额=当月总金额-当月已经抽中的金额-(预设的当月扫码人数-当月已经扫码的人数)*1.08-(预设的当月的非第1次扫码次数-当月已经非第1次扫码次数)*0.08
                                //(预设的当月扫码人数-当月已经扫码的人数)*1.08 ：确保剩下的金额足够分配剩下的人第一次扫码金额
                                //(预设的当月的非第1次扫码次数-当月已经非第1次扫码次数)*0.08:确保剩下的非第一次扫码次数分配最低值
                                double remainMoney = limitMoney - monthTotal - (firstScanCount - mFirstScanCount) * 1.08 - (otherScanCount - mOtherScanCount - 1) * minMoney;
                                
                                //double remainMoney = limitmoney - monthtotal -  (sdcanyucount - montycanyucount - 1) * minmoney;
                                int remainCount = otherScanCount - mOtherScanCount;
                                if (remainMoney < remainCount * minMoney)
                                {
                                    money = 0.18;
                                }
                                else if (remainCount == 1)
                                {
                                    if (remainMoney > 10)  //防止最后一个过大
                                    {
                                        money = 0.88;
                                    }
                                    else
                                    {
                                        money = Convert.ToDouble(remainMoney.ToString("0.00"));
                                    }
                                }
                                else
                                {
                                    //(_redPacket.remainMoney - (_redPacket.remainCount - 1) * min)：保存剩余金额可以足够的去分配剩余的红包数量
                                    double max = (remainMoney - (remainCount - 1) * minMoney) / remainCount * 2;
                                    money = r.NextDouble() * max;
                                    money = Convert.ToDouble((money <= minMoney ? minMoney : money).ToString("0.00"));
                                }
                            }
                        }
                        else  //有中大奖的记录后生成个小红包
                        {
                            aa = r.Next(8, 108);
                            money = Convert.ToDouble(((float)aa / 100).ToString("0.00"));
                        }
                    }
                }
            }
            catch (Exception ex) //有异常返回0.18
            {
                money = 0.18;
                RedPack.AddAlertLog(openid,ex.ToString(), "scan生成红包时异常");
            }
            if (money < 0.08 || money > 88) //抽中的金额异常处理
            {
                money = 0.18;
            }
            return money;
        }

        /// <summary>
        /// 付款操作
        /// </summary>
        /// <param name="openid">微信openid</param>
        /// <returns></returns>
        public static RJson PayAction(string openid)
        {
            RJson r = new RJson();
            //r.message = "test";
            //r.result_status = "succ";
            //return r;
            string ids = "0";//付款ids默认0
            int payFlag = 0; //付款未成功
            try
            {
                //openid = "o_-IA0YU6JgzTvKINab0B30y0L_Y";//我的openid用于测试
                //openid = "o11Z-joTwit-1j2QLx_-vAzwsvvQ";
                //获取用户扫描但是未提现的金额总和
                string sql = string.Format(@"SELECT  ISNULL(STUFF((SELECT ','+  CONVERT(VARCHAR(20),Id) 
			                                              FROM dbo.RedPackScanRecord 
			                                              WHERE  OpenId = '{0}' AND IsPay = 0  for xml path('')),1,1,''),0) Ids,
                                            ISNULL(SUM(Money),0) Total FROM RedPackScanRecord  WHERE OpenId = '{0}' AND IsPay = 0", openid);
                var dt = SqlHelper2.ExecuteDataTable(sql);
                ids = dt.Rows[0][0].ToString();
                double totalTxMoney = Convert.ToDouble(dt.Rows[0][1]);  //未提现金额

                if (ids == "0")
                {
                    r.message = "无可提取的额度";
                }
                else if (totalTxMoney < 1)
                {
                    r.message = "不足一元不能提取成功";
                }
                else
                {
                    //先置为已提现
                    sql = string.Format(@"UPDATE dbo.RedPackScanRecord SET IsPay =1  WHERE Id IN ({0})", ids);
                    LogHelper.WriteMsg("先置为已提现-openid:" + openid + ",ids:" + ids);
                    SqlHelper2.ExecuteNonQuery(CommandType.Text, sql);
                   

                    string strPayResult = CorpPay((totalTxMoney * 100).ToString(), openid);  //实际付款
                    LogHelper.WriteMsg("付款结果-openid:" + openid + ",ids:" + ids + ",结果:" + strPayResult);
                    PayResultModel model = XmlHelpler.GetWxPayResult(strPayResult);   //付款结果转类

                    int payid = 0;
                    if (model.result_code == "SUCCESS")  //支付成功,增加纪录
                    {
                        payFlag = 1;
                        sql = string.Format(@"INSERT INTO dbo.RedPackPayRecord
                                    ( OpenId ,
                                      PayAmout ,
                                      WxResult_Code ,
                                      WxPartner_Trade_No ,
                                      WxPayment_No ,
                                      WxPayment_Time ,
                                      WxReturnXML ,
                                      ScanRecordId
                                    )
                            VALUES  ( '{0}' , -- OpenId - varchar(50)
                                       {1} , -- PayAmout - float
                                      '{2}' , 
                                      '{3}' , 
                                      '{4}' , 
                                      '{5}' , 
                                      '{6}' ,
                                      '{7}'
                                    ); 
                            UPDATE dbo.RedPackScanRecord SET IsPay =1 ,PayId = (SELECT scope_identity()) WHERE Id IN ({7})
                             ", openid, totalTxMoney, model.result_code, model.partner_trade_no,
                                      model.payment_no, model.payment_time, strPayResult, ids);
                        LogHelper.WriteMsg("支付成功,增加纪录-openid:" + openid + ",ids:" + ids + ",sql:" + sql);
                        SqlHelper2.ExecuteNonQuery(CommandType.Text, sql);
                        //payid = Convert.ToInt32(SqlHelper2.ExecuteScalar(CommandType.Text, sql));

                        //sql = string.Format(@"UPDATE dbo.RedPackScanRecord SET IsPay =1 ,PayId = {0} WHERE Id IN ({1})",
                        //                        payid, ids);
                        //SqlHelper2.ExecuteNonQuery(CommandType.Text, sql);

                        r.message = "提取成功";
                        r.result_status = "succ";   
                    }
                    else
                    {
                        //付款失败  置为IsPay =0未付款
                        sql = string.Format(@"UPDATE dbo.RedPackScanRecord SET IsPay =0  WHERE Id IN ({0})", ids);
                        LogHelper.WriteMsg("支付失败,增加纪录-openid:" + openid + ",ids:" + ids + ",sql:" + sql);
                        SqlHelper2.ExecuteNonQuery(CommandType.Text, sql);
                        //支付失败 增加报警信息
                        AddAlertLog(openid, "红包" + ids + "支付时:" + strPayResult, "pay");
                        r.message = "提取失败,请联系管理员";
                    }
                }

                //double totalTxMoney = Convert.ToDouble(SqlHelper2.ExecuteScalar(CommandType.Text, sql));
            }
            catch (Exception ex)
            {
                //LogHelper.WriteMsg("PayAction异常-openid:" + openid + ",结果:" + ex.ToString());//支付失败 增加报警信息
                AddAlertLog(openid, ex.ToString(), "error"); //支付失败 增加报警信息
                r.message = "提取失败,请联系管理员";
            }
            return r;
        }


        /// <summary>
        /// 企业付款(请求需要双向证书)
        /// 企业付款业务是基于微信支付商户平台的资金管理能力，为了协助商户方便地实现企业向个人付款，
        /// 针对部分有开发能力的商户，提供通过API完成企业付款的功能。 比如目前的保险行业向客户退保、给付、理赔。
        /// 企业付款将使用商户的可用余额，需确保可用余额充足。查看可用余额、充值、提现请登录商户平台“资金管理”进行操作。https://pay.weixin.qq.com/ 
        /// 注意：与商户微信支付收款资金并非同一账户，需要单独充值。
        /// </summary>
        /// <param name="amount">企业支付数据</param>
        /// <param name="openid">微信openid</param>
        /// <returns></returns>
        public static string CorpPay(string amount, string openid)  //
        {
            string nowDay = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            //parameters.Add("mch_appid", "wx6de1aa1032b410fc");//公众账号appid， 注意是mch_appid，而非wxappid
            parameters.Add("mch_appid", "wx9657251bde1a71c4");//公众账号appid， 注意是mch_appid，而非wxappid  餐饮积分的
            //parameters.Add("mchid", "1489094092");//商户号, 注意是mchid而非mch_id
            parameters.Add("mchid", "1249285801");//商户号, 注意是mchid而非mch_id 餐饮积分的
            parameters.Add("nonce_str", StringOperate.GetRandomString(16));//随机字符串
            parameters.Add("spbill_create_ip", "58.57.17.2");//终端ip      
            parameters.Add("partner_trade_no", nowDay);//交易单号
            parameters.Add("device_info", "");//终端ip            
            parameters.Add("openid", openid); //我的openid用于测试
            parameters.Add("check_name", "NO_CHECK");
            parameters.Add("re_user_name", "");
            parameters.Add("amount", amount);
            parameters.Add("desc", "恭喜您，味达美尚品生抽的红包已领取成功！");
            parameters = parameters.OrderBy(p => p.Key).ToDictionary(p => p.Key, o => o.Value);
            parameters.Add("sign", StringOperate.Str2MD5(StringOperate.MakeSign(parameters, ConfigurationManager.AppSettings["APISecret"].ToString())));//最后生成签名

            string requestXml = XmlHelpler.GetCorpPayXml(parameters);

            var url = string.Format("https://api.mch.weixin.qq.com/mmpaymkttransfers/promotion/transfers");
            var responseXml = WeiXinHelper.SendCorpPayWithCert(requestXml, url);
            return responseXml;
        }

        /// <summary>
        /// 增加报警信息
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="content"></param>
        /// <param name="alertType">报警类型</param>
        public static void AddAlertLog(string openid, string content, string alertType)
        {
            string sql = string.Format(@"INSERT INTO dbo.RedPackAlert( OpenId , AlertContent ,AlertType)
                                     VALUES  ( '{0}' ,'{1}','{2}')", openid, content.Length > 1000 ? content.Substring(0, 1000) : content, alertType);
            SqlHelper2.ExecuteNonQuery(CommandType.Text, sql);
        }


        /// <summary>
        /// 红包支付
        /// </summary>
        /// <returns></returns>
         public static string RedPackPay()  //
        {
            //CheckAccount();//检查AccountInfo的对象属性值
            IDictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("nonce_str", StringOperate.GetRandomString(16));//随机字符串
            parameters.Add("mch_billno", "201807311513001");//商户订单号
            parameters.Add("mch_id", "1489094092");//商户号, 注意是mchid而非mch_id
            parameters.Add("wxappid", "wx6f67aee8f096b87f");//微信分配的公众账号ID（企业号corpid即为此appId）。在微信开放平台（open.weixin.qq.com）申请的移动应用appid无法使用该接口。
            parameters.Add("send_name", "味达美餐饮");
            parameters.Add("re_openid", "o_-IA0YU6JgzTvKINab0B30y0L_Y");
            parameters.Add("total_amount", "0.5");//终端ip      
            parameters.Add("total_num", "1");//终端ip            
            parameters.Add("wishing", "谢谢");
            parameters.Add("client_ip", "58.57.17.2");
            parameters.Add("act_name", "尚品生抽活动");
            parameters.Add("remark", "多买多发");
            parameters.Add("scene_id", "PRODUCT_1");
            parameters.Add("risk_info", "");
            parameters.Add("consume_mch_id", "");

            parameters = parameters.OrderBy(p => p.Key).ToDictionary(p => p.Key, o => o.Value);
            parameters.Add("sign", StringOperate.Str2MD5(StringOperate.MakeSign(parameters, ConfigurationManager.AppSettings["APISecret"].ToString())));//最后生成签名

            string requestXml = XmlHelpler.GetRedPackPayXml(parameters);

            var url = string.Format("https://api.mch.weixin.qq.com/mmpaymkttransfers/sendredpack");
            var a = WeiXinHelper.SendCorpPayWithCert(requestXml, url);
            return a;
        }
        /// <summary>
        /// 字符串MD5加密
        /// </summary>
        /// <param name="sStr"></param>
        /// <returns></returns>
        public static string Str2MD5(string sStr)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sStr, "MD5");
        }
        /// <summary>
        /// 通过openid查询是否已经注册 
        /// 1:已注册的可正常扫码的会员; 
        /// 0:未注册的会员;
        /// -1:队员,不能扫码;  
        /// -2:已注册的岗位是调味品供货商的会员,不能扫码
        /// </summary>
        /// <param name="openid">微信openid</param>
        /// <returns></returns>
        public static int GetIsRegist(string openid)
        {
//            string sql = string.Format(@"SELECT   b.UserType FROM dbo.RegistMember a JOIN dbo.OpenIdAssociated b
//                                            ON a.MemberId = b.UserId 
//                                            WHERE   b.OpenId = '{0}' order by b.UserType ", openid);
//            var userType = SqlHelper2.ExecuteScalar(CommandType.Text, sql);
//            //userType用户类型  1是队员  2是会员
//            if (userType == null)
//            {
//                return 0;
//            }
//            else if (userType.ToString() == "1")
//            {
//                return -1;
//            }
//            else
//            {
//                return 1;    
//            }
            string sql = string.Format(@"SELECT   b.UserType,a.Position,c.JobType FROM dbo.RegistMember a 
                            JOIN dbo.OpenIdAssociated b ON a.MemberId = b.UserId 
                            LEFT JOIN dbo.MemberProfile c ON c.MemberId = a.MemberId
                            WHERE   b.OpenId = '{0}' order by b.UserType
                             ", openid);
            DataTable dt = SqlHelper2.ExecuteDataTable(sql);
            var userType = SqlHelper2.ExecuteScalar(CommandType.Text, sql);
            //userType用户类型  1是队员  2是会员
            if (dt.Rows.Count == 0)   //未注册
            {
                return 0;
            }
            else if (dt.Rows[0]["UserType"].ToString()=="1")  //是队员
            {
                return -1;
            }
            else if (dt.Rows[0]["Position"].ToString() == "调味品供货商" || dt.Rows[0]["JobType"].ToString() == "调味品供货商") //会员岗位是调味品供货商
            {
                return -2;
            }
            else    //已经注册的 符合扫码条件的
            {
                return 1;
            }
        }
       
    


        public  static void CreateQrCodeByMD5(int number)
        {
            //压缩包存储路径
            var basePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var qrCodePath = Convert.ToString(ConfigurationManager.AppSettings["QrCodeStorePath"]);
            var fileName = string.Format("{0}.xls", Guid.NewGuid().ToString());
            var serverPath = qrCodePath + fileName;
            var filePath = basePath + serverPath;
//            string sql = string.Format(@"INSERT INTO dbo.Hfh_RedPackQrCodePackage
//                                        ( Num ,State )VALUES  ({0},0) 
//                                         Select @@Identity ", number);
//            var packageid = SqlHelper2.ExecuteScalar(CommandType.Text, sql);
            string now = "";
            string guid = "";
            string url = "";
            string code = "";
            string sql = "";
            List<string> qrCodeList = new List<string>();
            int count = number / 100;
            //生成二维码
            for (int i = 1; i <= count; i++)
            {
                sql = "INSERT INTO dbo.RedPackCode ( Code , QrcUrl  )VALUES ";
                for (int s = 1; s <= 100; s++)
                {
                    now = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    guid = Guid.NewGuid().ToString();
                    code = guid.Substring(0, 6) + Str2MD5(guid + now).Substring(0, 6);
                    //sn = Confusion(code);
                    url = ConfigurationManager.AppSettings["QrCodeUrl"] + "&scode=" + code;
                    sql += " ( '" + code + "' , '" + url + "' ),"; 
                }

                sql = sql.Substring(0, sql.Length - 1);
//                sql = string.Format(@"INSERT INTO dbo.RedPackCode
//                                        ( Code , QrcUrl  )
//                                        VALUES  ( '{0}' , '{1}' )",  code,url );
                SqlHelper2.ExecuteNonQuery(CommandType.Text, sql);
                //qrCodeList.Add(url);
            }

            //生成Excel
            //ExcelExtend.SaveExcel<string>(qrCodeList, qrCodePath, fileName);

            //sql = "update Hfh_RedPackQrCodePackage set state =1,DownLoadUrl = '" + serverPath + "' where packageid = " + packageid.ToString();
            //SqlHelper2.ExecuteNonQuery(CommandType.Text, sql);
        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string CreateQrCode(int number, int goodsid = 0)
        {

            //压缩包存储路径
            var basePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var qrCodePath = Convert.ToString(ConfigurationManager.AppSettings["QrCodeStorePath"]);
            var fileName = string.Format("{0}.xls", Guid.NewGuid().ToString());
            var serverPath = qrCodePath + fileName;
            var filePath = basePath + serverPath;
            string sql = string.Format(@"INSERT INTO dbo.Hfh_RedPackQrCodePackage
                                        ( Num ,State )VALUES  ({0},0) 
                                         Select @@Identity ", number);
            var packageid = SqlHelper2.ExecuteScalar(CommandType.Text, sql);
            string now = "";
            string guid = "";
            string safe = "beautiful";
            string url = "";
            string sn = "";
            string code = "";
            List<string> qrCodeList = new List<string>();
            //生成二维码
            for (int i = 1; i <= number; i++)
            {
                now = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                guid = Guid.NewGuid().ToString();
                code = now.Substring(0, 8) + now.Substring(14) + packageid.ToString().PadLeft(10, '0') + Guid.NewGuid().ToString().Substring(0, 8);
                sn = Confusion(code);
                url = ConfigurationManager.AppSettings["QrCodeUrl"] + "?sn=" + sn;
                sql = string.Format(@"INSERT INTO dbo.Hfh_RedPackQrCode
                                        ( QrcUrl , Code , State ,PackageId )
                                        VALUES  ( '{0}' , '{1}' , 1 , {2}  )", url, code, packageid);
                SqlHelper2.ExecuteNonQuery(CommandType.Text, sql);
                qrCodeList.Add(url);
            }

            //生成Excel
            ExcelExtend.SaveExcel<string>(qrCodeList, qrCodePath, fileName);

            sql = "update Hfh_RedPackQrCodePackage set state =1,DownLoadUrl = '" + serverPath + "' where packageid = " + packageid.ToString();
            SqlHelper2.ExecuteNonQuery(CommandType.Text, sql);
            return "succ";
        }
        /// <summary>
        /// 判断二维码是否有效
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string CheckSn(string sn, string openid)
        {
            //验证二维码是否有效
            var code = RedPack.Confusion(sn);   //解码
            string sql = string.Format(@"SELECT a.State,b.IsPreSend,b.IsRealSend ,b.OpenId FROM Hfh_RedPackQrCode a 
                                        LEFT   JOIN  Hfh_RedPackSendRecord b ON b.QrcId = a.QrcId
                                        WHERE a.state = 1 and  a.Code = '{0}'", code);
            DataTable dt = SqlHelper2.ExecuteDataTable(sql);
            if (dt.Rows.Count == 1)
            {
                //var state = Convert.ToInt16(dt.Rows[0]["State"]).ToString();
                var IsPreSend = dt.Rows[0]["IsPreSend"] == null ? "0" : dt.Rows[0]["IsPreSend"].ToString();
                var IsRealSend = dt.Rows[0]["IsRealSend"] == null ? "0" : dt.Rows[0]["IsRealSend"].ToString();
                var sendopenid = dt.Rows[0]["OpenId"] == null ? "" : dt.Rows[0]["OpenId"].ToString();
                if (IsRealSend == "1")
                {
                    return "二维码已失效：红包已成功发送";
                }
                else
                {
                    if (IsPreSend == "1")
                    {
                        if (sendopenid == openid)
                        {
                            return "请关注公众号领取红包";
                        }
                        else
                        {
                            return "二维码已失效:红包已被他人领取";
                        }
                    }
                    else
                    {
                        return "succ";
                    }
                }

            }
            else
            {
                return "fail";
            }
        }
        /// <summary>
        /// 混淆字符串
        /// </summary>
        /// <param name="encryptString">混淆字符串</param>
        public static string Confusion(string content)
        {
            //混淆方法将字符串两段a和b，反转a和b的顺序，再返回b+a
            try
            {
                if (content.Length == 29)
                {
                    var strPrev = content.Substring(0, 10);
                    var strAfter = content.Substring(10);
                    var strNew = new string(strAfter.Reverse().ToArray()) + new string(strPrev.Reverse().ToArray());
                    return strNew;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}

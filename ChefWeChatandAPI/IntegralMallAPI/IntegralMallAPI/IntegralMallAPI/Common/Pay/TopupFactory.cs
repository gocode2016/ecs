using IntegralMallAPI.Common.ofpay;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace IntegralMallAPI.Common
{

    public class TopupFactory
    {
        /// <summary>
        /// 充值入口
        /// </summary>
        public static RetCode topup(TopupModel model)
        {
            switch (model.PaymentType)
            {
                case 1:
                    {
                        return null;
                    }

                case 2:
                    {
                        return null;
                    }

                case 4:
                    {
                        return topupOfpay(model);
                    }
                case 41:
                case 43:
                    //提卡密
                    return TakeCard(model);
            }

            return new RetCode();
        }

        /// <summary>
        /// 欧飞充值
        /// </summary>
        public static RetCode topupOfpay(TopupModel model)
        {
            var result = OfpayInterface.topup(model.Account, model.Value, model.ExtendInfo);
            //var result = new RetCode() { retcode = "1" };
            return result;
        }


        /// <summary>
        /// 油卡充值
        /// </summary>
        public static RetCode Sinopec(TopupModel model)
        {
            var result = OfpayInterface.Sinopec(model.Account, model.CardTel, model.Value, model.ExtendInfo);
            return result;
        }
        /// <summary>
        /// 提取卡密
        /// </summary>
        public static RetCode TakeCard(TopupModel model)
        {
            //var result = OfpayInterface.TakeCard(model.CardCode, model.CardNum, model.CardTel, model.ExtendInfo);
            var result = new RetCode() { retcode = "0",cardno = "卡号请查收短信",cardpws = "密码请查收短信"};
            return result;
        }
    }

    /// <summary>
    /// 充值实体
    /// </summary>
    public class TopupModel
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// 充值账户，如手机号，电话号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 卡的联系方式
        /// </summary>
        public string CardTel { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public string CardNum { get; set; }
        /// <summary>
        /// 面值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 其他扩展信息
        /// </summary>
        public string ExtendInfo { get; set; }
        /// <summary>
        /// 支付接口类型
        /// </summary>
        public int PaymentType { get; set; }
        /// <summary>
        /// 根据卡类型和面额返回编码
        /// </summary>
        /// <returns>礼品卡的编码</returns>
        public string CardCode
        {
            get {
                var code = string.Empty;
                switch (PaymentType)
                {
                    case 41:
                        switch (int.Parse(Value))
                        {
                            case 50:
                                code = "64313605";
                                break;
                            case 100:
                                code = "64313604";
                                break;
                            case 500:
                                code = "64313602";
                                break;
                            case 1000:
                                code = "64313601";
                                break;
                        }
                        break;
                    case 43:
                        switch (int.Parse(Value))
                        {
                            case 5:
                                code = "1227400";
                                break;
                            case 10:
                                code = "1227401";
                                break;
                            case 50:
                                code = "1227408";
                                break;
                            case 100:
                                code = "1227402";
                                break;
                            case 200:
                                code = "1227403";
                                break;
                            case 300:
                                code = "1227404";
                                break;
                            case 500:
                                code = "1227405";
                                break;
                            case 800:
                                code = "1227406";
                                break;
                            case 1000:
                                code = "1227407";
                                break;
                        }
                        break;
                }
                return code;
            }
        }
    }
}
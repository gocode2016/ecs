using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreditSysWeChat.Common
{
    public static class RquestHelper
    {
        /// <summary>
        /// 根据参数生成查询条件
        /// 参数Name形式：q_rsm_str_name
        /// q:固定，表示查询
        /// rsm:可变，可为空，为表名在查询语句的简写，如：RegistSaleMan AS rsm
        /// str:可变，查询条件的形式，包括：str(字符),int(数字),bd(开始时间),ed(结束时间)
        /// name:可变，字段名
        /// </summary>
        /// <param name="param">Request.QueryString</param>
        /// <returns>SQL查询条件</returns>
        public static string SearchCondition(System.Collections.Specialized.NameValueCollection param)
        {
            StringBuilder strCondition = new StringBuilder();
            if (param.AllKeys.Count() > 0)
            {                
                foreach (string key in param.AllKeys)
                {                                           
                    if (key.IndexOf('_') > -1)
                    {
                        var queryName = key.Split('_');
                        if (queryName.Length == 4 && queryName[0] == "q") 
                        {
                            var queryValue = param[key];
                            var queryPrefix = queryName[1];
                            if (!string.IsNullOrEmpty(queryPrefix)) 
                            {
                                queryPrefix += ".";
                            }
                            var queryType = queryName[2];
                            var queryField = queryPrefix + queryName[3];
                            if (strCondition.Length > 0) 
                            {
                                queryField = string.Format(" AND {0}", queryField);
                            }
                            if (!string.IsNullOrEmpty(queryValue)) 
                            {
                                switch (queryType)
                                {
                                    case "str":
                                        strCondition.AppendFormat(" {0} like '%{1}%' ", queryField, queryValue);
                                        break;
                                    case "int":
                                        strCondition.AppendFormat(" {0} = {1} ", queryField, queryValue);
                                        break;
                                    case "bd":
                                        queryValue = DateTime.Parse(queryValue).ToString();
                                        strCondition.AppendFormat(" {0} >= '{1}' ", queryField, queryValue);
                                        break;
                                    case "ed":
                                        queryValue = DateTime.Parse(queryValue).AddDays(1).AddSeconds(-1).ToString();
                                        strCondition.AppendFormat(" {0} <= '{1}' ", queryField, queryValue);
                                        break;
                                    default:
                                        break;
                                }                                
                            }
                        }
                    }
                }
            }
            return strCondition.ToString();
        }
        public static string ToSql(this System.Collections.Specialized.NameValueCollection param)
        {
            StringBuilder strCondition = new StringBuilder();
            if (param.AllKeys.Count() > 0)
            {
                foreach (string key in param.AllKeys)
                {
                    if (key.IndexOf('_') > -1)
                    {
                        var queryName = key.Split('_');
                        if (queryName.Length == 4 && queryName[0] == "q")
                        {
                            var queryValue = param[key];
                            var queryPrefix = queryName[1];
                            if (!string.IsNullOrEmpty(queryPrefix))
                            {
                                queryPrefix += ".";
                            }
                            var queryType = queryName[2];
                            var queryField = queryPrefix + queryName[3];
                            if (strCondition.Length > 0)
                            {
                                queryField = string.Format(" AND {0}", queryField);
                            }
                            if (!string.IsNullOrEmpty(queryValue))
                            {
                                switch (queryType)
                                {
                                    case "str":
                                        strCondition.AppendFormat(" {0} like '%{1}%' ", queryField, queryValue);
                                        break;
                                    case "int":
                                        strCondition.AppendFormat(" {0} = {1} ", queryField, queryValue);
                                        break;
                                    case "bd":
                                        queryValue = DateTime.Parse(queryValue).ToString();
                                        strCondition.AppendFormat(" {0} >= '{1}' ", queryField, queryValue);
                                        break;
                                    case "ed":
                                        queryValue = DateTime.Parse(queryValue).AddDays(1).AddSeconds(-1).ToString();
                                        strCondition.AppendFormat(" {0} <= '{1}' ", queryField, queryValue);
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            return strCondition.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaiPinKuAPI.Common
{
    public class PageHelper
    {
        /// <summary>
        /// 获取分页的sql
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string GetPagerSql(int page,int pagesize,string sql)
        {
            int begin = pagesize*(page-1)+1;
            int end= pagesize*page;
            var sqlpager = string.Format(@"select * from ( {0}) t where t.RowId BETWEEN {1} AND {2}", sql, begin, end);
            return sqlpager;
        }
        /// <summary>
        ///获取总条数的sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string GetTotalCountSql(string sql)
        {
            return "select count(1) TotalCount from ( "+sql+") t ";
        }
        /// <summary>
        /// 返回总页数
        /// </summary>
        /// <param name="totalcount">总条数</param>
        /// <param name="pagesize">每页条数</param>
        /// <returns></returns>
        public static int GetTotalPage(int totalcount,int pagesize)
        {
            int a = totalcount / pagesize; //取整
            int b = totalcount % pagesize; //取余
            if (b > 0)
            {
                 return a+1;
            }
            else
            {
                return a;
            }
        }
    }
}
using CreditSysWeChat.ActionFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CreditSysWeChat.Controllers
{
    /// <summary>
    /// 主页面控制器
    /// </summary>
    [AuthorizeFilter]
    public class HomeController : Controller 
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}

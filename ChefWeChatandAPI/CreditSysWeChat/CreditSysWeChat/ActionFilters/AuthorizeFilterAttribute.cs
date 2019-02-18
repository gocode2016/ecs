using CreditSysWeChat.Common;
using CreditSysWeChat.WeiXin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CreditSysWeChat.ActionFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AuthorizeFilterAttribute : AuthorizeAttribute
    {
        string controllerName = string.Empty;
        string actionName = string.Empty;
        string returnUrl = string.Empty;

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            string url = httpContext.Request.Url.ToString().ToLower();
            if (httpContext.Request.Cookies["WeiXinUser"] == null)
            {
                if (!httpContext.Request.IsLocal)
                {
                    returnUrl = BasicApi.Authorize(url);
                }
                else
                {
                    HttpCookie cookie = new HttpCookie("WeiXinUser");//初使化并设置Cookie的名称
                    DateTime dt = DateTime.Now;
                    TimeSpan ts = new TimeSpan(0, 15, 0, 0, 0);//过期时间为1分钟
                    cookie.Expires = dt.Add(ts);//设置过期时间
                    //cookie.Values["UserId"] = "30425";
                    //cookie.Values["UserType"] = "1";
                    cookie.Values["UserId"] = "31142";
                    cookie.Values["UserType"] = "2";
                    //cookie.Values["PositionId"] = "4";
                    cookie.Values["Openid"] = "o11Z-jte0u1RkdzEnXJee6OAp_CM";//"o11Z-jqJhjDeu5xjW7dzaBfzloZQ2";
                    cookie.Values["Nickname"] = httpContext.Server.UrlEncode("小三");
                    cookie.Values["Sex"] = "1";
                    cookie.Values["Province"] = httpContext.Server.UrlEncode("山东");
                    cookie.Values["City"] = httpContext.Server.UrlEncode("烟台");
                    cookie.Values["Country"] = httpContext.Server.UrlEncode("中国");
                    cookie.Values["Headimgurl"] = "http://wx.qlogo.cn/mmopen/g3MonUZtNHkdmzicIlibx6iaFqAc56vxLSUfpb6n5WKSYVY0ChQKkiaJSgQ1dZuTOgvLLrhJbERQQ4eMsv84eavHiaiceqxibJxCfHe/46";
                    httpContext.Response.AppendCookie(cookie);
                }
            }
            else
            {
                try
                {
                    var cookies = httpContext.Request.Cookies["WeiXinUser"];
                    //CreditSys.Core.Common.LogHelper.WriteLog("UserId：" + cookies["UserId"] + ",UserType:" + cookies["UserType"] + ",Openid:" + cookies["Openid"] + ",Nickname:" + cookies["Nickname"] + ",url:" + url);//日志

                    //openid丢失
                    if (string.IsNullOrEmpty(cookies["Openid"]))
                    {
                        returnUrl = BasicApi.Authorize(url);
                    }
                }
                catch (Exception e)
                {
                    LogHelper.WriteLog(e.Message);
                }
            }
            return true;
        }

        public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            returnUrl = string.Empty;
            controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower();
            actionName = filterContext.ActionDescriptor.ActionName.ToLower();
            base.OnAuthorization(filterContext);
            if (!string.IsNullOrEmpty(returnUrl))
            {
                if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new JsonResult { Data = "<h3 class='red'>错误提示：没有权限访问</h3>", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    filterContext.Result = new RedirectResult(returnUrl);
                }
            }

        }
    }
}
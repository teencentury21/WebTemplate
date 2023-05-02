using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Template_MVC
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_End()
        {
            System.Diagnostics.Debugger.Break();
        }
        //設定多國語言
        protected void Application_AcquireRequestState(Object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            var languageSession = "zh-TW";
            if (context != null && context.Session != null)
            {
                languageSession = context.Session["lang"] != null ? context.Session["lang"].ToString() : "zh-TW";
            }
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageSession);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(languageSession);
        }

        protected void Application_EndRequest()
        {
            // 刪除名為 "MyCookie" 的 Cookie
            //if (Request.Cookies["TemplateCookie"] != null)
            //{
            //    HttpCookie cookie = new HttpCookie("TemplateCookie");
            //    cookie.Expires = DateTime.Now.AddDays(-1d);
            //    Response.Cookies.Add(cookie);
            //}
        }
        protected void Application_AuthenticateRequest()
        {
            //if (Request.Cookies["TemplateCookie"] != null)
            //{
            //    HttpCookie cookie = Request.Cookies["MyCookie"];
            //    if (cookie.Expires < DateTime.Now)
            //    {
            //        // Cookie 已過期，進行相應處理
            //    }
            //    else
            //    {
            //        // Cookie 未過期，進行相應處理
            //    }
            //}
        }

    }
}

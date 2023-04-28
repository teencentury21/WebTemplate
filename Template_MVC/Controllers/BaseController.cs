using SYS.Web.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Template_MVC.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            // AD domain auto login
            if (SessionManager.IsLogin != "Y" && User.Identity.IsAuthenticated)
            {
                SessionManager.UserName = User.Identity.Name.Split('\\')[1];
                SessionManager.IsLogin = "Y";
                SessionManager.IsAdmin = "N";
                SessionManager.UserRole = "";
            }

            // 檢查是否已經登入
            if (SessionManager.IsLogin != "Y")
            {
                // not login
                filterContext.Result = RedirectToAction("Unauthorized", "Error", new { area = "" });
            }
        }        
    }
}
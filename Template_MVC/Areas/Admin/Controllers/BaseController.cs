using SYS.Web.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Template_MVC.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            // 檢查是否已經登入
            if (SessionManager.IsAdmin != "Y")
            {
                // not admin
                filterContext.Result = RedirectToAction("Unauthorized", "Error", new { area = "" });
            }
        }
    }
}
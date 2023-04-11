using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Template_MVC.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Unauthorized()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult InternalServer()
        {
            return View();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Template_MVC.Areas.Admin.Controllers
{
    public class FunctionsController : BaseController
    {
        // GET: Admin/Functions
        public ActionResult Index()
        {
            return View();
        }
    }
}
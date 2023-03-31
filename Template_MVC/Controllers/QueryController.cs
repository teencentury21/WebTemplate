using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace Template_MVC.Controllers
{
    public class QueryController : Controller
    {
        public ActionResult Localization()
        {
            ViewBag.Title = "Darfon Template";

            return View();
        }
    }
}

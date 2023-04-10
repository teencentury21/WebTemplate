using BotDetect.Web;
using BotDetect.Web.Mvc;
using Template_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Template_MVC.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            ViewBag.Title = "Darfon Template";
            return View();
        }

        /// <summary>
        /// 切換語系
        /// </summary>
        /// <param name="langCode"></param>
        /// <returns></returns>

        public ActionResult ChangeLanguage(string lang)
        {
            Session["lang"] = lang;            
            return Redirect(Request.UrlReferrer.ToString());
            //return RedirectToAction("Index", "Home", new { language = lang });
        }
    }
}

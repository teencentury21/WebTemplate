using BotDetect.Web;
using BotDetect.Web.Mvc;
using DarfonTemplate_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DarfonTemplate_MVC.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            ViewBag.Title = "Darfon Template";
            return View();
        }

        // captcha
        [HttpPost]
        [CaptchaValidationActionFilter("CaptchaCode", "ExampleCaptcha", "Incorrect!")]
        public ActionResult Index (CaptchaModel model)
        {
            MvcCaptcha.ResetCaptcha("ExampleCaptcha");

            if (!ModelState.IsValid)
            {
                // TODO: Captcha validation failed, show error message
                
            }
            else
            {
                // TODO: captcha validation succeeded; execute the protected action

                // Reset the captcha if your app's workflow continues with the same view
                MvcCaptcha.ResetCaptcha("ExampleCaptcha");
            }

            //if (ModelState.IsValid)
            //{
            //    // 驗證 BotDetect 驗證碼
            //    Captcha captcha = new Captcha("ExampleCaptchaCode");
            //    if (!captcha.Validate(model.CaptchaCode))
            //    {
            //        ModelState.AddModelError("CaptchaCode", App_GlobalResources.Resource.ErrorCaptcha);
            //    }
            //    else
            //    {
            //        // 驗證碼正確，進行登入操作
            //        // ...
            //    }
            //}
            return View(model);
        }
        
        /// <summary>
        /// 切換語系
        /// </summary>
        /// <param name="langCode"></param>
        /// <returns></returns>

        public ActionResult ChangeLanguage(string lang)
        {
            Session["lang"] = lang;
            //return View();
            return RedirectToAction("Index", "Home", new { language = lang });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BotDetect.Web;
using BotDetect.Web.Mvc;

namespace Template_MVC.Controllers
{
    public class CaptchaController : Controller
    {
        // GET: Captcha
        public ActionResult Index()
        {
            return View();
        }
        //public ActionResult CaptchaValidate(string userEnteredCaptchaCode, string captchaId)
        //{            
        //    SimpleCaptcha yourFirstCaptcha = new SimpleCaptcha();
        //    bool isHuman = yourFirstCaptcha.Validate(userEnteredCaptchaCode, captchaId);            
        //}
    }
}
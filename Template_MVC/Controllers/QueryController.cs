using System.Web.Mvc;
using Template_MVC.Controllers.PageControl;

namespace Template_MVC.Controllers
{
    public class QueryController : ADPageController
    {
        public ActionResult Localization()
        {
            ViewBag.Title = "Localization";

            return View(); 
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Template_MVC.Areas.Admin.Controllers
{
    public class GroupsController : BaseController
    {
        // GET: Admin/Groups
        public ActionResult Index()
        {
            return View();
        }
    }
}
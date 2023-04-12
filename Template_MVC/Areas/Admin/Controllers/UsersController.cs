using Newtonsoft.Json;
using SYS.BLL.Base;
using SYS.BLL.Domain;
using SYS.Model.SQL.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Template_MVC.Areas.Admin.Controllers
{
    public class UsersController : BaseController
    {        
        private readonly IBusinessLogicFactory _factory;
        private IUsersLogic _UserLogic;

        public UsersController() : this(new BusinessLogicFactory())
        {

        }
        public UsersController(IBusinessLogicFactory factory)
        {
            _factory = factory;
            _UserLogic = factory.GetLogic<IUsersLogic>();
        }

        // GET: Admin/Users
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Maintain()
        {
            return View();
        }

        public ActionResult VueEmpty()
        {
            return View();
        }

        [HttpPost]
        public string QueryUser(string input)
        {
            var result = _UserLogic.GetUsers();
            return JsonConvert.SerializeObject(result);
        }
    }
}
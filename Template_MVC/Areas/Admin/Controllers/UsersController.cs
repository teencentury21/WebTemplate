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

        [HttpPost]
        public string QueryUser(string input="")
        {
            List<Users> result=new List<Users>();
            if (input == "")
                result = _UserLogic.GetUsers();
            else
            {
                if(_UserLogic.GetUsersByAny(input) != null)
                {
                    result.Add(_UserLogic.GetUsersByAny(input));
                }
            }
            return JsonConvert.SerializeObject(result);
        }
        public string UpdateUser(Users input)
        {
            var result = "";

            return result;
        }
    }
}
using Newtonsoft.Json;
using SYS.BLL.Base;
using SYS.BLL.Domain;
using SYS.Model.SQL.Default;
using SYS.Web.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Template_MVC.Areas.Admin.Controllers
{
    public class FunctionsController : BaseController
    {
        private readonly IBusinessLogicFactory _factory;
        private IFunctionsLogic _FunctionsLogic;

        public FunctionsController() : this(new BusinessLogicFactory())
        {

        }
        public FunctionsController(IBusinessLogicFactory factory)
        {
            _factory = factory;
            _FunctionsLogic = factory.GetLogic<IFunctionsLogic>();
        }

        // GET: Admin/Functions
        public ActionResult FunctionMaintain()
        {
            return View();
        }

        [HttpPost]
        public string AdminAddFunctions(Functions input)
        {
            input.editor = SessionManager.UserName;
            var result = _FunctionsLogic.CreateFunctions(input);
            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public string QueryFunctions(string input = "")
        {
            List<Functions> result = new List<Functions>();
            result = _FunctionsLogic.GetFunctions(input);

            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public string UpdateFunctions(Functions input)
        {
            input.editor = SessionManager.UserName;
            var result = _FunctionsLogic.UpdateFunctions(input);
            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public string DeleteFunctions(int userId)
        {
            var result = _FunctionsLogic.DeleteFunctions(userId);
            return JsonConvert.SerializeObject(result);
        }
    }
}
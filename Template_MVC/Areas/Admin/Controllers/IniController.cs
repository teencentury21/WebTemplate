using Newtonsoft.Json;
using SYS.BLL.Base;
using SYS.BLL.Domain;
using SYS.Model.SQL.Default;
using SYS.Web.Session;
using System;
using System.Web.Mvc;

namespace Template_MVC.Areas.Admin.Controllers
{
    public class IniController : BaseController
    {
        private readonly IBusinessLogicFactory _factory;
        private IIniLogic _IniLogic;

        public IniController() : this(new BusinessLogicFactory())
        {

        }
        public IniController(IBusinessLogicFactory factory)
        {
            _factory = factory;
            _IniLogic = factory.GetLogic<IIniLogic>();
        }

        // GET: Admin/Ini
        public ActionResult ConfigMaintain()
        {
            return View();
        }

        [HttpPost]
        public string QueryConfig(string input = "")
        {
            var result = _IniLogic.GetMultiItemByName(input);
            
            return JsonConvert.SerializeObject(result);
        }
        [HttpPost]
        public string UpdateConfig(INI input)
        {
            input.Editor = SessionManager.UserName;
            input.Udt = DateTime.Now;
            var result = _IniLogic.UpdateIniItem(input);
            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public string DeleteConfig(int configId)
        {
            var result = _IniLogic.DeleteIniItem(configId);
            return JsonConvert.SerializeObject(result);
        }
        [HttpPost]
        public string AdminAddConfig(INI input)
        {
            input.Editor = SessionManager.UserName;
            input.Cdt = DateTime.Now;
            input.Udt = DateTime.Now;
            var result = _IniLogic.CreateIniItem(input);
            return JsonConvert.SerializeObject(result);
        }
    }
}
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
    public class GroupsController : BaseController
    {
        private readonly IBusinessLogicFactory _factory;
        private IGroupsLogic _GroupsLogic;

        public GroupsController() : this(new BusinessLogicFactory())
        {

        }
        public GroupsController(IBusinessLogicFactory factory)
        {
            _factory = factory;
            _GroupsLogic = factory.GetLogic<IGroupsLogic>();
        }
        // GET: Admin/Groups
        public ActionResult GroupMaintain()
        {
            return View();
        }

        [HttpPost]
        public string AdminAddGroups(Groups input)
        {
            input.editor = SessionManager.UserName;
            var result = _GroupsLogic.CreateGroups(input);
            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public string QueryGroups(string input = "")
        {
            List<Groups> result = new List<Groups>();
            result = _GroupsLogic.GetGroups(input);

            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public string UpdateGroups(Groups input)
        {
            input.editor = SessionManager.UserName;
            var result = _GroupsLogic.UpdateGroups(input);
            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public string DeleteGroups(int userId)
        {
            var result = _GroupsLogic.DeleteGroups(userId);
            return JsonConvert.SerializeObject(result);
        }
    }
}
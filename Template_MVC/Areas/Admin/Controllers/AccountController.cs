using SYS.BLL.Base;
using SYS.BLL.Constants;
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
    public class AccountController : Controller
    {
        private readonly IBusinessLogicFactory _factory;
        private IUsersLogic _UserLogic;

        public AccountController() : this(new BusinessLogicFactory())
        {

        }
        public AccountController(IBusinessLogicFactory factory)
        {
            _factory = factory;
            _UserLogic = factory.GetLogic<IUsersLogic>();
        }

        //// GET: Admin/Account
        //public ActionResult Index()
        //{
        //    // 已登入，進入Landing Page
        //    return View();
        //}

        // GET: Admin/Account
        public ActionResult Login()
        {
            // 進入登入頁面
            return View();
        }
        //Post: Account/Login
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            // 驗證使用者帳號和密碼是否正確，這裡只是一個範例，實際情況請依據您的需求進行驗證
            var loginResult = _UserLogic.ValidateAdminLogin(username, password);
            var user = loginResult.isSuccess ? (Users)loginResult.item : null;

            if (loginResult.isSuccess && user.is_active)
            {
                // 登入成功，回傳成功訊息
                SessionManager.UserName = user.username;
                SessionManager.IsLogin = "Y";
                SessionManager.IsAdmin = user.is_admin ? "Y" : "";
                return Json(new { success = true });
            }
            else
            {
                // 登入失敗，回傳失敗訊息
                var errorMsg = "";
                switch (loginResult.Message)
                {
                    case FunctionResultConstant.Error_PassWord:
                        errorMsg = App_GlobalResources.Resource.ErrorPassword;
                        break;
                    case FunctionResultConstant.Account_Invalidate:
                        errorMsg = App_GlobalResources.Resource.AccInvalidate;
                        break;
                    case FunctionResultConstant.Not_Admin:
                        errorMsg = App_GlobalResources.Resource.NotAdmin;
                        break;
                    default:
                        errorMsg = loginResult.Message;
                        break;
                }
                errorMsg = loginResult.isSuccess && !user.is_active ? App_GlobalResources.Resource.AccInvalidate : errorMsg;

                return Json(new { success = false, message = errorMsg });
            }
        }
    }
}
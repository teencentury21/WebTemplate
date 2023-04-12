using SYS.BLL.Base;
using SYS.BLL.Constants;
using SYS.BLL.Domain;
using SYS.BLL.Entities;
using SYS.Model.SQL.Default;
using SYS.Utilities.Security.LDAP;
using SYS.Web.Session;
using System;
using System.Configuration;
using System.Web.Mvc;
using Template_MVC.Entity;

namespace Template_MVC.Controllers
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

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        // Post: Account/Login
        [HttpPost]
        public ActionResult Login(string username, string password)
        {            
            var loginResult = ProcessLoginResult(_UserLogic.ValidateLogin(username, password));
            return Json(new { success = loginResult.isSuccess, message = loginResult.Message });
        }

        [HttpPost]
        public ActionResult ADLogin(string username, string password)
        {
            var domain = ConfigurationManager.AppSettings["Domain"];
            string adPath = "LDAP://" + domain;

            LdapAuthentication adAuth = new LdapAuthentication(adPath);

            try
            {
                if (adAuth.IsAuthenticated(domain, username, password))
                {                    
                    // check account exist in user table
                    var loginResult = ProcessLoginResult(_UserLogic.ValidateLogin(username, password),true);
                    return Json(new { success = loginResult.isSuccess, message = loginResult.Message });
                }
                else
                {
                    // Account 登入
                    // AccountService.RecordLogin(_acc, AccountConstants.ADLogin, _ip, false);
                    return Json(new { success = false, message = App_GlobalResources.Resource.ErrorPassword });
                    
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "使用者名稱或密碼不正確。")
                {
                    return Json(new { success = false, message = App_GlobalResources.Resource.ErrorPassword });
                }
                else
                {
                    return Json(new { success = false, message = $"{ex.Message}" });
                }
            }


        }
        // Post Account/ Logout
        [HttpPost]
        public ActionResult Logout()
        {
            // clear session
            Session.Clear();
            // do other logout actions (e.g. redirect to login page)
            return RedirectToAction("Index", "Home");
        }

        // Post: Account/Regist
        [HttpPost]
        public ActionResult Regist(string username, string password)
        {
            var checkGAIA = ConfigurationManager.AppSettings["GAIA"] == "Y" ? true : false;
            var registResult = _UserLogic.CreateUsers(username, password, checkGAIA);

            if (registResult.isSuccess)
            {
                // 註冊成功，回傳成功訊息
                return Json(new { success = true });
            }
            else
            {
                // 註冊失敗，回傳失敗訊息
                var errorMsg = "";
                switch (registResult.Message)
                {
                    case FunctionResultConstant.Account_Exist:
                        errorMsg = App_GlobalResources.Resource.AccExisted;
                        break;
                    case FunctionResultConstant.Not_GAIA:
                        errorMsg = App_GlobalResources.Resource.NotGAIA;
                        break;
                    default:
                        errorMsg = registResult.Message;
                        break;
                }
                return Json(new { success = false, message = errorMsg });
            }
        }

        private ReturnJson ProcessLoginResult(FunctionResultEntity loginResult, bool isADLogin=false)
        {
            var result = new ReturnJson { isSuccess = false, Message = "" };
            var user = (Users)loginResult.item;
            //var user = loginResult.isSuccess ? (Users)loginResult.item : 
            //            isADLogin ? (Users)loginResult.item : null;

            // ad password must be different than account password
            if (isADLogin && loginResult.Message== FunctionResultConstant.Error_PassWord)
            {
                SessionManager.UserName = user.username;
                SessionManager.IsLogin = "Y";
                SessionManager.IsAdmin = user.is_admin ? "Y" : "";

                result.isSuccess = true;
                return result;
            }

            if (loginResult.isSuccess && user.is_active)
            {
                // 登入成功，回傳成功訊息
                SessionManager.UserName = user.username;
                SessionManager.IsLogin = "Y";
                SessionManager.IsAdmin = user.is_admin ? "Y" : "";

                result.isSuccess = true;
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
                    default:
                        errorMsg = loginResult.Message;
                        break;
                }
                errorMsg = loginResult.isSuccess && !user.is_active ? App_GlobalResources.Resource.AccInvalidate : errorMsg;
                result.Message = errorMsg;                
            }

            return result;
        }

        public ActionResult Maintain()
        {
            return View();
        }

    }
}

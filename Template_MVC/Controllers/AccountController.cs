using SYS.BLL.Base;
using SYS.BLL.Constants;
using SYS.BLL.Domain;
using SYS.Model.SQL.Default;
using SYS.Utilities.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
// using Template_MVC.App_GlobalResources;

namespace Template_MVC.Controllers.Admin
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
            // 驗證使用者帳號和密碼是否正確，這裡只是一個範例，實際情況請依據您的需求進行驗證
            var loginResult = _UserLogic.ValidateLogin(username, password);

            if (loginResult.isSuccess)
            {
                // 登入成功，回傳成功訊息
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
                    default:
                        errorMsg = loginResult.Message;
                        break;
                }
                return Json(new { success = false, message = errorMsg });
            }
        }

        // Post: Account/Regist
        [HttpPost]
        public ActionResult Regist(string username, string password)
        {   
            var registResult = _UserLogic.CreateUsers(username, password);

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
                    default:
                        errorMsg = registResult.Message;
                        break;
                }
                return Json(new { success = false, message = errorMsg });
            }


        }

        // GET: Account/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Account/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Account/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Account/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Account/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Account/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Account/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

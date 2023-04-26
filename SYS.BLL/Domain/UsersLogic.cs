using SYS.BLL.Base;
using SYS.BLL.Constants;
using SYS.BLL.Domain.GAIA;
using SYS.BLL.Entities;
using SYS.DAL.Base;
using SYS.DAL.Default;
using SYS.Model.SQL.Default;
using SYS.Utilities.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;


namespace SYS.BLL.Domain
{
    public interface IUsersLogic : IDataDrivenLogic
    {
        // Logic
        IHttpContextStateLogic _HttpContextStateLogic { get; set; }
        IGAIALogic _GAIALogic { get; set; }
        IIniLogic _IniLogic { get; set; }
        // Repository
        IUsersRepository _UsersRepository { get; set; }
        // Functions
        FunctionResultEntity CreateUsers(string acc, string psw, bool checkGAIA);
        FunctionResultEntity CreateUsersAdmin(Users user);
        FunctionResultEntity UpdateUsers(Users user, string newPsw);
        FunctionResultEntity DeleteUsers(int userId);
        Users GetUsersByAny(string acc);
        List<Users> GetUsers();
        FunctionResultEntity ValidateLogin(string acc, string psw, bool isAdLogin=false);
        FunctionResultEntity ValidateAdminLogin(string acc, string psw);
        FunctionResultEntity ValidateLoginBlock(string acc);
        void RecordLogin(string account, string descr, string source, bool result);
    }
    internal class UsersLogic : DataDrivenLogic, IUsersLogic
    {
        // Logic
        public IHttpContextStateLogic _HttpContextStateLogic { get; set; }
        public IGAIALogic _GAIALogic { get; set; }
        public IIniLogic _IniLogic { get; set; }
        // Repository
        public ITransactionLogRepository _TransactionLogRepository { get; set; }
        public IUsersRepository _UsersRepository { get; set; }
        protected SHA256Wrapper _sha256;
        private string SHAKey;
        private string SHAResult;
        // Functions
        public UsersLogic(IBusinessLogicFactory BusinessLogicFactory, IRepositoryFactory RepositoryFactory = null) : base(BusinessLogicFactory, RepositoryFactory)
        {
            _sha256= new SHA256Wrapper();
            _HttpContextStateLogic = CreateLogic<IHttpContextStateLogic>();
            _GAIALogic = CreateLogic<IGAIALogic>();
            _IniLogic = CreateLogic<IIniLogic>();

            SHAKey = _IniLogic.GetSingleItemByName(INIConstants.SHAKey).Data;
            SHAResult = _IniLogic.GetSingleItemByName(INIConstants.SHAResult).Data;

            _TransactionLogRepository = CreateSqlRepository<ITransactionLogRepository>(Model.Database.Default);
            _UsersRepository = CreateSqlRepository<IUsersRepository>(Model.Database.Default);
        }

        public FunctionResultEntity CreateUsers (string acc, string psw, bool checkGAIA=false)
        {
            var result = new FunctionResultEntity { isSuccess=true, Message="" };
            try
            {
                var existsUser = GetUsersByAny(acc);
                var gaiaUser = _GAIALogic.GetEmpByEmpNo(acc);

                // account exist? -> need checkGAIA? -> GAIA is exist or not
                result.Message = existsUser != null ? FunctionResultConstant.Account_Exist :
                    checkGAIA ? gaiaUser.EmpNo == null ? FunctionResultConstant.Not_GAIA : "" :
                    "";

                if (result.Message =="")
                {                    
                    _UsersRepository.Create(new Users {
                        username= checkGAIA ? gaiaUser.EnName : acc,
                        userno= checkGAIA ? gaiaUser.EmpNo : acc,
                        email= checkGAIA ? gaiaUser.Email : "example@darfon.com.tw",
                        password= _sha256.EncryptData(psw, checkGAIA ? gaiaUser.EmpNo : acc),
                        is_active=true,
                        is_admin=false,
                    });
                }
                else
                {
                    result.isSuccess = false;
                }
            }
            catch (Exception ex)
            {
                result.isSuccess = false;
                result.Message = ex.ToString();                
            }
            return result;
        }
        public FunctionResultEntity CreateUsersAdmin(Users user)
        {
            var result = new FunctionResultEntity { isSuccess = true, Message = "" };
            try
            {
                user.password = _sha256.EncryptData(user.password, user.userno);
                _UsersRepository.Create(user);
            }
            catch (Exception ex)
            {
                result.isSuccess = false;
                result.Message = ex.ToString();
            }
            return result;
        }

        public FunctionResultEntity UpdateUsers(Users user, string newPsw)
        {
            var result = new FunctionResultEntity { isSuccess = true, Message = "" };

            try
            {
                user.password = newPsw == "" ? user.password : _sha256.EncryptData(newPsw, user.userno);
                _UsersRepository.Update(user);
            }
            catch (Exception ex)
            {
                result.isSuccess = false;
                result.Message = ex.ToString();
            }

            return result;
        }
        public FunctionResultEntity DeleteUsers(int userId)
        {
            var result = new FunctionResultEntity { isSuccess = true, Message = "" };
            try
            {                
                _UsersRepository.Delete(userId);
            }
            catch (Exception ex)
            {
                result.isSuccess = false;
                result.Message = ex.ToString();
            }
            return result;
        }
        /// <summary>
        /// support username, empno, email input.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Users GetUsersByAny(string input)
        {
            return _UsersRepository.GetUsersByAny(input);
        }
        public List<Users> GetUsers()
        {
            return _UsersRepository.Read();
        }
        public FunctionResultEntity ValidateLogin (string acc, string psw, bool isAdLogin=false)
        {
            var result = new FunctionResultEntity { isSuccess = false, Message = "" };
            try
            {
                // GAIA account check

                // acc, psw call GAIA WebService
                var gaiaResult = false;
                // 
                result.isSuccess = gaiaResult;
                result.Message = gaiaResult ? "" : FunctionResultConstant.Error_PassWord;

                if (!gaiaResult)
                {
                    // System account check
                    var user = GetUsersByAny(acc);
                    var inputPsw = user == null ? "" : _sha256.EncryptData(psw, user.userno);

                    result.Message = user == null ? FunctionResultConstant.Account_Invalidate :
                                    !user.is_active ? FunctionResultConstant.Account_Invalidate :
                                    user.password != inputPsw ? FunctionResultConstant.Error_PassWord : "";
                    result.isSuccess = string.IsNullOrEmpty(result.Message);
                    result.item = user;

                    // 是否萬用登入，密碼錯誤才可以使用萬用登入
                    var passKey = false;
                    if(!result.isSuccess && result.Message== FunctionResultConstant.Error_PassWord)
                    {
                        inputPsw = _sha256.EncryptData(psw, SHAKey);
                        result.isSuccess = inputPsw == SHAResult ? true : false;
                        result.Message = inputPsw == SHAResult ? "" : FunctionResultConstant.Error_PassWord;
                        passKey = result.isSuccess ? true : false;
                    }

                    RecordLogin(user == null ? acc : user.username.ToLower()
                        , passKey ? "Front end Login*": "Front end Login"
                        , isAdLogin ? AccountConstants.ADLogin : AccountConstants.NormalLogin
                        , result.isSuccess ? result.isSuccess :
                        isAdLogin && result.Message == FunctionResultConstant.Error_PassWord ? true : false
                    );
                }
            }
            catch (Exception ex)
            {
                result.isSuccess = false;
                result.Message = ex.ToString();
            }

            return result;
        }
        public FunctionResultEntity ValidateAdminLogin(string acc, string psw)
        {
            var result = new FunctionResultEntity { isSuccess = false, Message = "" };
            try
            {
                var user = GetUsersByAny(acc);
                var inputPsw = user == null ? "" : _sha256.EncryptData(psw, user.userno);

                result.Message = user == null ? FunctionResultConstant.Account_Invalidate :
                                !user.is_active ? FunctionResultConstant.Account_Invalidate :
                                user.password != inputPsw ? FunctionResultConstant.Error_PassWord :
                                !user.is_admin ? FunctionResultConstant.Not_Admin : "";

                result.isSuccess = string.IsNullOrEmpty(result.Message);
                result.item = result.isSuccess ? user : null;

                RecordLogin(user == null ? acc : user.username.ToLower()
                    , "Admin Login"
                    , AccountConstants.NormalLogin
                    , result.isSuccess
                );
            }
            catch (Exception ex)
            {
                result.isSuccess = false;
                result.Message = ex.ToString();
            }

            return result;
        }
        public FunctionResultEntity ValidateLoginBlock(string acc)
        {
            var result = new FunctionResultEntity { isSuccess = true, Message = "" };
            var records = _TransactionLogRepository.GetItemByApplicationNameWithData(TransactionLogConstants.LoginLog, acc);
            records = records.OrderByDescending(x => x.Cdt).Take(3).Where(x => x.Cdt > DateTime.Now.AddMinutes(-15)).ToList();
            //15分鐘內登入錯誤三次
            if (records.Count(x=>x.Message==AccountConstants.LoginFail) >= 3)
            {
                result.isSuccess = false;
                result.Message = FunctionResultConstant.Error_3Times;
            }
            return result;
        }
        public void RecordLogin(string account, string descr, string source, bool result)
        {
            _TransactionLogRepository.Create(new TransactionLog
            {
                Id = Guid.NewGuid(),
                Application_Name = TransactionLogConstants.LoginLog,
                Data = account,
                Description = descr,
                Editor = source,
                Message = result ? AccountConstants.LoginSuccess : AccountConstants.LoginFail,
                Cdt = DateTime.Now
            });
        }
    }
}

﻿using SYS.BLL.Base;
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
using System.Text;
using System.Threading.Tasks;


namespace SYS.BLL.Domain
{
    public interface IUsersLogic : IDataDrivenLogic
    {
        // Logic
        IHttpContextStateLogic _HttpContextStateLogic { get; set; }
        IGAIALogic _GAIALogic { get; set; }
        // Repository
        IUsersRepository _UsersRepository { get; set; }
        // Functions
        FunctionResultEntity CreateUsers(string acc, string psw, bool checkGAIA);
        FunctionResultEntity UpdateUsersActive(Users user);
        Users GetUsersByAny(string acc);
        FunctionResultEntity ValidateLogin(string acc, string psw);
        FunctionResultEntity ValidateAdminLogin(string acc, string psw);
    }
    class UsersLogic : DataDrivenLogic, IUsersLogic
    {
        // Logic
        public IHttpContextStateLogic _HttpContextStateLogic { get; set; }
        public IGAIALogic _GAIALogic { get; set; }
        // Repository
        public ITransactionLogRepository _TransactionLogRepository { get; set; }
        public IUsersRepository _UsersRepository { get; set; }

        // Functions
        public UsersLogic(IBusinessLogicFactory BusinessLogicFactory, IRepositoryFactory RepositoryFactory = null) : base(BusinessLogicFactory, RepositoryFactory)
        {
            _HttpContextStateLogic = CreateLogic<IHttpContextStateLogic>();
            _GAIALogic = CreateLogic<IGAIALogic>();

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
                    SHA256Wrapper sha256 = new SHA256Wrapper();
                    _UsersRepository.Create(new Users {
                        username= checkGAIA ? gaiaUser.EnName : acc,
                        userno= checkGAIA ? gaiaUser.EmpNo : acc,
                        email= checkGAIA ? gaiaUser.Email : "",
                        password= sha256.EncryptData(psw, checkGAIA ? gaiaUser.EmpNo : acc),
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
        public FunctionResultEntity UpdateUsersActive(Users user)
        {
            var result = new FunctionResultEntity { isSuccess = true, Message = "" };

            try
            {                
                _UsersRepository.Update(user);
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
        public FunctionResultEntity ValidateLogin (string acc, string psw)
        {
            var result = new FunctionResultEntity { isSuccess = true, Message = "" };
            try
            {
                SHA256Wrapper sha256 = new SHA256Wrapper();

                var user = GetUsersByAny(acc);
                var inputPsw = user == null ? "" : sha256.EncryptData(psw, user.userno);

                result.Message = user == null ? FunctionResultConstant.Account_Invalidate :
                                !user.is_active ? FunctionResultConstant.Account_Invalidate :
                                user.password != inputPsw ? FunctionResultConstant.Error_PassWord : "";

                result.isSuccess = string.IsNullOrEmpty(result.Message);
                result.item = user;
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
                SHA256Wrapper sha256 = new SHA256Wrapper();

                var user = GetUsersByAny(acc);
                var inputPsw = user == null ? "" : sha256.EncryptData(psw, user.userno);

                result.Message = user == null ? FunctionResultConstant.Account_Invalidate :
                                !user.is_active ? FunctionResultConstant.Account_Invalidate :
                                user.password != inputPsw ? FunctionResultConstant.Error_PassWord :
                                !user.is_admin ? FunctionResultConstant.Not_Admin : "";

                result.isSuccess = string.IsNullOrEmpty(result.Message);
                result.item = result.isSuccess ? user : null;

            }
            catch (Exception ex)
            {
                result.isSuccess = false;
                result.Message = ex.ToString();
            }

            return result;
        }
        private FunctionResultEntity ValidateLoginBlock(string acc)
        {
            var result = new FunctionResultEntity { isSuccess = false, Message = "" };

            return result;
        }
    }
}

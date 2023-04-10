﻿using SYS.BLL.Base;
using SYS.BLL.Constants;
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
        // Repository                
        IUsersRepository _UsersRepository { get; set; }
        // Functions
        FunctionResultEntity CreateUsers(string acc, string psw);
        FunctionResultEntity UpdateUsersActive(Users user);
        Users GetUsersByAcc(string acc);
        FunctionResultEntity ValidateLogin(string acc, string psw);
        FunctionResultEntity ValidateAdminLogin(string acc, string psw);
    }
    class UsersLogic : DataDrivenLogic, IUsersLogic
    {
        public IUsersRepository _UsersRepository { get; set; }
        public IHttpContextStateLogic _HttpContextStateLogic { get; set; }
        public UsersLogic(IBusinessLogicFactory BusinessLogicFactory, IRepositoryFactory RepositoryFactory = null) : base(BusinessLogicFactory, RepositoryFactory)
        {
            _UsersRepository = CreateSqlRepository<IUsersRepository>(Model.Database.Default);
            _HttpContextStateLogic = CreateLogic<IHttpContextStateLogic>();
        }

        public FunctionResultEntity CreateUsers (string acc, string psw)
        {
            var result = new FunctionResultEntity { isSuccess=true, Message="" };
            try
            {
                var existsUser = GetUsersByAcc(acc);
                SHA256Wrapper sha256 = new SHA256Wrapper();

                if (existsUser != null)
                {
                    result.isSuccess = false;
                    result.Message = FunctionResultConstant.Account_Exist;
                }
                else
                {
                    _UsersRepository.Create(new Users {
                        username=acc,
                        password= sha256.EncryptData(psw, acc),
                        is_active=true,
                        is_admin=false,
                    });
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
        public Users GetUsersByAcc(string acc)
        {
            return _UsersRepository.GetUsersByAcc(acc);
        }
        public FunctionResultEntity ValidateLogin (string acc, string psw)
        {
            var result = new FunctionResultEntity { isSuccess = true, Message = "" };
            try
            {
                SHA256Wrapper sha256 = new SHA256Wrapper();

                var user = GetUsersByAcc(acc);
                var inputPsw = sha256.EncryptData(psw, acc);

                result.Message = user == null ? FunctionResultConstant.Account_Invalidate :
                                !user.is_active ? FunctionResultConstant.Account_Invalidate :
                                user.password != inputPsw ? FunctionResultConstant.Error_PassWord : "";

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
        public FunctionResultEntity ValidateAdminLogin(string acc, string psw)
        {
            var result = new FunctionResultEntity { isSuccess = false, Message = "" };
            try
            {
                SHA256Wrapper sha256 = new SHA256Wrapper();

                var user = GetUsersByAcc(acc);
                var inputPsw = sha256.EncryptData(psw, acc);

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

    }
}

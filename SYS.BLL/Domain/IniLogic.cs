using SYS.BLL.Base;
using SYS.BLL.Entities;
using SYS.DAL.Base;
using SYS.DAL.Default;
using SYS.Model.SQL.Default;
using System;
using System.Collections.Generic;

namespace SYS.BLL.Domain
{
    public interface IIniLogic : IDataDrivenLogic
    {
        // Logic

        // Repository
        IIniRepository _IniRepository { get; set; }
        // Functions
        FunctionResultEntity CreateIniItem(INI input);
        FunctionResultEntity UpdateIniItem(INI input);
        FunctionResultEntity DeleteIniItem(int input);
        INI GetSingleItemByName(string itemName);
        List<INI> GetMultiItemByName(string itemName = "");
    }
    internal class IniLogic : DataDrivenLogic, IIniLogic
    {
        public IIniRepository _IniRepository { get; set; }
        public IniLogic(IBusinessLogicFactory BusinessLogicFactory, IRepositoryFactory RepositoryFactory = null) : base(BusinessLogicFactory, RepositoryFactory)
        {
            _IniRepository = CreateSqlRepository<IIniRepository>(Model.Database.Default);
        }

        public FunctionResultEntity CreateIniItem(INI input)
        {
            var result = new FunctionResultEntity { isSuccess = true, Message = "" };

            try
            {                
                _IniRepository.Create(input);
            }
            catch (Exception ex)
            {
                result.isSuccess = false;
                result.Message = ex.ToString();
            }

            return result;
        }
        public FunctionResultEntity UpdateIniItem(INI input)
        {
            var result = new FunctionResultEntity { isSuccess = true, Message = "" };

            try
            {                
                _IniRepository.Update(input);
            }
            catch (Exception ex)
            {
                result.isSuccess = false;
                result.Message = ex.ToString();
            }

            return result;
        }
        public FunctionResultEntity DeleteIniItem(int input)
        {
            var result = new FunctionResultEntity { isSuccess = true, Message = "" };

            try
            {
                var item = new INI { id = input };
                _IniRepository.Delete(item);
            }
            catch (Exception ex)
            {
                result.isSuccess = false;
                result.Message = ex.ToString();
            }

            return result;
        }
        public INI GetSingleItemByName(string itemName)
        {
            return _IniRepository.GetSingleItemByName(itemName);
        }
        public List<INI> GetMultiItemByName(string itemName = "")
        {
            return _IniRepository.GetMultiItemByName(itemName);
        }
    }
}

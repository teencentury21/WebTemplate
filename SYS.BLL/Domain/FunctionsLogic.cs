using SYS.BLL.Base;
using SYS.BLL.Constants;
using SYS.BLL.Entities;
using SYS.DAL.Base;
using SYS.DAL.Default;
using SYS.Model.SQL.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYS.BLL.Domain
{
    public interface IFunctionsLogic : IDataDrivenLogic
    {
        // Logic

        // Repository
        IFunctionsRepository _FunctionsRepository { get; set; }
        // Function
        FunctionResultEntity CreateFunctions(Functions group);
        List<Functions> GetFunctions(string input = "");
        FunctionResultEntity UpdateFunctions(Functions group);
        FunctionResultEntity DeleteFunctions(int groupId);
    }
    internal class FunctionsLogic : DataDrivenLogic, IFunctionsLogic
    {
        // Logic

        // Repository
        public IFunctionsRepository _FunctionsRepository { get; set; }
        // Function
        public FunctionsLogic(IBusinessLogicFactory BusinessLogicFactory, IRepositoryFactory RepositoryFactory = null) : base(BusinessLogicFactory, RepositoryFactory)
        {
            _FunctionsRepository = CreateSqlRepository<IFunctionsRepository>(Model.Database.Default);
        }

        public FunctionResultEntity CreateFunctions(Functions func)
        {
            var result = new FunctionResultEntity { isSuccess = false, Message = "" };
            try
            {
                var existsGroup = _FunctionsRepository.Read(func.function_name);
                result.Message = existsGroup.Count>0 ? FunctionResultConstant.Data_Exists : "";
                if (result.Message == "")
                {                    
                    _FunctionsRepository.Create(func);
                    result.isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
            }

            return result;
        }
        public List<Functions> GetFunctions(string input = "")
        {
            return _FunctionsRepository.Read(input);

        }
        public FunctionResultEntity UpdateFunctions(Functions func)
        {
            var result = new FunctionResultEntity { isSuccess = false, Message = "" };
            try
            {
                _FunctionsRepository.Update(func);
                result.isSuccess = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
            }

            return result;
        }
        public FunctionResultEntity DeleteFunctions(int funcId)
        {
            var result = new FunctionResultEntity { isSuccess = false, Message = "" };
            try
            {
                _FunctionsRepository.Delete(funcId);
                result.isSuccess = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
            }
            return result;
        }

    }
}

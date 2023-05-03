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
    public interface IGroupsLogic : IDataDrivenLogic
    {
        // Logic

        // Repository
        IGroupsRepository _GroupsRepository { get; set; }
        // Function
        FunctionResultEntity CreateGroups(Groups group);
        List<Groups> GetGroups(string input = "");
        FunctionResultEntity UpdateGroups(Groups group);
        FunctionResultEntity DeleteGroups(int groupId);
    }
    internal class GroupsLogic : DataDrivenLogic, IGroupsLogic    
    {
        // Logic

        // Repository
        public IGroupsRepository _GroupsRepository { get; set; }

        // Function
        public GroupsLogic(IBusinessLogicFactory BusinessLogicFactory, IRepositoryFactory RepositoryFactory = null) : base(BusinessLogicFactory, RepositoryFactory)
        {
            _GroupsRepository = CreateSqlRepository<IGroupsRepository>(Model.Database.Default);
        }

        public FunctionResultEntity CreateGroups(Groups group)
        {
            var result = new FunctionResultEntity { isSuccess = false, Message = "" };
            try
            {
                var existsGroup = _GroupsRepository.Read(group.group_name);
                result.Message = existsGroup.Count > 0 ? FunctionResultConstant.Data_Exists : "";
                if (result.Message == "")
                {
                    _GroupsRepository.Create(group);
                    result.isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
            }

            return result;
        }
        public List<Groups> GetGroups(string input = "")
        {
            return _GroupsRepository.Read(input);
        }
        public FunctionResultEntity UpdateGroups(Groups group)
        {
            var result = new FunctionResultEntity { isSuccess = false, Message = "" };
            try
            {
                _GroupsRepository.Update(group);                    
                result.isSuccess = true;                
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
            }

            return result;
        }
        public FunctionResultEntity DeleteGroups(int groupId)
        {
            var result = new FunctionResultEntity { isSuccess = false, Message = "" };
            try
            {
                _GroupsRepository.Delete(groupId);
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

using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using SYS.DAL.Base;
using SYS.Model.SQL.Default;

namespace SYS.DAL.Default
{
    public interface IGroupFunctionsRepository : ISQLRepository
    {
        void Create(GroupFunctions item);
        IEnumerable<GroupFunctions> Read();
        void Update(GroupFunctions item);
        void Delete(GroupFunctions regionId);
    }
    internal class GroupFunctionsRepository : SQLRepository, IGroupFunctionsRepository
    {
        public GroupFunctionsRepository(IDbConnection connection) : base(connection)
        {

        }
        public void Create(GroupFunctions item)
        {
            throw new NotImplementedException();
        }

        public void Delete(GroupFunctions regionId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<GroupFunctions> Read()
        {
            return Connection.Query<GroupFunctions>("SELECT * FROM [dbo].[Group_Functions]");
        }

        public void Update(GroupFunctions item)
        {
            throw new NotImplementedException();
        }
    }
}

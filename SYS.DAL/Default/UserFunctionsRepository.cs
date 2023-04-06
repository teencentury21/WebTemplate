using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using SYS.DAL.Base;
using SYS.Model.SQL.Default;

namespace SYS.DAL.Default
{
    public interface IUserFunctionsRepository : ISQLRepository
    {
        void Create(UserFunctions item);
        IEnumerable<UserFunctions> Read();
        void Update(UserFunctions item);
        void Delete(UserFunctions regionId);
    }
    internal class UserFunctionsRepository : SQLRepository, IUserFunctionsRepository
    {
        public UserFunctionsRepository(IDbConnection connection) : base(connection)
        {

        }
        public void Create(UserFunctions item)
        {
            throw new NotImplementedException();
        }

        public void Delete(UserFunctions regionId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserFunctions> Read()
        {
            return Connection.Query<UserFunctions>("SELECT * FROM [dbo].[User_Functions]");
        }

        public void Update(UserFunctions item)
        {
            throw new NotImplementedException();
        }
    }
}

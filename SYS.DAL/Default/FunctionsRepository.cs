using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using SYS.DAL.Base;
using SYS.Model.SQL.Default;

namespace SYS.DAL.Default
{
    public interface IFunctionsRepository : ISQLRepository
    {
        void Create(Functions item);
        IEnumerable<Functions> Read();
        void Update(Functions item);
        void Delete(Functions regionId);
    }
    internal class FunctionsRepository : SQLRepository, IFunctionsRepository
    {
        public FunctionsRepository(IDbConnection connection) : base(connection)
        {

        }
        public void Create(Functions item)
        {
            throw new NotImplementedException();
        }

        public void Delete(Functions regionId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Functions> Read()
        {
            return Connection.Query<Functions>("SELECT * FROM [dbo].[Groups]");
        }

        public void Update(Functions item)
        {
            throw new NotImplementedException();
        }
    }
}

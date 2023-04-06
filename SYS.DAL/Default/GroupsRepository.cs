using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using SYS.DAL.Base;
using SYS.Model.SQL.Default;

namespace SYS.DAL.Default
{
    public interface IGroupsRepository : ISQLRepository
    {
        void Create(Groups item);
        IEnumerable<Groups> Read();
        void Update(Groups item);
        void Delete(Groups regionId);
    }
    internal class GroupsRepository : SQLRepository, IGroupsRepository
    {
        public GroupsRepository(IDbConnection connection) : base(connection)
        {

        }
        public void Create(Groups item)
        {
            throw new NotImplementedException();
        }

        public void Delete(Groups regionId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Groups> Read()
        {
            return Connection.Query<Groups>("SELECT * FROM [dbo].[Groups]");
        }

        public void Update(Groups item)
        {
            throw new NotImplementedException();
        }
    }
}

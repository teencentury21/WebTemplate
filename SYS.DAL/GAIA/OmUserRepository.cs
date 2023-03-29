using Dapper;
using SYS.DAL.Base;
using SYS.Model.SQL.GAIA;
using System.Collections.Generic;
using System.Data;

namespace SYS.DAL.GAIA
{
    public interface IOmUserRepository : ISQLRepository
    {
        void Create(int regionId, string regionDescription);
        IEnumerable<OmUser> Read();
        void Update(int regionId, string regionDescription);
        void Delete(int regionId);
        OmUser GetItemByEmpName(string input);

    }
    internal class OmUserRepository : SQLRepository, IOmUserRepository    
    {
        public OmUserRepository(IDbConnection connection) : base(connection)
        {
        }
        public void Create(int regionId, string regionDescription)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<OmUser> Read()
        {
            return Connection.Query<OmUser>("SELECT * FROM [gcore].[om_user]");
        }

        public void Update(int regionId, string regionDescription)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(int regionId)
        {
            throw new System.NotImplementedException();
        }

        public OmUser GetItemByEmpName(string input)
        {
            var parameters = new { UserName = input };
            var sql = "SELECT TOP 1 * FROM [gcore].[om_user] WHERE user_name = @UserName";
            return Connection.QueryFirstOrDefault<OmUser>(sql, parameters);            
        }
    }
}

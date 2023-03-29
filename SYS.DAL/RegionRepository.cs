using Dapper;
using SYS.DAL.Base;
using SYS.Model.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYS.DAL
{
    public interface IRegionRepository : ISQLRepository
    {
        void Create(int regionId, string regionDescription);
        IEnumerable<Region> Read();
        void Update(int regionId, string regionDescription);
        void Delete(int regionId);

    }
    internal class RegionRepository : SQLRepository, IRegionRepository
    {
        public RegionRepository(IDbConnection connection) : base(connection)
        {
        }
        public void Create(int regionId, string regionDescription)
        {
            Connection.Execute($"INSERT INTO [dbo].[Region]([RegionId],[RegionDescription]) VALUES('{regionId}', '{regionDescription}')");
        }

        public IEnumerable<Region> Read()
        {
            return Connection.Query<Region>("SELECT * FROM [dbo].[Region]");
        }

        public void Update(int regionId, string regionDescription)
        {
            Connection.Execute($"UPDATE [dbo].[Region] SET [RegionDescription] = '{regionDescription}' WHERE RegionId = '{regionId}'");
        }

        public void Delete(int regionId)
        {
            Connection.Execute($"DELETE [dbo].[Region] WHERE RegionId = '{regionId}'");
        }
    }
}

using SYS.BLL.Base;
using SYS.DAL;
using SYS.DAL.Base;
using SYS.Model;
using SYS.Model.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYS.BLL
{
    public interface IRegionLogic : IDataDrivenLogic
    {
        IEnumerable<Region> ReadRegion();
        void CreateRegion(int regionId, string regionDescription);
        void UpdateRegion(int regionId, string regionDescription);
        void DeleteRegion(int regionId);
    }
    internal class RegionLogic : DataDrivenLogic, IRegionLogic
    {
        public RegionLogic(IBusinessLogicFactory BusinessLogicFactory, IRepositoryFactory RepositoryFactory = null) : base(BusinessLogicFactory, RepositoryFactory)
        {
        }

        public IEnumerable<Region> ReadRegion()
        {
            var dbContext = CreateSqlRepository<IRegionRepository>(Database.Northwind);
            return dbContext.Read();
        }

        public void CreateRegion(int regionId, string regionDescription)
        {
            var dbContext = CreateSqlRepository<IRegionRepository>(Database.Northwind);
            dbContext.Create(regionId, regionDescription);
        }

        public void UpdateRegion(int regionId, string regionDescription)
        {
            var dbContext = CreateSqlRepository<IRegionRepository>(Database.Northwind);
            dbContext.Update(regionId, regionDescription);
        }

        public void DeleteRegion(int regionId)
        {
            var dbContext = CreateSqlRepository<IRegionRepository>(Database.Northwind);
            dbContext.Delete(regionId);
        }
    }
}

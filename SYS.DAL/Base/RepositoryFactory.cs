using Ninject;
using Ninject.Parameters;
using SYS.DAL.ChatBot;
using SYS.DAL.Default;
using SYS.DAL.GAIA;
using SYS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYS.DAL.Base
{
    public interface IRepositoryFactory : IDisposable
    {
        // DataBase Data
        TRepository GetSqlRepository<TRepository>(Database database);

        // API Data
        TRepository GetWebRepository<TRepository>(string apiHost);

        // File Data
        TRepository GetFolderRepository<TRepository>(string directoryPath);
    }
    public class RepositoryFactory : IRepositoryFactory
    {
        private IKernel _kernel;
        private Dictionary<string, IDbConnection> ConnectionPool;
        private readonly string connection = nameof(connection);
        private readonly string apiHost = nameof(apiHost);
        private readonly string directoryPath = nameof(directoryPath);

        public RepositoryFactory(AppSettings appSettings)
        {
            if (_kernel == null)
            {
                this._kernel = new StandardKernel();
            }

            if (ConnectionPool == null)
            {
                ConnectionPool = new Dictionary<string, IDbConnection>();
            }

            if (ConnectionPool.Any())
            {
                ConnectionPool.Clear();
            }

            foreach (var item in appSettings.Connections)
            {
                ConnectionPool.Add(item.Key, new SqlConnection(item.Value[appSettings.Stage]));
            }

            _kernel
                .Bind<IRepositoryFactory>()
                .ToConstant(this)
                .InSingletonScope();
            // Example
            _kernel.Bind<IRegionRepository>().To<RegionRepository>().InSingletonScope().WithConstructorArgument(connection, context => null);
            // GAIA
            _kernel.Bind<IOmStaffRepository>().To<OmStaffRepository>().InSingletonScope().WithConstructorArgument(connection, context => null);
            _kernel.Bind<IOmUserRepository>().To<OmUserRepository>().InSingletonScope().WithConstructorArgument(connection, context => null);
            // ChatBot
            _kernel.Bind<IAccountRegistRepository>().To<AccountRegistRepository>().InSingletonScope().WithConstructorArgument(connection, context => null);
            // Default
            _kernel.Bind<ITransactionLogRepository>().To<TransactionLogRepository>().InSingletonScope().WithConstructorArgument(connection, context => null);

        }

        public void Dispose()
        {
            if (this._kernel != null)
            {
                this._kernel.Dispose();
            }
            GC.SuppressFinalize(this);
        }

        public TRepository GetSqlRepository<TRepository>(Database database)
        {
            var inject = new IParameter[]
            {
                new ConstructorArgument(connection, ConnectionPool[database.ToString()])
            };

            return _kernel.Get<TRepository>(inject);
        }

        public TRepository GetWebRepository<TRepository>(string apiHost)
        {
            var inject = new IParameter[]
            {
                new ConstructorArgument(nameof(apiHost), apiHost)
            };

            return _kernel.Get<TRepository>(inject);
        }

        public TRepository GetFolderRepository<TRepository>(string directoryPath)
        {
            var inject = new IParameter[]
            {
                new ConstructorArgument(nameof(directoryPath), directoryPath)
            };

            return _kernel.Get<TRepository>(inject);
        }
    }
}

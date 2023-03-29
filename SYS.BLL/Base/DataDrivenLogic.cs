using SYS.DAL.Base;
using SYS.Model;
using System;
using System.Transactions;

namespace SYS.BLL.Base
{
    /// <summary>
    /// 資料導向邏輯介面
    /// </summary>
    public interface IDataDrivenLogic : IBusinessLogic
    {
        /// <summary>
        /// 資料儲存庫
        /// </summary>
        IRepositoryFactory RepositoryFactory { get; }
    }

    internal abstract class DataDrivenLogic : BusinessLogic, IDataDrivenLogic
    {
        public IRepositoryFactory RepositoryFactory { get; private set; }

        public DataDrivenLogic(IBusinessLogicFactory BusinessLogicFactory, IRepositoryFactory RepositoryFactory = null) : base(BusinessLogicFactory)
        {
            this.RepositoryFactory = RepositoryFactory ?? new RepositoryFactory(AppSettings);
        }

        protected override void Dispose(bool Disposing)
        {
            if (this.RepositoryFactory != null)
            {
                this.RepositoryFactory.Dispose();
            }
            base.Dispose(Disposing);
        }

        /// <summary>
        /// 開啟資料交易
        /// </summary>
        /// <param name="TransactionIsolation">交易隔離層級</param>
        /// <param name="TransactionTimeout">交易逾時秒數</param>
        /// <returns></returns>
        protected virtual TransactionScope OpenTransactionScope(IsolationLevel TransactionIsolation = IsolationLevel.ReadCommitted, int TransactionTimeout = 60)
        {
            if (Transaction.Current == null)
            {
                return new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = TransactionIsolation, Timeout = new TimeSpan(0, 0, TransactionTimeout) });
            }
            else
            {
                return new TransactionScope(Transaction.Current);
            }
        }

        /// <summary>
        /// 建立SQL儲存體物件
        /// </summary>
        /// <typeparam name="TRepository">儲存體型別</typeparam>
        /// <param name="database">連線系統名稱</param>
        /// <returns></returns>
        protected virtual TRepository CreateSqlRepository<TRepository>(Database database = Database.Northwind)
        {
            return RepositoryFactory.GetSqlRepository<TRepository>(database);
        }

        /// <summary>
        /// 建立Web儲存體物件
        /// </summary>
        /// <typeparam name="TRepository">儲存體型別</typeparam>
        /// <param name="apiHost">網站位址</param>
        /// <returns></returns>
        protected virtual TRepository CreateWebRepository<TRepository>(string apiHost)
        {
            return RepositoryFactory.GetWebRepository<TRepository>(apiHost);
        }

        /// <summary>
        /// 建立Folder儲存體物件
        /// </summary>
        /// <typeparam name="TRepository">儲存體型別</typeparam>
        /// <param name="directoryPath">目錄路徑</param>
        /// <returns></returns>
        protected virtual TRepository CreateFolderRepository<TRepository>(string directoryPath)
        {
            return RepositoryFactory.GetFolderRepository<TRepository>(directoryPath);
        }
    }
}

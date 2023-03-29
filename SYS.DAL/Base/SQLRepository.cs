using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYS.DAL.Base
{
    public interface ISQLRepository : IDisposable
    {
        /// <summary>
        /// 資料庫連線
        /// </summary>
        IDbConnection Connection { get; }
    }
    internal abstract class SQLRepository : ISQLRepository
    {
        public IDbConnection Connection { get; private set; }

        protected SQLRepository(IDbConnection Connection)
        {
            this.Connection = Connection ?? throw new ArgumentNullException(nameof(Connection));
        }

        protected virtual void Dispose(bool Disposing)
        {
            if (Connection != null && Connection.State == ConnectionState.Closed)
            {
                Connection.Dispose();
            }
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 取得執行命令
        /// </summary>
        /// <param name="SqlCommandType">命令類型</param>
        /// <param name="SqlCommandText">命令文字</param>
        /// <param name="Parameters">執行參數</param>
        /// <param name="Transaction">交易</param>
        /// <param name="Timeout">逾時秒數</param>
        /// <returns></returns>
        protected virtual CommandDefinition GetCommand(CommandType SqlCommandType, string SqlCommandText, object Parameters = null, IDbTransaction Transaction = null, int Timeout = 30) => new CommandDefinition(SqlCommandText, Parameters, commandType: SqlCommandType, transaction: Transaction, commandTimeout: Timeout);

    }
}

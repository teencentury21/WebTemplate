using Dapper;
using SYS.DAL.Base;
using SYS.Model.SQL.Default;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYS.DAL.Default
{
    public interface ITransactionLogRepository : ISQLRepository
    {
        void Create(TransactionLog item);
        void Create(string application_Name, string data, string description, string message, string editor);
        IEnumerable<TransactionLog> Read();
        void Update(TransactionLog item);
        void Delete(int regionId);
        List<TransactionLog> GetItmeByApplicationName(string applicationName);
        List<TransactionLog> GetItmeByAppNameNData(string applicationName, string data);
    }
    internal class TransactionLogRepository : SQLRepository, ITransactionLogRepository
    {
        public TransactionLogRepository(IDbConnection connection) : base(connection)
        {
        }
        public void Create(TransactionLog item)
        {
            string strSql = "INSERT INTO [dbo].[Transaction_Log] ([Id] ,[Application_Name] ,[Data] ,[Description] ,[Editor] ,[Message] ,[Cdt])" +
                 $" VALUES('{item.Id}', '{item.Application_Name}', '{item.Data}', '{item.Description}', '{item.Editor}', '{item.Message}', '{item.Cdt.ToString("yyyy/MM/dd HH:mm:ss")}')";
            Connection.Execute(strSql);
        }
        public void Create(string application_Name, string data, string description, string message, string editor)
        {
            string strSql = "INSERT INTO [dbo].[Transaction_Log] ([Id] ,[Application_Name] ,[Data] ,[Description] ,[Editor] ,[Message])" +
                 $" VALUES('{Guid.NewGuid().ToString()}', '{application_Name}', '{data}', '{description}', '{editor}', '{message}')";
            Connection.Execute(strSql);
        }
        public IEnumerable<TransactionLog> Read()
        {
            return Connection.Query<TransactionLog>("SELECT * FROM [dbo].[Transaction_Log]");
        }
        public void Update(TransactionLog item)
        {
            string strSql = "UPDATE [dbo].[Transaction_Log] SET" +                
                  $" [Application_Name] = '{item.Application_Name}'" +
                  $" ,[Data] = '{item.Data}'" +
                  $" ,[Description] = '{item.Description}'" +
                  $" ,[Editor] = '{item.Editor}'" +
                  $" ,[Message] = '{item.Message}'" +                  
                $" WHERE Id='{item.Id}'";
            Connection.Execute(strSql);
        }
        public void Delete(int regionId)
        {
            throw new NotImplementedException();
        }

        public List<TransactionLog> GetItmeByApplicationName(string applicationName)
        {
            var parameters = new { ApplicationName = applicationName };
            var sql = "SELECT * FROM [dbo].[Transaction_Log] WHERE Application_Name = @ApplicationName ORDER BY Cdt DESC";
            return Connection.Query<TransactionLog>(sql, parameters).ToList();
        }

        public List<TransactionLog> GetItmeByAppNameNData(string applicationName, string data)
        {
            var parameters = new { ApplicationName = applicationName, Data = data };
            var sql = "SELECT * FROM [dbo].[Transaction_Log] WHERE Application_Name = @ApplicationName AND Data = @Data ORDER BY Cdt DESC";
            return Connection.Query<TransactionLog>(sql, parameters).ToList();
        }
    }
}

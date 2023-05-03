using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using SYS.DAL.Base;
using SYS.Model.SQL.Default;
using System.Linq;

namespace SYS.DAL.Default
{
    public interface IFunctionsRepository : ISQLRepository
    {
        void Create(Functions item);
        List<Functions> Read(string input = "");
        void Update(Functions item);
        void Delete(int regionId);
    }
    internal class FunctionsRepository : SQLRepository, IFunctionsRepository
    {
        public FunctionsRepository(IDbConnection connection) : base(connection)
        {

        }
        public void Create(Functions func)
        {
            var sql = @"INSERT INTO [Functions] ([parent_function_id] ,[function_name] ,[function_description] ,[editor])
                   VALUES (@parent_function_id, @function_name, @function_description, @editor )";
            Connection.Execute(sql, func);
        }


        public List<Functions> Read(string input = "")
        {
            var sql = "SELECT * FROM [dbo].[Functions]";
            sql = input == "" ? sql : $"{sql} WHERE function_name LIKE '%' + @condiction + '%'";

            return Connection.Query<Functions>(sql, new { condiction = input.ToLower() }).ToList();            
        }

        public void Update(Functions func)
        {
            func.udt = DateTime.Now;

            var sql = @"UPDATE Functions SET udt = @udt";
            sql = func.parent_function_id == null ? sql : $"{sql}, parent_function_id = @parent_function_id";
            sql = func.function_name == null ? sql : $"{sql}, function_name = @function_name";
            sql = func.function_description == null ? sql : $"{sql}, function_description= @function_description";
            sql = func.editor == null ? sql : $"{sql}, editor= @editor";
            sql = $"{sql} WHERE function_id = @function_id";

            Connection.Execute(sql, func);
        }
        public void Delete(int funcId)
        {
            var user = new Functions { function_id = funcId };
            var sql = @"DELETE FROM [dbo].[Functions] WHERE [function_id] = @function_id";

            Connection.Execute(sql, user);

        }
    }
}

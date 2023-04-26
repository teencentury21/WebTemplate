using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using SYS.DAL.Base;
using SYS.Model.SQL.Default;
using System.Linq;

namespace SYS.DAL.Default
{
    public interface IIniRepository : ISQLRepository
    {
        // Functions
        void Create(INI item);
        List<INI> Read();
        void Update(INI item);
        void Delete(INI item);
        INI GetSingleItemByName(string itemName);
        List<INI> GetMultiItemByName(string itemName);
    }

    internal class IniRepository : SQLRepository, IIniRepository
    {
        public IniRepository(IDbConnection connection) : base(connection)
        {

        }
        public void Create(INI user)
        {
            var sql = @"INSERT INTO [dbo].[INI] ([Item] ,[Data] ,[Description] ,[Editor] ,[Cdt] ,[Udt])
                VALUES (@Item ,@Data ,@Description ,@Editor ,@Cdt ,@Udt)";
            Connection.Execute(sql, user);
        }

        public void Delete(INI item)
        {
            // var item = new INI { id = itemId };
            var sql = @"DELETE FROM [dbo].[INI] WHERE [id] = @id";

            Connection.Execute(sql, item);
        }

        public List<INI> Read()
        {
            return Connection.Query<INI>("SELECT * FROM [dbo].[INI] ORDER BY id").ToList();
        }

        public void Update(INI item)
        {
            var sql = @"UPDATE INI SET Udt = @Udt, ";
            sql = item.Data == null ? sql : sql += "Data=@Data, ";
            sql = item.Item == null ? sql : sql += "Item = @Item, ";
            sql = item.Description == null ? sql : sql += "Description = @Description, ";
            sql = item.Editor == null ? sql : sql += "Editor = @Editor ";            
            sql += " WHERE id = @id";

            Connection.Execute(sql, item);
        }

        public INI GetSingleItemByName(string itemName)
        {
            var sql = itemName == "" ? "SELECT * FROM INI" : "SELECT TOP 1 * FROM INI WHERE Item = @Item ORDER BY id";
            return Connection.Query<INI>(sql, new { Item = itemName }).FirstOrDefault();
        }
        public List<INI> GetMultiItemByName(string itemName)
        {
            var sql = itemName == "" ? "SELECT * FROM INI" : "SELECT * FROM INI WHERE Item = @Item ORDER BY id";
            return Connection.Query<INI>(sql, new { Item = itemName }).ToList();
        }
    }

}

using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using SYS.DAL.Base;
using SYS.Model.SQL.Default;
using System.Linq;

namespace SYS.DAL.Default
{
    public interface IGroupsRepository : ISQLRepository
    {
        void Create(Groups item);
        List<Groups> Read(string input = "");
        void Update(Groups item);
        void Delete(int regionId);
    }
    internal class GroupsRepository : SQLRepository, IGroupsRepository
    {
        public GroupsRepository(IDbConnection connection) : base(connection)
        {

        }
        public void Create(Groups group)
        {
            var sql = @"INSERT INTO [Groups] ([group_name] ,[group_description] ,[editor])
                   VALUES (@group_name, @group_description, @editor )";
            Connection.Execute(sql, group);
        }
        public List<Groups> Read(string input = "")
        {
            var sql = "SELECT * FROM [dbo].[Groups]";
            sql = input == "" ? sql : $"{sql} WHERE group_name LIKE '%' + @condiction + '%'";

            return Connection.Query<Groups>(sql, new { condiction = input.ToLower() }).ToList();
        }

        public void Update(Groups group)
        {
            group.udt = DateTime.Now;

            var sql = @"UPDATE Groups SET udt = @udt";
            sql = group.group_name == null ? sql : $"{sql}, group_name = @group_name";
            sql = group.group_description == null ? sql : $"{sql}, group_description= @group_description";
            sql = group.editor == null ? sql : $"{sql}, editor= @editor";
            sql = $"{sql} WHERE group_id = @group_id";

            Connection.Execute(sql, group);
        }
        public void Delete(int groupId)
        {
            var user = new Groups { group_id = groupId };
            var sql = @"DELETE FROM [dbo].[Groups] WHERE [group_id] = @group_id";

            Connection.Execute(sql, user);
        }

    }
}

using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using SYS.DAL.Base;
using SYS.Model.SQL.Default;
using System.Linq;
using System.Data.SqlClient;

namespace SYS.DAL.Default
{
    public interface IUsersRepository : ISQLRepository
    {
        // Functions
        void Create(Users item);
        IEnumerable<Users> Read();
        void Update(Users item);
        void Delete(int regionId);

        Users GetUsersByAcc(string acc);
    }
    internal class UsersRepository : SQLRepository, IUsersRepository
    {
        public UsersRepository(IDbConnection connection) : base(connection)
        {

        }
        public void Create(Users user)
        {
            var sql = @"INSERT INTO [dbo].[Users] ([username], [password], [is_admin], [is_active])
            VALUES (@username, @password, @is_admin, @is_active)";
            Connection.Execute(sql, user);
        }
        public IEnumerable<Users> Read()
        {
            return Connection.Query<Users>("SELECT * FROM [dbo].[Users]");
        }

        public void Update(Users user)
        {
            var sql = @"UPDATE [dbo].[Users] SET [password] = @password, [is_admin] = @is_admin, [is_active] = @is_active
            WHERE [user_id] = @user_id";

            Connection.Execute(sql, user);
        }

        public void Delete(int userId)
        {
            var user = new Users { user_id = userId };
            var sql = @"DELETE FROM [dbo].[Users] WHERE [user_id] = @user_id";

            Connection.Execute(sql, user);

        }

        public Users GetUsersByAcc(string acc)
        {
            return Connection.Query<Users>("SELECT * FROM Users WHERE username = @username AND is_active=1",
                    new { username = acc }).FirstOrDefault();
        }
    }

}

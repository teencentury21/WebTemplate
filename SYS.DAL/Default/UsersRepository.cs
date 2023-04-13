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
        List<Users> Read();
        void Update(Users item);
        void Delete(int regionId);

        Users GetUsersByAny(string acc);
    }
    internal class UsersRepository : SQLRepository, IUsersRepository
    {
        public UsersRepository(IDbConnection connection) : base(connection)
        {

        }
        public void Create(Users user)
        {
            //var sql = @"INSERT INTO Users (username, userno, password, is_active, is_admin, email, role, Setting, Remark)
            //       VALUES (@username, @userno, @password, @is_active, @is_admin, @email, @role, @Setting, @Remark)";
            var sql = @"INSERT INTO Users (username, userno, password, is_active, is_admin, email)
                   VALUES (@username, @userno, @password, @is_active, @is_admin, @email)";
            Connection.Execute(sql, user);
        }
        public List<Users> Read()
        {
            return Connection.Query<Users>("SELECT * FROM [dbo].[Users]").ToList();
        }

        public void Update(Users user)
        {
            var sql = @"UPDATE Users
                   SET password = @password,
                       is_active = @is_active,
                       is_admin = @is_admin,                       
                       role = @role,
                       LastLogin = @LastLogin,
                       Setting = @Setting,
                       Remark = @Remark,
                       Cdt = @Cdt
                   WHERE user_id = @user_id";

            Connection.Execute(sql, user);
        }

        public void Delete(int userId)
        {
            var user = new Users { user_id = userId };
            var sql = @"DELETE FROM [dbo].[Users] WHERE [user_id] = @user_id";

            Connection.Execute(sql, user);

        }

        public Users GetUsersByAny(string input)
        {
            return Connection.Query<Users>("SELECT * FROM Users WHERE (LOWER(username) = @condiction OR LOWER(email) = @condiction OR userno = @condiction) AND is_active=1",
                    new { condiction = input.ToLower() }).FirstOrDefault();
        }
    }

}

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
        List<Users> Read(string input = "");
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
            var sql = @"INSERT INTO Users (username, userno, password, is_active, is_admin, email)
                   VALUES (@username, @userno, @password, @is_active, @is_admin, @email)";
            Connection.Execute(sql, user);
        }
        public List<Users> Read(string input = "")
        {
            var sql = "SELECT * FROM [dbo].[Users]";
            sql = input == "" ? sql : $"{sql} WHERE (LOWER(username) LIKE '%' + @condiction + '%' OR LOWER(email) LIKE '%' + @condiction + '%' OR userno LIKE '%' + @condiction + '%' )";
            return Connection.Query<Users>(sql, new { condiction = input.ToLower() }).ToList();
        }

        public void Update(Users user)
        {
            user.udt = DateTime.Now;

            var sql = @"UPDATE Users SET password = @password, is_active = @is_active, is_admin = @is_admin, udt = @udt";            
            sql = user.email == null ? sql : $"{sql}, email = @email";
            sql = user.role == null ? sql : $"{sql}, role = @role";
            sql = user.setting == null ? sql : $"{sql}, setting = @setting";
            sql = user.remark == null ? sql : $"{sql}, remark = @remark";
            sql = user.lastlogin == null ? sql : $"{sql}, lastLogin = @lastLogin";
            sql = $"{sql} WHERE user_id = @user_id";

            Connection.Execute(sql, user);
        }

        public void Delete(int userId)
        {
            var user = new Users { user_id = userId };
            var sql = @"DELETE FROM [dbo].[Users] WHERE [user_id] = @user_id";

            Connection.Execute(sql, user);

        }
        /// <summary>
        /// Get single by username/ userno/ email
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Users GetUsersByAny(string input)
        {
            return Connection.Query<Users>("SELECT * FROM Users WHERE (LOWER(username) = @condiction OR LOWER(email) = @condiction OR userno = @condiction) AND is_active=1",
                    new { condiction = input.ToLower() }).FirstOrDefault();
        }
    }

}

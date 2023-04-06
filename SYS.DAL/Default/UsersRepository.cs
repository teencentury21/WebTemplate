using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using SYS.DAL.Base;
using SYS.Model.SQL.Default;

namespace SYS.DAL.Default
{
    public interface IUsersRepository : ISQLRepository
    {
        void Create(Users item);
        IEnumerable<Users> Read();
        void Update(Users item);
        void Delete(int regionId);
    }
    internal class UsersRepository : SQLRepository, IUsersRepository
    {
        public UsersRepository(IDbConnection connection) : base(connection)
        {

        }
        public void Create(Users item)
        {
            //string strSql = "INSERT INTO [dbo].[Account_Regist]([Id],[Line_Id],[Emp_Id],[Emp_no],[Emp_phone],[Emp_mail],[Active],[Setting],[Cdt])" +
            //     $" VALUES('{item.Id}', '{item.Line_Id}', '{item.Emp_Id}', '{item.Emp_no}', '{item.Emp_phone}', '{item.Emp_mail}', '{item.Active}', '{item.Setting}', '{item.Cdt.ToString("yyyy/MM/dd HH:mm:ss")}' )";
            //Connection.Execute(strSql);

            //string strSql = "INSERT INTO [dbo].[Account_Regist]([Id],[Line_Id],[Emp_Id],[Emp_no],[Emp_phone],[Emp_mail],[Active],[Setting],[Cdt])" +
            //     " VALUES(@id, @lineId, @empId, @empNo, @empPhone, @empMail, @active, @setting, @cdt )";
            //新增多筆參數
            // string strSql ="INSERT INTO Users(col1,col2) VALUES (@c1,@c2);" ;
            //dynamic datas = new[]{ new { c1 = "A", c2 = "A2" }
            //    , new { c1 = "B", c2 = "B2" }
            //    , new { c1 = "C", c2 = "C2" }};
        }
        public IEnumerable<Users> Read()
        {
            return Connection.Query<Users>("SELECT * FROM [dbo].[Account_Regist]");
        }

        public void Update(Users item)
        {
            //string strSql = "UPDATE [dbo].[Account_Regist] SET" +
            //    $" [Line_Id] = '{item.Line_Id}'" +
            //    $" ,[Emp_Id] = '{item.Emp_Id}'" +
            //    $" ,[Emp_no] = '{item.Emp_no}'" +
            //    $" ,[Emp_phone] = '{item.Emp_phone}'" +
            //    $" ,[Emp_mail] = '{item.Emp_mail}'" +
            //    $" ,[Active] = '{item.Active}'" +
            //    $" ,[Setting] = '{item.Setting}'" +
            //    $" ,[Cdt] = '{item.Cdt.ToString("yyyy/MM/dd HH:mm:ss")}'" +
            //    $" WHERE Id='{item.Id}'";
            //Connection.Execute(strSql);
        }

        public void Delete(int regionId)
        {
            throw new NotImplementedException();
        }
    }

}

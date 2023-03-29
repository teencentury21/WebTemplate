using Dapper;
using SYS.DAL.Base;
using SYS.Model.SQL.ChatBot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYS.DAL.ChatBot
{
    public interface IAccountRegistRepository : ISQLRepository
    {
        void Create(AccountRegist item);
        IEnumerable<AccountRegist> Read();
        void Update(AccountRegist item);
        void Delete(int regionId);
        AccountRegist GetAccountByLineId(string lineId);
        AccountRegist GetAccountByEmpNo(string empNo);

    }
    internal class AccountRegistRepository : SQLRepository, IAccountRegistRepository
    {
        public AccountRegistRepository(IDbConnection connection) : base(connection)
        {
        }

        public void Create(AccountRegist item)
        {
            string strSql = "INSERT INTO [dbo].[Account_Regist]([Id],[Line_Id],[Emp_Id],[Emp_no],[Emp_phone],[Emp_mail],[Active],[Setting],[Cdt])" +
                 $" VALUES('{item.Id}', '{item.Line_Id}', '{item.Emp_Id}', '{item.Emp_no}', '{item.Emp_phone}', '{item.Emp_mail}', '{item.Active}', '{item.Setting}', '{item.Cdt.ToString("yyyy/MM/dd HH:mm:ss")}' )";
            Connection.Execute(strSql);

            //string strSql = "INSERT INTO [dbo].[Account_Regist]([Id],[Line_Id],[Emp_Id],[Emp_no],[Emp_phone],[Emp_mail],[Active],[Setting],[Cdt])" +
            //     " VALUES(@id, @lineId, @empId, @empNo, @empPhone, @empMail, @active, @setting, @cdt )";
            //新增多筆參數
            // string strSql ="INSERT INTO Users(col1,col2) VALUES (@c1,@c2);" ;
            //dynamic datas = new[]{ new { c1 = "A", c2 = "A2" }
            //    , new { c1 = "B", c2 = "B2" }
            //    , new { c1 = "C", c2 = "C2" }};
        }
        public IEnumerable<AccountRegist> Read()
        {
            return Connection.Query<AccountRegist>("SELECT * FROM [dbo].[Account_Regist]");
        }

        public void Update(AccountRegist item)
        {
            string strSql = "UPDATE [dbo].[Account_Regist] SET" +                
                $" [Line_Id] = '{item.Line_Id}'" +
                $" ,[Emp_Id] = '{item.Emp_Id}'" +
                $" ,[Emp_no] = '{item.Emp_no}'" +
                $" ,[Emp_phone] = '{item.Emp_phone}'" +
                $" ,[Emp_mail] = '{item.Emp_mail}'" +
                $" ,[Active] = '{item.Active}'" +
                $" ,[Setting] = '{item.Setting}'" +
                $" ,[Cdt] = '{item.Cdt.ToString("yyyy/MM/dd HH:mm:ss")}'" +         
                $" WHERE Id='{item.Id}'";
            Connection.Execute(strSql);
        }

        public void Delete(int regionId)
        {
            throw new NotImplementedException();
        }

        public AccountRegist GetAccountByEmpNo(string empNo)
        {
            var parameters = new { EmpNo = empNo };
            var sql = "SELECT TOP 1 * FROM [dbo].[Account_Regist] WHERE Emp_no = @EmpNo";
            return Connection.QueryFirstOrDefault<AccountRegist>(sql, parameters);

        }

        public AccountRegist GetAccountByLineId(string lineId)
        {
            var parameters = new { LineId = lineId };
            var sql = "SELECT TOP 1 * FROM [dbo].[Account_Regist] WHERE Line_Id = @LineId";
            return Connection.QueryFirstOrDefault<AccountRegist>(sql, parameters);
        }

        
    }
}

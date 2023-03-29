using Dapper;
using SYS.DAL.Base;
using SYS.Model.SQL.GAIA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SYS.DAL.GAIA
{
    public interface IOmStaffRepository : ISQLRepository
    {
        void Create(int regionId, string regionDescription);
        IEnumerable<OmStaff> Read();
        void Update(int regionId, string regionDescription);
        void Delete(int regionId);
        OmStaff GetItemByEmpNo(string input);
        List<OmStaff> GetEmpByName(string lang, string name, bool limited = true);


    }
    internal class OmStaffRepository :SQLRepository, IOmStaffRepository
    {
        public OmStaffRepository(IDbConnection connection) : base(connection)
        {
        }
        public void Create(int regionId, string regionDescription)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<OmStaff> Read()
        {
            return Connection.Query<OmStaff>("SELECT * FROM [dbo].[om_staff]");
        }

        public void Update(int regionId, string regionDescription)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(int regionId)
        {
            throw new System.NotImplementedException();
        }

        public OmStaff GetItemByEmpNo(string input)
        {
            var parameters = new { StaffNo = input};
            var sql = "SELECT TOP 1 * FROM [gcore].[om_staff] WHERE staff_no = @StaffNo";
            return Connection.QueryFirstOrDefault<OmStaff>(sql, parameters);
            
        }

        public List<OmStaff> GetEmpByName(string lang, string name, bool limited = true)
        {            
            var parameters = new { Name = $"%{name.ToLower()}%" };
            if (lang == "en")
            {
                if (limited)
                {
                    var sql = "SELECT TOP 5 * FROM [gcore].[om_staff] WHERE LOWER(alias_name) LIKE @Name";
                    return Connection.Query<OmStaff>(sql, parameters).ToList();
                    
                }
                else
                {
                    var sql = "SELECT * FROM [gcore].[om_staff] WHERE LOWER(alias_name) LIKE @Name"; 
                    return Connection.Query<OmStaff>(sql, parameters).ToList();
                }
            }
            else if (lang == "zh-tw")
            {
                var sql = "SELECT * FROM [gcore].[om_staff] WHERE name = @Name";
                return Connection.Query<OmStaff>(sql, parameters).ToList();
            }
            else
            {
                return new List<OmStaff>();
            }
        }



    }
    
}

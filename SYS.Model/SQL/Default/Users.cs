using System;

namespace SYS.Model.SQL.Default
{
    public class Users
    {
        public bool is_active { get; set; }
        public bool is_admin { get; set; }
        public DateTime cdt { get; set; }
        public DateTime udt { get; set; }
        public DateTime lastlogin { get; set; }
        public int user_id { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string remark { get; set; }
        public string role { get; set; }
        public string setting { get; set; }
        public string username { get; set; }
        public string userno { get; set; }
    }
}

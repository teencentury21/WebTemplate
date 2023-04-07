using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYS.Model.SQL.Default
{
    public class Users
    {
        public bool is_active { get; set; }
        public bool is_admin { get; set; }
        public int user_id { get; set; }
        public string password { get; set; }
        public string username { get; set; }
    }
}

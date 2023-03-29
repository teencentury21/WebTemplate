using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYS.Model.SQL.ChatBot
{
    public class AccountRegist
    {
        public bool Active { get; set; }
        public DateTime Cdt { get; set; }
        public string Emp_Id { get; set; }
        public string Emp_mail { get; set; }
        public string Emp_no { get; set; }
        public string Emp_phone { get; set; }
        public Guid Id { get; set; }
        public string Line_Id { get; set; }
        public string Setting { get; set; }
    }
}

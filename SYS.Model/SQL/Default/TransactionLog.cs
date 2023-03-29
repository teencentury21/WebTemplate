using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYS.Model.SQL.Default
{
    public class TransactionLog
    {
        public DateTime Cdt { get; set; }
        public string Application_Name { get; set; }
        public string Data { get; set; }
        public string Description { get; set; }
        public string Editor { get; set; }
        public Guid Id { get; set; }
        public string Message { get; set; }
    } // class Transaction_Log
}

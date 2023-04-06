using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYS.Model.SQL.Default
{
    public class Functions
    {
        public int function_id { get; set; }
        public int? parent_function_id { get; set; }
        public string function_description { get; set; }
        public string function_name { get; set; }
    }
}

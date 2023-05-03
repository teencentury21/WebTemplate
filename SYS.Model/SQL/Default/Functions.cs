using System;

namespace SYS.Model.SQL.Default
{
    public class Functions
    {
        public int function_id { get; set; }
        public int? parent_function_id { get; set; }
        public string function_description { get; set; }
        public string function_name { get; set; }
        public string editor { get; set; }
        public DateTime cdt { get; set; }
        public DateTime udt { get; set; }

    }
}

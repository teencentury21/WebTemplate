using System;

namespace SYS.Model.SQL.Default
{
    public class Groups
    {
        public int group_id { get; set; }
        public string group_name { get; set; }
        public string group_description { get; set; }
        public string editor { get; set; }
        public DateTime cdt { get; set; }
        public DateTime udt { get; set; }

    } // class Groups
}

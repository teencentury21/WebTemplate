using System;

namespace SYS.Model.SQL.GAIA
{
    public class OmStaff
    {
        public DateTime created_on { get; set; }
        public DateTime? birthday { get; set; }
        public DateTime? changed_on { get; set; }
        public DateTime? departure_on { get; set; }
        public DateTime? entry_on { get; set; }
        public int gender { get; set; }
        public int is_active { get; set; }
        public int is_user { get; set; }
        public string alias_name { get; set; }
        public string area { get; set; }
        public string changed_by { get; set; }
        public string company_id { get; set; }
        public string created_by { get; set; }
        public string email { get; set; }
        public string ext_no { get; set; }
        public string id { get; set; }
        public string job_code_id { get; set; }
        public string job_type { get; set; }
        public string location { get; set; }
        public string mobile { get; set; }
        public string muid { get; set; }
        public string name { get; set; }
        public string org_id { get; set; }
        public string position { get; set; }
        public string site_code { get; set; }
        public string staff_no { get; set; }
        public string weixin_id { get; set; }
    } // class om_staff
}

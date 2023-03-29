using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYS.Model.SQL.GAIA
{
    public class OmUser
    {
        public DateTime changed_psw_on { get; set; }
        public DateTime created_on { get; set; }
        public DateTime? active_end_on { get; set; }
        public DateTime? active_start_on { get; set; }
        public DateTime? changed_on { get; set; }
        public int is_active { get; set; }
        public int is_chang_psw { get; set; }
        public int is_limit_usbkey { get; set; }
        public int is_sync { get; set; }
        public int user_type { get; set; }
        public string account { get; set; }
        public string changed_by { get; set; }
        public string created_by { get; set; }
        public string email { get; set; }
        public string history_psw { get; set; }
        public string id { get; set; }
        public string integrated_account { get; set; }
        public string mobile { get; set; }
        public string muid { get; set; }
        public string psw { get; set; }
        public string psw_salt { get; set; }
        public string scope_id { get; set; }
        public string site_code { get; set; }
        public string usbkey_no { get; set; }
        public string user_desc { get; set; }
        public string user_name { get; set; }
        public string weixin_id { get; set; }
    } // class om_user
}

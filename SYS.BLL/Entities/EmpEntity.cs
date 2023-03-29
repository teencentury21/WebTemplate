using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYS.BLL.Entities
{
    public class EmpEntity
    {
        public string GAIAId { get; set; }
        public string EmpId { get; set; }
        public string EmpNo { get; set; }
        public string Name { get; set; }
        public string EnName { get; set; }
        public string Email { get; set; }
        public string Site { get; set; }
        public string ExtNo { get; set; }
        public string IsDL { get; set; }
        public int IsActive { get; set; }
        public string remark { get; set; }
    }
}

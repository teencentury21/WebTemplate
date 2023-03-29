using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYS.BLL.Entities.Mail
{
    public class MailEntity
    {
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}

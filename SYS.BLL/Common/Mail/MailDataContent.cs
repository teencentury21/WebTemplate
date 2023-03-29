using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYS.BLL.Common.Mail
{
    public class MailDataContent : MailContent
    {
        internal override string GetContentResult()
        {
            var sb = new StringBuilder();

            sb.Append(HtmlContentAttributeNames.ContentStart.Value);
            sb.Append(base.GetContentResult());
            sb.Append(HtmlContentAttributeNames.ContentEnd.Value);

            return sb.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYS.BLL.Common.Mail
{
    public class HtmlMailRoot
    {
        private readonly List<MailContent> _contents = new List<MailContent>();

        public MailDataContent AddDataContent()
        {
            var content = new MailDataContent();

            _contents.Add(content);

            return content;
        }

        public MailContent AddNormalContent()
        {
            var content = new MailContent();

            _contents.Add(content);

            return content;
        }

        public string GetContentResult()
        {
            var sb = new StringBuilder();

            sb.Append("<!DOCTYPE html>");
            sb.Append("<html lang=\"en\">");
            sb.Append("<head>");
            sb.Append("<title></title>");
            sb.Append("<meta charset=\"utf-8\" />");
            sb.Append("</head>");
            sb.Append("<body>");

            foreach (var content in _contents)
            {
                sb.Append(content.GetContentResult());
            }

            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();
        }
    }
}

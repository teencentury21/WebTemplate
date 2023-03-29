using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYS.BLL.Common.Mail
{
    public class MailContent
    {
        private readonly List<string> _contents = new List<string>();

        internal MailContent()
        {
        }

        public MailContent AppendFormat(string content, params object[] data)
        {
            _contents.Add(string.Format(content, data));

            return this;
        }

        public MailContent Append(string content)
        {
            _contents.Add(content);

            return this;
        }

        public MailContent Append(MailContentKey key)
        {
            var content = string.Format("{0}{1}{2}", HtmlContentAttributeNames.KeyStart.Value, key.Content, HtmlContentAttributeNames.KeyEnd.Value);

            _contents.Add(content);

            return this;
        }

        public MailContent Append(MailContentStatus status)
        {
            var content = string.Format("{0}{1}{2}", HtmlContentAttributeNames.StatusStart.Value, status.Content, HtmlContentAttributeNames.StatusEnd.Value);

            _contents.Add(content);

            return this;
        }

        internal virtual string GetContentResult()
        {
            var sb = new StringBuilder();

            sb.Append(string.Join("", _contents));

            return sb.ToString();
        }
    }
}

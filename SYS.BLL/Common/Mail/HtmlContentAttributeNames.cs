using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYS.BLL.Common.Mail
{
    public class HtmlContentAttributeNames : Descriptor<string>
    {
        private static readonly HtmlContentAttributeNames _contentStart = new HtmlContentAttributeNames("<!--[CONTENT START]-->");
        private static readonly HtmlContentAttributeNames _contentEnd = new HtmlContentAttributeNames("<!--[CONTENT END]-->");
        private static readonly HtmlContentAttributeNames _keyStart = new HtmlContentAttributeNames("<!--[KEY START]");
        private static readonly HtmlContentAttributeNames _keyEnd = new HtmlContentAttributeNames("[KEY END]-->");
        private static readonly HtmlContentAttributeNames _statusStart = new HtmlContentAttributeNames("<!--[STATUS START]");
        private static readonly HtmlContentAttributeNames _statusEnd = new HtmlContentAttributeNames("[STATUS END]-->");

        public static HtmlContentAttributeNames ContentStart
        {
            get { return _contentStart; }
        }

        public static HtmlContentAttributeNames ContentEnd
        {
            get { return _contentEnd; }
        }

        public static HtmlContentAttributeNames KeyStart
        {
            get { return _keyStart; }
        }

        public static HtmlContentAttributeNames KeyEnd
        {
            get { return _keyEnd; }
        }

        public static HtmlContentAttributeNames StatusStart
        {
            get { return _statusStart; }
        }

        public static HtmlContentAttributeNames StatusEnd
        {
            get { return _statusEnd; }
        }

        public HtmlContentAttributeNames()
        {
        }

        protected HtmlContentAttributeNames(string value)
            : base(value)
        {
        }
    }
}

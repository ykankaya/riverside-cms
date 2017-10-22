using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Utilities.Mail
{
    public class EmailContent
    {
        public string Subject { get; set; }
        public string HtmlBody { get; set; }
        public string PlainTextBody { get; set; }
    }
}

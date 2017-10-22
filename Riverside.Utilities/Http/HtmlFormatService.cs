using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Utilities.Http
{
    public class HtmlFormatService : IHtmlFormatService
    {
        public string FormatMultiLine(string text)
        {
            return text.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "<br />");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Utilities.Http
{
    public interface IEncodeDecodeService
    {
        string HtmlEncode(string text);
        string UrlEncode(string text);
    }
}

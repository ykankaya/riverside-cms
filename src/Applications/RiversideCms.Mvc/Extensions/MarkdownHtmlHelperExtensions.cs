using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonMark;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RiversideCms.Mvc.Extensions
{
    public static class MarkdownHtmlHelperExtensions
    {
        public static IHtmlContent MarkdownToHtml(this IHtmlHelper htmlHelper, string markdown)
        {
            string html = CommonMarkConverter.Convert(markdown);
            return new HtmlString(html);
        }
    }
}

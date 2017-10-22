using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Riverside.Utilities.Http;

namespace Riverside.UI.Extensions
{
    public static class TextHtmlHelperExtensions
    {
        public static IHtmlContent FormatMultiline(this IHtmlHelper htmlHelper, string text)
        {
            IHtmlFormatService htmlFormatService = (IHtmlFormatService)htmlHelper.ViewContext.HttpContext.RequestServices.GetService(typeof(IHtmlFormatService));
            return new HtmlString(htmlFormatService.FormatMultiLine(text));
        }
    }
}

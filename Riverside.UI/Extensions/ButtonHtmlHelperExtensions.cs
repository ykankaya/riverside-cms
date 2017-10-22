using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Riverside.UI.Controls;
using Riverside.UI.Routes;

namespace Riverside.UI.Extensions
{
    public static class ButtonHtmlHelperExtensions
    {
        private static void GetButtonHtml(IUrlHelper urlHelper, StringBuilder sb, Button button)
        {
            UrlParameters parameters = button.UrlParameters;
            string iconHtml = null;
            if (button.Icon != null)
                iconHtml = string.Format("<i class=\"fa fa-fw {0}\"></i>", button.Icon);
            sb.AppendFormat("<a href=\"{0}\">", urlHelper.GetUrl(parameters));
            if (!button.IconRight)
                sb.Append(iconHtml + " ");
            sb.Append(button.Text);
            if (button.IconRight)
                sb.Append(" " + iconHtml);
            sb.Append("</a>");
        }

        public static IHtmlContent ButtonListItem<TModel>(this IHtmlHelper<TModel> htmlHelper, Button button)
        {
            // Create string builder for storing HTML
            StringBuilder sb = new StringBuilder();

            // Use tag builder for li element
            sb.Append("<li");
            switch (button.State)
            {
                case ButtonState.Active: sb.Append(" class=\"active\""); break;
                case ButtonState.Disabled: sb.Append(" class=\"disabled\""); break;
            }
            sb.Append(">");

            // Get URL helper
            IUrlHelperFactory urlHelperFactory = (IUrlHelperFactory)htmlHelper.ViewContext.HttpContext.RequestServices.GetService(typeof(IUrlHelperFactory));
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(htmlHelper.ViewContext);

            // Get button HTML
            GetButtonHtml(urlHelper, sb, button);

            // Close list item element
            sb.Append("</li>");

            // Return HTML as MvcHtmlString
            return new HtmlString(sb.ToString());
        }
    }
}

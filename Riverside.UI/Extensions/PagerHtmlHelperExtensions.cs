using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Riverside.UI.Controls;
using Riverside.UI.Resources;
using Riverside.UI.Routes;

namespace Riverside.UI.Extensions
{
    public static class PagerHtmlHelperExtensions
    {
        private const int RollingButtonsSize = 2;

        private static UrlParameters CopyUrlParameters(UrlParameters urlParameters, int page)
        {
            RouteValueDictionary routeValueDictionary = (urlParameters == null || urlParameters.RouteValues == null) ? new RouteValueDictionary() : new RouteValueDictionary(urlParameters.RouteValues);
            if (page > 1)
                routeValueDictionary.Add("page", page);
            return new UrlParameters
            {
                ActionName = urlParameters == null ? null : urlParameters.ActionName,
                ControllerName = urlParameters == null ? null : urlParameters.ControllerName,
                Fragment = urlParameters == null ? null : urlParameters.Fragment,
                HostName = urlParameters == null ? null : urlParameters.HostName,
                Protocol = urlParameters == null ? null : urlParameters.Protocol,
                RouteName = urlParameters == null ? null : urlParameters.RouteName,
                RouteValues = routeValueDictionary
            };
        }

        private static List<Button> ListPagerButtons(Pager pager)
        {
            // List of buttons that will be returned
            List<Button> buttons = new List<Button>();

            // If there are multiple pages, then get buttons to display on pager
            if (pager.PageCount > 1)
            {
                // Get 1-based page index
                int activePageIndex = pager.PageIndex;

                // First and previous buttons always exist, but might be disabled
                buttons.Add(new Button { Icon = "fa-angle-double-left", Text = string.Format("<span class=\"pager-button-label\">{0}</span>", PagerResource.FirstButtonLabel), UrlParameters = CopyUrlParameters(pager.UrlParameters, 1), State = activePageIndex == 1 ? ButtonState.Disabled : ButtonState.Enabled });
                buttons.Add(new Button { Icon = "fa-angle-left", Text = string.Format("<span class=\"pager-button-label\">{0}</span>", PagerResource.PreviousButtonLabel), UrlParameters = CopyUrlParameters(pager.UrlParameters, Math.Max(activePageIndex - 1, 1)), State = activePageIndex == 1 ? ButtonState.Disabled : ButtonState.Enabled });

                // Add rolling buttons either side of current active page
                int beginPageIndex = Math.Max(activePageIndex - RollingButtonsSize, 1);
                int endPageIndex = Math.Min(activePageIndex + RollingButtonsSize, pager.PageCount);
                for (int pageIndex = beginPageIndex; pageIndex <= endPageIndex; pageIndex++)
                {
                    string buttonText = null;
                    if (pageIndex == activePageIndex)
                        buttonText = pageIndex.ToString() + " <span class=\"sr-only\">" + PagerResource.CurrentButtonLabel + "</span>";
                    else
                        buttonText = pageIndex.ToString();
                    buttons.Add(new Button { Text = buttonText, UrlParameters = CopyUrlParameters(pager.UrlParameters, pageIndex), State = pageIndex == activePageIndex ? ButtonState.Active : ButtonState.Enabled });
                }

                // Next button always exists, but might be disabled
                buttons.Add(new Button { Icon = "fa-angle-right", IconRight = true, Text = string.Format("<span class=\"pager-button-label\">{0}</span>", PagerResource.NextButtonLabel), UrlParameters = CopyUrlParameters(pager.UrlParameters, Math.Min(activePageIndex + 1, pager.PageCount)), State = activePageIndex == pager.PageCount ? ButtonState.Disabled : ButtonState.Enabled });

                // Last button
                buttons.Add(new Button { Icon = "fa-angle-double-right", IconRight = true, Text = string.Format("<span class=\"pager-button-label\">{0}</span>", PagerResource.LastButtonLabel), UrlParameters = CopyUrlParameters(pager.UrlParameters, pager.PageCount), State = activePageIndex == pager.PageCount ? ButtonState.Disabled : ButtonState.Enabled });
            }

            // Return pager buttons
            return buttons;
        }

        public static IHtmlContent Pager<TModel>(this IHtmlHelper<TModel> htmlHelper, Pager pager)
        {
            // Create string builder for storing HTML
            StringBuilder sb = new StringBuilder();

            // Get pager buttons
            List<Button> buttons = ListPagerButtons(pager);

            // Only output HTML if there are any buttons
            if (buttons.Count > 0)
            {
                // Use tag builder for pager root element
                sb.Append("<ul class=\"pagination\">");

                // Generate pager buttons HTML
                foreach (Button button in buttons)
                    sb.Append(htmlHelper.ButtonListItem(button));

                // Close pager root element
                sb.Append("</ul>");
            }

            // Return HTML as MvcHtmlString
            return new HtmlString(sb.ToString());
        }

        public static IHtmlContent PagerText<TModel>(this IHtmlHelper<TModel> htmlHelper, Pager pager)
        {
            // Create string builder for storing HTML
            StringBuilder sb = new StringBuilder();

            // Work out whether we need to display the pages text. i.e. are there multiple pages to display?
            if (pager.PageCount > 1)
            {
                // Use tag builder for pages root element
                sb.Append("<p>");

                // Get pager text
                sb.Append(string.Format(PagerResource.PageAndCountText, pager.PageIndex, pager.PageCount));

                // End paragraph tag
                sb.Append("</p>");
            }

            // Return HTML as MvcHtmlString
            return new HtmlString(sb.ToString());
        }
    }
}

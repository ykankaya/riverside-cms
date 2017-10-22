using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Riverside.Cms.Core.Pages;
using Riverside.UI.Web;

namespace Riverside.Cms.Core.Extensions
{
    /// <summary>
    /// Contains HTML helper extensions.
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Renders page link.
        /// </summary>
        /// <param name="htmlHelper">This is an HtmlHelper extension method.</param>
        /// <param name="pageLinkViewModel">Contains all of the information required to render a page link.</param>
        /// <returns>MvcHtmlString containing page link.</returns>
        public static IHtmlContent PageLink(this IHtmlHelper htmlHelper, PageLinkViewModel pageLink)
        {
            // If page ID not specified, then render link to the current page (this might be the case on a special administration page, where PageId is not set)
            if (pageLink.Page.PageId == 0)
            {
                string pathAndQuery = string.Format("{0}{1}", htmlHelper.ViewContext.HttpContext.Request.Path, htmlHelper.ViewContext.HttpContext.Request.QueryString);
                string pageLinkHtml = string.Format("<a href=\"{0}\">{1}</a>", pathAndQuery, htmlHelper.Encode(pageLink.LinkText));
                return new HtmlString(pageLinkHtml);
            }

            // Otherwise, render link to page specified
            if (pageLink.Page.ParentPageId == null)
            {
                return htmlHelper.RouteLink(pageLink.LinkText, "HomePage", pageLink.RouteValues, pageLink.HtmlAttributes);
            }
            else
            {
                IWebHelperService webHelperService = (IWebHelperService)htmlHelper.ViewContext.HttpContext.RequestServices.GetService(typeof(IWebHelperService));
                RouteValueDictionary routeValueDictionary = pageLink.RouteValues == null ? new RouteValueDictionary() : new RouteValueDictionary(pageLink.RouteValues);
                RouteValueDictionary htmlAttributesDictionary = pageLink.HtmlAttributes == null ? new RouteValueDictionary() : new RouteValueDictionary(pageLink.HtmlAttributes);
                routeValueDictionary.Add("pageid", pageLink.Page.PageId);
                routeValueDictionary.Add("description", webHelperService.UrlFriendly(pageLink.Description));
                return htmlHelper.RouteLink(pageLink.LinkText, "ReadPage", routeValueDictionary, (IDictionary<string, object>)htmlAttributesDictionary);
            }
        }

        /// <summary>
        /// Renders page link.
        /// </summary>
        /// <param name="htmlHelper">This is an HtmlHelper extension method.</param>
        /// <param name="page">The page whose link is rendered.</param>
        /// <param name="linkText">Text that should appear on link.</param>
        /// <param name="description">SEO text that is displayed just before query string.</param>
        /// <param name="routeValues">Route values that should be added to the link.</param>
        /// <param name="htmlAttributes">Html attributes that should be applied to anchor tag.</param>
        /// <returns>MvcHtmlString containing page link.</returns>
        public static IHtmlContent PageLink(this IHtmlHelper htmlHelper, Page page, string linkText, string description, object routeValues, object htmlAttributes = null)
        {
            // Construct page link info from method arguments
            PageLinkViewModel pageLink = new PageLinkViewModel {
                Description    = description,
                HtmlAttributes = htmlAttributes,
                LinkText       = linkText,
                Page           = page,
                RouteValues    = routeValues
            };

            // And return resulting HTML
            return PageLink(htmlHelper, pageLink);
        }

        /// <summary>
        /// Renders page link.
        /// </summary>
        /// <param name="htmlHelper">This is an HtmlHelper extension method.</param>
        /// <param name="page">The page whose link is rendered.</param>
        /// <param name="linkText">Text that should appear on link.</param>
        /// <param name="routeValues">Route values that should be added to the link.</param>
        /// <param name="htmlAttributes">Html attributes that should be applied to anchor tag.</param>
        /// <returns>MvcHtmlString containing page link.</returns>
        public static IHtmlContent PageLink(this IHtmlHelper htmlHelper, Page page, string linkText, object routeValues, object htmlAttributes = null)
        {
            return PageLink(htmlHelper, page, linkText, page.Name, routeValues, htmlAttributes);
        }

        /// <summary>
        /// Renders page link.
        /// </summary>
        /// <param name="htmlHelper">This is an HtmlHelper extension method.</param>
        /// <param name="page">The page whose link is rendered.</param>
        /// <param name="linkText">Text that should appear on link.</param>
        /// <param name="htmlAttributes">Html attributes that should be applied to anchor tag.</param>
        /// <returns>MvcHtmlString containing page link.</returns>
        public static IHtmlContent PageLink(this IHtmlHelper htmlHelper, Page page, string linkText, object htmlAttributes = null)
        {
            return PageLink(htmlHelper, page, linkText, page.Name, null, htmlAttributes);
        }

        /// <summary>
        /// Renders page link.
        /// </summary>
        /// <param name="htmlHelper">This is an HtmlHelper extension method.</param>
        /// <param name="page">The page whose link is rendered.</param>
        /// <param name="htmlAttributes">Html attributes that should be applied to anchor tag.</param>
        /// <returns>MvcHtmlString containing page link.</returns>
        public static IHtmlContent PageLink(this IHtmlHelper htmlHelper, Page page, object htmlAttributes = null)
        {
            return PageLink(htmlHelper, page, page.Name, page.Name, null, htmlAttributes);
        }
    }
}

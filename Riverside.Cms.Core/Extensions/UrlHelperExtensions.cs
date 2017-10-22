using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Riverside.Cms.Core.Administration;
using Riverside.Cms.Core.Pages;
using Riverside.UI.Extensions;
using Riverside.UI.Web;

namespace Riverside.Cms.Core.Extensions
{
    /// <summary>
    /// UrlHelper extensions for rendering SEO friendly URLs.
    /// </summary>
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Gets URL for an admin action.
        /// </summary>
        /// <param name="urlHelper">This is a UrlHelper extension method.</param>
        /// <param name="action">The administration action whose URL is returned.</param>
        /// <param name="routeValues">Route values that should be added to the URL.</param>
        /// <returns>String containing admin URL.</returns>
        public static string AdminUrl(this IUrlHelper urlHelper, AdministrationAction action, object routeValues)
        {
            switch (action)
            {
                case AdministrationAction.UpdateTheme:
                    return urlHelper.RouteUrl("AdminUpdateTheme", routeValues);

                case AdministrationAction.CreatePage:
                    return urlHelper.RouteUrl("AdminCreatePage", routeValues);

                case AdministrationAction.CreateMasterPage:
                    return urlHelper.RouteUrl("AdminCreateMasterPage", routeValues);

                case AdministrationAction.UpdatePage:
                    return urlHelper.RouteUrl("AdminUpdatePage", routeValues);

                case AdministrationAction.UpdateMasterPage:
                    return urlHelper.RouteUrl("AdminUpdateMasterPage", routeValues);

                case AdministrationAction.UpdateMasterPageZone:
                    return urlHelper.RouteUrl("AdminUpdateMasterPageZone", routeValues);

                case AdministrationAction.UpdateMasterPageZones:
                    return urlHelper.RouteUrl("AdminUpdateMasterPageZones", routeValues);

                case AdministrationAction.UpdatePageElement:
                    return urlHelper.RouteUrl("AdminUpdatePageElement", routeValues);

                case AdministrationAction.UpdateMasterPageElement:
                    return urlHelper.RouteUrl("AdminUpdateMasterPageElement", routeValues);

                case AdministrationAction.UpdatePageZone:
                    return urlHelper.RouteUrl("AdminUpdatePageZone", routeValues);

                case AdministrationAction.LogonUser:
                    return urlHelper.RouteUrl("LogonUser", routeValues);

                case AdministrationAction.LogoffUser:
                    return urlHelper.RouteUrl("LogoffUser", routeValues);

                case AdministrationAction.CreateUser:
                    return urlHelper.RouteUrl("CreateUser", routeValues);

                case AdministrationAction.ChangePassword:
                    return urlHelper.RouteUrl("ChangePassword", routeValues);

                case AdministrationAction.UpdateUser:
                    return urlHelper.RouteUrl("UpdateUser", routeValues);

                case AdministrationAction.ForgottenPassword:
                    return urlHelper.RouteUrl("ForgottenPassword", routeValues);

                default:
                    return null;
            }
        }

        /// <summary>
        /// Gets URL for an admin action.
        /// </summary>
        /// <param name="urlHelper">This is a UrlHelper extension method.</param>
        /// <param name="action">The administration action whose URL is returned.</param>
        /// <returns>String containing admin URL.</returns>
        public static string AdminUrl(this IUrlHelper urlHelper, AdministrationAction action)
        {
            return AdminUrl(urlHelper, action, null);
        }

        /// <summary>
        /// Renders admin URL.
        /// </summary>
        /// <param name="urlHelper">This is a UrlHelper extension method.</param>
        /// <param name="option">The administration option whose URL is rendered.</param>
        /// <returns>String containing admin URL.</returns>
        public static string AdminUrl(this IUrlHelper urlHelper, IAdministrationOption option)
        {
            string returnUrl = string.Format("{0}{1}", urlHelper.ActionContext.HttpContext.Request.Path, urlHelper.ActionContext.HttpContext.Request.QueryString);
            switch (option.Action)
            {
                case AdministrationAction.UpdateTheme:
                    return AdminUrl(urlHelper, option.Action, null);

                case AdministrationAction.CreatePage:
                    return AdminUrl(urlHelper, option.Action, new { masterPageId = ((CreatePageOption)option).MasterPageId });

                case AdministrationAction.CreateMasterPage:
                    return AdminUrl(urlHelper, option.Action, new { returnurl = returnUrl });

                case AdministrationAction.UpdatePage:
                    return AdminUrl(urlHelper, option.Action, new { pageId = ((UpdatePageOption)option).PageId });

                case AdministrationAction.UpdateMasterPage:
                    return AdminUrl(urlHelper, option.Action, new { masterPageId = ((UpdateMasterPageOption)option).MasterPageId, returnurl = returnUrl });

                case AdministrationAction.UpdatePageElement:
                    return AdminUrl(urlHelper, option.Action, new { pageId = ((UpdatePageElementOption)option).PageId, elementId = ((UpdatePageElementOption)option).ElementId, returnurl = returnUrl });

                case AdministrationAction.UpdateMasterPageElement:
                    return AdminUrl(urlHelper, option.Action, new { masterPageId = ((UpdateMasterPageElementOption)option).MasterPageId, elementId = ((UpdateMasterPageElementOption)option).ElementId, returnurl = returnUrl });

                case AdministrationAction.UpdatePageZone:
                    return AdminUrl(urlHelper, option.Action, new { pageId = ((UpdatePageZoneOption)option).PageId, pageZoneId = ((UpdatePageZoneOption)option).PageZoneId });

                case AdministrationAction.UpdateMasterPageZone:
                    return AdminUrl(urlHelper, option.Action, new { masterPageId = ((UpdateMasterPageZoneOption)option).MasterPageId, masterPageZoneId = ((UpdateMasterPageZoneOption)option).MasterPageZoneId, returnurl = returnUrl });

                case AdministrationAction.UpdateMasterPageZones:
                    return AdminUrl(urlHelper, option.Action, new { masterPageId = ((UpdateMasterPageZonesOption)option).MasterPageId, returnurl = returnUrl });

                default:
                    return AdminUrl(urlHelper, option.Action, null);
            }
        }

        /// <summary>
        /// Renders a page URL with route values.
        /// </summary>
        /// <param name="urlHelper">This is a UrlHelper extension method.</param>
        /// <param name="page">The page whose link is rendered.</param>
        /// <param name="description">SEO text that is displayed just before query string.</param>
        /// <param name="routeValues">Route values that should be added to the link.</param>
        /// <returns>String containing page URL.</returns>
        public static string PageUrl(this IUrlHelper urlHelper, Page page, string description, object routeValues)
        {
            IWebHelperService webHelperService = (IWebHelperService)urlHelper.ActionContext.HttpContext.RequestServices.GetService(typeof(IWebHelperService));
            RouteValueDictionary routeValueDictionary = routeValues == null ? new RouteValueDictionary() : new RouteValueDictionary(routeValues);
            routeValueDictionary.Add("pageid", page.PageId);
            routeValueDictionary.Add("description", webHelperService.UrlFriendly(description));
            return urlHelper.RouteUrl("ReadPage", routeValueDictionary);
        }

        /// <summary>
        /// Renders a page URL with route values.
        /// </summary>
        /// <param name="urlHelper">This is a UrlHelper extension method.</param>
        /// <param name="page">The page whose link is rendered.</param>
        /// <param name="routeValues">Route values that should be added to the link.</param>
        /// <returns>String containing page URL.</returns>
        public static string PageUrl(this IUrlHelper urlHelper, Page page, object routeValues)
        {
            return PageUrl(urlHelper, page, page.Name, routeValues);
        }

        /// <summary>
        /// Renders a page URL.
        /// </summary>
        /// <param name="urlHelper">This is a UrlHelper extension method.</param>
        /// <param name="page">The page whose link is rendered.</param>
        /// <returns>String containing page URL.</returns>
        public static string PageUrl(this IUrlHelper urlHelper, Page page)
        {
            return PageUrl(urlHelper, page, null);
        }
    }
}

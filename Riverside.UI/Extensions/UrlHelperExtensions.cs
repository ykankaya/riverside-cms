using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Riverside.UI.Routes;
using Riverside.UI.Web;

namespace Riverside.UI.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string UrlFriendly(this IUrlHelper urlHelper, string text)
        {
            IWebHelperService webHelperService = (IWebHelperService)urlHelper.ActionContext.HttpContext.RequestServices.GetService(typeof(IWebHelperService));
            return webHelperService.UrlFriendly(text);
        }

        public static string GetUrl(this IUrlHelper urlHelper, UrlParameters urlParameters)
        {
            IWebHelperService webHelperService = (IWebHelperService)urlHelper.ActionContext.HttpContext.RequestServices.GetService(typeof(IWebHelperService));
            return webHelperService.GetUrl(urlParameters);
        }

        public static string GetUrl(this IUrlHelper urlHelper, UrlParameters urlParameters, object routeValues)
        {
            IWebHelperService webHelperService = (IWebHelperService)urlHelper.ActionContext.HttpContext.RequestServices.GetService(typeof(IWebHelperService));
            return webHelperService.GetUrl(urlParameters, routeValues);
        }
    }
}

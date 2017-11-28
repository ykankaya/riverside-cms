using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Riverside.UI.Routes;

namespace Riverside.UI.Web
{
    public class WebHelperService : IWebHelperService
    {
        private IActionContextAccessor _actionContextAccessor;
        private IHostingEnvironment _hostingEnvironment;
        private IHttpContextAccessor _httpContextAccessor;
        private IUrlHelperFactory _urlHelperFactory;

        private const string VirtualPathPrefix = "~/";

        public WebHelperService(IActionContextAccessor actionContextAccessor, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, IUrlHelperFactory urlHelperFactory)
        {
            _actionContextAccessor = actionContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _urlHelperFactory = urlHelperFactory;
        }

        public string MapPath(string path)
        {
            if (path.StartsWith(VirtualPathPrefix))
                path = path.Substring(VirtualPathPrefix.Length);
            string webRootPath = _hostingEnvironment.WebRootPath;
            return Path.Combine(webRootPath, path);
        }

        public string ContentPath(string path)
        {
            if (path.StartsWith(VirtualPathPrefix))
                path = path.Substring(VirtualPathPrefix.Length);
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            return Path.Combine(contentRootPath, path);
        }

        public void SetItem<T>(string key, T value)
        {
            _httpContextAccessor.HttpContext.Items[key] = value;
        }

        public T GetItem<T>(string key)
        {
            if (_httpContextAccessor.HttpContext.Items.ContainsKey(key))
                return (T)_httpContextAccessor.HttpContext.Items[key];
            return default(T);
        }

        public IDictionary<object, object> GetItems()
        {
            return _httpContextAccessor.HttpContext.Items;
        }

        public string GetRequestScheme()
        {
            return _httpContextAccessor.HttpContext.Request.Scheme;
        }

        public string GetRequestHost()
        {
            return _httpContextAccessor.HttpContext.Request.Host.Host;
        }

        public string GetRequestUrl()
        {
            return string.Format("{0}://{1}{2}{3}",
                _httpContextAccessor.HttpContext.Request.Scheme,
                _httpContextAccessor.HttpContext.Request.Host,
                _httpContextAccessor.HttpContext.Request.Path,
                _httpContextAccessor.HttpContext.Request.QueryString
            );
        }

        public string RouteUrl(string routeName)
        {
            UrlRouteContext context = new UrlRouteContext
            {
                RouteName = routeName
            };
            IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);
            return urlHelper.RouteUrl(context);
        }

        public string RouteUrl(string routeName, object routeValues)
        {
            UrlRouteContext context = new UrlRouteContext
            {
                RouteName = routeName,
                Values = routeValues
            };
            IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);
            return urlHelper.RouteUrl(context);
        }

        public void CreateCookie(string name, string value, DateTime? expires)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Append(name, value, new CookieOptions { Expires = expires });
        }

        public void DeleteCookie(string name)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(name);
        }

        public string GetCookie(string name)
        {
            if (!_httpContextAccessor.HttpContext.Request.Cookies.ContainsKey(name))
                return null;
            return _httpContextAccessor.HttpContext.Request.Cookies[name];
        }

        public ClaimsPrincipal GetCurrentUser()
        {
            if (_httpContextAccessor.HttpContext.User != null && _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                return _httpContextAccessor.HttpContext.User;
            return null;
        }

        public void UpdateCurrentUser(ClaimsPrincipal principal)
        {
            _httpContextAccessor.HttpContext.User = principal;
        }

        private string GetActionUrl(UrlParameters urlParameters, object routeValues)
        {
            UrlActionContext actionContext = new UrlActionContext
            {
                Action = urlParameters.ActionName,
                Controller = urlParameters.ControllerName,
                Fragment = urlParameters.Fragment,
                Host = urlParameters.HostName,
                Protocol = urlParameters.Protocol,
                Values = routeValues != null ? routeValues : urlParameters.RouteValues
            };
            IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);
            return urlHelper.Action(actionContext);
        }

        private string GetRouteUrl(UrlParameters urlParameters, object routeValues)
        {
            UrlRouteContext routeContext = new UrlRouteContext
            {
                Fragment = urlParameters.Fragment,
                Host = urlParameters.HostName,
                Protocol = urlParameters.Protocol,
                RouteName = urlParameters.RouteName,
                Values = routeValues != null ? routeValues : urlParameters.RouteValues
            };
            IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);
            return urlHelper.RouteUrl(routeContext);
        }

        public string GetUrl(UrlParameters urlParameters, object routeValues)
        {
            if (urlParameters.IsRoute)
                return GetRouteUrl(urlParameters, routeValues);
            else
                return GetActionUrl(urlParameters, routeValues);
        }

        public string GetUrl(UrlParameters urlParameters)
        {
            if (urlParameters.IsRoute)
                return GetRouteUrl(urlParameters, null);
            else
                return GetActionUrl(urlParameters, null);
        }

        /// <summary>
        /// Credit: http://meta.stackoverflow.com/questions/7435/non-us-ascii-characters-dropped-from-full-profile-url (Jeff Atwood's answer)
        /// </summary>
        /// <param name="c">Character.</param>
        /// <returns>ASCII version of character.</returns>
        private string RemapInternationalCharToAscii(char c)
        {
            string s = c.ToString().ToLowerInvariant();
            if ("àåáâäãåą".Contains(s))
            {
                return "a";
            }
            else if ("èéêëę".Contains(s))
            {
                return "e";
            }
            else if ("ìíîïı".Contains(s))
            {
                return "i";
            }
            else if ("òóôõöøőð".Contains(s))
            {
                return "o";
            }
            else if ("ùúûüŭů".Contains(s))
            {
                return "u";
            }
            else if ("çćčĉ".Contains(s))
            {
                return "c";
            }
            else if ("żźž".Contains(s))
            {
                return "z";
            }
            else if ("śşšŝ".Contains(s))
            {
                return "s";
            }
            else if ("ñń".Contains(s))
            {
                return "n";
            }
            else if ("ýÿ".Contains(s))
            {
                return "y";
            }
            else if ("ğĝ".Contains(s))
            {
                return "g";
            }
            else if (c == 'ř')
            {
                return "r";
            }
            else if (c == 'ł')
            {
                return "l";
            }
            else if (c == 'đ')
            {
                return "d";
            }
            else if (c == 'ß')
            {
                return "ss";
            }
            else if (c == 'Þ')
            {
                return "th";
            }
            else if (c == 'ĥ')
            {
                return "h";
            }
            else if (c == 'ĵ')
            {
                return "j";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Returns URL safe and SEO friendly version of string.
        /// Credit: http://stackoverflow.com/questions/25259/how-does-stack-overflow-generate-its-seo-friendly-urls (Jeff Atwood's answer)
        /// </summary>
        /// <param name="text">Text whose URL safe and SEO friendly version is returned.</param>
        /// <returns>URL safe and SEO friendly version of string.</returns>
        public string UrlFriendly(string text)
        {
            if (text == null)
                return "";
            const int maxlen = 80;
            int len = text.Length;
            bool prevdash = false;
            var sb = new StringBuilder(len);
            char c;

            for (int i = 0; i < len; i++)
            {
                c = text[i];
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    sb.Append(c);
                    prevdash = false;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    // Tricky way to convert to lowercase
                    sb.Append((char)(c | 32));
                    prevdash = false;
                }
                else if (c == ' ' || c == ',' || c == '.' || c == '/' ||
                    c == '\\' || c == '-' || c == '_' || c == '=')
                {
                    if (!prevdash && sb.Length > 0)
                    {
                        sb.Append('-');
                        prevdash = true;
                    }
                }
                else if ((int)c >= 128)
                {
                    int prevlen = sb.Length;
                    sb.Append(RemapInternationalCharToAscii(c));
                    if (prevlen != sb.Length) prevdash = false;
                }
                if (i == maxlen) break;
            }

            if (prevdash)
                return sb.ToString().Substring(0, sb.Length - 1);
            else
                return sb.ToString();
        }

        public string MachineName()
        {
            return Environment.MachineName;
        }

        /// <summary>
        /// Returns root URL of current request. For example, the URI "http://localhost:7823/article/1" has root URI "http://localhost:7823".
        /// </summary>
        /// <returns>Root URL of current request.</returns>
        public string GetRootUrl()
        {
            return string.Format("{0}://{1}", _httpContextAccessor.HttpContext.Request.Scheme, _httpContextAccessor.HttpContext.Request.Host);
        }

        /// <summary>
        /// Performs a redirect (returns HTTP status code 302) from the requested URL to the specified URL.
        /// </summary>
        /// <param name="url">The location to redirect the request to.</param>
        public void Redirect(string url)
        {
            _httpContextAccessor.HttpContext.Response.Redirect(url);
        }

        public IQueryCollection Query()
        {
            return _httpContextAccessor.HttpContext.Request.Query;
        }

        public RouteValueDictionary GetRouteValueDictionary()
        {
            RouteData routeData = _httpContextAccessor.HttpContext.GetRouteData();
            return routeData.Values;
        }

        public string HtmlEncode(string text)
        {
            return System.Net.WebUtility.HtmlEncode(text);
        }

        public string UrlEncode(string text)
        {
            return System.Net.WebUtility.UrlEncode(text);
        }

        public string FormatMultiLine(string text)
        {
            return text.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "<br />");
        }

        public string GetHostFromUrl(string url, bool removeWww)
        {
            Uri uri = new Uri(url);
            string host = uri.Host;
            const string wwwPrefix = "www.";
            if (removeWww && host.StartsWith(wwwPrefix))
                host = host.Substring(wwwPrefix.Length);
            return host;
        }
    }
}

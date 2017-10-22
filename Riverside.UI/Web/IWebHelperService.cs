using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Riverside.UI.Routes;
using Riverside.Utilities.Http;

namespace Riverside.UI.Web
{
    public interface IWebHelperService : IEncodeDecodeService
    {
        /// <summary>
        /// Returns the physical file path that corresponds to the specified virtual path on the web server.
        /// </summary>
        /// <param name="path">Path like "~/webs".</param>
        /// <returns>Physical file path.</returns>
        string MapPath(string path);

        /// <summary>
        /// Sets item in key / value collection that can be used to share data between an IHttpModule and an IHttpHandler interface during an HTTP request.
        /// </summary>
        /// <typeparam name="T">The type of the object stored.</typeparam>
        /// <param name="key">Key identifying object stored.</param>
        /// <param name="value">The object stored.</param>
        void SetItem<T>(string key, T value);

        /// <summary>
        /// Gets item in key / value collection that can be used to share data between an IHttpModule and an IHttpHandler interface during an HTTP request.
        /// </summary>
        /// <typeparam name="T">The type of the object stored.</typeparam>
        /// <param name="key">Key identifying object stored.</param>
        /// <returns>The object (or default value of T if object not found).</returns>
        T GetItem<T>(string key);

        /// <summary>
        /// Gets items collection that can be used to share data between an IHttpModule and an IHttpHandler interface during an HTTP request.
        /// </summary>
        /// <returns>The items collection.</returns>
        IDictionary<object, object> GetItems();

        /// <summary>
        /// Gets the HTTP request scheme.
        /// </summary>
        /// <returns>HTTP request scheme.</returns>
        string GetRequestScheme();

        /// <summary>
        /// Gets the HTTP request host.
        /// </summary>
        /// <returns>HTTP request host.</returns>
        string GetRequestHost();

        /// <summary>
        /// Get absolute URI for page request.
        /// </summary>
        /// <returns>Complete page URL.</returns>
        string GetRequestUrl();

        /// <summary>
        /// Generates a URL with an absolute path, which contains a route name.
        /// </summary>
        /// <param name="routeName">Route name.</param>
        /// <returns>URL.</returns>
        string RouteUrl(string routeName);

        /// <summary>
        /// Generates a URL with an absolute path, which contains a route name and route values.
        /// </summary>
        /// <param name="routeName">Route name.</param>
        /// <param name="routeValues">Route values.</param>
        /// <returns>URL.</returns>
        string RouteUrl(string routeName, object routeValues);

        /// <summary>
        /// Creates a new cookie. Used to create cookies server-side, which are then transmitted to the client in the "Set-Cookie" header.
        /// </summary>
        /// <param name="name">Cookie name.</param>
        /// <param name="value">Cookie value.</param>
        /// <param name="expires">Cookie expiry.</param>
        void CreateCookie(string name, string value, DateTime? expires);

        /// <summary>
        /// Deletes a named cookie.
        /// </summary>
        /// <param name="name">Cookie name.</param>
        void DeleteCookie(string name);

        /// <summary>
        /// Retrieves a cookie value (or null if no cookie found).
        /// </summary>
        /// <param name="name">Name of cookie to retrieve.</param>
        /// <returns>Cookie value (null if cookie not found).</returns>
        string GetCookie(string name);

        /// <summary>
        /// Gets logged on user.
        /// </summary>
        /// <returns>Principal of logged on user.</returns>
        ClaimsPrincipal GetCurrentUser();

        /// <summary>
        /// Updates logged on user.
        /// </summary>
        /// <param name="principal">Principal of user.</param>
        /// <returns>Principal of logged on user.</returns>
        void UpdateCurrentUser(ClaimsPrincipal principal);

        /// <summary>
        /// Gets URL given url parameters.
        /// </summary>
        /// <param name="urlParameters">URL parameters.</param>
        /// <returns>URL.</returns>
        string GetUrl(UrlParameters urlParameters);

        /// <summary>
        /// Gets URL given url parameters.
        /// </summary>
        /// <param name="urlParameters">URL parameters.</param>
        /// <param name="routeValues">Route values.</param>
        /// <returns>URL.</returns>
        string GetUrl(UrlParameters urlParameters, object routeValues);

        /// <summary>
        /// Returns URL safe and SEO friendly version of string.
        /// </summary>
        /// <param name="text">Text whose URL safe and SEO friendly version is returned.</param>
        /// <returns>URL safe and SEO friendly version of string.</returns>
        string UrlFriendly(string text);

        /// <summary>
        /// Returns name of machine hosting website.
        /// </summary>
        /// <returns>Server machine name.</returns>
        string MachineName();

        /// <summary>
        /// Returns root URL of current request. For example, the URI "http://localhost:7823/article/1" has root URI "http://localhost:7823".
        /// </summary>
        /// <returns>Root URL of current request.</returns>
        string GetRootUrl();

        /// <summary>
        /// Performs a redirect (returns HTTP status code 302) from the requested URL to the specified URL.
        /// </summary>
        /// <param name="url">The location to redirect the request to.</param>
        void Redirect(string url);

        /// <summary>
        /// Gets query string.
        /// </summary>
        /// <returns>Query collection.</returns>
        IQueryCollection Query();

        /// <summary>
        /// Gets route values for current request.
        /// </summary>
        /// <returns>Route value dictionary.</returns>
        RouteValueDictionary GetRouteValueDictionary();

        /// <summary>
        /// Ensures carriage returns in specified text string are rendered as a line breaks.
        /// </summary>
        string FormatMultiLine(string text);

        /// <summary>
        /// From a URL like "http://www.example.com/about?test=example", returns the host "example.com" if removeWww is true or 
        /// "www.example.com" if removeWww is false.
        /// </summary>
        /// <param name="url">The URL whose host is returned.</param>
        /// <param name="removeWww">Set true to remove "www" from the beginning of host.</param>
        /// <returns>Host component of URL.</returns>
        string GetHostFromUrl(string url, bool removeWww);
    }
}

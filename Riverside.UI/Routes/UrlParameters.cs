using System;
using System.Collections.Generic;
using System.Text;
using Riverside.Utilities.Http;

namespace Riverside.UI.Routes
{
    /// <summary>
    /// Contains variables used to generate URLs.
    /// </summary>
    public class UrlParameters
    {
        /// <summary>
        /// Indicates whether this parameters object specifies an action or route URL. If a route name is specified (not null),
        /// then this object specifies a route URL.
        /// </summary>
        public bool IsRoute
        {
            get
            {
                return RouteName != null;
            }
        }

        /// <summary>
        /// The name of the route that may be used to generate the URL.
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// The name of the controller action that may be used to generate the URL.
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// The name of the controller that may be used to generate the URL.
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// Route values used to determine URL.
        /// </summary>
        public object RouteValues { get; set; }

        /// <summary>
        /// The protocol for the URL, such as "http" or "https".
        /// </summary>
        public string Protocol { get; set; }

        /// <summary>
        /// The host name for the URL.
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// The URL fragment name (the anchor name).
        /// </summary>
        public string Fragment { get; set; }
    }
}

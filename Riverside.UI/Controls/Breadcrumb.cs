using System;
using System.Collections.Generic;
using System.Text;
using Riverside.UI.Routes;
using Riverside.Utilities.Http;

namespace Riverside.UI.Controls
{
    /// <summary>
    /// Holds information about a breadcrumb. i.e. the name of the breadcrumb and URL parameters determining hyperlink that is rendered.
    /// </summary>
    public class Breadcrumb
    {
        /// <summary>
        /// The name of the breadcrumb that is displayed to the end user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Determines breadcrumb hyperlink.
        /// </summary>
        public UrlParameters UrlParameters { get; set; }

        /// <summary>
        /// Determines order of breadcrumbs. Breadcrumbs with lower sort orders are displayed before breadcrumbs with higher sort orders.
        /// </summary>
        public int SortOrder { get; set; }
    }
}

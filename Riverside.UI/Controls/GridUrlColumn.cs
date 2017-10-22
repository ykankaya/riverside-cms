using System;
using System.Collections.Generic;
using System.Text;
using Riverside.UI.Routes;

namespace Riverside.UI.Controls
{
    /// <summary>
    /// Holds information about a grid column that displays a hyperlink.
    /// </summary>
    public class GridUrlColumn : GridColumn
    {
        /// <summary>
        /// URL parameters determining hyperlink.
        /// </summary>
        public UrlParameters UrlParameters { get; set; }

        /// <summary>
        /// Holds the names of route values and the corresponding names of object properties that are used to populate route values.
        /// </summary>
        public List<RoutePropertyPair> RoutePropertyPairs { get; set; }
    }
}

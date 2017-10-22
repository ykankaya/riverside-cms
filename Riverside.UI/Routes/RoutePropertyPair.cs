using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.UI.Routes
{
    /// <summary>
    /// Holds the name of a route value and the name of an object property that is used to populate the route value.
    /// </summary>
    public class RoutePropertyPair
    {
        /// <summary>
        /// The name of the route value to be populated.
        /// </summary>
        public string RouteValueName { get; set; }

        /// <summary>
        /// The name of the object property whose value is retrieved to populate a route value.
        /// </summary>
        public string PropertyName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Riverside.UI.Controls
{
    /// <summary>
    /// Navigation item view model.
    /// </summary>
    public class NavigationItemViewModel
    {
        /// <summary>
        /// The name of the navigaiton item that is displayed to the end user.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Navigation item hyperlink.
        /// </summary>
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        /// <summary>
        /// Navigation item icon.
        /// </summary>
        [JsonProperty(PropertyName = "icon")]
        public string Icon { get; set; }

        /// <summary>
        /// Flag indicating whether or not this navigation item is active.
        /// </summary>
        [JsonProperty(PropertyName = "active")]
        public bool Active { get; set; }
    }
}

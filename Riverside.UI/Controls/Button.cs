using System;
using System.Collections.Generic;
using System.Text;
using Riverside.UI.Routes;
using Riverside.Utilities.Http;

namespace Riverside.UI.Controls
{
    /// <summary>
    /// Holds information required to render a toolbar button.
    /// </summary>
    public class Button
    {
        /// <summary>
        /// Indicates whether button is enabled, disabled or active (i.e. pressed).
        /// </summary>
        public ButtonState State { get; set; }

        /// <summary>
        /// Text to display on button.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Name of icon to display to the left of button text. Set null for no icon.
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Indicates whether icon should be rendered before or after button text. The default value is false, which means render icon before button text.
        /// </summary>
        public bool IconRight { get; set; }

        /// <summary>
        /// Parameters used to generate URL on button.
        /// </summary>
        public UrlParameters UrlParameters { get; set; }
    }
}

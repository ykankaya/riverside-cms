using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Riverside.UI.Grids
{
    /// <summary>
    /// Contains information required to render a pager.
    /// </summary>
    public class Pager
    {
        /// <summary>
        /// First button label.
        /// </summary>
        [JsonProperty(PropertyName = "firstButtonLabel")]
        public string FirstButtonLabel { get; set; }

        /// <summary>
        /// Previous button label.
        /// </summary>
        [JsonProperty(PropertyName = "previousButtonLabel")]
        public string PreviousButtonLabel { get; set; }

        /// <summary>
        /// Next button label.
        /// </summary>
        [JsonProperty(PropertyName = "nextButtonLabel")]
        public string NextButtonLabel { get; set; }

        /// <summary>
        /// Last button label.
        /// </summary>
        [JsonProperty(PropertyName = "lastButtonLabel")]
        public string LastButtonLabel { get; set; }

        /// <summary>
        /// Summary text (e.g. Page 1 of 2).
        /// </summary>
        [JsonProperty(PropertyName = "summary")]
        public string Summary { get; set; }

        /// <summary>
        /// 1-based index of the current page.
        /// </summary>
        [JsonProperty(PropertyName = "page")]
        public int Page { get; set; }

        /// <summary>
        /// The total page count.
        /// </summary>
        [JsonProperty(PropertyName = "pageCount")]
        public int PageCount { get; set; }
    }
}

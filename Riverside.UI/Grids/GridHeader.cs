using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Riverside.UI.Grids
{
    /// <summary>
    /// Grid header details.
    /// </summary>
    public class GridHeader
    {
        /// <summary>
        /// Grid header label.
        /// </summary>
        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }
    }
}

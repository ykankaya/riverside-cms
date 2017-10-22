using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Riverside.UI.Grids
{
    /// <summary>
    /// Grid of data.
    /// </summary>
    public class Grid
    {
        /// <summary>
        /// The grid rows that make up this grid.
        /// </summary>
        [JsonProperty(PropertyName = "rows")]
        public List<GridRow> Rows { get; set; }

        /// <summary>
        /// The headers at the top of each grid column.
        /// </summary>
        [JsonProperty(PropertyName = "headers")]
        public List<GridHeader> Headers { get; set; }

        /// <summary>
        /// Grid search terms.
        /// </summary>
        [JsonProperty(PropertyName = "search")]
        public string Search { get; set; }

        /// <summary>
        /// Set true if grid is updating. This might be used, for example, to render a visual cue in the UI.
        /// </summary>
        [JsonProperty(PropertyName = "updating")]
        public bool Updating { get; set; }

        /// <summary>
        /// Message that is displayed if the grid has no rows.
        /// </summary>
        [JsonProperty(PropertyName = "emptyMessage")]
        public string EmptyMessage { get; set; }

        /// <summary>
        /// Used to page through grid rows.
        /// </summary>
        [JsonProperty(PropertyName = "pager")]
        public Pager Pager { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Riverside.UI.Grids
{
    /// <summary>
    /// Base class for grid cells.
    /// </summary>
    /// <typeparam name="TValue">Type of underlying cell value.</typeparam>
    public abstract class GridCell<TValue> : IGridCell<TValue>
    {
        /// <summary>
        /// Cell value.
        /// </summary>
        [JsonProperty(PropertyName = "value")]
        public TValue Value { get; set; }

        /// <summary>
        /// When specified, cell rendered within hyperlink.
        /// </summary>
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        /// <summary>
        /// Indicates the type of grid cell.
        /// </summary>
        [JsonProperty(PropertyName = "cellType")]
        public abstract GridCellType GridCellType { get; }

        /// <summary>
        /// Returns grid cell value.
        /// </summary>
        /// <returns>Cell value.</returns>
        public override string ToString()
        {
            return string.Format("{0}", Value);
        }
    }
}

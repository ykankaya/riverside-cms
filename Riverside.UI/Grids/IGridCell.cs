using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Riverside.UI.Grids
{
    /// <summary>
    /// Base interface for all grid cells.
    /// </summary>
    public interface IGridCell
    {
        /// <summary>
        /// URL of cell hyperlink.
        /// </summary>
        [JsonProperty(PropertyName = "url")]
        string Url { get; set; }
    }

    /// <summary>
    /// Interface that all grid cells implement.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying cell value.</typeparam>
    public interface IGridCell<TValue> : IGridCell
    {
        /// <summary>
        /// Cell value.
        /// </summary>
        [JsonProperty(PropertyName = "value")]
        TValue Value { get; set; }
    }
}

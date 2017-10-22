using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Riverside.UI.Grids
{
    /// <summary>
    /// A row of grid cells.
    /// </summary>
    public class GridRow
    {
        /// <summary>
        /// The grid cells that make up this row.
        /// </summary>
        [JsonProperty(PropertyName = "cells")]
        public List<IGridCell> Cells { get; set; }
    }
}

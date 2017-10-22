using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Riverside.UI.Grids
{
    /// <summary>
    /// Text cell.
    /// </summary>
    public class TextCell : GridCell<string>
    {
        /// <summary>
        /// Indicates the type of grid cell.
        /// </summary>
        [JsonProperty(PropertyName = "cellType")]
        public override GridCellType GridCellType { get { return GridCellType.TextCell; } }
    }
}

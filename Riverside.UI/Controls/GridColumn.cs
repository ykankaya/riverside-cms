using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.UI.Controls
{
    /// <summary>
    /// Holds information about a grid column.
    /// </summary>
    public class GridColumn : IGridColumn
    {
        /// <summary>
        /// The user friendly display name of the grid column.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// The name of the grid item property whose value is used to populate cells in this column.
        /// </summary>
        public string ItemPropertyName { get; set; }
    }
}

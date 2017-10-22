using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.UI.Controls
{
    /// <summary>
    /// Interface that all grid columns must implement.
    /// </summary>
    public interface IGridColumn
    {
        /// <summary>
        /// The user friendly display name of the grid column.
        /// </summary>
        string DisplayName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Pages
{
    /// <summary>
    /// Contains page zone element related information.
    /// </summary>
    public class PageZoneElementInfo
    {
        /// <summary>
        /// Identifies an individual page zone element.
        /// </summary>
        public long PageZoneElementId { get; set; }

        /// <summary>
        /// Determines the order in which page zone elements are rendered within a page zone.
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Identifies the type of this element.
        /// </summary>
        public Guid ElementTypeId { get; set; }

        /// <summary>
        /// Name of element.
        /// </summary>
        public string Name { get; set; }
    }
}

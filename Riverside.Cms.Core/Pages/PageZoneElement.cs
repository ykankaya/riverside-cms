using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;

namespace Riverside.Cms.Core.Pages
{
    /// <summary>
    /// Content in a page zone. Note: It does not make sense to foreign key back to master page zone elements in this model, because we may allow additional elements
    /// to be added to a page zone or elements to be removed from a page zone such that there would be no relation to the original master page zone elements.
    /// </summary>
    public class PageZoneElement
    {
        /// <summary>
        /// Identifies website.
        /// </summary>
        public long TenantId { get; set; }

        /// <summary>
        /// The page that this page zone element is associated with.
        /// </summary>
        public long PageId { get; set; }

        /// <summary>
        /// The page zone that this page zone element is associated with.
        /// </summary>
        public long PageZoneId { get; set; }

        /// <summary>
        /// Identifies an individual page zone element.
        /// </summary>
        public long PageZoneElementId { get; set; }

        /// <summary>
        /// When zone configurable, determines the order in which page zone elements are rendered within a page zone.
        /// </summary>
        public int? SortOrder { get; set; }

        /// <summary>
        /// Identifies the type of this element.
        /// </summary>
        public Guid ElementTypeId { get; set; }

        /// <summary>
        /// Unique element identifier.
        /// </summary>
        public long ElementId { get; set; }

        /// <summary>
        /// May contain information about the element that this page zone element references.
        /// </summary>
        public IElementSettings Element { get; set; }

        /// <summary>
        /// The parent page zone.
        /// </summary>
        public PageZone Parent { get; set; }

        /// <summary>
        /// Used to foreign key back to master page zone element that this element is based on (if zone not configurable).
        /// </summary>
        public long? MasterPageId { get; set; }

        /// <summary>
        /// Used to foreign key back to master page zone element that this element is based on (if zone not configurable).
        /// </summary>
        public long? MasterPageZoneId { get; set; }

        /// <summary>
        /// Used to foreign key back to master page zone element that this element is based on (if zone not configurable).
        /// </summary>
        public long? MasterPageZoneElementId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;

namespace Riverside.Cms.Core.MasterPages
{
    /// <summary>
    /// A master page zone element.
    /// </summary>
    public class MasterPageZoneElement
    {
        /// <summary>
        /// The website that this master page zone element belongs to.
        /// </summary>
        public long TenantId { get; set; }

        /// <summary>
        /// The master page that this master page zone element belongs to.
        /// </summary>
        public long MasterPageId { get; set; }

        /// <summary>
        /// The master page zone that this master page zone element belongs to.
        /// </summary>
        public long MasterPageZoneId { get; set; }

        /// <summary>
        /// Uniquely identifies this master page zone element.
        /// </summary>
        public long MasterPageZoneElementId { get; set; }

        /// <summary>
        /// Can be used to determine the master page zone that this element belongs to if master page zone is in process of being created and a
        /// master page zone ID has not yet been assigned.
        /// </summary>
        public int MasterPageZoneSortOrder { get; set; }

        /// <summary>
        /// Determines the position of this element relative to other elements in the parent zone.
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Identifies element instance that will appear in master page zone.
        /// </summary>
        public long ElementId { get; set; }

        /// <summary>
        /// The parent master page zone.
        /// </summary>
        public MasterPageZone Parent { get; set; }

        /// <summary>
        /// Element specific details.
        /// </summary>
        public ElementSettings Element { get; set; }

        /// <summary>
        /// HTML that is rendered before element.
        /// </summary>
        public string BeginRender { get; set; }

        /// <summary>
        /// HTML that is rendered after element.
        /// </summary>
        public string EndRender { get; set; }
    }
}

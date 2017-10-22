using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;

namespace Riverside.Cms.Core.MasterPages
{
    /// <summary>
    /// Contains the type of element that can be inserted into a configurable master page zone.
    /// </summary>
    public class MasterPageZoneElementType
    {
        /// <summary>
        /// The website that this master page zone element type belongs to.
        /// </summary>
        public long TenantId { get; set; }

        /// <summary>
        /// The master page that this master page zone element type belongs to.
        /// </summary>
        public long MasterPageId { get; set; }

        /// <summary>
        /// The master page zone that this master page zone element type belongs to.
        /// </summary>
        public long MasterPageZoneId { get; set; }

        /// <summary>
        /// Identifies type of element that can be inserted into a configrable master page zone.
        /// </summary>
        public Guid ElementTypeId { get; set; }

        /// <summary>
        /// Can be used to determine the master page zone that this element type belongs to if master page zone is in process of being created and a
        /// master page zone ID has not yet been assigned.
        /// </summary>
        public int MasterPageZoneSortOrder { get; set; }

        /// <summary>
        /// Element type details.
        /// </summary>
        public ElementType ElementType { get; set; }
    }
}

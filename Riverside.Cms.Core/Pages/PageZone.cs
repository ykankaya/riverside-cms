using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.MasterPages;

namespace Riverside.Cms.Core.Pages
{
    /// <summary>
    /// Contains page zone elements for a given page.
    /// </summary>
    public class PageZone
    {
        /// <summary>
        /// Identifies website.
        /// </summary>
        public long TenantId { get; set; }

        /// <summary>
        /// The page that this zone is associated with.
        /// </summary>
        public long PageId { get; set; }

        /// <summary>
        /// Uniquely identifies this page zone.
        /// </summary>
        public long PageZoneId { get; set; }

        /// <summary>
        /// The master page that this page zone is based on.
        /// </summary>
        public long MasterPageId { get; set; }

        /// <summary>
        /// The master page zone that this page zone is based on.
        /// </summary>
        public long MasterPageZoneId { get; set; }

        /// <summary>
        /// Page zone elements (the elements associated with this page zone).
        /// </summary>
        public List<PageZoneElement> PageZoneElements { get; set; }

        /// <summary>
        /// The master page zone associated with this page zone. 
        /// </summary>
        public MasterPageZone MasterPageZone { get; set; }
    }
}

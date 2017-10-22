using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.MasterPages
{
    /// <summary>
    /// A zone within a master page.
    /// </summary>
    public class MasterPageZone
    {
        /// <summary>
        /// The website that this page zone belongs to.
        /// </summary>
        public long TenantId { get; set; }

        /// <summary>
        /// The master page that this master page zone belongs to.
        /// </summary>
        public long MasterPageId { get; set; }

        /// <summary>
        /// Uniquely identifies this master page zone.
        /// </summary>
        public long MasterPageZoneId { get; set; }

        /// <summary>
        /// The name of this master page zone.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Determines the order of this master page zone relative to other zones on master page.
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// The zone admin type.
        /// </summary>
        public MasterPageZoneAdminType AdminType { get; set; }

        /// <summary>
        /// The zone content type.
        /// </summary>
        public MasterPageZoneContentType ContentType { get; set; }

        /// <summary>
        /// HTML that is rendered just after start of master page zone.
        /// </summary>
        public string BeginRender { get; set; }

        /// <summary>
        /// HTML that is rendered just before end of master page zone.
        /// </summary>
        public string EndRender { get; set; }

        /// <summary>
        /// The types of element that can be inserted into a configurable master page zone.
        /// </summary>
        public List<MasterPageZoneElementType> MasterPageZoneElementTypes { get; set; }

        /// <summary>
        /// The master page zone elements that make up this master page zone.
        /// </summary>
        public List<MasterPageZoneElement> MasterPageZoneElements { get; set; }
    }
}

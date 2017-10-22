using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Templates
{
    /// <summary>
    /// An element within a template page zone.
    /// </summary>
    public class TemplatePageZoneElement
    {
        /// <summary>
        /// The template that this template page zone element belongs to.
        /// </summary>
        public long TenantId { get; set; }

        /// <summary>
        /// The template page that this template page zone element belongs to.
        /// </summary>
        public long TemplatePageId { get; set; }

        /// <summary>
        /// The template page zone that this template page zone element belongs to.
        /// </summary>
        public long TemplatePageZoneId { get; set; }

        /// <summary>
        /// Uniquely identifies this template page zone element.
        /// </summary>
        public long TemplatePageZoneElementId { get; set; }

        /// <summary>
        /// Determines the position of this element relative to other elements in the parent zone.
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Identifies type of element that will appear in master page zone.
        /// </summary>
        public Guid ElementTypeId { get; set; }

        /// <summary>
        /// Identifies element that will appear in master page zone.
        /// </summary>
        public long ElementId { get; set; }

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

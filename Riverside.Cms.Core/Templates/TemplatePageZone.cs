using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.MasterPages;

namespace Riverside.Cms.Core.Templates
{
    /// <summary>
    /// A zone within a template page.
    /// </summary>
    public class TemplatePageZone
    {
        /// <summary>
        /// The template that this template page zone belongs to.
        /// </summary>
        public long TenantId { get; set; }

        /// <summary>
        /// The template page that this template page zone belongs to.
        /// </summary>
        public long TemplatePageId { get; set; }

        /// <summary>
        /// Uniquely identifies this template page zone.
        /// </summary>
        public long TemplatePageZoneId { get; set; }

        /// <summary>
        /// The name of this template page zone.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Determines the order of this template page zone relative to other zones on template page.
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// The zone admin type that will be used in the master page zone created by this template page zone.
        /// </summary>
        public MasterPageZoneAdminType AdminType { get; set; }

        /// <summary>
        /// The zone content type that will be used in the master page zone created by this template page zone.
        /// </summary>
        public MasterPageZoneContentType ContentType { get; set; }

        /// <summary>
        /// HTML that is rendered just after start of template page zone.
        /// </summary>
        public string BeginRender { get; set; }

        /// <summary>
        /// HTML that is rendered just before end of template page zone.
        /// </summary>
        public string EndRender { get; set; }

        /// <summary>
        /// The types of element that can be inserted into a configurable template page zone.
        /// </summary>
        public List<TemplatePageZoneElementType> TemplatePageZoneElementTypes { get; set; }

        /// <summary>
        /// The template page zone elements that make up this template page zone.
        /// </summary>
        public List<TemplatePageZoneElement> TemplatePageZoneElements { get; set; }
    }
}

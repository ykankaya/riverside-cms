using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Templates
{
    /// <summary>
    /// Contains the type of element that can be inserted into a configurable template page zone.
    /// </summary>
    public class TemplatePageZoneElementType
    {
        /// <summary>
        /// The template that this template page zone element type belongs to.
        /// </summary>
        public long TenantId { get; set; }

        /// <summary>
        /// The template page that this template page zone element type belongs to.
        /// </summary>
        public long TemplatePageId { get; set; }

        /// <summary>
        /// The template page zone that this template page zone element type belongs to.
        /// </summary>
        public long TemplatePageZoneId { get; set; }

        /// <summary>
        /// Identifies type of element that can be inserted into a configrable template page zone.
        /// </summary>
        public Guid ElementTypeId { get; set; }
    }
}

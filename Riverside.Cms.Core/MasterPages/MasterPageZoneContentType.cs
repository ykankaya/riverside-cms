using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Resources;

namespace Riverside.Cms.Core.MasterPages
{
    /// <summary>
    /// Identifies the content within a master page zone.
    /// </summary>
    public enum MasterPageZoneContentType
    {
        // The default type of any zone
        [Display(ResourceType = typeof(MasterPageResource), Name = "ZoneContentTypeStandardLabel")]
        Standard,

        // Indicates zones containing main page content
        [Display(ResourceType = typeof(MasterPageResource), Name = "ZoneContentTypeMainLabel")]
        Main,

        // Indicates comment zones
        [Display(ResourceType = typeof(MasterPageResource), Name = "ZoneContentTypeCommentLabel")]
        Comment
    }
}

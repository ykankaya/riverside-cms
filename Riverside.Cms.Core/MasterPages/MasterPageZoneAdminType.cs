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
    /// Identifies the different types of master page zones. Currently two types are supported:
    /// static (can't be changed) and editable (can be updated by an administrator).
    /// </summary>
    public enum MasterPageZoneAdminType
    {
        // Static zones have fixed content 
        [Display(ResourceType = typeof(MasterPageResource), Name = "ZoneAdminTypeStaticLabel")]
        Static,

        // Editable zones have content that changes from page to page and is editable by end users
        [Display(ResourceType = typeof(MasterPageResource), Name = "ZoneAdminTypeEditableLabel")]
        Editable,

        // Configurable zones can have new content added to a zone and old content removed from a zone. Configurable zones must contain at least one content element.
        [Display(ResourceType = typeof(MasterPageResource), Name = "ZoneAdminTypeConfigurableLabel")]
        Configurable
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Cms.Services.Core.Domain
{
    public enum AdminType
    {
        // Static zones have fixed content 
        Static,

        // Editable zones have content that changes from page to page and is editable by end users
        Editable,

        // Configurable zones can have new content added to a zone and old content removed from a zone. Configurable zones must contain at least one content element.
        Configurable
    }
}

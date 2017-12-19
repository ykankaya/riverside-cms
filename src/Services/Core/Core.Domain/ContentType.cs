using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Cms.Services.Core.Domain
{
    public enum ContentType
    {
        // The default type of any zone
        Standard,

        // Indicates zones containing main page content
        Main,

        // Indicates comment zones
        Comment
    }
}

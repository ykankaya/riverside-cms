using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Cms.Services.Element.Domain
{
    public interface IElementSettings
    {
        long TenantId { get; set; }
        Guid ElementTypeId { get; set; }
        long ElementId { get; set; }
        string Name { get; set; }
    }
}

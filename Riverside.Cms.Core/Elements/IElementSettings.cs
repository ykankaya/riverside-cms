using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Elements
{
    /// <summary>
    /// Interface that all element types must implement.
    /// </summary>
    public interface IElementSettings
    {
        long TenantId { get; set; }
        Guid ElementTypeId { get; set; }
        long ElementId { get; set; }
        string Name { get; set; }
    }
}

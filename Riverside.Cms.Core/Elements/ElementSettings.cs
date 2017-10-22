using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Elements
{
    /// <summary>
    /// Base class for element models.
    /// </summary>
    public class ElementSettings : IElementSettings
    {
        public long TenantId { get; set; }
        public Guid ElementTypeId { get; set; }
        public long ElementId { get; set; }
        public string Name { get; set; }
    }
}

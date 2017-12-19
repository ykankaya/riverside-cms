using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Cms.Services.Core.Domain
{
    public class MasterPageZoneElement
    {
        public long TenantId { get; set; }

        public long MasterPageId { get; set; }
        public long MasterPageZoneId { get; set; }
        public long MasterPageZoneElementId { get; set; }

        public int SortOrder { get; set; }
        public long ElementId { get; set; }
        public string BeginRender { get; set; }
        public string EndRender { get; set; }

        public IEnumerable<Guid> ElementTypeIds { get; set; }
    }
}

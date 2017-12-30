using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Cms.Services.Core.Client
{
    public class MasterPageZone
    {
        public long TenantId { get; set; }

        public long MasterPageId { get; set; }
        public long MasterPageZoneId { get; set; }

        public int SortOrder { get; set; }
        public AdminType AdminType { get; set; }
        public ContentType ContentType { get; set; }
        public string Name { get; set; }
        public string BeginRender { get; set; }
        public string EndRender { get; set; }

        public IEnumerable<Guid> ElementTypeIds { get; set; }

        public List<MasterPageZoneElement> MasterPageZoneElements { get; set; }
    }
}

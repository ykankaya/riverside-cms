using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Cms.Services.Core.Client
{
    public class PageZoneElement
    {
        public long TenantId { get; set; }

        public long PageId { get; set; }
        public long PageZoneId { get; set; }
        public long PageZoneElementId { get; set; }

        public int? SortOrder { get; set; }

        public Guid ElementTypeId { get; set; }
        public long ElementId { get; set; }

        public long? MasterPageId { get; set; }
        public long? MasterPageZoneId { get; set; }
        public long? MasterPageZoneElementId { get; set; }
    }
}

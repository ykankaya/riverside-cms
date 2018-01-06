using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Cms.Services.Core.Client
{
    public class PageViewZone
    {
        public long TenantId { get; set; }

        public long MasterPageId { get; set; }
        public long MasterPageZoneId { get; set; }
        public long PageId { get; set; }

        public string BeginRender { get; set; }
        public string EndRender { get; set; }

        public List<PageViewZoneElement> PageViewZoneElements { get; set; }
    }
}

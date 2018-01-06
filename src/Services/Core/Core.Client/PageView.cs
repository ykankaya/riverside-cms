using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Cms.Services.Core.Client
{
    public class PageView
    {
        public long TenantId { get; set; }
        public long MasterPageId { get; set; }
        public long PageId { get; set; }

        public string Title { get; set; }
        public string BeginRender { get; set; }
        public string EndRender { get; set; }

        public List<PageViewZone> PageViewZones { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Cms.Services.Core.Client
{
    public class Page
    {
        public long TenantId { get; set; }

        public long PageId { get; set; }
        public long? ParentPageId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public DateTime? Occurred { get; set; }

        public long MasterPageId { get; set; }

        public long? ImageUploadId { get; set; }
        public long? PreviewImageUploadId { get; set; }
        public long? ThumbnailImageUploadId { get; set; }

        public List<PageZone> PageZones { get; set; }
    }
}

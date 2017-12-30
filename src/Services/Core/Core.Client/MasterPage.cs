using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Cms.Services.Core.Client
{
    public class MasterPage
    {
        public long TenantId { get; set; }

        public long MasterPageId { get; set; }
        public string Name { get; set; }
        public string PageName { get; set; }
        public string PageDescription { get; set; }
        public long? AncestorPageId { get; set; }
        public int? AncestorPageLevel { get; set; }
        public PageType PageType { get; set; }
        public bool HasOccurred { get; set; }
        public string BeginRender { get; set; }
        public string EndRender { get; set; }

        public bool HasImage { get; set; }
        public int? ThumbnailImageWidth { get; set; }
        public int? ThumbnailImageHeight { get; set; }
        public ResizeMode? ThumbnailImageResizeMode { get; set; }
        public int? PreviewImageWidth { get; set; }
        public int? PreviewImageHeight { get; set; }
        public ResizeMode? PreviewImageResizeMode { get; set; }
        public int? ImageMinWidth { get; set; }
        public int? ImageMinHeight { get; set; }

        public bool Creatable { get; set; }
        public bool Deletable { get; set; }
        public bool Taggable { get; set; }
        public bool Administration { get; set; }

        public List<MasterPageZone> MasterPageZones { get; set; }
    }
}

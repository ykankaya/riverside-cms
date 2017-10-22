using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.Html
{
    public class HtmlUpload
    {
        public long TenantId { get; set; }
        public long ElementId { get; set; }
        public long HtmlUploadId { get; set; }
        public long? ImageTenantId { get; set; }
        public long? ThumbnailImageUploadId { get; set; }
        public long? PreviewImageUploadId { get; set; }
        public long? ImageUploadId { get; set; }
        public long? UploadTenantId { get; set; }
        public long? UploadId { get; set; }
    }
}

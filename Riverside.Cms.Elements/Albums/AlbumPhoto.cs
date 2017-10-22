using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.Albums
{
    public class AlbumPhoto
    {
        public long TenantId { get; set; }
        public long ElementId { get; set; }
        public long AlbumPhotoId { get; set; }
        public long ImageTenantId { get; set; }
        public long ThumbnailImageUploadId { get; set; }
        public long PreviewImageUploadId { get; set; }
        public long ImageUploadId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; }
    }
}

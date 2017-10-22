using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Uploads
{
    /// <summary>
    /// Stores thumbnail, preview and original image upload identifiers.
    /// </summary>
    public class ImageUploadIds
    {
        public long ThumbnailImageUploadId { get; set; }
        public long PreviewImageUploadId { get; set; }
        public long ImageUploadId { get; set; }
    }
}

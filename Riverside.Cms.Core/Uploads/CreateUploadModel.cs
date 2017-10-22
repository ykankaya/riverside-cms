using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Uploads
{
    /// <summary>
    /// Contains information about the content of an upload.
    /// </summary>
    public class CreateUploadModel
    {
        /// <summary>
        /// Tenant that upload is associated with.
        /// </summary>
        public long TenantId { get; set; }

        /// <summary>
        /// The type of the upload content (e.g. "image/png").
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// The actual upload content.
        /// </summary>
        public byte[] Content { get; set; }

        /// <summary>
        /// The name of the upload (e.g. "MyImage.png").
        /// </summary>
        public string Name { get; set; }
    }
}

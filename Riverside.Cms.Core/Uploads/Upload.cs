using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Uploads
{
    /// <summary>
    /// Holds information about a file upload.
    /// </summary>
    public class Upload
    {
        /// <summary>
        /// Identifies tenant that upload associated with.
        /// </summary>
        public long TenantId { get; set; }

        /// <summary>
        /// Identifies upload.
        /// </summary>
        public long UploadId { get; set; }

        /// <summary>
        /// The name of the upload (e.g. "MyImage.png").
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Upload size in bytes.
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// The type of this upload (document, image etc).
        /// </summary>
        public UploadType UploadType { get; set; }

        /// <summary>
        /// The actual upload content.
        /// </summary>
        public byte[] Content { get; set; }

        /// <summary>
        /// The type of the upload content (e.g. "image/png")
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Indicates whether or not file upload has been committed.
        /// </summary>
        public bool Committed { get; set; }

        /// <summary>
        /// Date and time file uploaded.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Date and time upload last updated.
        /// </summary>
        public DateTime Updated { get; set; }
    }
}

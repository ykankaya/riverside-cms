using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Uploads
{
    /// <summary>
    /// Holds upload content and upload content type.
    /// </summary>
    public class UploadContent
    {
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
    }
}

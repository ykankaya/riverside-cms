using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Uploads
{
    /// <summary>
    /// The different categories of upload that are supported by the CMS.
    /// </summary>
    public enum UploadType
    {
        Upload, // Generic file upload
        Image   // Image (a 2D array of pixels that has width and height)
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.UI.Forms
{
    /// <summary>
    /// Form services should implement this service if they wish to handle uploads.
    /// </summary>
    public interface IFormUploadService
    {
        /// <summary>
        /// Handles form upload.
        /// </summary>
        /// <param name="id">Form identifier.</param>
        /// <param name="context">Form context.</param>
        /// <param name="name">The name of the upload (e.g. "MyImage.png").</param>
        /// <param name="contentType">The type of the upload content (e.g. "image/png").</param>
        /// <param name="content">Byte buffer containing uploaded content.</param>
        /// <returns>Result of form upload post.</returns>
        FormResult PostUpload(string id, string context, string name, string contentType, byte[] content);
    }
}

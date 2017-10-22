using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Riverside.Utilities.Drawing
{
    public interface IImageAnalysisService
    {
        /// <summary>
        /// Gets image dimensions from the supplier image content. If content cannot be identified as an image, then null is returned.
        /// </summary>
        /// <param name="content">Image content.</param>
        /// <returns>Width and height of image (or null if content is not identified as an image).</returns>
        Size? GetImageDimensions(byte[] content);

        /// <summary>
        /// Resizes an image.
        /// </summary>
        /// <param name="sourceContent">The content of the original source image that is resized.</param>
        /// <param name="resizeInfo">Describes how image is resized.</param>
        /// <returns>Resized image contents.</returns>
        byte[] ResizeImage(byte[] sourceContent, ResizeInfo resizeInfo);
    }
}

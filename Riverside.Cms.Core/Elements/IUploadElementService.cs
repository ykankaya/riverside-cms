using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Uploads;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Elements
{
    /// <summary>
    /// Interface that element services must support in order to work with file uploads.
    /// </summary>
    public interface IUploadElementService
    {
        /// <summary>
        /// From a single uploaded file, may create thumbnail, preview and original images in underlying storage that may eventually be associated with element.
        /// </summary>
        /// <param name="tenantId">Website that element belongs to.</param>
        /// <param name="elementId">Element identifier.</param>
        /// <param name="model">Image upload.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Identifiers of newly created thumbnail, preview and source images.</returns>
        long PrepareImages(long tenantId, long elementId, CreateUploadModel model, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// From a single uploaded file, may create thumbnail, preview and original images in underlying storage that may eventually be associated with element.
        /// </summary>
        /// <param name="tenantId">Website that element belongs to.</param>
        /// <param name="elementId">Element identifier.</param>
        /// <param name="model">Image upload.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Identifiers of newly created thumbnail, preview and source images.</returns>
        ImageUploadIds PrepareImages2(long tenantId, long elementId, CreateUploadModel model, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Retrieves the specified upload.
        /// </summary>
        /// <param name="tenantId">Website that element belongs to.</param>
        /// <param name="elementId">Element identifier.</param>
        /// <param name="uploadId">Identifies element upload.</param>
        /// <param name="format">The format of the image retrieved (e.g. "preview" or "thumbnail").</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Upload details (null if image not found).</returns>
        Upload ReadUpload(long tenantId, long elementId, long uploadId, string format, IUnitOfWork unitOfWork = null);
    }
}

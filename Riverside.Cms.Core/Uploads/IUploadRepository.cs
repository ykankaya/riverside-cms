using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Uploads
{
    /// <summary>
    /// Interface for upload repositories.
    /// </summary>
    public interface IUploadRepository
    {
        /// <summary>
        /// Creates a new upload.
        /// </summary>
        /// <param name="upload">New upload details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Newly allocated upload identifier.</returns>
        long CreateUpload(Upload upload, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Creates a new image.
        /// </summary>
        /// <param name="image">New image details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Newly allocated upload identifier.</returns>
        long CreateImage(Image image, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Gets upload details.
        /// </summary>
        /// <param name="tenantId">Identifies tenant whose upload is returned.</param>
        /// <param name="uploadId">Identifies the upload to return.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Upload details (or null if upload not found).</returns>
        Upload Read(long tenantId, long uploadId, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Update upload details.
        /// </summary>
        /// <param name="upload">Updated upload details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void UpdateUpload(Upload upload, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Update image details.
        /// </summary>
        /// <param name="upload">Updated image details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void UpdateImage(Image image, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Deletes an upload.
        /// </summary>
        /// <param name="tenantId">Identifies tenant whose upload is to be deleted.</param>
        /// <param name="uploadId">Identifies the upload to delete.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void Delete(long tenantId, long uploadId, IUnitOfWork unitOfWork = null);
    }
}

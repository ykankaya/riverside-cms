using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Uploads
{
    /// <summary>
    /// Interface for upload management services.
    /// </summary>
    public interface IUploadService
    {
        /// <summary>
        /// From content type, determine the more generic upload type.
        /// </summary>
        /// <param name="contentType">Content type (e.g. "image/png").</param>
        /// <returns>Upload type.</returns>
        UploadType GetUploadType(string contentType);

        /// <summary>
        /// Returns storage hierarchy used for uncommitted uploads.
        /// </summary>
        /// <returns>Uncomitted storage hierarchy.</returns>
        List<string> GetUncommittedStorageHierarchy();

        /// <summary>
        /// Creates a new upload with uncommitted state.
        /// </summary>
        /// <param name="model">The upload to create.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Newly allocated upload identfier.</returns>
        long Create(CreateUploadModel model, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Commits an upload.
        /// </summary>
        /// <param name="tenantId">Tenant identifier.</param>
        /// <param name="uploadId">Upload identifier.</param>
        /// <param name="storageHierarchy">New location of upload, once committed. E.g. { "Uploads" > "Users" }.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void Commit(long tenantId, long uploadId, List<string> storageHierarchy, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Retrieves upload content.
        /// </summary>
        /// <param name="tenantId">Tenant identifier.</param>
        /// <param name="uploadId">Upload identifer.</param>
        /// <param name="storageHierarchy">Location of upload. E.g. { "Uploads" > "Users" }.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Upload details.</returns>
        Upload Read(long tenantId, long uploadId, List<string> storageHierarchy, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Deletes an upload.
        /// </summary>
        /// <param name="tenantId">Tenant identifier.</param>
        /// <param name="uploadId">Upload identifier.</param>
        /// <param name="storageHierarchy">Location of upload. E.g. { "Uploads" > "Users" }.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void Delete(long tenantId, long uploadId, List<string> storageHierarchy, IUnitOfWork unitOfWork = null);
    }
}

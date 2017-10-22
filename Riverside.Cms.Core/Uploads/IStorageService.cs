using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Uploads;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Uploads
{
    /// <summary>
    /// Interface for services that interact with underlying storage systems.
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// Creates upload in underlying storage.
        /// </summary>
        /// <param name="upload">The upload to create in underlying storage.</param>
        /// <param name="storageHierarchy">Location of created upload. E.g. { "Uploads" > "Users" }.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void Create(Upload upload, List<string> storageHierarchy, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Gets upload content from underlying storage.
        /// </summary>
        /// <param name="tenantId">The tenant whose upload is returned.</param>
        /// <param name="storageHierarchy">Location of created upload. E.g. { "Uploads" > "Users" }.</param>
        /// <param name="upload">Identifies upload whose content and content type are retrieved.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Upload content.</returns>
        UploadContent Read(long tenantId, List<string> storageHierarchy, string upload, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Gets upload content from underlying storage.
        /// </summary>
        /// <param name="upload">Identifies upload whose content and content type are retrieved.</param>
        /// <param name="storageHierarchy">Location of created upload. E.g. { "Uploads" > "Users" }.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Upload content.</returns>
        UploadContent Read(Upload upload, List<string> storageHierarchy, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Gets list of uploads found at a storage location.
        /// </summary>
        /// <param name="tenantId">Tenant identifier.</param>
        /// <param name="storageHierarchy">Location of files. E.g. { "Uploads" > "Users" }.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>List of files.</returns>
        List<string> List(long tenantId, List<string> storageHierarchy, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Deletes an upload from underlying storage.
        /// </summary>
        /// <param name="upload">The upload to delete from underlying storage.</param>
        /// <param name="storageHierarchy">Location of created upload. E.g. { "Uploads" > "Users" }.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void Delete(Upload upload, List<string> storageHierarchy, IUnitOfWork unitOfWork = null);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Uploads;

namespace Riverside.Cms.Core.Uploads
{
    /// <summary>
    /// Interface for services that provide configuration values for Azure storage services.
    /// </summary>
    public interface IAzureConfigurationService
    {
        /// <summary>
        /// Gets configuration for storage.
        /// </summary>
        /// <param name="upload">Upload details.</param>
        /// <param name="storageHierarchy">Location of blob. E.g. { "Uploads" > "Users" }.</param>
        /// <returns>Storage configuration.</returns>
        AzureStorageConfiguration GetStorageConfiguration(Upload upload, List<string> storageHierarchy);

        /// <summary>
        /// Gets configuration for storage folder.
        /// </summary>
        /// <param name="tenantId">Tenant identifier.</param>
        /// <param name="storageHierarchy">Location of blob. E.g. { "Uploads" > "Users" }.</param>
        /// <returns>Storage configuration.</returns>
        AzureStorageConfiguration GetStorageConfiguration(long tenantId, List<string> storageHierarchy);

        /// <summary>
        /// Gets configuration for storage file.
        /// </summary>
        /// <param name="tenantId">Tenant identifier.</param>
        /// <param name="storageHierarchy">Location of blob. E.g. { "Uploads" > "Users" }.</param>
        /// <param name="upload">Name of uploaded file.</param>
        /// <returns>Storage configuration.</returns>
        AzureStorageConfiguration GetStorageConfiguration(long tenantId, List<string> storageHierarchy, string upload);
    }
}

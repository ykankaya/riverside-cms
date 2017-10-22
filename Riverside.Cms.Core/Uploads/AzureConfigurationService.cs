using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Riverside.Cms.Core.Uploads;

namespace Riverside.Cms.Core.Uploads
{
    /// <summary>
    /// Service provides configuration values for Azure storage services.
    /// </summary>
    public class AzureConfigurationService : IAzureConfigurationService
    {
        private IOptions<AzureStorageOptions> _options;

        public AzureConfigurationService(IOptions<AzureStorageOptions> options)
        {
            _options = options;
        }

        /// <summary>
        /// Gets configuration for storage.
        /// </summary>
        /// <param name="upload">Upload details.</param>
        /// <param name="storageHierarchy">Location of blob. E.g. { "Uploads" > "Users" }.</param>
        /// <returns>Storage configuration.</returns>
        public AzureStorageConfiguration GetStorageConfiguration(Upload upload, List<string> storageHierarchy)
        {
            // Get blob container name, blob name and connection string
            string blobContainerName = "tenant-" + upload.TenantId;
            string blobName = string.Format("{0}-{1}", upload.UploadId, upload.Name);
            string connectionString = _options.Value.BlobStorageConnectionString;

            // Get blob path
            StringBuilder sb = new StringBuilder();
            foreach (string folder in storageHierarchy)
                sb.AppendFormat("{0}/", folder);
            sb.Append(blobName);
            string blobPath = sb.ToString();

            // Return result
            return new AzureStorageConfiguration
            {
                BlobContainerName = blobContainerName,
                BlobName = blobName,
                BlobPath = blobPath,
                ConnectionString = connectionString
            };
        }

        /// <summary>
        /// Gets configuration for storage folder.
        /// </summary>
        /// <param name="tenantId">Tenant identifier.</param>
        /// <param name="storageHierarchy">Location of blob. E.g. { "Uploads" > "Users" }.</param>
        /// <returns>Storage configuration.</returns>
        public AzureStorageConfiguration GetStorageConfiguration(long tenantId, List<string> storageHierarchy)
        {
            // Get blob container name, blob name and connection string
            string blobContainerName = "tenant-" + tenantId;
            string connectionString = _options.Value.BlobStorageConnectionString;

            // Get blob path
            StringBuilder sb = new StringBuilder();
            foreach (string folder in storageHierarchy)
                sb.AppendFormat("{0}/", folder);
            string blobPath = sb.ToString();

            // Return result
            return new AzureStorageConfiguration
            {
                BlobContainerName = blobContainerName,
                BlobPath = blobPath,
                ConnectionString = connectionString
            };
        }

        /// <summary>
        /// Gets configuration for storage file.
        /// </summary>
        /// <param name="tenantId">Tenant identifier.</param>
        /// <param name="storageHierarchy">Location of blob. E.g. { "Uploads" > "Users" }.</param>
        /// <param name="upload">Name of uploaded file.</param>
        /// <returns>Storage configuration.</returns>
        public AzureStorageConfiguration GetStorageConfiguration(long tenantId, List<string> storageHierarchy, string upload)
        {
            // Get blob container name, blob name and connection string
            string blobContainerName = "tenant-" + tenantId;
            string connectionString = _options.Value.BlobStorageConnectionString;

            // Get blob path
            StringBuilder sb = new StringBuilder();
            foreach (string folder in storageHierarchy)
                sb.AppendFormat("{0}/", folder);
            sb.Append(upload);
            string blobPath = sb.ToString();

            // Return result
            return new AzureStorageConfiguration
            {
                BlobContainerName = blobContainerName,
                BlobPath = blobPath,
                BlobName = upload,
                ConnectionString = connectionString
            };
        }
    }
}

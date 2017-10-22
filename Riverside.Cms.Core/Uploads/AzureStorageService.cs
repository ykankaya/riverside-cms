using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Riverside.Cms.Core.Uploads;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Uploads
{
    /// <summary>
    /// Implements storage of files.
    /// </summary>
    public class AzureStorageService : IStorageService
    {
        // Member variables
        private IAzureConfigurationService _azureConfigurationService;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="azureConfigurationService">Azure configuration service.</param>
        public AzureStorageService(IAzureConfigurationService azureConfigurationService)
        {
            _azureConfigurationService = azureConfigurationService;
        }

        /// <summary>
        /// Creates upload in underlying storage.
        /// </summary>
        /// <param name="upload">The upload to create in underlying storage.</param>
        /// <param name="storageHierarchy">Location of created upload. E.g. { "Uploads" > "Users" }.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Create(Upload upload, List<string> storageHierarchy, IUnitOfWork unitOfWork = null)
        {
            // Get Azure storage configuration
            AzureStorageConfiguration configuration = _azureConfigurationService.GetStorageConfiguration(upload, storageHierarchy);

            // Retrieve storage account from connection string
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(configuration.ConnectionString);

            // Create the blob client
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container
            CloudBlobContainer blobContainer = blobClient.GetContainerReference(configuration.BlobContainerName);

            // Create the container if it doesn't already exist
            bool create = blobContainer.CreateIfNotExistsAsync().Result;

            // Retrieve reference to blob
            CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(configuration.BlobPath);

            // Set blob content type
            blockBlob.Properties.ContentType = upload.ContentType;

            // Upload content to block blob
            blockBlob.UploadFromByteArrayAsync(upload.Content, 0, upload.Content.Length).Wait();
        }

        /// <summary>
        /// Gets upload content from underlying storage.
        /// </summary>
        /// <param name="tenantId">The tenant whose upload is returned.</param>
        /// <param name="storageHierarchy">Location of created upload. E.g. { "Uploads" > "Users" }.</param>
        /// <param name="upload">Identifies upload whose content and content type are retrieved.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Upload content.</returns>
        public UploadContent Read(long tenantId, List<string> storageHierarchy, string upload, IUnitOfWork unitOfWork = null)
        {
            // Get Azure storage configuration
            AzureStorageConfiguration configuration = _azureConfigurationService.GetStorageConfiguration(tenantId, storageHierarchy, upload);

            // Retrieve storage account from connection string
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(configuration.ConnectionString);

            // Create the blob client
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container
            CloudBlobContainer blobContainer = blobClient.GetContainerReference(configuration.BlobContainerName);

            // Retrieve reference to blob
            CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(configuration.BlobPath);

            // Save blob contents to byte array
            byte[] content = null;
            using (MemoryStream ms = new MemoryStream())
            {
                blockBlob.DownloadToStreamAsync(ms).Wait();
                ms.Seek(0, SeekOrigin.Begin);
                content = new byte[ms.Length];
                ms.Read(content, 0, (int)ms.Length);
            }

            // Construct upload content return type
            return new UploadContent { Content = content, ContentType = blockBlob.Properties.ContentType };
        }

        /// <summary>
        /// Gets upload content from underlying storage.
        /// </summary>
        /// <param name="upload">Identifies upload whose content and content type are retrieved.</param>
        /// <param name="storageHierarchy">Location of created upload. E.g. { "Uploads" > "Users" }.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Upload content.</returns>
        public UploadContent Read(Upload upload, List<string> storageHierarchy, IUnitOfWork unitOfWork = null)
        {
            // Get Azure storage configuration
            AzureStorageConfiguration configuration = _azureConfigurationService.GetStorageConfiguration(upload, storageHierarchy);

            // Retrieve storage account from connection string
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(configuration.ConnectionString);

            // Create the blob client
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container
            CloudBlobContainer blobContainer = blobClient.GetContainerReference(configuration.BlobContainerName);

            // Retrieve reference to blob
            CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(configuration.BlobPath);

            // Save blob contents to byte array
            byte[] content = null;
            using (MemoryStream ms = new MemoryStream())
            {
                blockBlob.DownloadToStreamAsync(ms).Wait();
                ms.Seek(0, SeekOrigin.Begin);
                content = new byte[ms.Length];
                ms.Read(content, 0, (int)ms.Length);
            }

            // Construct upload content return type
            return new UploadContent { Content = content, ContentType = blockBlob.Properties.ContentType };
        }

        /// <summary>
        /// Gets list of uploads found at a storage location.
        /// </summary>
        /// <param name="tenantId">Tenant identifier.</param>
        /// <param name="storageHierarchy">Location of files. E.g. { "Uploads" > "Users" }.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>List of files.</returns>
        public List<string> List(long tenantId, List<string> storageHierarchy, IUnitOfWork unitOfWork = null)
        {
            // Get Azure storage configuration
            AzureStorageConfiguration configuration = _azureConfigurationService.GetStorageConfiguration(tenantId, storageHierarchy);

            // Retrieve storage account from connection string
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(configuration.ConnectionString);

            // Create the blob client
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container
            CloudBlobContainer blobContainer = blobClient.GetContainerReference(configuration.BlobContainerName);
            CloudBlobDirectory blobDirectory = blobContainer.GetDirectoryReference(configuration.BlobPath);

            // Get uploads
            List<string> uploads = new List<string>();
            BlobContinuationToken token = null;
            do
            {
                BlobResultSegment results = blobDirectory.ListBlobsSegmentedAsync(token).Result;
                foreach (IListBlobItem item in results.Results)
                {
                    string upload = item.Uri.AbsoluteUri.Substring(item.Uri.AbsoluteUri.LastIndexOf("/") + 1);
                    uploads.Add(upload);
                }
                token = results.ContinuationToken;
            }
            while (token != null);

            // Return the result
            return uploads;
        }

        /// <summary>
        /// Deletes an upload from underlying storage.
        /// </summary>
        /// <param name="upload">The upload to delete from underlying storage.</param>
        /// <param name="storageHierarchy">Location of created upload. E.g. { "Uploads" > "Users" }.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Delete(Upload upload, List<string> storageHierarchy, IUnitOfWork unitOfWork = null)
        {
            // Get Azure storage configuration
            AzureStorageConfiguration configuration = _azureConfigurationService.GetStorageConfiguration(upload, storageHierarchy);

            // Retrieve storage account from connection string
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(configuration.ConnectionString);

            // Create the blob client
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container
            CloudBlobContainer blobContainer = blobClient.GetContainerReference(configuration.BlobContainerName);

            // Retrieve reference to blob
            CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(configuration.BlobPath);

            // If block blob exists, delete it
            if (blockBlob.ExistsAsync().Result)
                blockBlob.DeleteAsync().Wait();
        }
    }
}

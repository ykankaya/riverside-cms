using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Riverside.Cms.Services.Storage.Domain;

namespace Riverside.Cms.Services.Storage.Infrastructure
{
    public class AzureBlobService : IBlobService
    {
        private readonly IOptions<AzureBlobOptions> _options;

        public AzureBlobService(IOptions<AzureBlobOptions> options)
        {
            _options = options;
        }

        public string GetBlobContainer(Blob blob)
        {
            return string.Format("tenant-{0}", blob.TenantId);
        }

        public string GetBlobName(Blob blob)
        {
            string blobName = blob.Path;
            if (!string.IsNullOrWhiteSpace(blobName))
                blobName += "/";
            blobName += string.Format("{0}-{1}", blob.BlobId, blob.Name);
            return blobName;
        }

        public async Task CreateBlobContentAsync(Blob blob, Stream stream)
        {
            string blobContainer = GetBlobContainer(blob);
            string blobName = GetBlobName(blob);

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_options.Value.StorageConnectionString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(blobContainer);

            await cloudBlobContainer.CreateIfNotExistsAsync();

            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(blobName);

            cloudBlockBlob.Properties.ContentType = blob.ContentType;

            await cloudBlockBlob.UploadFromStreamAsync(stream);
        }

        public async Task<Stream> ReadBlobContentAsync(Blob blob)
        {
            string blobContainer = GetBlobContainer(blob);
            string blobName = GetBlobName(blob);

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_options.Value.StorageConnectionString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(blobContainer);
            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(blobName);

            Stream target = new MemoryStream();

            await cloudBlockBlob.DownloadToStreamAsync(target);

            target.Position = 0;

            return target;
        }

        public async Task DeleteBlobContentAsync(Blob blob)
        {
            string blobContainer = GetBlobContainer(blob);
            string blobName = GetBlobName(blob);

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_options.Value.StorageConnectionString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(blobContainer);
            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(blobName);

            await cloudBlockBlob.DeleteIfExistsAsync();
        }
    }
}

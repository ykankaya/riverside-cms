using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Services.Storage.Domain
{
    public class StorageService : IStorageService
    {
        private readonly IBlobService _blobService;
        private readonly IStorageRepository _storageRepository;

        public StorageService(IBlobService blobService, IStorageRepository storageRepository)
        {
            _blobService = blobService;
            _storageRepository = storageRepository;
        }

        public Task<IEnumerable<Blob>> SearchBlobsAsync(long tenantId, string path)
        {
            if (path == null)
                path = string.Empty;
            return _storageRepository.SearchBlobsAsync(tenantId, path);
        }

        public async Task<long> CreateBlobAsync(long tenantId, Blob blob, Stream stream)
        {
            DateTime utcNow = DateTime.UtcNow;
            blob.Created = utcNow;
            blob.Updated = utcNow;
            blob.Size = (int)stream.Length;
            if (blob.Path == null)
                blob.Path = string.Empty;
            blob.BlobId = await _storageRepository.CreateBlobAsync(tenantId, blob);
            await _blobService.CreateBlobContentAsync(blob, stream);
            return blob.BlobId;
        }

        public Task<Blob> ReadBlobAsync(long tenantId, long blobId)
        {
            return _storageRepository.ReadBlobAsync(tenantId, blobId);
        }

        public async Task<BlobContent> ReadBlobContentAsync(long tenantId, long blobId)
        {
            Blob blob = await _storageRepository.ReadBlobAsync(tenantId, blobId);
            BlobContent blobContent = new BlobContent
            {
                Type = blob.ContentType,
                Stream = await _blobService.ReadBlobContentAsync(blob)
            };
            return blobContent;
        }

        public async Task DeleteBlobAsync(long tenantId, long blobId)
        {
            Blob blob = await _storageRepository.ReadBlobAsync(tenantId, blobId);
            if (blob == null)
                return;
            await _blobService.DeleteBlobContentAsync(blob);
            await _storageRepository.DeleteBlobAsync(tenantId, blobId);
        }
    }
}

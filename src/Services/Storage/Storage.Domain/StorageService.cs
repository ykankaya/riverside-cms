using System;
using System.Collections.Generic;
using System.IO;
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

        public async Task<long> CreateBlobAsync(long tenantId, Blob blob, Stream stream)
        {
            DateTime utcNow = DateTime.UtcNow;
            blob.Created = utcNow;
            blob.Updated = utcNow;
            blob.Size = (int)stream.Length;
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
    }
}

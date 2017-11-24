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
        private readonly IImageService _imageService;
        private readonly IStorageRepository _storageRepository;

        public StorageService(IBlobService blobService, IImageService imageService, IStorageRepository storageRepository)
        {
            _blobService = blobService;
            _imageService = imageService;
            _storageRepository = storageRepository;
        }

        private bool ContentTypeIsImage(string contentType)
        {
            switch (contentType)
            {
                case ContentTypes.Gif:
                case ContentTypes.Jpeg:
                case ContentTypes.Png:
                    return true;

                default:
                    return false;
            }
        }

        private BlobImage GetBlobImage(Blob blob, Stream stream)
        {
            ImageMetadata metadata = _imageService.GetImageMetadata(stream);
            stream.Position = 0;
            BlobImage blobImage = new BlobImage
            {
                BlobId = blob.BlobId,
                ContentType = blob.ContentType,
                Created = blob.Created,
                Name = blob.Name,
                Path = blob.Path,
                Size = blob.Size,
                TenantId = blob.TenantId,
                Updated = blob.Updated
            };
            blobImage.Width = metadata.Width;
            blobImage.Height = metadata.Height;
            return blobImage;
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

            if (ContentTypeIsImage(blob.ContentType))
                blob = GetBlobImage(blob, stream);

            blob.BlobId = await _storageRepository.CreateBlobAsync(tenantId, blob);
            await _blobService.CreateBlobContentAsync(blob, stream);
            return blob.BlobId;
        }

        public async Task<long> ResizeBlobAsync(long tenantId, long sourceBlobId, string path, ResizeOptions options)
        {
            Blob blob = await _storageRepository.ReadBlobAsync(tenantId, sourceBlobId);
            Stream imageStream = await _blobService.ReadBlobContentAsync(blob);

            blob.Path = path;
            Stream resizedImageStream = _imageService.ResizeImage(imageStream, options);

            return await CreateBlobAsync(tenantId, blob, resizedImageStream);
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

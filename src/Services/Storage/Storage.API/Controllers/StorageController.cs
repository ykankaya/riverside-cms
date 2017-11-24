using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Riverside.Cms.Services.Storage.Domain;

namespace Storage.API.Controllers
{
    public class StorageController : Controller
    {
        private readonly IStorageService _storageService;

        public StorageController(IStorageService storageService)
        {
            _storageService = storageService;
        }

        private bool PostIsCreateAction(IFormFile file, long? sourceBlobId, ResizeMode? mode, int? width, int? height)
        {
            return file != null && sourceBlobId == null && mode == null && width == null && height == null;
        }

        private bool PostIsResizeAction(IFormFile file, long? sourceBlobId, ResizeMode? mode, int? width, int? height)
        {
            return file == null && sourceBlobId != null && mode != null && width != null && height != null;
        }

        private async Task<IActionResult> CreateBlobWork(long tenantId, string path, IFormFile file)
        {
            Blob blob = new Blob
            {
                TenantId = tenantId,
                ContentType = file.ContentType,
                Name = file.FileName,
                Path = path
            };
            Stream stream = file.OpenReadStream();
            long blobId = await _storageService.CreateBlobAsync(tenantId, blob, stream);
            return CreatedAtAction(nameof(ReadBlob), new { tenantId = tenantId, blobId = blobId }, null);
        }

        private async Task<IActionResult> ResizeBlob(long tenantId, string path, long sourceBlobId, ResizeMode mode, int width, int height)
        {
            ResizeOptions options = new ResizeOptions
            {
                Width = width,
                Height = height,
                Mode = mode
            };
            long destinationBlobId = await _storageService.ResizeBlobAsync(tenantId, sourceBlobId, path, options);
            return CreatedAtAction(nameof(ReadBlob), new { tenantId = tenantId, blobId = destinationBlobId }, null);
        }

        [HttpGet]
        [Route("api/v1/storage/tenants/{tenantId:int}/blobs")]
        [ProducesResponseType(typeof(IEnumerable<Blob>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> SearchBlobs(long tenantId, [FromQuery]string path)
        {
            IEnumerable<Blob> blobs = await _storageService.SearchBlobsAsync(tenantId, path);
            return Ok(blobs);
        }

        [HttpGet]
        [Route("api/v1/storage/tenants/{tenantId:int}/blobs/{blobId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Blob), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ReadBlob(long tenantId, long blobId)
        {
            Blob blob = await _storageService.ReadBlobAsync(tenantId, blobId);
            if (blob == null)
                return NotFound();
            return Ok(blob);
        }

        [HttpGet]
        [Route("api/v1/storage/tenants/{tenantId:int}/blobs/{blobId:int}/content")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ReadBlobContent(long tenantId, long blobId)
        {
            BlobContent blobContent = await _storageService.ReadBlobContentAsync(tenantId, blobId);
            if (blobContent == null)
                return NotFound();
            return File(blobContent.Stream, blobContent.Type);
        }

        [HttpPost]
        [Route("api/v1/storage/tenants/{tenantId:int}/blobs")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateBlob(long tenantId, string path, IFormFile file, [FromQuery(Name = "source")]long? sourceBlobId, [FromQuery(Name = "resize")]ResizeMode? mode, [FromQuery]int? width, [FromQuery]int? height)
        {
            bool create = PostIsCreateAction(file, sourceBlobId, mode, width, height);
            bool resize = PostIsResizeAction(file, sourceBlobId, mode, width, height);
            if ((create && resize) || (!create && !resize))
                return BadRequest();

            if (create)
                return await CreateBlobWork(tenantId, path, file);
            else
                return await ResizeBlob(tenantId, path, sourceBlobId.Value, mode.Value, width.Value, height.Value);
        }

        [HttpDelete]
        [Route("api/v1/storage/tenants/{tenantId:int}/blobs/{blobId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteBlob(long tenantId, long blobId)
        {
            Blob blob = await _storageService.ReadBlobAsync(tenantId, blobId);
            if (blob == null)
                return NotFound();
            await _storageService.DeleteBlobAsync(tenantId, blobId);
            return NoContent();
        }
    }
}
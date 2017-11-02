using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
        public async Task<IActionResult> ReadFileContent(long tenantId, long blobId)
        {
            BlobContent blobContent = await _storageService.ReadBlobContentAsync(tenantId, blobId);
            if (blobContent == null)
                return NotFound();
            return File(blobContent.Stream, blobContent.Type);
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Uploads;

namespace Riverside.Cms.Core.ApiControllers
{
    public class UploadsController : Controller
    {
        private IAuthenticationService _authenticationService;
        private IUploadService _uploadService;

        public UploadsController(IAuthenticationService authenticationService, IUploadService uploadService)
        {
            _authenticationService = authenticationService;
            _uploadService = uploadService;
        }

        /// <summary>
        /// Called when file uploaded posted to this controller.
        /// Credit: http://stackoverflow.com/questions/10320232/how-to-accept-a-file-post-asp-net-mvc-4-webapi (Mike Wasson's answer) (Old)
        /// Credit: https://docs.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads (New)
        /// </summary>
        /// <param name="files">Uploaded files.</param>
        /// <returns>An HTTP action result.</returns>
        public async Task<IActionResult> Post(List<IFormFile> files)
        {
            // Get tenant identifier
            long tenantId = _authenticationService.TenantId;

            // Process each file
            List<long> uploadIds = new List<long>();
            foreach (IFormFile file in files)
            {
                // Get file
                string name = file.FileName.Trim('\"');
                string contentType = file.ContentType;
                byte[] content;
                using (MemoryStream ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    content = ms.ToArray();
                }

                // Create upload
                CreateUploadModel model = new CreateUploadModel
                {
                    TenantId = tenantId,
                    Name = name,
                    Content = content,
                    ContentType = contentType
                };
                long uploadId = _uploadService.Create(model);
                uploadIds.Add(uploadId);
            }

            // Convert upload IDs to comma separated string and return this
            string responseContent = string.Join(",", uploadIds);

            // Everything completed successfully - return list of newly allocated upload identifiers
            return Ok(responseContent);
        }
    }
}

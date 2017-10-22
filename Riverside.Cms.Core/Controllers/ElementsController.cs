using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Uploads;

namespace Riverside.Cms.Core.Controllers
{
    /// <summary>
    /// Controller for elements.
    /// </summary>
    public class ElementsController : Controller
    {
        // Member variables
        private IAuthenticationService _authenticationService;
        private IElementFactory _elementFactory;
        private IElementService _elementService;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="authenticationService">Authentication service.</param>
        /// <param name="elementFactory">For the dynamic creation of element services.</param>
        /// <param name="elementService">Provides access to elements.</param>
        public ElementsController(IAuthenticationService authenticationService, IElementFactory elementFactory, IElementService elementService)
        {
            _authenticationService = authenticationService;
            _elementFactory = elementFactory;
            _elementService = elementService;
        }

        /// <summary>
        /// Gets an elements's image.
        /// </summary>
        /// <param name="elementId">The element whose image is returned.</param>
        /// <param name="uploadId">Identifies element upload.</param>
        /// <param name="format">The format that is retrieve "thumbnail" or "preview".</param>
        /// <returns>An ActionResult.</returns>
        [HttpGet]
        [ResponseCache(Duration = 86400)]
        public IActionResult ReadUpload(long elementId, long uploadId, string format)
        {
            // Get element service
            long tenantId = _authenticationService.TenantId;
            IElementSettings elementSettings = _elementService.Read(tenantId, elementId, false);
            IBasicElementService elementService = _elementFactory.GetElementService(elementSettings.ElementTypeId);

            // If images not supported, go no further
            if (!(elementService is IUploadElementService))
                return null;

            // Get specified image
            if (format == null)
                format = "preview";
            Upload upload = ((IUploadElementService)elementService).ReadUpload(tenantId, elementId, uploadId, format);
            if (upload == null)
                return null;

            // Return file content result
            return new FileContentResult(upload.Content, upload.ContentType) { FileDownloadName = upload.Name };
        }
    }
}

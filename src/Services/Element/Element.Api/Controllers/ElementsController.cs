using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Riverside.Cms.Services.Element.Domain;

namespace Element.Api.Controllers
{
    public class ElementsController : Controller
    {
        private readonly IElementService _elementService;

        public ElementsController(IElementService elementService)
        {
            _elementService = elementService;
        }

        [HttpGet]
        [Route("api/v1/element/tenants/{tenantId:int}/elements/{elementId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ElementSettings), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ReadElement(long tenantId, long elementId)
        {
            ElementSettings elementSettings = await _elementService.ReadElementAsync(tenantId, elementId);
            if (elementSettings == null)
                return NotFound();
            return Ok(elementSettings);
        }

        [HttpGet]
        [Route("api/v1/element/tenants/{tenantId:int}/elements/{elementId:int}/pages/{pageId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ElementContent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ReadElementContent(long tenantId, long elementId, long pageId)
        {
            ElementContent elementContent = await _elementService.ReadElementContentAsync(tenantId, elementId, pageId);
            if (elementContent == null)
                return NotFound();
            return Ok(elementContent);
        }
    }
}

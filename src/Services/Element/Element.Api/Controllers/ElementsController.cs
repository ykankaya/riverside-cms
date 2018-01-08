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
        private readonly IPageHeaderElementService _pageHeaderElementService;

        public ElementsController(IPageHeaderElementService pageHeaderElementService)
        {
            _pageHeaderElementService = pageHeaderElementService;
        }

        [HttpGet]
        [Route("api/v1/element/tenants/{tenantId:int}/elementtypes/1cbac30c-5deb-404e-8ea8-aabc20c82aa8/elements/{elementId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PageHeaderElementSettings), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ReadElement(long tenantId, long elementId)
        {
            PageHeaderElementSettings elementSettings = await _pageHeaderElementService.ReadElementAsync(tenantId, elementId);
            if (elementSettings == null)
                return NotFound();
            return Ok(elementSettings);
        }

        [HttpGet]
        [Route("api/v1/element/tenants/{tenantId:int}/elementtypes/1cbac30c-5deb-404e-8ea8-aabc20c82aa8/elements/{elementId:int}/view")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PageHeaderElementView), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetElementView(long tenantId, long elementId, [FromQuery]long pageId)
        {
            PageHeaderElementView elementView = await _pageHeaderElementService.GetElementViewAsync(tenantId, elementId, pageId);
            if (elementView == null)
                return NotFound();
            return Ok(elementView);
        }
    }
}

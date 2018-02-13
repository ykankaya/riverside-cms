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
        private readonly IFooterElementService _footerElementService;
        private readonly IPageHeaderElementService _pageHeaderElementService;

        public ElementsController(IFooterElementService footerElementService, IPageHeaderElementService pageHeaderElementService)
        {
            _footerElementService = footerElementService;
            _pageHeaderElementService = pageHeaderElementService;
        }

        // FOOTER

        [HttpGet]
        [Route("api/v1/element/tenants/{tenantId:int}/elementtypes/f1c2b384-4909-47c8-ada7-cd3cc7f32620/elements/{elementId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(FooterElementSettings), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ReadFooterElement(long tenantId, long elementId)
        {
            FooterElementSettings elementSettings = await _footerElementService.ReadElementSettingsAsync(tenantId, elementId);
            if (elementSettings == null)
                return NotFound();
            return Ok(elementSettings);
        }

        [HttpGet]
        [Route("api/v1/element/tenants/{tenantId:int}/elementtypes/f1c2b384-4909-47c8-ada7-cd3cc7f32620/elements/{elementId:int}/view")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(FooterElementView), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetFooterElementView(long tenantId, long elementId, [FromQuery]long pageId)
        {
            FooterElementView elementView = await _footerElementService.GetElementViewAsync(tenantId, elementId, pageId);
            if (elementView == null)
                return NotFound();
            return Ok(elementView);
        }

        // PAGE HEADER

        [HttpGet]
        [Route("api/v1/element/tenants/{tenantId:int}/elementtypes/1cbac30c-5deb-404e-8ea8-aabc20c82aa8/elements/{elementId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PageHeaderElementSettings), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ReadPageHeaderElement(long tenantId, long elementId)
        {
            PageHeaderElementSettings elementSettings = await _pageHeaderElementService.ReadElementSettingsAsync(tenantId, elementId);
            if (elementSettings == null)
                return NotFound();
            return Ok(elementSettings);
        }

        [HttpGet]
        [Route("api/v1/element/tenants/{tenantId:int}/elementtypes/1cbac30c-5deb-404e-8ea8-aabc20c82aa8/elements/{elementId:int}/view")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PageHeaderElementView), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPageHeaderElementView(long tenantId, long elementId, [FromQuery]long pageId)
        {
            PageHeaderElementView elementView = await _pageHeaderElementService.GetElementViewAsync(tenantId, elementId, pageId);
            if (elementView == null)
                return NotFound();
            return Ok(elementView);
        }
    }
}

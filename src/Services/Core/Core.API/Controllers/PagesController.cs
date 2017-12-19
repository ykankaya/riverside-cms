using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Riverside.Cms.Services.Core.Domain;

namespace Core.API.Controllers
{
    public class PagesController : Controller
    {
        private readonly IPageService _pageService;

        public PagesController(IPageService pageService)
        {
            _pageService = pageService;
        }

        [HttpGet]
        [Route("api/v1/core/tenants/{tenantId:int}/pages/{pageId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Page), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ReadPage(long tenantId, long pageId)
        {
            Page page = await _pageService.ReadPageAsync(tenantId, pageId);
            if (page == null)
                return NotFound();
            return Ok(page);
        }

        [HttpGet]
        [Route("api/v1/core/tenants/{tenantId:int}/pages/{pageId:int}/zones")]
        [ProducesResponseType(typeof(IEnumerable<PageZone>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> SearchPageZones(long tenantId, long pageId)
        {
            IEnumerable<PageZone> pageZones = await _pageService.SearchPageZonesAsync(tenantId, pageId);
            return Ok(pageZones);
        }

        [HttpGet]
        [Route("api/v1/core/tenants/{tenantId:int}/pages/{pageId:int}/zones/{pageZoneId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PageZone), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ReadPageZone(long tenantId, long pageId, long pageZoneId)
        {
            PageZone pageZone = await _pageService.ReadPageZoneAsync(tenantId, pageId, pageZoneId);
            if (pageZone == null)
                return NotFound();
            return Ok(pageZone);
        }

        [HttpGet]
        [Route("api/v1/core/tenants/{tenantId:int}/pages/{pageId:int}/zones/{pageZoneId:int}/elements")]
        [ProducesResponseType(typeof(IEnumerable<PageZoneElement>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> SearchPageZoneElements(long tenantId, long pageId, long pageZoneId)
        {
            IEnumerable<PageZoneElement> pageZoneElements = await _pageService.SearchPageZoneElementsAsync(tenantId, pageId, pageZoneId);
            return Ok(pageZoneElements);
        }

        [HttpGet]
        [Route("api/v1/core/tenants/{tenantId:int}/pages/{pageId:int}/zones/{pageZoneId:int}/elements/{pageZoneElementId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PageZoneElement), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ReadPageZoneElement(long tenantId, long pageId, long pageZoneId, long pageZoneElementId)
        {
            PageZoneElement pageZoneElement = await _pageService.ReadPageZoneElementAsync(tenantId, pageId, pageZoneId, pageZoneElementId);
            if (pageZoneElement == null)
                return NotFound();
            return Ok(pageZoneElement);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Riverside.Cms.Services.Core.Domain;

namespace Core.API.Controllers
{
    public class PageViewsController : Controller
    {
        private readonly IPageViewService _pageViewService;

        public PageViewsController(IPageViewService pageViewService)
        {
            _pageViewService = pageViewService;
        }

        [HttpGet]
        [Route("api/v1/core/tenants/{tenantId:int}/pageviews/{pageId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PageView), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ReadPageView(long tenantId, long pageId)
        {
            PageView pageView = await _pageViewService.ReadPageViewAsync(tenantId, pageId);
            if (pageView == null)
                return NotFound();
            return Ok(pageView);
        }

        [HttpGet]
        [Route("api/v1/core/tenants/{tenantId:int}/pageviews/{pageId:int}/zones")]
        [ProducesResponseType(typeof(IEnumerable<PageViewZone>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> SearchPageViewZones(long tenantId, long pageId)
        {
            IEnumerable<PageViewZone> pageViewZones = await _pageViewService.SearchPageViewZonesAsync(tenantId, pageId);
            return Ok(pageViewZones);
        }

        [HttpGet]
        [Route("api/v1/core/tenants/{tenantId:int}/pages/{pageId:int}/zones/{masterPageZoneId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PageZone), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ReadPageZone(long tenantId, long pageId, long masterPageZoneId)
        {
            PageViewZone pageViewZone = await _pageViewService.ReadPageViewZoneAsync(tenantId, pageId, masterPageZoneId);
            if (pageViewZone == null)
                return NotFound();
            return Ok(pageViewZone);
        }

        [HttpGet]
        [Route("api/v1/core/tenants/{tenantId:int}/pageviews/{pageId:int}/zones/{masterPageZoneId:int}/elements")]
        [ProducesResponseType(typeof(IEnumerable<PageViewZoneElement>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> SearchPageZoneElements(long tenantId, long pageId, long masterPageZoneId)
        {
            IEnumerable<PageViewZoneElement> pageViewZoneElements = await _pageViewService.SearchPageViewZoneElementsAsync(tenantId, pageId, masterPageZoneId);
            return Ok(pageViewZoneElements);
        }
    }
}

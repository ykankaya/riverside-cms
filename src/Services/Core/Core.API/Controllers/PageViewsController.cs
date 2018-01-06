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
    }
}

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
        private readonly ICodeSnippetElementService _codeSnippetElementService;
        private readonly IFooterElementService _footerElementService;
        private readonly IPageHeaderElementService _pageHeaderElementService;

        public ElementsController(ICodeSnippetElementService codeSnippetElementService, IFooterElementService footerElementService, IPageHeaderElementService pageHeaderElementService)
        {
            _codeSnippetElementService = codeSnippetElementService;
            _footerElementService = footerElementService;
            _pageHeaderElementService = pageHeaderElementService;
        }

        // CODE SNIPPET

        [HttpGet]
        [Route("api/v1/element/tenants/{tenantId:int}/elementtypes/5401977d-865f-4a7a-b416-0a26305615de/elements/{elementId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(CodeSnippetElementSettings), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ReadCodeSnippetElementSettings(long tenantId, long elementId)
        {
            CodeSnippetElementSettings elementSettings = await _codeSnippetElementService.ReadElementSettingsAsync(tenantId, elementId);
            if (elementSettings == null)
                return NotFound();
            return Ok(elementSettings);
        }

        // FOOTER

        [HttpGet]
        [Route("api/v1/element/tenants/{tenantId:int}/elementtypes/f1c2b384-4909-47c8-ada7-cd3cc7f32620/elements/{elementId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(FooterElementSettings), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ReadFooterElementSettings(long tenantId, long elementId)
        {
            FooterElementSettings elementSettings = await _footerElementService.ReadElementSettingsAsync(tenantId, elementId);
            if (elementSettings == null)
                return NotFound();
            return Ok(elementSettings);
        }

        [HttpGet]
        [Route("api/v1/element/tenants/{tenantId:int}/elementtypes/f1c2b384-4909-47c8-ada7-cd3cc7f32620/elements/{elementId:int}/content")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(FooterElementContent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ReadFooterElementContent(long tenantId, long elementId, [FromQuery]long pageId)
        {
            FooterElementContent elementContent = await _footerElementService.ReadElementContentAsync(tenantId, elementId, pageId);
            if (elementContent == null)
                return NotFound();
            return Ok(elementContent);
        }

        // PAGE HEADER

        [HttpGet]
        [Route("api/v1/element/tenants/{tenantId:int}/elementtypes/1cbac30c-5deb-404e-8ea8-aabc20c82aa8/elements/{elementId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PageHeaderElementSettings), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ReadPageHeaderElementSettings(long tenantId, long elementId)
        {
            PageHeaderElementSettings elementSettings = await _pageHeaderElementService.ReadElementSettingsAsync(tenantId, elementId);
            if (elementSettings == null)
                return NotFound();
            return Ok(elementSettings);
        }

        [HttpGet]
        [Route("api/v1/element/tenants/{tenantId:int}/elementtypes/1cbac30c-5deb-404e-8ea8-aabc20c82aa8/elements/{elementId:int}/content")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PageHeaderElementContent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ReadPageHeaderElementContent(long tenantId, long elementId, [FromQuery]long pageId)
        {
            PageHeaderElementContent elementContent = await _pageHeaderElementService.ReadElementContentAsync(tenantId, elementId, pageId);
            if (elementContent == null)
                return NotFound();
            return Ok(elementContent);
        }
    }
}

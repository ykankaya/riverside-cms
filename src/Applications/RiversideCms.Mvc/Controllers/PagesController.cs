using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Riverside.Cms.Services.Core.Client;
using Riverside.Cms.Services.Element.Client;
using RiversideCms.Mvc.Models;
using RiversideCms.Mvc.Services;

namespace RiversideCms.Mvc.Controllers
{
    public class PagesController : Controller
    {
        private readonly IElementServiceFactory _elementServiceFactory;
        private readonly IPageViewService _pageViewService;

        private const long TenantId = 6;

        public PagesController(IElementServiceFactory elementServiceFactory, IPageViewService pageViewService)
        {
            _elementServiceFactory = elementServiceFactory;
            _pageViewService = pageViewService;
        }

        private async Task<ElementRender> GetElementRender(long tenantId, Guid elementTypeId, long elementId, long pageId)
        {
            IElementSettings elementSettings = await _elementServiceFactory.ReadElementSettingsAsync(tenantId, elementTypeId, elementId);
            object elementContent = await _elementServiceFactory.ReadElementContentAsync(tenantId, elementTypeId, elementId, pageId);
            if (elementSettings == null)
                return new ElementRender { PartialViewName = "~/Views/Elements/NotFound.cshtml" };

            ElementView elementView = new ElementView
            {
                Settings = elementSettings,
                Content = elementContent
            };
            return new ElementRender
            {
                PartialViewName = $"~/Views/Elements/{elementTypeId}.cshtml",
                ElementView = elementView
            };
        }

        [HttpGet]
        public async Task<IActionResult> Read(long pageId)
        {
            long tenantId = TenantId;

            PageView pageView = await _pageViewService.ReadPageViewAsync(tenantId, pageId);
            pageView.PageViewZones = await _pageViewService.SearchPageViewZonesAsync(tenantId, pageId);
            foreach (PageViewZone pageViewZone in pageView.PageViewZones)
                pageViewZone.PageViewZoneElements = await _pageViewService.SearchPageViewZoneElementsAsync(tenantId, pageId, pageViewZone.MasterPageZoneId);

            Dictionary<long, ElementRender> elements = new Dictionary<long, ElementRender>();
            foreach (PageViewZone pageViewZone in pageView.PageViewZones)
            {
                foreach (PageViewZoneElement pageViewZoneElement in pageViewZone.PageViewZoneElements)
                {
                    if (!elements.ContainsKey(pageViewZoneElement.ElementId))
                        elements.Add(pageViewZoneElement.ElementId, await GetElementRender(tenantId, pageViewZoneElement.ElementTypeId, pageViewZoneElement.ElementId, pageId));
                }
            }

            PageRender pageRender = new PageRender
            {
                View = pageView,
                Elements = elements
            };

            return View("Read", pageRender);
        }
    }
}

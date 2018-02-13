using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Riverside.Cms.Services.Core.Client;
using Riverside.Cms.Services.Element.Client;
using RiversideCms.Mvc.Models;

namespace RiversideCms.Mvc.Controllers
{
    public class PagesController : Controller
    {
        private readonly IPageViewService _pageViewService;

        private readonly IFooterElementService _footerElementService;
        private readonly IPageHeaderElementService _pageHeaderElementService;

        private const long TenantId = 6;

        public PagesController(IFooterElementService footerElementService, IPageHeaderElementService pageHeaderElementService, IPageViewService pageViewService)
        {
            _footerElementService = footerElementService;
            _pageHeaderElementService = pageHeaderElementService;
            _pageViewService = pageViewService;
        }

        private async Task<ElementRender> GetElementRender(long elementId, long pageId)
        {
            ElementRender elementRender = null;

            Guid elementTypeId = new Guid("1cbac30c-5deb-404e-8ea8-aabc20c82aa8");
            elementId = 162;

            switch (elementTypeId.ToString())
            {
                case "f1c2b384-4909-47c8-ada7-cd3cc7f32620":
                    FooterElementSettings footerElementSettings = await _footerElementService.ReadElementSettingsAsync(TenantId, elementId);
                    FooterElementContent footerElementContent = await _footerElementService.ReadElementContentAsync(TenantId, elementId, pageId);
                    ElementView<FooterElementSettings, FooterElementContent> footerElementView = new ElementView<FooterElementSettings, FooterElementContent>()
                    {
                        Settings = footerElementSettings,
                        Content = footerElementContent
                    };
                    elementRender = new ElementRender
                    {
                        PartialViewName = "~/Views/Elements/Footer.cshtml",
                        ElementView = footerElementView
                    };
                    break;

                case "1cbac30c-5deb-404e-8ea8-aabc20c82aa8":
                    PageHeaderElementSettings pageHeaderElementSettings = await _pageHeaderElementService.ReadElementSettingsAsync(TenantId, elementId);
                    PageHeaderElementContent pageHeaderElementContent = await _pageHeaderElementService.ReadElementContentAsync(TenantId, elementId, pageId);
                    ElementView<PageHeaderElementSettings, PageHeaderElementContent> pageHeaderElementView = new ElementView<PageHeaderElementSettings, PageHeaderElementContent>()
                    {
                        Settings = pageHeaderElementSettings,
                        Content = pageHeaderElementContent
                    };
                    elementRender = new ElementRender
                    {
                        PartialViewName = "~/Views/Elements/PageHeader.cshtml",
                        ElementView = pageHeaderElementView
                    };
                    break;
            }

            return elementRender;
        }

        [HttpGet]
        public async Task<IActionResult> Read(long pageId)
        {
            PageView pageView = await _pageViewService.ReadPageViewAsync(TenantId, pageId);
            pageView.PageViewZones = await _pageViewService.SearchPageViewZonesAsync(TenantId, pageId);
            foreach (PageViewZone pageViewZone in pageView.PageViewZones)
                pageViewZone.PageViewZoneElements = await _pageViewService.SearchPageViewZoneElementsAsync(TenantId, pageId, pageViewZone.MasterPageZoneId);

            Dictionary<long, ElementRender> elements = new Dictionary<long, ElementRender>();
            foreach (PageViewZone pageViewZone in pageView.PageViewZones)
            {
                foreach (PageViewZoneElement pageViewZoneElement in pageViewZone.PageViewZoneElements)
                {
                    if (!elements.ContainsKey(pageViewZoneElement.ElementId))
                        elements.Add(pageViewZoneElement.ElementId, await GetElementRender(pageViewZoneElement.ElementId, pageId));
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

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

        private readonly ICodeSnippetElementService _codeSnippetElementService;
        private readonly IFooterElementService _footerElementService;
        private readonly IPageHeaderElementService _pageHeaderElementService;

        private const long TenantId = 6;

        public PagesController(ICodeSnippetElementService codeSnippetElementService, IFooterElementService footerElementService, IPageHeaderElementService pageHeaderElementService, IPageViewService pageViewService)
        {
            _pageViewService = pageViewService;

            _codeSnippetElementService = codeSnippetElementService;
            _footerElementService = footerElementService;
            _pageHeaderElementService = pageHeaderElementService;
        }

        private async Task<ElementRender> GetElementRender(long tenantId, Guid elementTypeId, long elementId, long pageId)
        {
            ElementRender elementRender = null;

            switch (elementTypeId.ToString())
            {
                case "5401977d-865f-4a7a-b416-0a26305615de":
                    CodeSnippetElementSettings codeSnippetElementSettings = await _codeSnippetElementService.ReadElementSettingsAsync(tenantId, elementId);
                    ElementView<CodeSnippetElementSettings> codeSnippetElementView = new ElementView<CodeSnippetElementSettings>()
                    {
                        Settings = codeSnippetElementSettings
                    };
                    elementRender = new ElementRender
                    {
                        PartialViewName = "~/Views/Elements/CodeSnippet.cshtml",
                        ElementView = codeSnippetElementView
                    };
                    break;

                case "f1c2b384-4909-47c8-ada7-cd3cc7f32620":
                    FooterElementSettings footerElementSettings = await _footerElementService.ReadElementSettingsAsync(tenantId, elementId);
                    FooterElementContent footerElementContent = await _footerElementService.ReadElementContentAsync(tenantId, elementId, pageId);
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
                    PageHeaderElementSettings pageHeaderElementSettings = await _pageHeaderElementService.ReadElementSettingsAsync(tenantId, elementId);
                    PageHeaderElementContent pageHeaderElementContent = await _pageHeaderElementService.ReadElementContentAsync(tenantId, elementId, pageId);
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

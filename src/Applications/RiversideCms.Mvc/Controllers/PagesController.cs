using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Riverside.Cms.Services.Core.Client;
using RiversideCms.Mvc.Models;

namespace RiversideCms.Mvc.Controllers
{
    public class PagesController : Controller
    {
        private readonly IMasterPageService _masterPageService;
        private readonly IPageService _pageService;

        private const long TenantId = 6;

        public PagesController(IMasterPageService masterPageService, IPageService pageService)
        {
            _masterPageService = masterPageService;
            _pageService = pageService;
        }

        [HttpGet]
        public async Task<IActionResult> Read(long id)
        {
            long pageId = id;
            Page page = await _pageService.ReadPageAsync(TenantId, pageId);
            page.PageZones = await _pageService.SearchPageZonesAsync(TenantId, pageId);
            foreach (PageZone pageZone in page.PageZones)
                pageZone.PageZoneElements = await _pageService.SearchPageZoneElementsAsync(TenantId, pageId, pageZone.PageZoneId);

            long masterPageId = page.MasterPageId;
            MasterPage masterPage = await _masterPageService.ReadMasterPageAsync(TenantId, masterPageId);
            masterPage.MasterPageZones = await _masterPageService.SearchMasterPageZonesAsync(TenantId, masterPageId);
            foreach (MasterPageZone masterPageZone in masterPage.MasterPageZones)
                masterPageZone.MasterPageZoneElements = await _masterPageService.SearchMasterPageZoneElementsAsync(TenantId, masterPageId, masterPageZone.MasterPageZoneId);

            PageViewModel pageViewModel = new PageViewModel
            {
                MasterPage = masterPage,
                Page = page
            };
            return View("Read", pageViewModel);
        }
    }
}

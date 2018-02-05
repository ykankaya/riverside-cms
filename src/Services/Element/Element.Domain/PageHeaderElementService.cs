using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Services.Core.Client;

namespace Riverside.Cms.Services.Element.Domain
{
    public class PageHeaderElementService : IPageHeaderElementService
    {
        private readonly IElementRepository<PageHeaderElementSettings> _elementRepository;
        private readonly IPageService _pageService;

        public PageHeaderElementService(IElementRepository<PageHeaderElementSettings> elementRepository, IPageService pageService)
        {
            _elementRepository = elementRepository;
            _pageService = pageService;
        }

        public Task<PageHeaderElementSettings> ReadElementAsync(long tenantId, long elementId)
        {
            return _elementRepository.ReadElementAsync(tenantId, elementId);
        }

        private async void PopulatePageHierarchy(Page page)
        {
            Page hierarchyPage = page;
            while (hierarchyPage != null && hierarchyPage.ParentPageId != null)
            {
                hierarchyPage.ParentPage = await _pageService.ReadPageAsync(page.TenantId, hierarchyPage.ParentPageId.Value);
                hierarchyPage = hierarchyPage.ParentPage;
            }
        }

        public async Task<PageHeaderElementView> GetElementViewAsync(long tenantId, long elementId, long pageId)
        {
            PageHeaderElementSettings elementSettings = await _elementRepository.ReadElementAsync(tenantId, elementId);

            Page page = await _pageService.ReadPageAsync(tenantId, pageId);

            if (elementSettings.ShowBreadcrumbs)
                PopulatePageHierarchy(page);

            PageHeaderElementContent elementContent = new PageHeaderElementContent
            {
                Page = page
            };

            PageHeaderElementView elementView = new PageHeaderElementView
            {
                Settings = elementSettings,
                Content = elementContent
            };

            return elementView;
        }
    }
}

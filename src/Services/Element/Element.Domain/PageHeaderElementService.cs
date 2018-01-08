using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Services.Core.Client;

namespace Riverside.Cms.Services.Element.Domain
{
    public class PageHeaderElementService : IElementService<PageHeaderElementSettings, PageHeaderElementContent>
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

        public async Task<PageHeaderElementContent> ReadElementContentAsync(long tenantId, long elementId, long pageId)
        {
            Page page = await _pageService.ReadPageAsync(tenantId, pageId);
            PageHeaderElementContent pageHeaderElementContent = new PageHeaderElementContent
            {
                Page = page
            };
            return pageHeaderElementContent;
        }
    }
}

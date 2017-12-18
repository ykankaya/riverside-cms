using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Services.Core.Domain
{
    public class PageService : IPageService
    {
        private readonly IPageRepository _pageRepository;

        public PageService(IPageRepository pageRepository)
        {
            _pageRepository = pageRepository;
        }

        public Task<Page> ReadPageAsync(long tenantId, long pageId)
        {
            return _pageRepository.ReadPageAsync(tenantId, pageId);
        }
    }
}

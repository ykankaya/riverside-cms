using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Services.Core.Domain
{
    public class PageViewService : IPageViewService
    {
        private readonly IMasterPageRepository _masterPageRepository;
        private readonly IPageRepository _pageRepository;

        public PageViewService(IMasterPageRepository masterPageRepository, IPageRepository pageRepository)
        {
            _masterPageRepository = masterPageRepository;
            _pageRepository = pageRepository;
        }

        public async Task<PageView> ReadPageViewAsync(long tenantId, long pageId)
        {
            Page page = await _pageRepository.ReadPageAsync(tenantId, pageId);
            if (page == null)
                return null;
            MasterPage masterPage = await _masterPageRepository.ReadMasterPageAsync(tenantId, page.MasterPageId);
            PageView pageView = new PageView
            {
                Title = page.Name,
                BeginRender = masterPage.BeginRender,
                EndRender = masterPage.EndRender
            };
            return pageView;
        }
    }
}

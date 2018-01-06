using System;
using System.Collections.Generic;
using System.Linq;
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
                TenantId = tenantId,
                MasterPageId = masterPage.MasterPageId,
                PageId = pageId,
                Title = page.Name,
                BeginRender = masterPage.BeginRender,
                EndRender = masterPage.EndRender
            };
            return pageView;
        }

        private PageViewZone GetPageViewZoneFromMasterPageZone(long pageId, MasterPageZone masterPageZone)
        {
            PageViewZone pageViewZone = new PageViewZone
            {
                TenantId = masterPageZone.TenantId,
                MasterPageId = masterPageZone.MasterPageId,
                MasterPageZoneId = masterPageZone.MasterPageZoneId,
                PageId = pageId,
                BeginRender = masterPageZone.BeginRender,
                EndRender = masterPageZone.EndRender
            };
            return pageViewZone;
        }

        private IEnumerable<PageViewZone> EnumeratePageViewZones(long pageId, IEnumerable<MasterPageZone> masterPageZones)
        {
            foreach (MasterPageZone masterPageZone in masterPageZones)
            {
                PageViewZone pageViewZone = GetPageViewZoneFromMasterPageZone(pageId, masterPageZone);
                yield return pageViewZone;
            }
        }

        public async Task<IEnumerable<PageViewZone>> SearchPageViewZonesAsync(long tenantId, long pageId)
        {
            Page page = await _pageRepository.ReadPageAsync(tenantId, pageId);
            if (page == null)
                return Enumerable.Empty<PageViewZone>();
            IEnumerable<MasterPageZone> masterPageZones = await _masterPageRepository.SearchMasterPageZonesAsync(tenantId, page.MasterPageId);
            return EnumeratePageViewZones(pageId, masterPageZones);
        }

        public async Task<PageViewZone> ReadPageViewZoneAsync(long tenantId, long pageId, long masterPageZoneId)
        {
            Page page = await _pageRepository.ReadPageAsync(tenantId, pageId);
            if (page == null)
                return null;
            MasterPageZone masterPageZone = await _masterPageRepository.ReadMasterPageZoneAsync(tenantId, page.MasterPageId, masterPageZoneId);
            if (masterPageZone == null)
                return null;
            return GetPageViewZoneFromMasterPageZone(pageId, masterPageZone);
        }
    }
}

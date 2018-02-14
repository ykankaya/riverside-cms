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
        private readonly IElementRepository _elementRepository;

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

        private IEnumerable<PageViewZoneElement> EnumeratePageViewZoneElements(IEnumerable<MasterPageZoneElement> masterPageZoneElements)
        {
            foreach (MasterPageZoneElement masterPageZoneElement in masterPageZoneElements)
            {
                PageViewZoneElement pageViewZoneElement = new PageViewZoneElement
                {
                    ElementTypeId = masterPageZoneElement.ElementTypeId,
                    ElementId = masterPageZoneElement.ElementId,
                    BeginRender = masterPageZoneElement.BeginRender,
                    EndRender = masterPageZoneElement.EndRender
                };
                yield return pageViewZoneElement;
            }
        }

        private IEnumerable<PageViewZoneElement> EnumeratePageViewZoneElements(IEnumerable<PageZoneElement> pageZoneElements)
        {
            foreach (PageZoneElement pageZoneElement in pageZoneElements)
            {
                PageViewZoneElement pageViewZoneElement = new PageViewZoneElement
                {
                    ElementTypeId = pageZoneElement.ElementTypeId,
                    ElementId = pageZoneElement.ElementId
                };
                yield return pageViewZoneElement;
            }
        }

        private IEnumerable<PageViewZoneElement> EnumeratePageViewZoneElements(IEnumerable<PageZoneElement> pageZoneElements, IEnumerable<MasterPageZoneElement> masterPageZoneElements)
        {
            foreach (MasterPageZoneElement masterPageZoneElement in masterPageZoneElements)
            {
                PageZoneElement pageZoneElement = pageZoneElements.Where(e => e.MasterPageZoneElementId == masterPageZoneElement.MasterPageZoneElementId).FirstOrDefault();
                if (pageZoneElement == null)
                    continue;
                PageViewZoneElement pageViewZoneElement = new PageViewZoneElement
                {
                    ElementTypeId = masterPageZoneElement.ElementTypeId,
                    ElementId = masterPageZoneElement.ElementId,
                    BeginRender = masterPageZoneElement.BeginRender,
                    EndRender = masterPageZoneElement.EndRender
                };
                yield return pageViewZoneElement;
            }
        }

        private async Task<long?> GetPageZoneId(long tenantId, long pageId, long masterPageZoneId)
        {
            IEnumerable<PageZone> pageZones = await _pageRepository.SearchPageZonesAsync(tenantId, pageId);
            PageZone pageZone = pageZones.Where(z => z.MasterPageZoneId == masterPageZoneId).FirstOrDefault();
            if (pageZone == null)
                return null;
            return pageZone.PageZoneId;
        }

        public async Task<IEnumerable<PageViewZoneElement>> SearchPageViewZoneElementsAsync(long tenantId, long pageId, long masterPageZoneId)
        {
            Page page = await _pageRepository.ReadPageAsync(tenantId, pageId);
            if (page == null)
                return Enumerable.Empty<PageViewZoneElement>();
            MasterPageZone masterPageZone = await _masterPageRepository.ReadMasterPageZoneAsync(tenantId, page.MasterPageId, masterPageZoneId);
            if (masterPageZone == null)
                return Enumerable.Empty<PageViewZoneElement>();
            switch (masterPageZone.AdminType)
            {
                case AdminType.Editable:
                    long? editablePageZoneId = await GetPageZoneId(tenantId, pageId, masterPageZoneId);
                    if (editablePageZoneId == null)
                        return Enumerable.Empty<PageViewZoneElement>();
                    IEnumerable<PageZoneElement> editablePageZoneElements = await _pageRepository.SearchPageZoneElementsAsync(tenantId, pageId, editablePageZoneId.Value);
                    IEnumerable<MasterPageZoneElement> editableMasterPageZoneElements = await _masterPageRepository.SearchMasterPageZoneElementsAsync(tenantId, page.MasterPageId, masterPageZoneId);
                    return EnumeratePageViewZoneElements(editablePageZoneElements, editableMasterPageZoneElements);

                case AdminType.Configurable:
                    long? pageZoneId = await GetPageZoneId(tenantId, pageId, masterPageZoneId);
                    if (pageZoneId == null)
                        return Enumerable.Empty<PageViewZoneElement>();
                    IEnumerable<PageZoneElement> pageZoneElements = await _pageRepository.SearchPageZoneElementsAsync(tenantId, pageId, pageZoneId.Value);
                    return EnumeratePageViewZoneElements(pageZoneElements);

                default: // AdminType.Static
                    IEnumerable<MasterPageZoneElement> masterPageZoneElements = await _masterPageRepository.SearchMasterPageZoneElementsAsync(tenantId, page.MasterPageId, masterPageZoneId);
                    return EnumeratePageViewZoneElements(masterPageZoneElements);
            }
        }
    }
}

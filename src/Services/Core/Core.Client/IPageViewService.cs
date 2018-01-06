using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Services.Core.Client
{
    public interface IPageViewService
    {
        Task<PageView> ReadPageViewAsync(long tenantId, long pageId);

        Task<List<PageViewZone>> SearchPageViewZonesAsync(long tenantId, long pageId);
        Task<PageViewZone> ReadPageViewZoneAsync(long tenantId, long pageId, long masterPageZoneId);

        Task<List<PageViewZoneElement>> SearchPageViewZoneElementsAsync(long tenantId, long pageId, long masterPageZoneId);
    }
}

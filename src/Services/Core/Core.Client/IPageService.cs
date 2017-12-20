using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Services.Core.Client
{
    public interface IPageService
    {
        Task<Page> ReadPageAsync(long tenantId, long pageId);

        Task<List<PageZone>> SearchPageZonesAsync(long tenantId, long pageId);
        Task<PageZone> ReadPageZoneAsync(long tenantId, long pageId, long pageZoneId);

        Task<List<PageZoneElement>> SearchPageZoneElementsAsync(long tenantId, long pageId, long pageZoneId);
        Task<PageZoneElement> ReadPageZoneElementAsync(long tenantId, long pageId, long pageZoneId, long pageZoneElementId);
    }
}

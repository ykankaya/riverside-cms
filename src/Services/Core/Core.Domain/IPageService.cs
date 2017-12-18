using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Services.Core.Domain
{
    public interface IPageService
    {
        Task<Page> ReadPageAsync(long tenantId, long pageId);

        Task<PageZone> ReadPageZoneAsync(long tenantId, long pageId, long pageZoneId);
    }
}

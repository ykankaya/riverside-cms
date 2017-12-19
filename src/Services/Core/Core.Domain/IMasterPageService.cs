using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Services.Core.Domain
{
    public interface IMasterPageService
    {
        Task<MasterPage> ReadMasterPageAsync(long tenantId, long masterPageId);

        Task<IEnumerable<MasterPageZone>> SearchMasterPageZonesAsync(long tenantId, long masterPageId);
        Task<MasterPageZone> ReadMasterPageZoneAsync(long tenantId, long masterPageId, long masterPageZoneId);
    }
}

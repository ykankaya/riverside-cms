using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Services.Core.Domain
{
    public interface IMasterPageRepository
    {
        Task<MasterPage> ReadMasterPageAsync(long tenantId, long masterPageId);

        Task<IEnumerable<MasterPageZone>> SearchMasterPageZonesAsync(long tenantId, long masterPageId);
        Task<MasterPageZone> ReadMasterPageZoneAsync(long tenantId, long masterPageId, long masterPageZoneId);

        Task<IEnumerable<MasterPageZoneElement>> SearchMasterPageZoneElementsAsync(long tenantId, long masterPageId, long masterPageZoneId);
        Task<MasterPageZoneElement> ReadMasterPageZoneElementAsync(long tenantId, long masterPageId, long masterPageZoneId, long masterPageZoneElementId);
    }
}

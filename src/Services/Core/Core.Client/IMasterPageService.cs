using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Services.Core.Client
{
    public interface IMasterPageService
    {
        Task<MasterPage> ReadMasterPageAsync(long tenantId, long masterPageId);

        Task<List<MasterPageZone>> SearchMasterPageZonesAsync(long tenantId, long masterPageId);
        Task<MasterPageZone> ReadMasterPageZoneAsync(long tenantId, long masterPageId, long masterPageZoneId);

        Task<List<MasterPageZoneElement>> SearchMasterPageZoneElementsAsync(long tenantId, long masterPageId, long masterPageZoneId);
        Task<MasterPageZoneElement> ReadMasterPageZoneElementAsync(long tenantId, long masterPageId, long masterPageZoneId, long masterPageZoneElementId);
    }
}

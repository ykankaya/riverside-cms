using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Services.Core.Domain
{
    public class MasterPageService : IMasterPageService
    {
        private readonly IMasterPageRepository _masterPageRepository;

        public MasterPageService(IMasterPageRepository masterPageRepository)
        {
            _masterPageRepository = masterPageRepository;
        }

        public Task<MasterPage> ReadMasterPageAsync(long tenantId, long masterPageId)
        {
            return _masterPageRepository.ReadMasterPageAsync(tenantId, masterPageId);
        }

        public Task<IEnumerable<MasterPageZone>> SearchMasterPageZonesAsync(long tenantId, long masterPageId)
        {
            return _masterPageRepository.SearchMasterPageZonesAsync(tenantId, masterPageId);
        }

        public Task<MasterPageZone> ReadMasterPageZoneAsync(long tenantId, long masterPageId, long masterPageZoneId)
        {
            return _masterPageRepository.ReadMasterPageZoneAsync(tenantId, masterPageId, masterPageZoneId);
        }

        public Task<MasterPageZoneElement> ReadMasterPageZoneElementAsync(long tenantId, long masterPageId, long masterPageZoneId, long masterPageZoneElementId)
        {
            return _masterPageRepository.ReadMasterPageZoneElementAsync(tenantId, masterPageId, masterPageZoneId, masterPageZoneElementId);
        }
    }
}

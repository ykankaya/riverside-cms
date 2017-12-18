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
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Services.Storage.Domain
{
    public interface IStorageRepository
    {
        Task<Blob> ReadBlobAsync(long tenantId, long uploadId);
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Services.Storage.Domain
{
    public interface IBlobService
    {
        Task CreateBlobContentAsync(Blob blob, Stream stream);
        Task<Stream> ReadBlobContentAsync(Blob blob);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Cms.Services.Storage.Domain
{
    public class Blob
    {
        public long TenantId { get; set; }
        public long UploadId { get; set; }
        public int Size { get; set; }
        public string ContentType { get; set; }
        public List<string> Location { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}

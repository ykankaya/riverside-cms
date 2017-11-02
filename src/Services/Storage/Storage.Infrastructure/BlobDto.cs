using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Cms.Services.Storage.Infrastructure
{
    public class BlobDto
    {
        public long TenantId { get; set; }
        public long BlobId { get; set; }
        public int Size { get; set; }
        public string ContentType { get; set; }
        public string Folder1 { get; set; }
        public string Folder2 { get; set; }
        public string Folder3 { get; set; }
        public string Name { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}

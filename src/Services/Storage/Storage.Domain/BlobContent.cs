using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Riverside.Cms.Services.Storage.Domain
{
    public class BlobContent
    {
        public Stream Stream { get; set; }
        public string Type { get; set; }
    }
}

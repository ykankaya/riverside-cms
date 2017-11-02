using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Cms.Services.Storage.Domain
{
    public class BlobImage : Blob
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }
}

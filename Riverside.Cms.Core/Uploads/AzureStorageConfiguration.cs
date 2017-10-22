using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Uploads
{
    public class AzureStorageConfiguration
    {
        public string BlobContainerName { get; set; }
        public string BlobName { get; set; }
        public string BlobPath { get; set; }
        public string ConnectionString { get; set; }
    }
}

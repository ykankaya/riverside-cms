using Riverside.Cms.Core.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.TagCloud
{
    public class TagCloudSettings : ElementSettings
    {
        public long? PageTenantId { get; set; }
        public long? PageId { get; set; }
        public string DisplayName { get; set; }
        public bool Recursive { get; set; }
        public string NoTagsMessage { get; set; }
    }
}

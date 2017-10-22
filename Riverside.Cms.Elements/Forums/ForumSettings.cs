using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;

namespace Riverside.Cms.Elements.Forums
{
    public class ForumSettings : ElementSettings
    {
        public int ThreadCount { get; set; }
        public int PostCount { get; set; }
        public long? OwnerTenantId { get; set; }
        public long? OwnerUserId { get; set; }
        public bool OwnerOnlyThreads { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Elements.Forums;

namespace Riverside.Cms.Elements.LatestThreads
{
    public class LatestThreadContent : ElementContent
    {
        public ForumThreads Threads { get; set; }
    }
}

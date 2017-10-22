using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.Forums
{
    public class ForumThreads : List<ForumThreadExtended>
    {
        /// <summary>
        /// The total number of threads that would have been returned if no paging in place.
        /// </summary>
        public int Total { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;

namespace Riverside.Cms.Elements.LatestThreads
{
    public class LatestThreadSettings : ElementSettings
    {
        /// <summary>
        /// In combination with PageId, used to determine page whose threads are displayed (can be null to display current page threads).
        /// </summary>
        public long? PageTenantId { get; set; }

        /// <summary>
        /// In combination with PageTenantId, used to determine page whose threads are displayed (can be null to display current page threads).
        /// </summary>
        public long? PageId { get; set; }

        /// <summary>
        /// Display name is a header that is rendered at the top of latest thread element. Can be left null if a latest thread header is not required.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// If true, latest thread element also shows latest threads of child pages below specified page.
        /// </summary>
        public bool Recursive { get; set; }

        /// <summary>
        /// The message displayed when no threads are displayed in the list.
        /// </summary>
        public string NoThreadsMessage { get; set; }

        /// <summary>
        /// If set, preamble text rendered above latest thread list. Can be left null if not required.
        /// </summary>
        public string Preamble { get; set; }

        /// <summary>
        /// The number of threads to display.
        /// </summary>
        public int PageSize { get; set; }
    }
}

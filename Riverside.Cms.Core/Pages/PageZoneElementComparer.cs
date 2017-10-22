using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Pages
{
    /// <summary>
    /// Used to sort list of page zone elements, based on sort order.
    /// </summary>
    public class PageZoneElementComparer : IComparer<PageZoneElement>
    {
        /// <summary>
        /// Returns comparison of two page zone elements, based on sort order.
        /// </summary>
        /// <param name="x">Page zone element 1.</param>
        /// <param name="y">Page zone element 2.</param>
        /// <returns>A signed integer that indicates the relative values of x and y.</returns>
        public int Compare(PageZoneElement x, PageZoneElement y)
        {
            return (x.SortOrder ?? 0) - (y.SortOrder ?? 0);
        }
    }
}

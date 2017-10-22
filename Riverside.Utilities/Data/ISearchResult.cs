using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Utilities.Data
{
    public interface ISearchResult<TItem>
    {
        /// <summary>
        /// The total number of items that would have been returned if no paging in place.
        /// </summary>
        int Total { get; set; }

        /// <summary>
        /// Collection of items found.
        /// </summary>
        IEnumerable<TItem> Items { get; set; }
    }
}

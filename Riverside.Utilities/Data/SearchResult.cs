using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Utilities.Data
{
    public class SearchResult<TItem> : ISearchResult<TItem>
    {
        public int Total { get; set; }
        public IEnumerable<TItem> Items { get; set; }
    }
}

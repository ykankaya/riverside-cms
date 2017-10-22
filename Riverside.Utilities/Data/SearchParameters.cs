using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Utilities.Data
{
    public class SearchParameters : ISearchParameters
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Search { get; set; }
    }
}

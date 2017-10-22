using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Utilities.Data
{
    public interface ISearchParameters
    {
        /// <summary>
        /// The zero-based index determining the page of items returned.
        /// </summary>
        int PageIndex { get; set; }

        /// <summary>
        /// The number of items returned per page.
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// Search terms used to filter results. Set null if no filter required.
        /// </summary>
        string Search { get; set; }
    }
}

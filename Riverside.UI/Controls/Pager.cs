using System;
using System.Collections.Generic;
using System.Text;
using Riverside.UI.Routes;

namespace Riverside.UI.Controls
{
    /// <summary>
    /// Holds all information required to render a pager.
    /// </summary>
    public class Pager
    {
        /// <summary>
        /// The 1-based index determining the page of items returned.
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// The number of items returned per page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// When known, set this value to the total number of items that would have been returned if no paging in place.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// The number of pages.
        /// </summary>
        public int PageCount
        {
            get
            {
                return ((Total - 1) / PageSize) + 1;
            }
        }

        /// <summary>
        /// URL parameters that determine search and paging links.
        /// </summary>
        public UrlParameters UrlParameters { get; set; }
    }
}

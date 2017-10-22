using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;

namespace Riverside.Cms.Core.Pages
{
    /// <summary>
    /// Information required to render a page hyperlink.
    /// </summary>
    public class PageLinkViewModel : IPageLink
    {
        /// <summary>
        /// Contains CMS information about a page.
        /// </summary>
        public Page Page { get; set; }

        /// <summary>
        /// The text that should appear on the page hyperlink.
        /// </summary>
        public string LinkText { get; set; }

        /// <summary>
        /// SEO text that is displayed just before any query string on the page hyperlink.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Route values that are added to the constructed hyperlink.
        /// </summary>
        public object RouteValues { get; set; }

        /// <summary>
        /// Html attributes that will adorn the page hyperlink.
        /// </summary>
        public object HtmlAttributes { get; set; }
    }
}

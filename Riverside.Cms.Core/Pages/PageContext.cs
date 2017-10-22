using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.MasterPages;
using Riverside.Cms.Core.Templates;
using Riverside.Cms.Core.Webs;

namespace Riverside.Cms.Core.Pages
{
    /// <summary>
    /// Contains salient information about the current page load.
    /// </summary>
    public class PageContext : IPageContext
    {
        /// <summary>
        /// The website whose page is being viewed.
        /// </summary>
        public Web Web { get; set; }

        /// <summary>
        /// The current page that is being viewed, including zones and elements.
        /// </summary>
        public Page Page { get; set; }

        /// <summary>
        /// Page hierarchy, excludes zones and elements.
        /// </summary>
        public Page Hierarchy { get; set; }

        /// <summary>
        /// The master page on which the current page is based.
        /// </summary>
        public MasterPage MasterPage { get; set; }

        /// <summary>
        /// Holds query string (or other) parameters.
        /// </summary>
        public IDictionary<string, string> Parameters { get; set; }

        /// <summary>
        /// Holds route values.
        /// </summary>
        public IDictionary<string, object> RouteValues { get; set; }

        /// <summary>
        /// List of selected tags that may be used to filter content.
        /// </summary>
        public IList<Tag> Tags { get; set; }

        /// <summary>
        /// True if page is for updating element content. TODO: Remove this hack.
        /// </summary>
        public bool UpdateElement { get; set; }
    }
}

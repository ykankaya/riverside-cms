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
    /// Contains information about the current page.
    /// </summary>
    public interface IPageContext
    {
        /// <summary>
        /// The website whose page is being viewed.
        /// </summary>
        Web Web { get; set; }

        /// <summary>
        /// The current page that is being viewed, including zones and elements.
        /// </summary>
        Page Page { get; set; }

        /// <summary>
        /// Page hierarchy, excludes zones and elements.
        /// </summary>
        Page Hierarchy { get; set; }

        /// <summary>
        /// The master page on which the current page is based.
        /// </summary>
        MasterPage MasterPage { get; set; }

        /// <summary>
        /// Holds query string (or other) parameters.
        /// </summary>
        IDictionary<string, string> Parameters { get; set; }

        /// <summary>
        /// Holds route values.
        /// </summary>
        IDictionary<string, object> RouteValues { get; set; }

        /// <summary>
        /// List of selected tags that may be used to filter content.
        /// </summary>
        IList<Tag> Tags { get; set; }

        /// <summary>
        /// True if page is for updating element content. TODO: Remove this hack.
        /// </summary>
        bool UpdateElement { get; set; }
    }
}

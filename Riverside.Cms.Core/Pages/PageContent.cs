using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;

namespace Riverside.Cms.Core.Pages
{
    /// <summary>
    /// Holds element content for page administration.
    /// </summary>
    public class PageContent : ElementContent
    {
        /// <summary>
        /// Form context, used whenever a form is required for a given page action.
        /// </summary>
        public string FormContext { get; set; }
    }
}

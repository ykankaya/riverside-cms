using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;

namespace Riverside.Cms.Core.Pages
{
    /// <summary>
    /// View model used to render a page zone element.
    /// </summary>
    public class PageZoneElementViewModel
    {
        /// <summary>
        /// Contains information used to render element.
        /// </summary>
        public IElementInfo ElementInfo { get; set; }

        /// <summary>
        /// HTML that is rendered before element.
        /// </summary>
        public string BeginRender { get; set; }

        /// <summary>
        /// HTML that is rendered after element.
        /// </summary>
        public string EndRender { get; set; }
    }
}

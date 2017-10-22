using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.MasterPages;

namespace Riverside.Cms.Core.Pages
{
    /// <summary>
    /// View model used to render a page zone.
    /// </summary>
    public class PageZoneViewModel
    {
        /// <summary>
        /// HTML that is rendered at start of page zone.
        /// </summary>
        public string BeginRender { get; set; }

        /// <summary>
        /// HTML that is rendered at end of page zone.
        /// </summary>
        public string EndRender { get; set; }

        /// <summary>
        /// The zone content type.
        /// </summary>
        public MasterPageZoneContentType ContentType { get; set; }

        /// <summary>
        /// The page zone element view models.
        /// </summary>
        public List<PageZoneElementViewModel> PageZoneElementViewModels { get; set; }
    }
}

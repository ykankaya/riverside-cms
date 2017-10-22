using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Administration;

namespace Riverside.Cms.Core.Pages
{
    /// <summary>
    /// View model used to render page.
    /// </summary>
    public class PageViewModel
    {
        /// <summary>
        /// Indicates whether or not master page associated with this page is the one master page determining layout when administering the CMS. 
        /// </summary>
        public bool Administration { get; set; }

        /// <summary>
        /// HTML that is rendered at start of page.
        /// </summary>
        public string BeginRender { get; set; }

        /// <summary>
        /// HTML that is rendered at end of page.
        /// </summary>
        public string EndRender { get; set; }

        /// <summary>
        /// Page zone view models.
        /// </summary>
        public List<PageZoneViewModel> PageZoneViewModels { get; set; }

        /// <summary>
        /// Page context.
        /// </summary>
        public IPageContext PageContext { get; set; }

        /// <summary>
        /// Administration options.
        /// </summary>
        public IAdministrationOptions Options { get; set; }

        /// <summary>
        /// The page title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Metadata keywords, used for search engine optimisation.
        /// </summary>
        public string MetaKeywords { get; set; }

        /// <summary>
        /// Metadata description, used for search engine optimisation.
        /// </summary>
        public string MetaDescription { get; set; }

        /// <summary>
        /// The relative path of site.css for the current website (e.g. "~/webs/{templateid}/assets/site.css").
        /// </summary>
        public string AssetStyleSheetPath { get; set; }

        /// <summary>
        /// The relative path of font option override for the current website (e.g. "~/webs/{templateid}/assets/option-font-Times New Roman.css").
        /// </summary>
        public string FontOptionStyleSheetPath { get; set; }

        /// <summary>
        /// The relative path of colour option override for the current website (e.g. "~/webs/{templateid}/assets/option-colour-Lime.css").
        /// </summary>
        public string ColourOptionStyleSheetPath { get; set; }

        /// <summary>
        /// TODO: Remove this.
        /// </summary>
        public string ForumAction { get; set; }
    }
}

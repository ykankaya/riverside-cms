using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Resources;

namespace Riverside.Cms.Core.Pages
{
    /// <summary>
    /// Identifies the different types of pages.
    /// </summary>
    public enum PageType
    {
        /// <summary>
        /// Pages of type folder can have child pages.
        /// </summary>
        [Display(ResourceType = typeof(PageResource), Name = "PageTypeFolderLabel")]
        Folder = 0,

        /// <summary>
        /// Pages of type document cannot have child pages. Documents are leaves in the page hierarchy.
        /// </summary>
        [Display(ResourceType = typeof(PageResource), Name = "PageTypeDocumentLabel")]
        Document = 1
    }
}

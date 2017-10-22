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
    /// Identifies the different page levels (ancestor relationships). The values of these enumerations (1, 2, 3) should not be changed.
    /// They values are used within the CMS code.
    /// </summary>
    public enum PageLevel
    {
        /// <summary>
        /// Parent (level 1).
        /// </summary>
        [Display(ResourceType = typeof(PageResource), Name = "PageLevelParentLabel")]
        Parent = 1,

        /// <summary>
        /// Grandparent (level 2).
        /// </summary>
        [Display(ResourceType = typeof(PageResource), Name = "PageLevelGrandparentLabel")]
        Grandparent = 2,

        /// <summary>
        /// Great grandparent (level 3).
        /// </summary>
        [Display(ResourceType = typeof(PageResource), Name = "PageLevelGreatGrandparentLabel")]
        GreatGrandparent = 3
    }
}

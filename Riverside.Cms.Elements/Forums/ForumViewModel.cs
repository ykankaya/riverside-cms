using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.Forums
{
    /// <summary>
    /// Contains information required by views to render forum components for different actions.
    /// </summary>
    public class ForumViewModel
    {
        /// <summary>
        /// Identifies forum action.
        /// </summary>
        public ForumAction Action { get; set; }
    }
}

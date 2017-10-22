using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;

namespace Riverside.Cms.Core.MasterPages
{
    /// <summary>
    /// Element content required for master page details and zones administration.
    /// </summary>
    public class MasterPageContent : ElementContent
    {
        /// <summary>
        /// Form context, used whenever a form is required for a given page action.
        /// </summary>
        public string FormContext { get; set; }
    }
}

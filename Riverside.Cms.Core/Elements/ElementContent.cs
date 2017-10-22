using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Elements
{
    /// <summary>
    /// Base class for dynamic element content.
    /// </summary>
    public class ElementContent : IElementContent
    {
        /// <summary>
        /// Name of partial view used to render element.
        /// </summary>
        public string PartialViewName { get; set; }
    }
}

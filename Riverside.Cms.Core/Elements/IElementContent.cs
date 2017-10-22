using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Elements
{
    /// <summary>
    /// Interface for classes that hold dynamic element content.
    /// </summary>
    public interface IElementContent
    {
        /// <summary>
        /// Name of partial view used to render element.
        /// </summary>
        string PartialViewName { get; set; }
    }
}

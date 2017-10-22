using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Elements
{
    /// <summary>
    /// Interface flags element content types that provide page links for other elements. These page links may be used
    /// to provide site navigation beyond the page level.
    /// </summary>
    public interface IPageLinkProvider
    {
        /// <summary>
        /// Page links that may be used to provide site navigation beyond the page level.
        /// </summary>
        /// <returns>List of page links.</returns>
        IList<IPageLink> PageLinks { get; set; }
    }
}

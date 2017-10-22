using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Pages;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Elements
{
    /// <summary>
    /// Interface that indicates a custom element service is for an element that implements site navigation.
    /// </summary>
    public interface INavigationElementService
    {
        /// <summary>
        /// Registers navigation pages with a site navigation element.
        /// </summary>
        /// <param name="tenantId">The tenant that site navigation element belongs to.</param>
        /// <param name="elementId">Identifies element that navigation pages are added to.</param>
        /// <param name="navigationPages">Navigation pages to be added.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void AddNavigationPages(long tenantId, long elementId, List<Page> navigationPages, IUnitOfWork unitOfWork = null);
    }
}

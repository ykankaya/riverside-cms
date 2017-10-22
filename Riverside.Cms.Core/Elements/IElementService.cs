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
    /// Interface for services that manage elements.
    /// </summary>
    public interface IElementService
    {
        /// <summary>
        /// Creates a new instance of a type of element settings.
        /// </summary>
        /// <param name="tenantId">Identifies the tenant that newly created element settings belong to.</param>
        /// <param name="elementTypeId">The type of the element whose instance is created and returned.</param>
        /// <returns>Newly created element settings instance.</returns>
        IElementSettings New(long tenantId, Guid elementTypeId);

        /// <summary>
        /// Creates and returns strongly typed element info instance, populated with supplied element settings and content.
        /// </summary>
        /// <param name="settings">Element settings.</param>
        /// <param name="content">Element content.</param>
        /// <returns>An element info object containing settings and content.</returns>
        IElementInfo NewInfo(IElementSettings settings, IElementContent content);

        /// <summary>
        /// Creates a new element.
        /// </summary>
        /// <param name="settings">New element settings.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Newly allocated element identifier.</returns>
        long Create(IElementSettings settings, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Creates new element based on existing element.
        /// </summary>
        /// <param name="sourceTenantId">The source tenant.</param>
        /// <param name="sourceElementId">Identifies element instance to copy.</param>
        /// <param name="destTenantId">The destination tenant.</param>
        /// <param name="sourceElementTypeId">The type of the element that is copied (destination will be same type).</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Newly allocated element identifier.</returns>
        long Copy(long sourceTenantId, long sourceElementId, long destTenantId, Guid sourceElementTypeId, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Gets element details.
        /// </summary>
        /// <param name="tenantId">Identifies the tenant that element belongs to.</param>
        /// <param name="elementTypeId">The type of the element to return.</param>
        /// <param name="elementId">Identifies the element to return.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Element settings (or null if element not found).</returns>
        IElementSettings Read(long tenantId, Guid elementTypeId, long elementId, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Reads primary element details.
        /// </summary>
        /// <param name="tenantId">Tenant that element associated with.</param>
        /// <param name="elementId">Element identifier.</param>
        /// <param name="loadCustomSettings">Set false to retrieve only those settings common to all elements. Set true to retrieve settings common to all elements and element specific settings.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Element setings (null if element not found).</returns>
        IElementSettings Read(long tenantId, long elementId, bool loadCustomSettings, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Updates an element's details.
        /// </summary>
        /// <param name="settings">Updated element settings.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void Update(IElementSettings settings, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Deletes an element.
        /// </summary>
        /// <param name="tenantId">The tenant that element to delete belongs to.</param>
        /// <param name="elementId">Identifies the element to delete.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void Delete(long tenantId, long elementId, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Deletes an element.
        /// </summary>
        /// <param name="tenantId">The tenant that element to delete belongs to.</param>
        /// <param name="elementType">The type of the element to delete.</param>
        /// <param name="elementId">Identifies the element to delete.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void Delete(long tenantId, Guid elementTypeId, long elementId, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Retrieves dynamic element content.
        /// </summary>
        /// <param name="settings">Contains element settings.</param>
        /// <param name="pageContext">Page context.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Element content.</returns>
        IElementContent GetContent(IElementSettings settings, IPageContext pageContext, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// For the list of supplied element key values, this method works out which elements are to do with site navigation and then
        /// registers the navigation pages with those elements. In this way, elements such as nav bars can be populated with pages.
        /// </summary>
        /// <param name="tenantId">The tenant whose navigation elements are updated.</param>
        /// <param name="elementKeyValues">List of element key values.</param>
        /// <param name="navigationPages">Pages that are to be added to site navigation elements.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void AddNavigationPages(long tenantId, List<ElementKeyValue> elementKeyValues, List<Page> navigationPages, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Gets all registered element types. 
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Collection of element types.</returns>
        IEnumerable<ElementType> ListTypes(IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Get list of all elements belonging to a tenant.
        /// </summary>
        /// <param name="tenantId">Tenant identifier.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Collection of elements.</returns>
        IEnumerable<IElementSettings> ListElements(long tenantId, IUnitOfWork unitOfWork = null);
    }
}

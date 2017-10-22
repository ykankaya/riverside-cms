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
    /// The minimum interface that should be implemented by elements. Basic elements are created programatically do not require their settings to be persisted in a database. 
    /// Basic elements are only required to supply an element type identifier, create new IElementSettings and IElementInfo objects, populate their settings and supply element content.
    /// </summary>
    public interface IBasicElementService
    {
        /// <summary>
        /// Returns GUID, identifying the type of element that this custom element service is associated with.
        /// </summary>
        Guid ElementTypeId { get; }

        /// <summary>
        /// Creates a new instance of a type of element settings.
        /// </summary>
        /// <param name="tenantId">Identifies the tenant that newly created element settings belong to.</param>
        /// <returns>Newly created element instance.</returns>
        IElementSettings New(long tenantId);

        /// <summary>
        /// Creates and returns strongly typed element info instance, populated with supplied element settings and content.
        /// </summary>
        /// <param name="settings">Element settings.</param>
        /// <param name="content">Element content.</param>
        /// <returns>An element info object containing settings and content.</returns>
        IElementInfo NewInfo(IElementSettings settings, IElementContent content);

        /// <summary>
        /// Retrieves dynamic element content.
        /// </summary>
        /// <param name="settings">Contains element settings.</param>
        /// <param name="pageContext">Page context.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Element content.</returns>
        IElementContent GetContent(IElementSettings settings, IPageContext pageContext, IUnitOfWork unitOfWork = null);
    }
}

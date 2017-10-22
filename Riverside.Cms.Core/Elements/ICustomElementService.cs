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
    /// Interface that custom elements must implement.
    /// </summary>
    public interface ICustomElementService
    {
        /// <summary>
        /// Returns GUID, identifying the type of element that this custom element service is associated with.
        /// </summary>
        Guid ElementTypeId { get; }

        /// <summary>
        /// Creates a new instance of a type of element settings.
        /// </summary>
        /// <returns>Newly created element instance.</returns>
        IElementSettings New();

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
        /// <param name="settings">New element details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void Create(IElementSettings settings, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Copies source element to destination element.
        /// </summary>
        /// <param name="sourceElementId">Identifies source element.</param>
        /// <param name="destElementId">Identifies destination element.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void Copy(long sourceElementId, long destElementId, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Populates element settings.
        /// </summary>
        /// <param name="settings">Element settings to be populated.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void Read(IElementSettings settings, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Updates an element's details.
        /// </summary>
        /// <param name="settings">Updated element settings.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void Update(IElementSettings settings, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Deletes an element.
        /// </summary>
        /// <param name="elementId">Identifies the element to delete.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void Delete(long elementId, IUnitOfWork unitOfWork = null);

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

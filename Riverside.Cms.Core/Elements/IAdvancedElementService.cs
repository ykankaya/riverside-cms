using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Elements
{
    /// <summary>
    /// This interface should be implemented by elements that persist their settings in a database. Most elements will implement this service. Administration elements that are 
    /// created programmatically, on the fly, are more likely to only implement the IBasicElementService.
    /// </summary>
    public interface IAdvancedElementService : IBasicElementService
    {
        /// <summary>
        /// Creates a new element.
        /// </summary>
        /// <param name="settings">New element details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void Create(IElementSettings settings, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Copies source element to destination element.
        /// </summary>
        /// <param name="sourceTenantId">Source tenant identifier.</param>
        /// <param name="sourceElementId">Identifies source element.</param>
        /// <param name="destTenantId">Destination tenant identifier.</param>
        /// <param name="destElementId">Identifies destination element.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void Copy(long sourceTenantId, long sourceElementId, long destTenantId, long destElementId, IUnitOfWork unitOfWork = null);

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
        /// <param name="tenantId">The tenant that element to delete belongs to.</param>
        /// <param name="elementId">Identifies the element to delete.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void Delete(long tenantId, long elementId, IUnitOfWork unitOfWork = null);
    }
}

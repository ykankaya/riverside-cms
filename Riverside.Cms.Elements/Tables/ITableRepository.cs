using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Elements.Tables
{
    /// <summary>
    /// Repository for table elements.
    /// </summary>
    public interface ITableRepository
    {
        /// <summary>
        /// Creates a new table element.
        /// </summary>
        /// <param name="settings">Table settings.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void Create(TableSettings settings, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Copies a table element.
        /// </summary>
        /// <param name="sourceTenantId">Source tenant identifier.</param>
        /// <param name="sourceElementId">Source element identifier.</param>
        /// <param name="destTenantId">Destination tenant identifier.</param>
        /// <param name="destElementId">Destination element identifier.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void Copy(long sourceTenantId, long sourceElementId, long destTenantId, long destElementId, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Reads a table element.
        /// </summary>
        /// <param name="settings">Table settings.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void Read(TableSettings settings, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Updates a table element.
        /// </summary>
        /// <param name="settings">Table settings.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void Update(TableSettings settings, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Deletes a table element.
        /// </summary>
        /// <param name="tenantId">Tenant identifier.</param>
        /// <param name="elementId">Element identifier.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void Delete(long tenantId, long elementId, IUnitOfWork unitOfWork = null);
    }
}

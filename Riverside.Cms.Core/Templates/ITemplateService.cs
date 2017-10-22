using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Templates
{
    /// <summary>
    /// Interface for template services.
    /// </summary>
    public interface ITemplateService
    {
        /// <summary>
        /// Get template details.
        /// </summary>
        /// <param name="tenantId">Tenant identifies template.</param>
        /// <param name="loadAll">Set true to load entire template rather than just top level template details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Template details (or null if template not found).</returns>
        Template Read(long tenantId, bool loadAll, IUnitOfWork unitOfWork = null);
    }
}

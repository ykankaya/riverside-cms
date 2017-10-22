using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Templates;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Templates
{
    /// <summary>
    /// Service for management of templates.
    /// </summary>
    public class TemplateService : ITemplateService
    {
        // Member variables
        private ITemplateRepository _templateRepository;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="templateRepository">Template repository.</param>
        public TemplateService(ITemplateRepository templateRepository)
        {
            _templateRepository = templateRepository;
        }

        /// <summary>
        /// Get template details.
        /// </summary>
        /// <param name="tenantId">Tenant identifies template.</param>
        /// <param name="loadAll">Set true to load entire template rather than just top level template details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Template details (or null if template not found).</returns>
        public Template Read(long tenantId, bool loadAll, IUnitOfWork unitOfWork = null)
        {
            return _templateRepository.Read(tenantId, loadAll, unitOfWork);
        }
    }
}

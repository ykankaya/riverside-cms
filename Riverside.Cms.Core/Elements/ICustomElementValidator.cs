using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Elements
{
    /// <summary>
    /// Interface for types that provide custom element validation.
    /// </summary>
    public interface ICustomElementValidator
    {
        /// <summary>
        /// Validates create model. Throws validation error exception if validation fails.
        /// </summary>
        /// <param name="settings">The element to validate.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void ValidateCreate(IElementSettings settings, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Validates update model. Throws validation error exception if validation fails.
        /// </summary>
        /// <param name="settings">The element to validate.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void ValidateUpdate(IElementSettings settings, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Validates deletion. Throws validation error exception if validation fails.
        /// </summary>
        /// <param name="tenantId">The tenant that element to delete belongs to.</param>
        /// <param name="elementTypeId">Type of element.</param>
        /// <param name="elementId">Identifies element to delete.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void ValidateDelete(long tenantId, Guid elementTypeId, long elementId, IUnitOfWork unitOfWork = null);
    }
}

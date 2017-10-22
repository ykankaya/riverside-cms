using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Tenants
{
    /// <summary>
    /// Interfaces for repositories that manage tenants.
    /// </summary>
    public interface ITenantRepository
    {
        /// <summary>
        /// Creates a new tenant.
        /// </summary>
        /// <param name="tenant">New tenant details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Newly allocated tenant identifier.</returns>
        long Create(Tenant tenant, IUnitOfWork unitOfWork = null);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Domains
{
    /// <summary>
    /// Interface for services that manage domains.
    /// </summary>
    public interface IDomainService
    {
        /// <summary>
        /// Searches domains.
        /// </summary>
        /// <param name="tenantId">Identifies website whose domains are searched.</param>
        /// <param name="parameters">Search and paging parameters.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Search result.</returns>
        ISearchResult<Domain> Search(long tenantId, ISearchParameters parameters, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Creates a new domain.
        /// </summary>
        /// <param name="domain">New domain details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Newly allocated domain identifier.</returns>
        long Create(Domain domain, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Gets domain details.
        /// </summary>
        /// <param name="tenantId">Identifies website whose domain is returned.</param>
        /// <param name="domainId">Identifies the domain to return.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Domain details (or null if domain not found).</returns>
        Domain Read(long tenantId, long domainId, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Gets domain details given a URL.
        /// </summary>
        /// <param name="url">URL identifying domain.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Domain details (or null if domain not found).</returns>
        Domain ReadByUrl(string url, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Updates a domain's details.
        /// </summary>
        /// <param name="domain">Updated domain details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void Update(Domain domain, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Deletes a domain.
        /// </summary>
        /// <param name="tenantId">Identifies website whose domain is to deleted.</param>
        /// <param name="domainId">Identifies the domain to delete.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void Delete(long tenantId, long domainId, IUnitOfWork unitOfWork = null);
    }
}

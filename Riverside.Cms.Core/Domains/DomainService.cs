using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Domains;
using Riverside.Cms.Core.Resources;
using Riverside.Utilities.Data;
using Riverside.Utilities.Validation;

namespace Riverside.Cms.Core.Domains
{
    /// <summary>
    /// Interface for services that manage domains.
    /// </summary>
    public class DomainService : IDomainService
    {
        // Member variables
        private IDomainRepository _domainRepository;
        private IDomainValidator _domainValidator;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="domainRepository">Domain repository.</param>
        /// <param name="domainValidator">Domain validator.</param>
        public DomainService(IDomainRepository domainRepository, IDomainValidator domainValidator)
        {
            _domainRepository = domainRepository;
            _domainValidator = domainValidator;
        }

        /// <summary>
        /// Searches domains.
        /// </summary>
        /// <param name="tenantId">Identifies website whose domains are searched.</param>
        /// <param name="parameters">Search and paging parameters.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Search result.</returns>
        public ISearchResult<Domain> Search(long tenantId, ISearchParameters parameters, IUnitOfWork unitOfWork = null)
        {
            if (parameters.Search == null)
                parameters.Search = string.Empty;
            else
                parameters.Search = parameters.Search.Trim();
            if (parameters.PageSize == 0)
                parameters.PageSize = 10;
            return _domainRepository.Search(tenantId, parameters, unitOfWork);
        }

        /// <summary>
        /// Creates a new domain.
        /// </summary>
        /// <param name="domain">New domain details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Newly allocated domain identifier.</returns>
        public long Create(Domain domain, IUnitOfWork unitOfWork = null)
        {
            try
            {
                // Perform validation
                _domainValidator.ValidateCreate(domain);

                // Create domain
                return _domainRepository.Create(domain, unitOfWork);
            }
            catch (ValidationErrorException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ValidationErrorException(new ValidationError(null, ApplicationResource.UnexpectedErrorMessage), ex);
            }
        }

        /// <summary>
        /// Gets domain details.
        /// </summary>
        /// <param name="tenantId">Identifies website whose domain is returned.</param>
        /// <param name="domainId">Identifies the domain to return.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Domain details (or null if domain not found).</returns>
        public Domain Read(long tenantId, long domainId, IUnitOfWork unitOfWork = null)
        {
            return _domainRepository.Read(tenantId, domainId, unitOfWork);
        }

        /// <summary>
        /// Gets domain details given a URL.
        /// </summary>
        /// <param name="url">URL identifying domain.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Domain details (or null if domain not found).</returns>
        public Domain ReadByUrl(string url, IUnitOfWork unitOfWork = null)
        {
            return _domainRepository.ReadByUrl(url, unitOfWork);
        }

        /// <summary>
        /// Updates a domain's details.
        /// </summary>
        /// <param name="domain">Updated domain details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Update(Domain domain, IUnitOfWork unitOfWork = null)
        {
            try
            {
                // Perform validation
                _domainValidator.ValidateUpdate(domain);

                // Update website details
                _domainRepository.Update(domain, unitOfWork);
            }
            catch (ValidationErrorException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ValidationErrorException(new ValidationError(null, ApplicationResource.UnexpectedErrorMessage), ex);
            }
        }

        /// <summary>
        /// Deletes a domain.
        /// </summary>
        /// <param name="tenantId">Identifies website whose domain is to deleted.</param>
        /// <param name="domainId">Identifies the domain to delete.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Delete(long tenantId, long domainId, IUnitOfWork unitOfWork = null)
        {
            try
            {
                _domainRepository.Delete(tenantId, domainId, unitOfWork);
            }
            catch (ValidationErrorException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ValidationErrorException(new ValidationError(null, ApplicationResource.UnexpectedErrorMessage), ex);
            }
        }
    }
}

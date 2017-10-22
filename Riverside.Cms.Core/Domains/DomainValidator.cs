using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Domains;
using Riverside.Cms.Core.Resources;
using Riverside.Utilities.Validation;

namespace Riverside.Cms.Core.Domains
{
    public class DomainValidator : IDomainValidator
    {
        // Member variables
        private IModelValidator _modelValidator;
        private IDomainRepository _domainRepository;

        public DomainValidator(IModelValidator modelValidator, IDomainRepository domainRepository)
        {
            _modelValidator = modelValidator;
            _domainRepository = domainRepository;
        }

        public void ValidateCreate(Domain domain, string keyPrefix = null)
        {
            // Do stock model validation
            _modelValidator.Validate(domain, keyPrefix);

            // Check domain URL specified is available
            Domain domainByUrl = _domainRepository.ReadByUrl(domain.Url);
            if (domainByUrl != null)
                throw new ValidationErrorException(new ValidationError(DomainPropertyNames.Url, DomainResource.UrlNotAvailableMessage, keyPrefix));
        }

        public void ValidateUpdate(Domain domain, string keyPrefix = null)
        {
            // Do stock model validation
            _modelValidator.Validate(domain, keyPrefix);

            // Check domain URL specified is available
            Domain domainByUrl = _domainRepository.ReadByUrl(domain.Url);
            if (!(domainByUrl == null || domainByUrl.Url == domain.Url))
                throw new ValidationErrorException(new ValidationError(DomainPropertyNames.Url, DomainResource.UrlNotAvailableMessage, keyPrefix));
        }
    }
}
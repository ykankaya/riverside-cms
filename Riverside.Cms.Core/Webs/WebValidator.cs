using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Domains;
using Riverside.Cms.Core.Webs;
using Riverside.Cms.Core.Resources;
using Riverside.Utilities.Validation;

namespace Riverside.Cms.Core.Webs
{
    /// <summary>
    /// Implements validation for web actions.
    /// </summary>
    public class WebValidator : IWebValidator
    {
        // Member variables
        private IDomainValidator _domainValidator;
        private IModelValidator  _modelValidator;
        private IWebRepository   _webRepository;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="domainValidator">Validates domain URL entered during website creation.</param>
        /// <param name="modelValidator">Model validator.</param>
        /// <param name="webRepository">Web repository.</param>
        public WebValidator(IDomainValidator domainValidator, IModelValidator modelValidator, IWebRepository webRepository)
        {
            _domainValidator = domainValidator;
            _modelValidator  = modelValidator;
            _webRepository   = webRepository;
        }

        /// <summary>
        /// Validates web details before creation. Throws validation error exception if validation fails.
        /// </summary>
        /// <param name="web">The model to validate.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        public void ValidateCreate(Web web, string keyPrefix = null)
        {
            // Do stock model validation
            _modelValidator.Validate(web, keyPrefix);

            // A single domain URL must be specified when creating a website
            if (web.Domains == null || web.Domains.Count != 1)
                throw new ValidationErrorException(new ValidationError(WebPropertyNames.FirstDomain + "." + DomainPropertyNames.Url, WebResource.UrlRequiredMessage, keyPrefix));

            // Check web name entered is available
            Web webByName = _webRepository.ReadByName(web.Name);
            if (webByName != null)
                throw new ValidationErrorException(new ValidationError(WebPropertyNames.Name, WebResource.NameNotAvailableMessage, keyPrefix));

            // Validate domain
            _domainValidator.ValidateCreate(web.Domains[0], WebPropertyNames.FirstDomain);
        }

        /// <summary>
        /// Validates web details before update. Throws validation error exception if validation fails.
        /// </summary>
        /// <param name="web">The model to validate.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        public void ValidateUpdate(Web web, string keyPrefix = null)
        {
            // Do stock model validation
            _modelValidator.Validate(web);

            // Check web name entered is available
            Web webByName = _webRepository.ReadByName(web.Name);
            if (!(webByName == null || webByName.TenantId == web.TenantId))
                throw new ValidationErrorException(new ValidationError(WebPropertyNames.Name, WebResource.NameNotAvailableMessage, keyPrefix));
        }
    }
}

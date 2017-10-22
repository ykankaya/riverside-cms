using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Webs;
using Riverside.Cms.Core.Resources;
using Riverside.Utilities.Validation;

namespace Riverside.Cms.Core.Domains
{
    public class Domain
    {
        public long TenantId { get; set; }
        public long DomainId { get; set; }

        [Display(ResourceType = typeof(DomainResource), Name = "UrlLabel")]
        [Required(ErrorMessageResourceType = typeof(DomainResource), ErrorMessageResourceName = "UrlRequiredMessage")]
        [StringLength(DomainLengths.UrlMaxLength, ErrorMessageResourceType = typeof(DomainResource), ErrorMessageResourceName = "UrlMaxLengthMessage")]
        [DataType(DataType.Url, ErrorMessageResourceType = typeof(DomainResource), ErrorMessageResourceName = "UrlInvalidMessage")]
        [RegularExpression(RegularExpression.Url, ErrorMessageResourceType = typeof(DomainResource), ErrorMessageResourceName = "UrlInvalidMessage")]
        public string Url { get; set; }

        [Display(ResourceType = typeof(DomainResource), Name = "RedirectUrlLabel")]
        [StringLength(DomainLengths.UrlMaxLength, ErrorMessageResourceType = typeof(DomainResource), ErrorMessageResourceName = "RedirectUrlMaxLengthMessage")]
        [DataType(DataType.Url, ErrorMessageResourceType = typeof(DomainResource), ErrorMessageResourceName = "RedirectUrlInvalidMessage")]
        [RegularExpression(RegularExpression.Url, ErrorMessageResourceType = typeof(DomainResource), ErrorMessageResourceName = "RedirectUrlInvalidMessage")]
        public string RedirectUrl { get; set; }

        public Web Web { get; set; }
    }
}

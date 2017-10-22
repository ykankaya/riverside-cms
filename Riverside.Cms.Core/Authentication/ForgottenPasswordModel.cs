using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Resources;
using Riverside.Utilities.Validation;

namespace Riverside.Cms.Core.Authentication
{
    public class ForgottenPasswordModel
    {
        public long TenantId { get; set; }

        [Display(ResourceType = typeof(AuthenticationResource), Name = "ForgottenPasswordEmailLabel")]
        [Required(ErrorMessageResourceType = typeof(AuthenticationResource), ErrorMessageResourceName = "ForgottenPasswordEmailRequiredMessage")]
        [StringLength(AuthenticationLengths.EmailMaxLength, ErrorMessageResourceType = typeof(AuthenticationResource), ErrorMessageResourceName = "ForgottenPasswordEmailMaxLengthMessage")]
        [DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof(AuthenticationResource), ErrorMessageResourceName = "ForgottenPasswordEmailInvalidMessage")]
        [RegularExpression(RegularExpression.Email, ErrorMessageResourceType = typeof(AuthenticationResource), ErrorMessageResourceName = "ForgottenPasswordEmailInvalidMessage")]
        public string Email { get; set; }
    }
}

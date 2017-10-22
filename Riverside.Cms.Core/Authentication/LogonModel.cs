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
    public class LogonModel
    {
        public long TenantId { get; set; }

        [Display(ResourceType = typeof(AuthenticationResource), Name = "RememberMeLabel")]
        public bool RememberMe { get; set; }

        [Display(ResourceType = typeof(AuthenticationResource), Name = "EmailLabel")]
        [Required(ErrorMessageResourceType = typeof(AuthenticationResource), ErrorMessageResourceName = "EmailRequiredMessage")]
        [StringLength(AuthenticationLengths.EmailMaxLength, ErrorMessageResourceType = typeof(AuthenticationResource), ErrorMessageResourceName = "EmailMaxLengthMessage")]
        [DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof(AuthenticationResource), ErrorMessageResourceName = "EmailInvalidMessage")]
        [RegularExpression(RegularExpression.Email, ErrorMessageResourceType = typeof(AuthenticationResource), ErrorMessageResourceName = "EmailInvalidMessage")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(AuthenticationResource), Name = "PasswordLabel")]
        [Required(ErrorMessageResourceType = typeof(AuthenticationResource), ErrorMessageResourceName = "PasswordRequiredMessage")]
        public string Password { get; set; }
    }
}

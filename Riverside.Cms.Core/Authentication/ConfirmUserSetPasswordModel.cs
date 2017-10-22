using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Resources;

namespace Riverside.Cms.Core.Authentication
{
    public class ConfirmUserSetPasswordModel
    {
        public long TenantId { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(AuthenticationResource), Name = "ConfirmUserPasswordLabel")]
        [Required(ErrorMessageResourceType = typeof(AuthenticationResource), ErrorMessageResourceName = "ConfirmUserPasswordRequiredMessage")]
        [StringLength(AuthenticationLengths.PasswordMaxLength, ErrorMessageResourceType = typeof(AuthenticationResource), ErrorMessageResourceName = "ConfirmUserPasswordLengthMessage", MinimumLength=AuthenticationLengths.PasswordMinLength)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(AuthenticationResource), Name = "ConfirmUserConfirmPasswordLabel")]
        [Required(ErrorMessageResourceType = typeof(AuthenticationResource), ErrorMessageResourceName = "ConfirmUserConfirmPasswordRequiredMessage")]
        [Compare("Password", ErrorMessageResourceType = typeof(AuthenticationResource), ErrorMessageResourceName = "ConfirmUserPasswordsDoNotMatchMessage")]
        public string ConfirmPassword { get; set; }

        public string ConfirmKey { get; set; }
    }
}

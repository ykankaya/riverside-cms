using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Resources;

namespace Riverside.Cms.Core.Authentication
{
    public class ResetPasswordModel
    {
        public long TenantId { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(AuthenticationResource), Name = "ResetPasswordPasswordLabel")]
        [Required(ErrorMessageResourceType = typeof(AuthenticationResource), ErrorMessageResourceName = "ResetPasswordPasswordRequiredMessage")]
        [StringLength(AuthenticationLengths.PasswordMaxLength, ErrorMessageResourceType = typeof(AuthenticationResource), ErrorMessageResourceName = "ResetPasswordPasswordLengthMessage", MinimumLength=AuthenticationLengths.PasswordMinLength)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(AuthenticationResource), Name = "ResetPasswordConfirmPasswordLabel")]
        [Required(ErrorMessageResourceType = typeof(AuthenticationResource), ErrorMessageResourceName = "ResetPasswordConfirmPasswordRequiredMessage")]
        [Compare("Password", ErrorMessageResourceType = typeof(AuthenticationResource), ErrorMessageResourceName = "ResetPasswordPasswordsDoNotMatchMessage")]
        public string ConfirmPassword { get; set; }

        public string ResetPasswordKey { get; set; }
    }
}

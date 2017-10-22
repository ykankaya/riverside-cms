using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Resources;

namespace Riverside.Cms.Core.Authentication
{
    public class ChangePasswordModel
    {
        public long TenantId { get; set; }
        public long UserId { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(AuthenticationResource), Name = "ChangePasswordCurrentPasswordLabel")]
        [Required(ErrorMessageResourceType = typeof(AuthenticationResource), ErrorMessageResourceName = "ChangePasswordCurrentPasswordRequiredMessage")]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(AuthenticationResource), Name = "ChangePasswordNewPasswordLabel")]
        [Required(ErrorMessageResourceType = typeof(AuthenticationResource), ErrorMessageResourceName = "ChangePasswordNewPasswordRequiredMessage")]
        [StringLength(AuthenticationLengths.PasswordMaxLength, ErrorMessageResourceType = typeof(AuthenticationResource), ErrorMessageResourceName = "ChangePasswordNewPasswordLengthMessage", MinimumLength=AuthenticationLengths.PasswordMinLength)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(AuthenticationResource), Name = "ChangePasswordConfirmPasswordLabel")]
        [Required(ErrorMessageResourceType = typeof(AuthenticationResource), ErrorMessageResourceName = "ChangePasswordConfirmPasswordRequiredMessage")]
        [Compare("NewPassword", ErrorMessageResourceType = typeof(AuthenticationResource), ErrorMessageResourceName = "ChangePasswordPasswordsDoNotMatchMessage")]
        public string ConfirmPassword { get; set; }
    }
}

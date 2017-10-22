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
    public class UpdateUserModel
    {
        public long TenantId { get; set; }
        public long UserId { get; set; }

        [Display(ResourceType = typeof(AuthenticationResource), Name = "UpdateUserAliasLabel")]
        [Required(ErrorMessageResourceType = typeof(AuthenticationResource), ErrorMessageResourceName = "UpdateUserAliasRequiredMessage")]
        [StringLength(AuthenticationLengths.AliasMaxLength, ErrorMessageResourceType = typeof(AuthenticationResource), ErrorMessageResourceName = "UpdateUserAliasMaxLengthMessage")]
        public string Alias { get; set; }

        [Display(ResourceType = typeof(AuthenticationResource), Name = "UpdateUserEmailLabel")]
        [Required(ErrorMessageResourceType = typeof(AuthenticationResource), ErrorMessageResourceName = "UpdateUserEmailRequiredMessage")]
        [StringLength(AuthenticationLengths.EmailMaxLength, ErrorMessageResourceType = typeof(AuthenticationResource), ErrorMessageResourceName = "UpdateUserEmailMaxLengthMessage")]
        [DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof(AuthenticationResource), ErrorMessageResourceName = "UpdateUserEmailInvalidMessage")]
        [RegularExpression(RegularExpression.Email, ErrorMessageResourceType = typeof(AuthenticationResource), ErrorMessageResourceName = "UpdateUserEmailInvalidMessage")]
        public string Email { get; set; }

        public long? ImageTenantId { get; set; }
        public long? ThumbnailImageUploadId { get; set; }
        public long? PreviewImageUploadId { get; set; }
        public long? ImageUploadId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Uploads;

namespace Riverside.Cms.Core.Authentication
{
    public interface IAuthenticationValidator
    {
        void ValidateCreateUser(CreateUserModel model, string keyPrefix = null);
        void ValidateConfirmUserStatus(ConfirmUserStatusModel model, string keyPrefix = null);
        void ValidateConfirmUserSetPassword(ConfirmUserSetPasswordModel model, string keyPrefix = null);
        void ValidateConfirmUser(ConfirmUserModel model, string keyPrefix = null);
        void ValidateLogon(LogonModel model, string keyPrefix = null);
        void ValidateChangePassword(ChangePasswordModel model, string keyPrefix = null);
        AuthenticationValidateResult ValidatePrepareImages(long tenantId, CreateUploadModel model, string keyPrefix = null);
        void ValidateUpdateUser(UpdateUserModel model, string keyPrefix = null);
        void ValidateForgottenPassword(ForgottenPasswordModel model, string keyPrefix = null);
        void ValidateResetPasswordStatus(ResetPasswordStatusModel model, string keyPrefix = null);
        void ValidateResetPassword(ResetPasswordModel model, string keyPrefix = null);
    }
}

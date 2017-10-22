using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Uploads;
using Riverside.Cms.Core.Webs;
using Riverside.Cms.Core.Resources;
using Riverside.Utilities.Drawing;
using Riverside.Utilities.Security;
using Riverside.Utilities.Text;
using Riverside.Utilities.Validation;

namespace Riverside.Cms.Core.Authentication
{
    /// <summary>
    /// Validates authentication related actions.
    /// </summary>
    public class AuthenticationValidator : IAuthenticationValidator
    {
        // Member variables
        private IAuthenticationConfigurationService _authenticationConfigurationService;
        private IImageAnalysisService _imageAnalysisService;
        private IModelValidator _modelValidator;
        private ISecurityService _securityService;
        private IStringService _stringService;
        private IUploadService _uploadService;
        private IUserRepository _userRepository;
        private IWebRepository _webRepository;

        public AuthenticationValidator(IAuthenticationConfigurationService authenticationConfigurationService, IImageAnalysisService imageAnalysisService, IModelValidator modelValidator, ISecurityService securityService, IStringService stringService, IUploadService uploadService, IUserRepository userRepository, IWebRepository webRepository)
        {
            _authenticationConfigurationService = authenticationConfigurationService;
            _imageAnalysisService = imageAnalysisService;
            _modelValidator = modelValidator;
            _securityService = securityService;
            _stringService = stringService;
            _uploadService = uploadService;
            _userRepository = userRepository;
            _webRepository = webRepository;
        }

        /// <summary>
        /// Validates information supplied to create a new user.
        /// </summary>
        /// <param name="model">Contains create user details.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        public void ValidateCreateUser(CreateUserModel model, string keyPrefix = null)
        {
            // Do stock model validation
            _modelValidator.Validate(model, keyPrefix);

            // Check that alias is available
            User userByAlias = _userRepository.ReadUserByAlias(model.TenantId, model.Alias.Trim());
            if (userByAlias != null)
                throw new ValidationErrorException(new ValidationError(AuthenticationPropertyNames.Alias, AuthenticationResource.AliasNotAvailableMessage, keyPrefix));

            // Check that email is available
            User userByEmail = _userRepository.ReadUserByEmail(model.TenantId, model.Email.Trim().ToLower());
            if (userByEmail != null)
                throw new ValidationErrorException(new ValidationError(AuthenticationPropertyNames.Email, AuthenticationResource.EmailNotAvailableMessage, keyPrefix));
        }

        /// <summary>
        /// Checks whether confirm key is valid string representation of a token.
        /// </summary>
        /// <param name="confirmKey">Token as string / key.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        /// <returns>Token.</returns>
        private Token ValidateConfirmUserToken(string confirmKey, string keyPrefix)
        {
            Token token;
            if (_securityService.ParseToken(confirmKey, out token))
                return token;
            else
                throw new ValidationErrorException(new ValidationError(null, AuthenticationResource.ConfirmUserTokenInvalidMessage, keyPrefix));
        }

        /// <summary>
        /// Checks whether confirmation token is valid (i.e. that it hasn't expired).
        /// </summary>
        /// <param name="token">Confirmation token.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        private void ValidateConfirmUserTokenExpiry(Token token, string keyPrefix)
        {
            if (_securityService.TokenExpired(token))
                throw new ValidationErrorException(new ValidationError(null, AuthenticationResource.ConfirmUserTokenExpiredMessage, keyPrefix));
        }

        /// <summary>
        /// Checks whether confirmation token identifies user and that the user is in the correct state.
        /// </summary>
        /// <param name="tenantId">Website identifier.</param>
        /// <param name="token">Confirmation token.</param>
        /// <param name="setPassword">True if confirm action to incorporate setting of password.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        private void ValidateConfirmUserUser(long tenantId, Token token, bool setPassword, string keyPrefix)
        {
            User user = _userRepository.ReadUserByConfirmToken(tenantId, token);
            if (user == null)
                throw new ValidationErrorException(new ValidationError(null, AuthenticationResource.ConfirmUserNotFoundMessage, keyPrefix));       // User must exist
            if (!user.Enabled)
                throw new ValidationErrorException(new ValidationError(null, AuthenticationResource.ConfirmUserDisabledMessage, keyPrefix));       // User must not be disabled
            if (user.Confirmed)
                throw new ValidationErrorException(new ValidationError(null, AuthenticationResource.ConfirmUserConfirmedMessage, keyPrefix));      // User must not be confirmed
            if (setPassword && user.PasswordChanged != null)
                throw new ValidationErrorException(new ValidationError(null, AuthenticationResource.ConfirmUserPasswordSetMessage, keyPrefix));    // User password must not already be set
            if (!setPassword && user.PasswordChanged == null)
                throw new ValidationErrorException(new ValidationError(null, AuthenticationResource.ConfirmUserPasswordNotSetMessage, keyPrefix)); // User password must already be set
        }

        /// <summary>
        /// Checks confirm token is not expired and identifies a user who can be confirmed.
        /// </summary>
        /// <param name="model">Contains confirmation token details.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        public void ValidateConfirmUserStatus(ConfirmUserStatusModel model, string keyPrefix = null)
        {
            Token token = ValidateConfirmUserToken(model.ConfirmKey, keyPrefix);
            ValidateConfirmUserTokenExpiry(token, keyPrefix);
            ValidateConfirmUserUser(model.TenantId, token, model.SetPassword, keyPrefix);
        }

        /// <summary>
        /// Performs main validation of supplied user confirmation details.
        /// </summary>
        /// <param name="model">Confirm user details.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        public void ValidateConfirmUserSetPassword(ConfirmUserSetPasswordModel model, string keyPrefix = null)
        {
            // Do stock validation
            _modelValidator.Validate(model, keyPrefix);

            // Check that new and confirm passwords are identical (required while model validator does not support this data annotation)
            if (model.Password != model.ConfirmPassword)
                throw new ValidationErrorException(new ValidationError(null, AuthenticationResource.ConfirmUserPasswordsDoNotMatchMessage, keyPrefix));

            // Check user status for confirmation action
            ValidateConfirmUserStatus(new ConfirmUserStatusModel { TenantId = model.TenantId, SetPassword = true, ConfirmKey = model.ConfirmKey }, keyPrefix);
        }

        /// <summary>
        /// Performs main validation of supplied user confirmation details.
        /// </summary>
        /// <param name="model">Confirm user details.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        public void ValidateConfirmUser(ConfirmUserModel model, string keyPrefix = null)
        {
            ValidateConfirmUserStatus(new ConfirmUserStatusModel { TenantId = model.TenantId, SetPassword = false, ConfirmKey = model.ConfirmKey }, keyPrefix);
        }

        /// <summary>
        /// Registers a password failure against specified user account. If password failure count exceeds "password failures before lock out", then user 
        /// has entered their password incorrectly too many times and will be locked out.
        /// </summary>
        /// <param name="user">User details.</param>
        private void RegisterPasswordFailure(User user)
        {
            // Get password failure before lockout count
            int passwordFailuresBeforeLockOut = _authenticationConfigurationService.GetPasswordFailuresBeforeLockOut(user.TenantId);

            // Increment password failures
            user.LastPasswordFailure = DateTime.UtcNow;
            user.PasswordFailures = user.PasswordFailures + 1;
            user.LockedOut = user.PasswordFailures > passwordFailuresBeforeLockOut;

            // Update user
            _userRepository.UpdateUser(user);
        }

        /// <summary>
        /// Performs main validation of supplied user credentials. If user credentials not valid, a validation error exception will be thrown by this method.
        /// </summary>
        /// <param name="model">User credentials to validate.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        public void ValidateLogon(LogonModel model, string keyPrefix = null)
        {
            // Do initial model checks (valid email, password required etc)
            _modelValidator.Validate(model, keyPrefix);

            // Get user given email address
            User user = _userRepository.ReadUserByEmail(model.TenantId, model.Email.Trim().ToLower());

            // The first condition that causes an invalid user is when user not found due to an invalid email address entered. In this case, user is null.
            if (user == null)
                throw new ValidationErrorException(new ValidationError(null, AuthenticationResource.LogonUserCredentialsInvalidMessage, keyPrefix));

            // If user has not been confirmed, they must first set their password before they can be validated.
            if (!user.Confirmed)
                throw new ValidationErrorException(new ValidationError(null, AuthenticationResource.LogonUserUnconfirmedMessage, keyPrefix));

            // User may have been disabled by an administrator, in which case it will not be possible to validate account.
            if (!user.Enabled)
                throw new ValidationErrorException(new ValidationError(null, AuthenticationResource.LogonUserDisabledMessage, keyPrefix));

            // If account locked out, check to see when last password failure occured. If lockout duration period expired, we can undo the lockout.
            if (user.LockedOut)
            {
                TimeSpan lockOutDuration = _authenticationConfigurationService.GetLockOutDuration(model.TenantId);
                if ((DateTime.UtcNow - (DateTime)user.LastPasswordFailure) > lockOutDuration)
                {
                    // Clear password failures associated with a user and sets user's locked out flag to false
                    user.LockedOut = false;
                    user.LastPasswordFailure = null;
                    user.PasswordFailures = 0;

                    // Do the update
                    _userRepository.UpdateUser(user);
                }
                else
                {
                    throw new UserLockedOutException(new ValidationError(null, AuthenticationResource.LogonUserLockedOutMessage, keyPrefix));
                }
            }

            // Finally, check password entered is correct and if not register a password failure which may lock user out
            byte[] userPasswordSalt = _stringService.GetBytes(user.PasswordSalt);
            byte[] userPasswordSaltedHash = _stringService.GetBytes(user.PasswordSaltedHash);
            byte[] logonPasswordSaltedHash = _securityService.EncryptPassword(model.Password, userPasswordSalt);
            if (!_stringService.ByteArraysEqual(logonPasswordSaltedHash, userPasswordSaltedHash))
            {
                RegisterPasswordFailure(user);
                if (user.LockedOut)
                    throw new ValidationErrorException(new ValidationError(null, AuthenticationResource.LogonUserLockedOutMessage, keyPrefix));
                else
                    throw new ValidationErrorException(new ValidationError(null, AuthenticationResource.LogonUserCredentialsInvalidMessage, keyPrefix));
            }
        }

        /// <summary>
        /// Performs change password validation.
        /// </summary>
        /// <param name="model">Change password details.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        public void ValidateChangePassword(ChangePasswordModel model, string keyPrefix = null)
        {
            // Do initial model checks (valid current and new passwords etc)
            _modelValidator.Validate(model);

            // Check that new and confirm passwords are identical (required while model validator does not support this data annotation)
            if (model.NewPassword != model.ConfirmPassword)
                throw new ValidationErrorException(new ValidationError(null, AuthenticationResource.ChangePasswordPasswordsDoNotMatchMessage, keyPrefix));

            // Get user given user id
            User user = _userRepository.ReadUser(model.TenantId, model.UserId);

            // User not found
            if (user == null)
                throw new ValidationErrorException(new ValidationError(null, AuthenticationResource.ChangePasswordUserNotFoundMessage, keyPrefix));

            // Only confirmed users can change their password
            if (!user.Confirmed)
                throw new ValidationErrorException(new ValidationError(null, AuthenticationResource.ChangePasswordUserUnconfirmedMessage, keyPrefix));

            // User may have been disabled by an administrator, in which case it will not be possible to validate account.
            if (!user.Enabled)
                throw new ValidationErrorException(new ValidationError(null, AuthenticationResource.ChangePasswordUserDisabledMessage, keyPrefix));

            // If account locked out, check to see when last password failure occured. If lockout duration period expired, we can undo the lockout.
            if (user.LockedOut)
                throw new UserLockedOutException(new ValidationError(null, AuthenticationResource.ChangePasswordUserLockedOutMessage, keyPrefix));

            // Finally, check password entered is correct and if not register a password failure which may lock user out
            byte[] userPasswordSalt = _stringService.GetBytes(user.PasswordSalt);
            byte[] userPasswordSaltedHash = _stringService.GetBytes(user.PasswordSaltedHash);
            byte[] passwordSaltedHash = _securityService.EncryptPassword(model.CurrentPassword, userPasswordSalt);
            if (!_stringService.ByteArraysEqual(passwordSaltedHash, userPasswordSaltedHash))
            {
                RegisterPasswordFailure(user);
                if (user.LockedOut)
                    throw new UserLockedOutException(new ValidationError(null, AuthenticationResource.ChangePasswordUserLockedOutMessage, keyPrefix));
                else
                    throw new ValidationErrorException(new ValidationError(AuthenticationPropertyNames.CurrentPassword, AuthenticationResource.ChangePasswordCurrentPasswordInvalidMessage, keyPrefix));
            }
        }

        /// <summary>
        /// Validates avatar upload details before images can be prepared for a user.
        /// </summary>
        /// <param name="tenantId">Website that user belongs to.</param>
        /// <param name="model">Uploaded image.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        /// <returns>Useful information retrieved during validation.</returns>
        public AuthenticationValidateResult ValidatePrepareImages(long tenantId, CreateUploadModel model, string keyPrefix = null)
        {
            // Check that website allows avatars
            Web web = _webRepository.Read(tenantId);
            if (!web.UserHasImage)
                throw new ValidationErrorException(new ValidationError(AuthenticationPropertyNames.Image, AuthenticationResource.UpdateUserImageNotAllowedMessage, keyPrefix));

            // Check that content type identifies an image
            UploadType uploadType = _uploadService.GetUploadType(model.ContentType);
            if (uploadType != UploadType.Image)
                throw new ValidationErrorException(new ValidationError(AuthenticationPropertyNames.Image, AuthenticationResource.UpdateUserImageInvalidMessage, keyPrefix));

            // Check that supplied upload is an image
            Size? size = _imageAnalysisService.GetImageDimensions(model.Content);
            if (size == null)
                throw new ValidationErrorException(new ValidationError(AuthenticationPropertyNames.Image, AuthenticationResource.UpdateUserImageInvalidMessage, keyPrefix));

            // Check image dimension constraints (minimum width and height)
            if (size.Value.Width < web.UserImageMinWidth.Value || size.Value.Height < web.UserImageMinHeight.Value)
                throw new ValidationErrorException(new ValidationError(AuthenticationPropertyNames.Image, string.Format(AuthenticationResource.UpdateUserImageDimensionsInvalidMessage, web.UserImageMinWidth.Value, web.UserImageMinHeight.Value), keyPrefix));

            // Return result
            return new AuthenticationValidateResult { Web = web, Size = size.Value };
        }

        /// <summary>
        /// Validates update user details.
        /// </summary>
        /// <param name="model">Updated user details.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        public void ValidateUpdateUser(UpdateUserModel model, string keyPrefix = null)
        {
            // Do stock validation
            _modelValidator.Validate(model, keyPrefix);

            // Image values must all be null or all specified
            bool allHaveValue = model.ImageTenantId.HasValue && model.ThumbnailImageUploadId.HasValue && model.PreviewImageUploadId.HasValue && model.ImageUploadId.HasValue;
            bool noneHaveValue = !model.ImageTenantId.HasValue && !model.ThumbnailImageUploadId.HasValue && !model.PreviewImageUploadId.HasValue && !model.ImageUploadId.HasValue;
            if (!(allHaveValue || noneHaveValue))
                throw new ValidationErrorException(new ValidationError(AuthenticationPropertyNames.Image, AuthenticationResource.UpdateUserImageInvalidMessage, keyPrefix));

            // Check alias entered is available
            User userByAlias = _userRepository.ReadUserByAlias(model.TenantId, model.Alias.Trim());
            if (!(userByAlias == null || userByAlias.UserId == model.UserId))
                throw new ValidationErrorException(new ValidationError(AuthenticationPropertyNames.Alias, AuthenticationResource.UpdateUserAliasNotAvailableMessage, keyPrefix));

            // Check email entered is available
            User userByEmail = _userRepository.ReadUserByEmail(model.TenantId, model.Email.Trim().ToLower());
            if (!(userByEmail == null || userByEmail.UserId == model.UserId))
                throw new ValidationErrorException(new ValidationError(AuthenticationPropertyNames.Email, AuthenticationResource.UpdateUserEmailNotAvailableMessage, keyPrefix));
        }

        /// <summary>
        /// Validates forgotten password action.
        /// </summary>
        /// <param name="model">Identifies user performing the forgotten password action.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        public void ValidateForgottenPassword(ForgottenPasswordModel model, string keyPrefix = null)
        {
            // Check that email address correctly supplied
            _modelValidator.Validate(model, keyPrefix);

            // Validate user
            User user = _userRepository.ReadUserByEmail(model.TenantId, model.Email.Trim().ToLower());
            if (user == null)
                throw new ValidationErrorException(new ValidationError(null, AuthenticationResource.ForgottenPasswordUserNotFoundMessage, keyPrefix));
            if (!user.Confirmed)
                throw new ValidationErrorException(new ValidationError(null, AuthenticationResource.ForgottenPasswordUserNotConfirmedMessage, keyPrefix));
            if (!user.Enabled)
                throw new ValidationErrorException(new ValidationError(null, AuthenticationResource.ForgottenPasswordUserDisabledMessage, keyPrefix));
        }

        /// <summary>
        /// Checks whether reset password key is valid string representation of a token.
        /// </summary>
        /// <param name="resetPasswordKey">Token as string / key.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        /// <returns>Token.</returns>
        private Token ValidateResetPasswordToken(string resetPasswordKey, string keyPrefix)
        {
            Token token;
            if (_securityService.ParseToken(resetPasswordKey, out token))
                return token;
            else
                throw new ValidationErrorException(new ValidationError(null, AuthenticationResource.ResetPasswordTokenInvalidMessage, keyPrefix));
        }

        /// <summary>
        /// Checks whether confirmation token is valid (i.e. that it hasn't expired).
        /// </summary>
        /// <param name="token">Confirmation token.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        private void ValidateResetPasswordTokenExpiry(Token token, string keyPrefix)
        {
            if (_securityService.TokenExpired(token))
                throw new ValidationErrorException(new ValidationError(null, AuthenticationResource.ResetPasswordTokenExpiredMessage, keyPrefix));
        }

        /// <summary>
        /// Checks whether reset password token identifies user and that the user is enabled.
        /// </summary>
        /// <param name="tenantId">Website identifier.</param>
        /// <param name="token">Verification token and website.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        private void ValidateResetPasswordUser(long tenantId, Token token, string keyPrefix)
        {
            User user = _userRepository.ReadUserByResetPasswordToken(tenantId, token);
            if (user == null)
                throw new ValidationErrorException(new ValidationError(null, AuthenticationResource.ResetPasswordUserNotFoundMessage, keyPrefix));
            if (!user.Enabled)
                throw new ValidationErrorException(new ValidationError(null, AuthenticationResource.ResetPasswordUserDisabledMessage, keyPrefix));
        }

        /// <summary>
        /// Checks token is not expired and identifies a user who can perform a reset password.
        /// </summary>
        /// <param name="model">Reset password verification token.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        public void ValidateResetPasswordStatus(ResetPasswordStatusModel model, string keyPrefix = null)
        {
            Token token = ValidateResetPasswordToken(model.ResetPasswordKey, keyPrefix);
            ValidateResetPasswordTokenExpiry(token, keyPrefix);
            ValidateResetPasswordUser(model.TenantId, token, keyPrefix);
        }

        /// <summary>
        /// Performs validation of reset password details.
        /// </summary>
        /// <param name="model">Confirm user details.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        public void ValidateResetPassword(ResetPasswordModel model, string keyPrefix = null)
        {
            // Do stock model validation
            _modelValidator.Validate(model, keyPrefix);

            // Check that new and confirm passwords are identical (required while model validator does not support this data annotation)
            if (model.Password != model.ConfirmPassword)
                throw new ValidationErrorException(new ValidationError(null, AuthenticationResource.ResetPasswordPasswordsDoNotMatchMessage, keyPrefix));

            // Do reset password specific validation
            ValidateResetPasswordStatus(new ResetPasswordStatusModel { TenantId = model.TenantId, ResetPasswordKey = model.ResetPasswordKey }, keyPrefix);
        }
    }
}

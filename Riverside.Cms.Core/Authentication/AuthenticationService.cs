using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Authorization;
using Riverside.Cms.Core.Domains;
using Riverside.Cms.Core.Uploads;
using Riverside.Cms.Core.Webs;
using Riverside.Cms.Core.Resources;
using Riverside.Utilities.Authorization;
using Riverside.Utilities.Data;
using Riverside.Utilities.Drawing;
using Riverside.Utilities.Http;
using Riverside.Utilities.Mail;
using Riverside.Utilities.Security;
using Riverside.Utilities.Text;
using Riverside.Utilities.Validation;
using Riverside.UI.Web;

namespace Riverside.Cms.Core.Authentication
{
    /// <summary>
    /// Authentication service implements authentication features such as login, logoff, change password, forgotten password etc.
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private IAuthenticationConfigurationService _authenticationConfigurationService;
        private IAuthenticationProviderService _authenticationProviderService;
        private IAuthenticationValidator _authenticationValidator;
        private IEmailService _emailService;
        private IImageAnalysisService _imageAnalysisService;
        private ISecurityService _securityService;
        private IStringService _stringService;
        private IUserRepository _userRepository;
        private IUnitOfWorkFactory _unitOfWorkFactory;
        private IUploadService _uploadService;
        private IWebHelperService _webHelperService;

        public AuthenticationService(IAuthenticationConfigurationService authenticationConfigurationService, IAuthenticationProviderService authenticationProviderService, IAuthenticationValidator authenticationValidator, IEmailService emailService, IImageAnalysisService imageAnalysisService, ISecurityService securityService, IStringService stringService, IUnitOfWorkFactory unitOfWorkFactory, IUserRepository userRepository, IUploadService uploadService, IWebHelperService webHelperService)
        {
            _authenticationConfigurationService = authenticationConfigurationService;
            _authenticationProviderService = authenticationProviderService;
            _authenticationValidator = authenticationValidator;
            _emailService = emailService;
            _imageAnalysisService = imageAnalysisService;
            _securityService = securityService;
            _stringService = stringService;
            _unitOfWorkFactory = unitOfWorkFactory;
            _uploadService = uploadService;
            _userRepository = userRepository;
            _webHelperService = webHelperService;
        }

        public long TenantId
        {
            get
            {
                return _webHelperService.GetItem<Web>("RiversideCmsWeb").TenantId;
            }
        }

        public Web Web
        {
            get
            {
                return _webHelperService.GetItem<Web>("RiversideCmsWeb");
            }
        }

        public Domain Domain
        {
            get
            {
                return _webHelperService.GetItem<Domain>("RiversideCmsDomain");
            }
        }

        public long CreateUser(CreateUserModel model)
        {
            // Validate supplied user credentials
            _authenticationValidator.ValidateCreateUser(model);

            // Remove extraneous white space
            string modelEmail = model.Email.Trim().ToLower();
            string modelAlias = model.Alias.Trim();

            // Get confirm verification token
            TimeSpan expiryTimeSpan = _authenticationConfigurationService.GetCreateUserExpiryTimeSpan(model.TenantId);
            Token confirmToken = _securityService.CreateToken(expiryTimeSpan);

            // Create user in respository
            List<string> roles = new List<string> { Roles.User };
            long userId = _userRepository.CreateUser(model.TenantId, modelEmail, modelAlias, roles, confirmToken);

            // Get details of email that will be sent to newly created user
            Email email = _authenticationConfigurationService.GetCreateUserEmail(Web, Domain, modelEmail, modelAlias, confirmToken);

            // Send email to newly created user
            _emailService.SendEmail(email);

            // Return newly allocated user identifier
            return userId;
        }

        public void CheckConfirmUserStatus(ConfirmUserStatusModel model)
        {
            _authenticationValidator.ValidateConfirmUserStatus(model);
        }

        public void ConfirmUserSetPassword(ConfirmUserSetPasswordModel model)
        {
            // Validate supplied confirmation details
            _authenticationValidator.ValidateConfirmUserSetPassword(model);

            // Get encrypted password
            int saltSize = _authenticationConfigurationService.GetPasswordSaltSize(model.TenantId);
            byte[] salt = _securityService.CreateSalt(saltSize);
            byte[] saltedHash = _securityService.EncryptPassword(model.Password, salt);

            // Flag user as confirmed in database and update user's password
            Token token = _securityService.DeserializeToken(model.ConfirmKey);

            // Get user
            User user = _userRepository.ReadUserByConfirmToken(model.TenantId, token);

            // Set user details
            DateTime passwordChanged = DateTime.UtcNow;
            user.Confirmed = true;
            user.PasswordSaltedHash = _stringService.GetString(saltedHash);
            user.PasswordSalt = _stringService.GetString(salt);
            user.ConfirmTokenValue = null;
            user.ConfirmTokenExpiry = null;
            user.LockedOut = false;
            user.LastPasswordFailure = null;
            user.PasswordFailures = 0;
            user.PasswordChanged = passwordChanged;

            // Update user
            _userRepository.UpdateUser(user);
        }

        public void Logon(LogonModel model)
        {
            try
            {
                // Validate supplied logon credentials
                _authenticationValidator.ValidateLogon(model);

                // Username and password valid, user not locked out. We can continue and log user on.
                User user = _userRepository.ReadUserByEmail(model.TenantId, model.Email.Trim().ToLower());

                // Get user details that we will persist during authenticated session
                AuthenticatedUser authenticatedUser = new AuthenticatedUser
                {
                    Alias = user.Alias,
                    Email = user.Email,
                    UserId = user.UserId,
                    TenantId = model.TenantId,
                    Roles = user.Roles.Select(r => r.Name).ToList()
                };
                AuthenticatedUserInfo authenticatedUserInfo = new AuthenticatedUserInfo
                {
                    User = authenticatedUser,
                    RememberMe = model.RememberMe
                };

                // Logon user using authentication provider
                _authenticationProviderService.LogonAuthenticatedUser(authenticatedUserInfo);
            }
            catch (UserLockedOutException)
            {
                Logoff();
                throw;
            }
        }

        public AuthenticatedUserInfo GetCurrentUser()
        {
            return _authenticationProviderService.GetCurrentUser();
        }

        public void EnsureLoggedOnUser()
        {
            if (GetCurrentUser() == null)
                throw new AuthorizationException();
        }

        public void Logoff()
        {
            _authenticationProviderService.Logoff();
        }

        public void ChangePassword(ChangePasswordModel model)
        {
            try
            {
                // Validate supplied details
                _authenticationValidator.ValidateChangePassword(model);

                // Get user
                User user = _userRepository.ReadUser(model.TenantId, model.UserId);

                // Get encrypted password
                int saltSize = _authenticationConfigurationService.GetPasswordSaltSize(model.TenantId);
                byte[] salt = _securityService.CreateSalt(saltSize);
                byte[] saltedHash = _securityService.EncryptPassword(model.NewPassword, salt);

                // Update user details
                user.Confirmed = true;
                user.PasswordSaltedHash = _stringService.GetString(saltedHash);
                user.PasswordSalt = _stringService.GetString(salt);
                user.LockedOut = false;
                user.LastPasswordFailure = null;
                user.PasswordFailures = 0;
                user.PasswordChanged = DateTime.UtcNow;

                // Change user's password
                _userRepository.UpdateUser(user);
            }
            catch (UserLockedOutException)
            {
                Logoff();
                throw;
            }
        }

        public User GetUser(long tenantId, long userId, IUnitOfWork unitOfWork = null)
        {
            return _userRepository.ReadUser(tenantId, userId, unitOfWork);
        }

        public ImageUploadIds PrepareImages(long tenantId, CreateUploadModel model, IUnitOfWork unitOfWork = null)
        {
            // If we don't have a unit of work in place, create one now so that we can rollback all changes in case of failure
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;

            // Begin work
            try
            {
                // Check that website allows avatars and that uploaded content is valid image
                AuthenticationValidateResult result = _authenticationValidator.ValidatePrepareImages(tenantId, model);

                // Create thumbnail model
                ResizeInfo thumbnailResizeInfo = new ResizeInfo
                {
                    Width = result.Web.UserThumbnailImageWidth.Value,
                    Height = result.Web.UserThumbnailImageHeight.Value,
                    ResizeMode = result.Web.UserThumbnailImageResizeMode.Value
                };
                byte[] thumbnailContent = _imageAnalysisService.ResizeImage(model.Content, thumbnailResizeInfo);
                CreateUploadModel thumbnailModel = new CreateUploadModel
                {
                    Content = thumbnailContent,
                    ContentType = model.ContentType,
                    Name = model.Name,
                    TenantId = model.TenantId
                };

                // Create preview model
                ResizeInfo previewResizeInfo = new ResizeInfo
                {
                    Width = result.Web.UserPreviewImageWidth.Value,
                    Height = result.Web.UserPreviewImageHeight.Value,
                    ResizeMode = result.Web.UserPreviewImageResizeMode.Value
                };
                byte[] previewContent = _imageAnalysisService.ResizeImage(model.Content, previewResizeInfo);
                CreateUploadModel previewModel = new CreateUploadModel
                {
                    Content = previewContent,
                    ContentType = model.ContentType,
                    Name = model.Name,
                    TenantId = model.TenantId
                };

                // Create uploads for thumbnail, preview and original image
                long thumbnailImageUploadId = _uploadService.Create(thumbnailModel, unitOfWork ?? localUnitOfWork);
                long previewImageUploadId = _uploadService.Create(previewModel, unitOfWork ?? localUnitOfWork);
                long imageUploadId = _uploadService.Create(model, unitOfWork ?? localUnitOfWork);

                // Commit work if local unit of work in place and return result
                if (localUnitOfWork != null)
                    localUnitOfWork.Commit();
                return new ImageUploadIds { ThumbnailImageUploadId = thumbnailImageUploadId, PreviewImageUploadId = previewImageUploadId, ImageUploadId = imageUploadId };
            }
            catch (ValidationErrorException)
            {
                if (localUnitOfWork != null)
                    localUnitOfWork.Rollback();
                throw;
            }
            catch (Exception ex)
            {
                if (localUnitOfWork != null)
                    localUnitOfWork.Rollback();
                throw new ValidationErrorException(new ValidationError(null, ApplicationResource.UnexpectedErrorMessage), ex);
            }
            finally
            {
                if (localUnitOfWork != null)
                    localUnitOfWork.Dispose();
            }
        }

        private List<string> GetUserImageStorageHierarchy()
        {
            return new List<string> { "users", "images" };
        }

        public bool UpdateUser(UpdateUserModel model, IUnitOfWork unitOfWork = null)
        {
            // If we don't have a unit of work in place, create one now so that we can rollback all changes in case of failure
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;

            // Do the user update
            try
            {
                // Validate supplied details (including checking that all or none of the image upload properties are specified)
                _authenticationValidator.ValidateUpdateUser(model);

                // Get existing user details 
                User user = _userRepository.ReadUser(model.TenantId, model.UserId);
                string currentAlias = user.Alias;
                string currentEmail = user.Email;

                // Get new details
                string newAlias = model.Alias.Trim();
                string newEmail = model.Email.Trim().ToLower();
                user.Alias = newAlias;
                user.Email = newEmail;

                // Has email address changed?
                bool reconfirm = (currentEmail != newEmail);
                if (reconfirm)
                {
                    // Get confirm verification token
                    TimeSpan expiryTimeSpan = _authenticationConfigurationService.GetUpdateUserExpiryTimeSpan(model.TenantId);
                    Token confirmToken = _securityService.CreateToken(expiryTimeSpan);

                    // Unconfirm account
                    user.Confirmed = false;
                    user.ConfirmTokenValue = confirmToken.Value.ToString();
                    user.ConfirmTokenExpiry = confirmToken.Expiry;

                    // Commit user images?
                    if (model.ImageUploadId.HasValue && user.ImageUploadId != model.ImageUploadId)
                    {
                        _uploadService.Commit(model.ImageTenantId.Value, model.ThumbnailImageUploadId.Value, GetUserImageStorageHierarchy(), unitOfWork ?? localUnitOfWork);
                        _uploadService.Commit(model.ImageTenantId.Value, model.PreviewImageUploadId.Value, GetUserImageStorageHierarchy(), unitOfWork ?? localUnitOfWork);
                        _uploadService.Commit(model.ImageTenantId.Value, model.ImageUploadId.Value, GetUserImageStorageHierarchy(), unitOfWork ?? localUnitOfWork);

                        user.ImageTenantId = model.ImageTenantId;
                        user.ThumbnailImageUploadId = model.ThumbnailImageUploadId;
                        user.PreviewImageUploadId = model.PreviewImageUploadId;
                        user.ImageUploadId = model.ImageUploadId;
                    }

                    // Update user details
                    _userRepository.UpdateUser(user, unitOfWork ?? localUnitOfWork);

                    // Get details of email that will be sent to user who must re-confirm their account
                    Email email = _authenticationConfigurationService.GetUpdateUserEmail(Web, Domain, newEmail, newAlias, confirmToken);

                    // Send email to newly created user
                    _emailService.SendEmail(email);
                }

                // If email address not changed
                if (!reconfirm)
                {
                    // Commit user images?
                    if (model.ImageUploadId.HasValue && user.ImageUploadId != model.ImageUploadId)
                    {
                        _uploadService.Commit(model.ImageTenantId.Value, model.ThumbnailImageUploadId.Value, GetUserImageStorageHierarchy(), unitOfWork ?? localUnitOfWork);
                        _uploadService.Commit(model.ImageTenantId.Value, model.PreviewImageUploadId.Value, GetUserImageStorageHierarchy(), unitOfWork ?? localUnitOfWork);
                        _uploadService.Commit(model.ImageTenantId.Value, model.ImageUploadId.Value, GetUserImageStorageHierarchy(), unitOfWork ?? localUnitOfWork);

                        user.ImageTenantId = model.ImageTenantId;
                        user.ThumbnailImageUploadId = model.ThumbnailImageUploadId;
                        user.PreviewImageUploadId = model.PreviewImageUploadId;
                        user.ImageUploadId = model.ImageUploadId;
                    }

                    // Update user details
                    _userRepository.UpdateUser(user, unitOfWork ?? localUnitOfWork);

                    // If email address not changed, update authenticated user details
                    AuthenticatedUserInfo userInfo = GetCurrentUser();
                    userInfo.User.Alias = user.Alias;
                    _authenticationProviderService.LogonAuthenticatedUser(userInfo);
                }

                // Commit work if local unit of work in place
                if (localUnitOfWork != null)
                    localUnitOfWork.Commit();

                // Return whether or not user must reconfirm address
                return reconfirm;
            }
            catch (ValidationErrorException)
            {
                if (localUnitOfWork != null)
                    localUnitOfWork.Rollback();
                throw;
            }
            catch (Exception ex)
            {
                if (localUnitOfWork != null)
                    localUnitOfWork.Rollback();
                throw new ValidationErrorException(new ValidationError(null, ApplicationResource.UnexpectedErrorMessage), ex);
            }
        }

        public void ConfirmUser(ConfirmUserModel model)
        {
            // Validate supplied confirmation details
            _authenticationValidator.ValidateConfirmUser(model);

            // Get user
            Token token = _securityService.DeserializeToken(model.ConfirmKey);
            User user = _userRepository.ReadUserByConfirmToken(model.TenantId, token);

            // Flag user as confirmed
            user.Confirmed = true;
            user.ConfirmTokenValue = null;
            user.ConfirmTokenExpiry = null;

            // Update user in database
            _userRepository.UpdateUser(user);
        }

        public void ForgottenPassword(ForgottenPasswordModel model)
        {
            // Validate supplied reset password info
            _authenticationValidator.ValidateForgottenPassword(model);

            // Get reset password token
            TimeSpan expiryTimeSpan = _authenticationConfigurationService.GetForgottenPasswordExpiryTimeSpan(model.TenantId);
            Token resetPasswordToken = _securityService.CreateToken(expiryTimeSpan);

            // Get user
            string modelEmail = model.Email.Trim().ToLower();
            User user = _userRepository.ReadUserByEmail(model.TenantId, modelEmail);

            // Sets a user's reset password token in preparation for a reset password action
            user.ResetPasswordTokenValue = resetPasswordToken.Value.ToString();
            user.ResetPasswordTokenExpiry = resetPasswordToken.Expiry;

            // Update user
            _userRepository.UpdateUser(user);

            // Get details of email that will be sent to user requesting password reset            
            Email email = _authenticationConfigurationService.GetForgottenPasswordEmail(Web, Domain, user.Email, user.Alias, resetPasswordToken);

            // Send forgotten password email to user
            _emailService.SendEmail(email);
        }

        public void CheckResetPasswordStatus(ResetPasswordStatusModel model)
        {
            _authenticationValidator.ValidateResetPasswordStatus(model);
        }

        public void ResetPassword(ResetPasswordModel model)
        {
            // Validate supplied confirmation details
            _authenticationValidator.ValidateResetPassword(model);

            // Get encrypted password
            int saltSize = _authenticationConfigurationService.GetPasswordSaltSize(model.TenantId);
            byte[] salt = _securityService.CreateSalt(saltSize);
            byte[] saltedHash = _securityService.EncryptPassword(model.Password, salt);

            // Get user by reset password token
            Token token = _securityService.DeserializeToken(model.ResetPasswordKey);
            User user = _userRepository.ReadUserByResetPasswordToken(model.TenantId, token);

            // Update password details
            user.PasswordSaltedHash = _stringService.GetString(saltedHash);
            user.PasswordSalt = _stringService.GetString(salt);
            user.ResetPasswordTokenValue = null;
            user.ResetPasswordTokenExpiry = null;
            user.LockedOut = false;
            user.LastPasswordFailure = null;
            user.PasswordFailures = 0;
            user.PasswordChanged = DateTime.UtcNow;

            // Update user in database
            _userRepository.UpdateUser(user);
        }

        public Image ReadThumbnailImage(long tenantId, long userId, IUnitOfWork unitOfWork = null)
        {
            Image image = null;
            User user = GetUser(tenantId, userId, unitOfWork);
            if (user.ThumbnailImageUploadId.HasValue)
                image = (Image)_uploadService.Read(user.ImageTenantId.Value, user.ThumbnailImageUploadId.Value, GetUserImageStorageHierarchy(), unitOfWork);
            return image;
        }

        public Image ReadPreviewImage(long tenantId, long userId, IUnitOfWork unitOfWork = null)
        {
            Image image = null;
            User user = GetUser(tenantId, userId, unitOfWork);
            if (user.PreviewImageUploadId.HasValue)
                image = (Image)_uploadService.Read(user.ImageTenantId.Value, user.PreviewImageUploadId.Value, GetUserImageStorageHierarchy(), unitOfWork);
            return image;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Domains;
using Riverside.Cms.Core.Uploads;
using Riverside.Cms.Core.Webs;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Authentication
{
    /// <summary>
    /// Interface for services that implement authentication features such as login, logoff, change password, forgotten password etc.
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Returns tenant identifier of current website that is being accessed.
        /// </summary>
        long TenantId { get; }

        /// <summary>
        /// Returns current website that is being accessed.
        /// </summary>
        Web Web { get; }

        /// <summary>
        /// Returns domain that website is being accessed from.
        /// </summary>
        Domain Domain { get; }

        /// <summary>
        /// Creates / registers / signs up a new user.
        /// </summary>
        /// <param name="model">Contains details of user to create (e.g. email address and alias).</param>
        /// <returns>Unique identifier allocated to newly created user.</returns>
        long CreateUser(CreateUserModel model);

        /// <summary>
        /// Performs initial validation before a confirm user attempt.
        /// </summary>
        /// <param name="model">Information required to confirm user.</param>
        void CheckConfirmUserStatus(ConfirmUserStatusModel model);

        /// <summary>
        /// Confirms a newly created user and sets initial password.
        /// </summary>
        /// <param name="model">Contains details used to confirm user.</param>
        void ConfirmUserSetPassword(ConfirmUserSetPasswordModel model);

        /// <summary>
        /// Logs user on.
        /// </summary>
        /// <param name="model">Contains user logon credentials.</param>
        void Logon(LogonModel model);

        /// <summary>
        /// Gets logged on user or returns null if no user logged on.
        /// </summary>
        /// <returns>Authenticated user details.</returns>
        AuthenticatedUserInfo GetCurrentUser();

        /// <summary>
        /// Throws an exception if there is no logged on user.
        /// </summary>
        void EnsureLoggedOnUser();

        /// <summary>
        /// Logs off the currently authenticated user.
        /// </summary>
        void Logoff();

        /// <summary>
        /// Changes a users password.
        /// </summary>
        /// <param name="model">Identifies user and new password.</param>
        void ChangePassword(ChangePasswordModel model);

        /// <summary>
        /// Gets user details given user identifier.
        /// </summary>
        /// <param name="tenantId">Website identifier.</param>
        /// <param name="userId">User identifier.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>User (or null if user does not exist).</returns>
        User GetUser(long tenantId, long userId, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// From a single uploaded file, creates thumbnail, preview and original images in underlying storage that may be associated with a user.
        /// </summary>
        /// <param name="tenantId">Website that user belongs to.</param>
        /// <param name="model">Image upload.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Identifiers of newly created thumbnail, preview and source images.</returns>
        ImageUploadIds PrepareImages(long tenantId, CreateUploadModel model, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Updates user details.
        /// </summary>
        /// <param name="model">Contains new user details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>True if user must be re-confirmed following update.</returns>
        bool UpdateUser(UpdateUserModel model, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Confirms a user without password change.
        /// </summary>
        /// <param name="model">Contains details used to confirm user.</param>
        void ConfirmUser(ConfirmUserModel model);

        /// <summary>
        /// Initiates a forgotten password request to reset a user's password.
        /// </summary>
        /// <param name="model">Identifies user who is resetting their password.</param>
        void ForgottenPassword(ForgottenPasswordModel model);

        /// <summary>
        /// Checks validity of reset password verification token. i.e. that a user exists for the given token and that the token has not expired.
        /// </summary>
        /// <param name="model">Identifies reset password token.</param>
        void CheckResetPasswordStatus(ResetPasswordStatusModel model);

        /// <summary>
        /// Resets a user's password.
        /// </summary>
        /// <param name="model">Reset password details.</param>
        void ResetPassword(ResetPasswordModel model);

        /// <summary>
        /// Gets thumbnail image associated with user (null if no thumbnail image).
        /// </summary>
        /// <param name="tenantId">Website identifier.</param>
        /// <param name="userId">User identifier.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Thumbnail image.</returns>
        Image ReadThumbnailImage(long tenantId, long userId, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Gets preview image associated with user (null if no preview image).
        /// </summary>
        /// <param name="tenantId">Website identifier.</param>
        /// <param name="userId">User identifier.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Preview image.</returns>
        Image ReadPreviewImage(long tenantId, long userId, IUnitOfWork unitOfWork = null);
    }
}

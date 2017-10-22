using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Authorization;

namespace Riverside.Cms.Core.Authentication
{
    /// <summary>
    /// Holds user information.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Identifies the website that the user belongs to.
        /// </summary>
        public long TenantId { get; set; }

        /// <summary>
        /// User identifier.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// User alias.
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// User alias.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// True if user confirmed, false if not. A value of true indicates user has confirmed their account following creation.
        /// </summary>
        public bool Confirmed { get; set; }

        /// <summary>
        /// Used by administrators to control access to website. Set false to prevent user accessing site.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// True if user locked out due to repeated attempts to access website with the wrong password within a given time span.
        /// </summary>
        public bool LockedOut { get; set; }

        /// <summary>
        /// Stores salted hash of password (or null if password not set).
        /// </summary>
        public string PasswordSaltedHash { get; set; }

        /// <summary>
        /// Stores salt used during password hashing.
        /// </summary>
        public string PasswordSalt { get; set; }

        /// <summary>
        /// The date and time password last changed (or null if password never changed).
        /// </summary>
        public DateTime? PasswordChanged { get; set; }

        /// <summary>
        /// The date and time password last incorrectly entered (or null if password never incorrectly entered).
        /// </summary>
        public DateTime? LastPasswordFailure { get; set; }

        /// <summary>
        /// The number of times password incorrectly entered within a given time span.
        /// </summary>
        public int PasswordFailures { get; set; }

        /// <summary>
        /// Reset password token value (or null if no current reset password verification token).
        /// </summary>
        public string ResetPasswordTokenValue { get; set; }

        /// <summary>
        /// Reset password token expiry (or null if no current reset password verification token).
        /// </summary>
        public DateTime? ResetPasswordTokenExpiry { get; set; }

        /// <summary>
        /// Confirm token value (or null if no current confirm verification token).
        /// </summary>
        public string ConfirmTokenValue { get; set; }

        /// <summary>
        /// Confirm token expiry (or null if no current confirm verification token).
        /// </summary>
        public DateTime? ConfirmTokenExpiry { get; set; }

        /// <summary>
        /// The tenant determining image upload primary key (null if no image).
        /// </summary>
        public long? ImageTenantId { get; set; }

        /// <summary>
        /// Thumbnail image upload identifier (null if no thumbnail image).
        /// </summary>
        public long? ThumbnailImageUploadId { get; set; }

        /// <summary>
        /// Preview image upload identifier (null if no preview image).
        /// </summary>
        public long? PreviewImageUploadId { get; set; }

        /// <summary>
        /// Source image upload identifier (null if no image).
        /// </summary>
        public long? ImageUploadId { get; set; }

        /// <summary>
        /// List of roles associated with the user.
        /// </summary>
        public List<Role> Roles { get; set; }
    }
}

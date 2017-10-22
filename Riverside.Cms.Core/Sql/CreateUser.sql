SET NOCOUNT ON

INSERT INTO
	cms.[User] (TenantId, Alias, Email, Confirmed, [Enabled], LockedOut, PasswordSaltedHash, PasswordSalt, PasswordChanged, LastPasswordFailure, PasswordFailures, ResetPasswordTokenValue, ResetPasswordTokenExpiry, ConfirmTokenValue, ConfirmTokenExpiry,
		ImageTenantId, ThumbnailImageUploadId, PreviewImageUploadId, ImageUploadId)
VALUES
	(@TenantId, @Alias, @Email, 0, 1, 0, NULL, NULL, NULL, NULL, 0, NULL, NULL, @ConfirmTokenValue, @ConfirmTokenExpiry,
		NULL, NULL, NULL, NULL)

SET @UserId = SCOPE_IDENTITY()

INSERT INTO
	cms.UserRole (TenantId, UserId, RoleId)
SELECT
	@TenantId,
	@UserId,
	cms.[Role].RoleId
FROM
	@UserRoles UserRoles
INNER JOIN
	cms.[Role]
ON
	UserRoles.Name = cms.[Role].Name
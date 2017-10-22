CREATE PROCEDURE [cms].[UpdateUser]
	@TenantId				  bigint,
	@UserId					  bigint,
	@Alias					  nvarchar(50),
	@Email					  nvarchar(256),
	@Confirmed				  bit,
	@Enabled				  bit,
	@LockedOut				  bit,
	@PasswordSaltedHash		  varchar(344),
	@PasswordSalt			  varchar(24),
	@PasswordChanged		  datetime,
	@LastPasswordFailure	  datetime,
	@PasswordFailures		  int,
	@ResetPasswordTokenValue  uniqueidentifier,
	@ResetPasswordTokenExpiry datetime,
	@ConfirmTokenValue		  uniqueidentifier,
	@ConfirmTokenExpiry		  datetime,
	@ImageTenantId			  bigint,
	@ThumbnailImageUploadId	  bigint,
	@PreviewImageUploadId	  bigint,
	@ImageUploadId			  bigint
AS

SET NOCOUNT ON

UPDATE
	cms.[User]
SET
	cms.[User].Alias					= @Alias,
	cms.[User].Email					= @Email,
	cms.[User].Confirmed				= @Confirmed,
	cms.[User].[Enabled]				= @Enabled,
	cms.[User].LockedOut				= @LockedOut,
	cms.[User].PasswordSaltedHash		= @PasswordSaltedHash,
	cms.[User].PasswordSalt				= @PasswordSalt,
	cms.[User].PasswordChanged			= @PasswordChanged,
	cms.[User].LastPasswordFailure		= @LastPasswordFailure,
	cms.[User].PasswordFailures			= @PasswordFailures,
	cms.[User].ResetPasswordTokenValue	= @ResetPasswordTokenValue,
	cms.[User].ResetPasswordTokenExpiry	= @ResetPasswordTokenExpiry,
	cms.[User].ConfirmTokenValue		= @ConfirmTokenValue,
	cms.[User].ConfirmTokenExpiry		= @ConfirmTokenExpiry,
	cms.[User].ImageTenantId			= @ImageTenantId,
	cms.[User].ThumbnailImageUploadId	= @ThumbnailImageUploadId,
	cms.[User].PreviewImageUploadId		= @PreviewImageUploadId,
	cms.[User].ImageUploadId			= @ImageUploadId
WHERE
	cms.[User].TenantId	= @TenantId AND
	cms.[User].UserId	= @UserId
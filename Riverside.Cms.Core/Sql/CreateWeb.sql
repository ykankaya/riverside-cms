SET NOCOUNT ON

INSERT INTO
	cms.Web (TenantId, Name, CreateUserEnabled, UserHasImage, UserThumbnailImageWidth, UserThumbnailImageHeight, UserThumbnailImageResizeMode,
		UserPreviewImageWidth, UserPreviewImageHeight, UserPreviewImageResizeMode, UserImageMinWidth, UserImageMinHeight,
		FontOption, ColourOption)
VALUES
	(@TenantId, @Name, @CreateUserEnabled, @UserHasImage, @UserThumbnailImageWidth, @UserThumbnailImageHeight, @UserThumbnailImageResizeMode,
		@UserPreviewImageWidth, @UserPreviewImageHeight, @UserPreviewImageResizeMode, @UserImageMinWidth, @UserImageMinHeight,
		@FontOption, @ColourOption)
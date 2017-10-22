SET NOCOUNT ON

UPDATE
	cms.Web
SET
	cms.Web.Name						 = @Name,
	cms.Web.CreateUserEnabled			 = @CreateUserEnabled,
	cms.Web.UserHasImage				 = @UserHasImage,
	cms.Web.UserThumbnailImageWidth		 = @UserThumbnailImageWidth,
	cms.Web.UserThumbnailImageHeight	 = @UserThumbnailImageHeight,
	cms.Web.UserThumbnailImageResizeMode = @UserThumbnailImageResizeMode,
	cms.Web.UserPreviewImageWidth		 = @UserPreviewImageWidth,
	cms.Web.UserPreviewImageHeight		 = @UserPreviewImageHeight,
	cms.Web.UserPreviewImageResizeMode	 = @UserPreviewImageResizeMode,
	cms.Web.UserImageMinWidth			 = @UserImageMinWidth,
	cms.Web.UserImageMinHeight			 = @UserImageMinHeight,
	cms.Web.FontOption					 = @FontOption,
	cms.Web.ColourOption				 = @ColourOption
WHERE
	cms.Web.TenantId = @TenantId
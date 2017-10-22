SET NOCOUNT ON

SELECT
	cms.Web.TenantId,
	cms.Web.Name,
	cms.Web.CreateUserEnabled,
	cms.Web.UserHasImage,
	cms.Web.UserThumbnailImageWidth,
	cms.Web.UserThumbnailImageHeight,
	cms.Web.UserThumbnailImageResizeMode,
	cms.Web.UserPreviewImageWidth,
	cms.Web.UserPreviewImageHeight,
	cms.Web.UserPreviewImageResizeMode,
	cms.Web.UserImageMinWidth,
	cms.Web.UserImageMinHeight,
	cms.Web.FontOption,
	cms.Web.ColourOption
FROM
	cms.Web
WHERE
	cms.Web.TenantId = @TenantId
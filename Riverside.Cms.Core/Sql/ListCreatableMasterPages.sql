SET NOCOUNT ON

SELECT
	cms.MasterPage.TenantId,
	cms.MasterPage.MasterPageId,
	cms.MasterPage.Name,
	cms.MasterPage.PageName,
	cms.MasterPage.PageDescription,
	cms.MasterPage.AncestorPageId,
	cms.MasterPage.AncestorPageLevel,
	cms.MasterPage.PageType,
	cms.MasterPage.HasOccurred,
	cms.MasterPage.HasImage,
	cms.MasterPage.ThumbnailImageWidth,
	cms.MasterPage.ThumbnailImageHeight,
	cms.MasterPage.ThumbnailImageResizeMode,
	cms.MasterPage.PreviewImageWidth,
	cms.MasterPage.PreviewImageHeight,
	cms.MasterPage.PreviewImageResizeMode,
	cms.MasterPage.ImageMinWidth,
	cms.MasterPage.ImageMinHeight,
	cms.MasterPage.Creatable,
	cms.MasterPage.Deletable,
	cms.MasterPage.Taggable,
	cms.MasterPage.Administration,
	cms.MasterPage.BeginRender,
	cms.MasterPage.EndRender
FROM
	cms.MasterPage
WHERE
	cms.MasterPage.TenantId = @TenantId AND
	cms.MasterPage.Creatable = 1
ORDER BY
	cms.MasterPage.Name
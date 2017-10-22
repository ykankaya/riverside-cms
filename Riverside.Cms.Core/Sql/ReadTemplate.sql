SET NOCOUNT ON

SELECT
	cms.Template.TenantId,
	cms.Template.Name,
	cms.Template.[Description],
	cms.Template.CreateUserEnabled,
	cms.Template.UserHasImage,
	cms.Template.UserThumbnailImageWidth,
	cms.Template.UserThumbnailImageHeight,
	cms.Template.UserThumbnailImageResizeMode,
	cms.Template.UserPreviewImageWidth,
	cms.Template.UserPreviewImageHeight,
	cms.Template.UserPreviewImageResizeMode,
	cms.Template.UserImageMinWidth,
	cms.Template.UserImageMinHeight
FROM
	cms.Template
WHERE
	cms.Template.TenantId = @TenantId

IF (@LoadAll = 1)
BEGIN
	SELECT
		cms.TemplatePage.TenantId,
		cms.TemplatePage.TemplatePageId,
		cms.TemplatePage.Name,
		cms.TemplatePage.PageName,
		cms.TemplatePage.PageDescription,
		cms.TemplatePage.PageType,
		cms.TemplatePage.HasOccurred,
		cms.TemplatePage.HasImage,
		cms.TemplatePage.ThumbnailImageWidth,
		cms.TemplatePage.ThumbnailImageHeight,
		cms.TemplatePage.ThumbnailImageResizeMode,
		cms.TemplatePage.PreviewImageWidth,
		cms.TemplatePage.PreviewImageHeight,
		cms.TemplatePage.PreviewImageResizeMode,
		cms.TemplatePage.ImageMinWidth,
		cms.TemplatePage.ImageMinHeight,
		cms.TemplatePage.AncestorPageLevel,
		cms.TemplatePage.ParentTemplatePageId,
		cms.TemplatePage.ShowOnNavigation,
		cms.TemplatePage.Creatable,
		cms.TemplatePage.Deletable,
		cms.TemplatePage.Taggable,
		cms.TemplatePage.Administration,
		cms.TemplatePage.BeginRender,
		cms.TemplatePage.EndRender
	FROM
		cms.TemplatePage
	WHERE
		cms.TemplatePage.TenantId = @TenantId
	ORDER BY
		cms.TemplatePage.TenantId,
		cms.TemplatePage.TemplatePageId

	SELECT
		cms.TemplatePageZone.TenantId,
		cms.TemplatePageZone.TemplatePageId,
		cms.TemplatePageZone.TemplatePageZoneId,
		cms.TemplatePageZone.Name,
		cms.TemplatePageZone.SortOrder,
		cms.TemplatePageZone.AdminType,
		cms.TemplatePageZone.ContentType,
		cms.TemplatePageZone.BeginRender,
		cms.TemplatePageZone.EndRender
	FROM
		cms.TemplatePageZone
	WHERE
		cms.TemplatePageZone.TenantId = @TenantId
	ORDER BY
		cms.TemplatePageZone.TenantId,
		cms.TemplatePageZone.TemplatePageId,
		cms.TemplatePageZone.SortOrder

	SELECT
		cms.TemplatePageZoneElementType.TenantId,
		cms.TemplatePageZoneElementType.TemplatePageId,
		cms.TemplatePageZoneElementType.TemplatePageZoneId,
		cms.TemplatePageZoneElementType.ElementTypeId
	FROM
		cms.TemplatePageZoneElementType
	WHERE
		cms.TemplatePageZoneElementType.TenantId = @TenantId
	ORDER BY
		cms.TemplatePageZoneElementType.TenantId,
		cms.TemplatePageZoneElementType.TemplatePageId,
		cms.TemplatePageZoneElementType.TemplatePageZoneId,
		cms.TemplatePageZoneElementType.ElementTypeId

	SELECT
		cms.TemplatePageZoneElement.TenantId,
		cms.TemplatePageZoneElement.TemplatePageId,
		cms.TemplatePageZoneElement.TemplatePageZoneId,
		cms.TemplatePageZoneElement.TemplatePageZoneElementId,
		cms.TemplatePageZoneElement.SortOrder,
		cms.TemplatePageZoneElement.ElementId,
		cms.TemplatePageZoneElement.BeginRender,
		cms.TemplatePageZoneElement.EndRender,
		cms.Element.ElementTypeId
	FROM
		cms.TemplatePageZoneElement
	INNER JOIN
		cms.Element
	ON
		cms.TemplatePageZoneElement.TenantId  = cms.Element.TenantId AND
		cms.TemplatePageZoneElement.ElementId = cms.Element.ElementId
	WHERE
		cms.TemplatePageZoneElement.TenantId = @TenantId
	ORDER BY
		cms.TemplatePageZoneElement.TenantId,
		cms.TemplatePageZoneElement.TemplatePageId,
		cms.TemplatePageZoneElement.TemplatePageZoneId,
		cms.TemplatePageZoneElement.SortOrder
END
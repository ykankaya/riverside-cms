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
	cms.MasterPage.TenantId     = @TenantId AND
	cms.MasterPage.MasterPageId = @MasterPageId

SELECT
	cms.MasterPageZone.TenantId,
	cms.MasterPageZone.MasterPageId,
	cms.MasterPageZone.MasterPageZoneId,
	cms.MasterPageZone.Name,
	cms.MasterPageZone.SortOrder,
	cms.MasterPageZone.AdminType,
	cms.MasterPageZone.ContentType,
	cms.MasterPageZone.BeginRender,
	cms.MasterPageZone.EndRender
FROM
	cms.MasterPageZone
WHERE
	cms.MasterPageZone.TenantId		= @TenantId AND
	cms.MasterPageZone.MasterPageId = @MasterPageId
ORDER BY
	cms.MasterPageZone.SortOrder

SELECT
	cms.MasterPageZoneElementType.TenantId,
	cms.MasterPageZoneElementType.MasterPageId,
	cms.MasterPageZoneElementType.MasterPageZoneId,
	cms.MasterPageZoneElementType.ElementTypeId,
	cms.ElementType.Name
FROM
	cms.MasterPageZoneElementType
INNER JOIN
	cms.ElementType
ON
	cms.MasterPageZoneElementType.ElementTypeId = cms.ElementType.ElementTypeId
WHERE
	cms.MasterPageZoneElementType.TenantId = @TenantId AND
	cms.MasterPageZoneElementType.MasterPageId = @MasterPageId
ORDER BY
	cms.MasterPageZoneElementType.TenantId,
	cms.MasterPageZoneElementType.MasterPageId,
	cms.MasterPageZoneElementType.MasterPageZoneId,
	cms.ElementType.Name

SELECT
	cms.MasterPageZoneElement.TenantId,
	cms.MasterPageZoneElement.MasterPageId,
	cms.MasterPageZoneElement.MasterPageZoneId,
	cms.MasterPageZoneElement.MasterPageZoneElementId,
	cms.MasterPageZoneElement.SortOrder,
	cms.MasterPageZoneElement.ElementId,
	cms.MasterPageZoneElement.BeginRender,
	cms.MasterPageZoneElement.EndRender,
	cms.Element.ElementTypeId,
	cms.Element.Name
FROM
	cms.MasterPageZoneElement
INNER JOIN
	cms.Element
ON
	cms.MasterPageZoneElement.TenantId  = cms.Element.TenantId AND
	cms.MasterPageZoneElement.ElementId = cms.Element.ElementId
WHERE
	cms.MasterPageZoneElement.TenantId     = @TenantId AND
	cms.MasterPageZoneElement.MasterPageId = @MasterPageId
ORDER BY
	cms.MasterPageZoneElement.MasterPageZoneId,
	cms.MasterPageZoneElement.SortOrder
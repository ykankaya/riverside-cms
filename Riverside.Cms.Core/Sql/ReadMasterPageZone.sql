SET NOCOUNT ON

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
	cms.MasterPageZone.TenantId			= @TenantId AND
	cms.MasterPageZone.MasterPageId		= @MasterPageId AND
	cms.MasterPageZone.MasterPageZoneId = @MasterPageZoneId

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
	cms.MasterPageZoneElementType.TenantId			= @TenantId AND
	cms.MasterPageZoneElementType.MasterPageId		= @MasterPageId AND
	cms.MasterPageZoneElementType.MasterPageZoneId	= @MasterPageZoneId	
ORDER BY
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
	cms.MasterPageZoneElement.TenantId			= @TenantId AND
	cms.MasterPageZoneElement.MasterPageId		= @MasterPageId AND
	cms.MasterPageZoneElement.MasterPageZoneId	= @MasterPageZoneId
ORDER BY
	cms.MasterPageZoneElement.SortOrder
SET NOCOUNT ON

SELECT
	cms.MasterPage.MasterPageId,
	cms.MasterPage.Name AS MasterPageName,
	cms.MasterPageZone.MasterPageZoneId,
	cms.MasterPageZone.Name AS MasterPageZoneName,
	cms.MasterPageZoneElement.MasterPageZoneElementId,
	cms.Element.Name,
	cms.Element.TenantId,
	cms.Element.ElementId,
	cms.Element.ElementTypeId
FROM
	cms.Element
INNER JOIN
	cms.MasterPageZoneElement
ON
	cms.Element.TenantId = cms.MasterPageZoneElement.TenantId AND
	cms.Element.ElementId = cms.MasterPageZoneElement.ElementId
INNER JOIN
	cms.MasterPageZone
ON
	cms.MasterPageZoneElement.TenantId = cms.MasterPageZone.TenantId AND
	cms.MasterPageZoneElement.MasterPageId = cms.MasterPageZone.MasterPageId AND
	cms.MasterPageZoneElement.MasterPageZoneId = cms.MasterPageZone.MasterPageZoneId
INNER JOIN
	cms.MasterPage
ON
	cms.MasterPageZone.TenantId = cms.MasterPage.TenantId AND
	cms.MasterPageZone.MasterPageId = cms.MasterPage.MasterPageId
WHERE
	cms.MasterPage.TenantId = @TenantId
ORDER BY
	cms.MasterPage.Name,
	cms.MasterPage.MasterPageId,
	cms.MasterPageZone.SortOrder,
	cms.MasterPageZoneElement.SortOrder
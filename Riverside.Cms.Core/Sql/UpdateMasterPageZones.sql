SET NOCOUNT ON

/*----- DELETE: Master page zone element types, master page zone elements and master page zones that are no longer used -----*/

DELETE
	cms.MasterPageZoneElementType
FROM
	cms.MasterPageZoneElementType
LEFT JOIN
	@MasterPageZones MasterPageZones
ON
	cms.MasterPageZoneElementType.MasterPageZoneId = MasterPageZones.MasterPageZoneId
WHERE
	cms.MasterPageZoneElementType.TenantId = @TenantId AND
	cms.MasterPageZoneElementType.MasterPageId = @MasterPageId AND
	MasterPageZones.MasterPageZoneId IS NULL

DELETE
	cms.MasterPageZoneElement
FROM
	cms.MasterPageZoneElement
LEFT JOIN
	@MasterPageZones MasterPageZones
ON
	cms.MasterPageZoneElement.MasterPageZoneId = MasterPageZones.MasterPageZoneId
WHERE
	cms.MasterPageZoneElement.TenantId = @TenantId AND
	cms.MasterPageZoneElement.MasterPageId = @MasterPageId AND
	MasterPageZones.MasterPageZoneId IS NULL

DELETE
	cms.MasterPageZone
FROM
	cms.MasterPageZone
LEFT JOIN
	@MasterPageZones MasterPageZones
ON
	cms.MasterPageZone.MasterPageZoneId = MasterPageZones.MasterPageZoneId
WHERE
	cms.MasterPageZone.TenantId = @TenantId AND
	cms.MasterPageZone.MasterPageId = @MasterPageId AND
	MasterPageZones.MasterPageZoneId IS NULL

/*----- UPDATE: Existing master page zones -----*/

UPDATE
	cms.MasterPageZone
SET
	cms.MasterPageZone.Name = MasterPageZones.Name,
	cms.MasterPageZone.SortOrder = MasterPageZones.SortOrder
FROM
	cms.MasterPageZone
INNER JOIN
	@MasterPageZones MasterPageZones
ON
	cms.MasterPageZone.TenantId = @TenantId AND
	cms.MasterPageZone.MasterPageId = @MasterPageId AND
	cms.MasterPageZone.MasterPageZoneId = MasterPageZones.MasterPageZoneId

/*----- CREATE: Insert new master page zones -----*/

INSERT INTO
	cms.MasterPageZone (TenantId, MasterPageId, Name, SortOrder, AdminType, ContentType, BeginRender, EndRender)
SELECT
	@TenantId, @MasterPageId, MasterPageZones.Name, MasterPageZones.SortOrder, MasterPageZones.AdminType, MasterPageZones.ContentType, MasterPageZones.BeginRender, MasterPageZones.EndRender
FROM
	@MasterPageZones MasterPageZones
WHERE
	MasterPageZones.MasterPageZoneId IS NULL
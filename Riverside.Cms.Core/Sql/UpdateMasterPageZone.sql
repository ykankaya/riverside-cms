SET NOCOUNT ON

/*----- DELETE: Master page zone element types and master page zone elements that are no longer used -----*/

DELETE
	cms.MasterPageZoneElementType
FROM
	cms.MasterPageZoneElementType
LEFT JOIN
	@MasterPageZoneElementTypes MasterPageZoneElementTypes
ON
	cms.MasterPageZoneElementType.ElementTypeId = MasterPageZoneElementTypes.ElementTypeId
WHERE
	cms.MasterPageZoneElementType.TenantId = @TenantId AND
	cms.MasterPageZoneElementType.MasterPageId = @MasterPageId AND
	cms.MasterPageZoneElementType.MasterPageZoneId = @MasterPageZoneId AND
	MasterPageZoneElementTypes.MasterPageZoneId IS NULL AND
	MasterPageZoneElementTypes.ElementTypeId IS NULL

DELETE
	cms.MasterPageZoneElement
FROM
	cms.MasterPageZoneElement
LEFT JOIN
	@MasterPageZoneElements MasterPageZoneElements
ON
	cms.MasterPageZoneElement.MasterPageZoneElementId = MasterPageZoneElements.MasterPageZoneElementId
WHERE
	cms.MasterPageZoneElement.TenantId = @TenantId AND
	cms.MasterPageZoneElement.MasterPageId = @MasterPageId AND
	cms.MasterPageZoneElement.MasterPageZoneId = @MasterPageZoneId AND
	MasterPageZoneElements.MasterPageZoneId IS NULL AND
	MasterPageZoneElements.MasterPageZoneElementId IS NULL

/*----- UPDATE: Existing master page zone elements and master page zone -----*/

UPDATE
	cms.MasterPageZoneElement
SET
	cms.MasterPageZoneElement.SortOrder   = MasterPageZoneElements.SortOrder,
	cms.MasterPageZoneElement.ElementId   = MasterPageZoneElements.ElementId,
	cms.MasterPageZoneElement.BeginRender = MasterPageZoneElements.BeginRender,
	cms.MasterPageZoneElement.EndRender   = MasterPageZoneElements.EndRender
FROM
	cms.MasterPageZoneElement
INNER JOIN
	@MasterPageZoneElements MasterPageZoneElements
ON
	cms.MasterPageZoneElement.TenantId = @TenantId AND
	cms.MasterPageZoneElement.MasterPageId = @MasterPageId AND
	cms.MasterPageZoneElement.MasterPageZoneId = @MasterPageZoneId AND
	cms.MasterPageZoneElement.MasterPageZoneElementId = MasterPageZoneElements.MasterPageZoneElementId

UPDATE
	cms.MasterPageZone
SET
	cms.MasterPageZone.Name			= @Name,
	cms.MasterPageZone.SortOrder	= @SortOrder,
	cms.MasterPageZone.AdminType	= @AdminType,
	cms.MasterPageZone.ContentType	= @ContentType,
	cms.MasterPageZone.BeginRender	= @BeginRender,
	cms.MasterPageZone.EndRender	= @EndRender
WHERE
	cms.MasterPageZone.TenantId			= @TenantId AND
	cms.MasterPageZone.MasterPageId		= @MasterPageId AND
	cms.MasterPageZone.MasterPageZoneId = @MasterPageZoneId

/*----- CREATE: Insert new master page zone elements and master page zone element types -----*/

INSERT INTO
	cms.MasterPageZoneElement (TenantId, MasterPageId, MasterPageZoneId, SortOrder, ElementId, BeginRender, EndRender)
SELECT
	@TenantId, @MasterPageId, @MasterPageZoneId, MasterPageZoneElements.SortOrder, MasterPageZoneElements.ElementId, MasterPageZoneElements.BeginRender, MasterPageZoneElements.EndRender
FROM
	@MasterPageZoneElements MasterPageZoneElements
INNER JOIN
	cms.MasterPageZone
ON
	cms.MasterPageZone.TenantId			= @TenantId AND
	cms.MasterPageZone.MasterPageId		= @MasterPageId AND
	cms.MasterPageZone.MasterPageZoneId = @MasterPageZoneId AND
	cms.MasterPageZone.SortOrder		= MasterPageZoneElements.MasterPageZoneSortOrder
WHERE
	MasterPageZoneElements.MasterPageZoneElementId IS NULL

INSERT INTO
	cms.MasterPageZoneElementType (TenantId, MasterPageId, MasterPageZoneId, ElementTypeId)
SELECT
	@TenantId, @MasterPageId, @MasterPageZoneId, MasterPageZoneElementTypes.ElementTypeId
FROM
	@MasterPageZoneElementTypes MasterPageZoneElementTypes
INNER JOIN
	cms.MasterPageZone
ON
	cms.MasterPageZone.TenantId			= @TenantId AND
	cms.MasterPageZone.MasterPageId		= @MasterPageId AND
	cms.MasterPageZone.MasterPageZoneId = @MasterPageZoneId AND
	cms.MasterPageZone.SortOrder		= MasterPageZoneElementTypes.MasterPageZoneSortOrder
LEFT JOIN
	cms.MasterPageZoneElementType
ON
	cms.MasterPageZone.TenantId = cms.MasterPageZoneElementType.TenantId AND
	cms.MasterPageZone.MasterPageId = cms.MasterPageZoneElementType.MasterPageId AND
	cms.MasterPageZone.MasterPageZoneId = cms.MasterPageZoneElementType.MasterPageZoneId AND
	cms.MasterPageZoneElementType.ElementTypeId = MasterPageZoneElementTypes.ElementTypeId
WHERE
	cms.MasterPageZoneElementType.ElementTypeId IS NULL
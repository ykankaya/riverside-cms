SET NOCOUNT ON

SELECT
	cms.PageZone.TenantId,
	cms.PageZone.PageId,
	cms.PageZone.PageZoneId,
	cms.PageZone.MasterPageId,
	cms.PageZone.MasterPageZoneId
FROM
	cms.PageZone
WHERE
	cms.PageZone.TenantId = @TenantId AND
	cms.PageZone.MasterPageId = @MasterPageId AND
	cms.PageZone.MasterPageZoneId = @MasterPageZoneId
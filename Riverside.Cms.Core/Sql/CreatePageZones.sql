SET NOCOUNT ON

INSERT INTO
	cms.PageZone (TenantId, PageId, MasterPageId, MasterPageZoneId)
SELECT
	@TenantId, PageZones.PageId, PageZones.MasterPageId, PageZones.MasterPageZoneId
FROM
	@PageZones PageZones
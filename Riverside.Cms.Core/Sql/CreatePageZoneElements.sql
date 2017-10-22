SET NOCOUNT ON

INSERT INTO
	cms.PageZoneElement (TenantId, PageId, PageZoneId, SortOrder, ElementId, MasterPageId, MasterPageZoneId, MasterPageZoneElementId)
SELECT
	@TenantId, PageZoneElements.PageId, PageZoneElements.PageZoneId, PageZoneElements.SortOrder, PageZoneElements.ElementId, PageZoneElements.MasterPageId, PageZoneElements.MasterPageZoneId, PageZoneElements.MasterPageZoneElementId
FROM
	@PageZoneElements PageZoneElements
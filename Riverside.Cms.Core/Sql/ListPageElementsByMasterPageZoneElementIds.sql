SET NOCOUNT ON

/*----- Get elements that may be removed when a master page zone is updated -----*/

SELECT DISTINCT
	cms.PageZoneElement.ElementId
FROM
	cms.PageZoneElement
INNER JOIN
	@MasterPageZoneElements MasterPageZoneElements
ON
	cms.PageZoneElement.MasterPageZoneElementId = MasterPageZoneElements.MasterPageZoneElementId
WHERE
	cms.PageZoneElement.TenantId		 = @TenantId AND
	cms.PageZoneElement.MasterPageId	 = @MasterPageId AND
	cms.PageZoneElement.MasterPageZoneId = @MasterPageZoneId
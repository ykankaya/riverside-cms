SET NOCOUNT ON

/*----- Delete page zone elements that reference soon to be deleted master page zone elements -----*/

DELETE
	cms.PageZoneElement
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
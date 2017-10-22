SET NOCOUNT ON

DELETE
	cms.PageZoneElement
FROM
	cms.PageZoneElement
INNER JOIN
	cms.PageZone
ON
	cms.PageZoneElement.TenantId   = cms.PageZone.TenantId AND
	cms.PageZoneElement.PageId	   = cms.PageZone.PageId AND
	cms.PageZoneElement.PageZoneId = cms.PageZone.PageZoneId
WHERE
	cms.PageZone.TenantId		  = @TenantId AND
	cms.PageZone.MasterPageId	  = @MasterPageId AND
	cms.PageZone.MasterPageZoneId = @MasterPageZoneId
SET NOCOUNT ON

DELETE
	cms.PageZone
WHERE
	cms.PageZone.TenantId		  = @TenantId AND
	cms.PageZone.MasterPageId	  = @MasterPageId AND
	cms.PageZone.MasterPageZoneId = @MasterPageZoneId
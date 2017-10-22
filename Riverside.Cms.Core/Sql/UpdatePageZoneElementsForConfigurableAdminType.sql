SET NOCOUNT ON

UPDATE
	cms.PageZoneElement
SET
	cms.PageZoneElement.MasterPageId			= NULL,
	cms.PageZoneElement.MasterPageZoneId		= NULL,
	cms.PageZoneElement.MasterPageZoneElementId	= NULL,
	cms.PageZoneElement.SortOrder				= cms.MasterPageZoneElement.SortOrder
FROM
	cms.PageZoneElement
INNER JOIN
	cms.MasterPageZoneElement
ON
	cms.PageZoneElement.TenantId				= cms.MasterPageZoneElement.TenantId AND
	cms.PageZoneElement.MasterPageId			= cms.MasterPageZoneElement.MasterPageId AND
	cms.PageZoneElement.MasterPageZoneId		= cms.MasterPageZoneElement.MasterPageZoneId AND
	cms.PageZoneElement.MasterPageZoneElementId	= cms.MasterPageZoneElement.MasterPageZoneElementId
WHERE
	cms.PageZoneElement.TenantId			= @TenantId AND
	cms.PageZoneElement.MasterPageId		= @MasterPageId AND
	cms.PageZoneElement.MasterPageZoneId	= @MasterPageZoneId
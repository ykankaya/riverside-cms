SET NOCOUNT ON

SELECT
	cms.[Page].TenantId,
	cms.[Page].PageId,
	cms.[Page].ParentPageId,
	cms.[Page].MasterPageId,
	cms.[Page].Name,
	cms.[Page].[Description],
	cms.[Page].Created,
	cms.[Page].Updated,
	cms.[Page].Occurred,
	cms.[Page].ImageTenantId,
	cms.[Page].ThumbnailImageUploadId,
	cms.[Page].PreviewImageUploadId,
	cms.[Page].ImageUploadId
FROM
	cms.[Page]
WHERE
	cms.[Page].TenantId = @TenantId AND
	cms.[Page].PageId   = @PageId

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
	cms.PageZone.PageId   = @PageId
ORDER BY
	cms.PageZone.PageZoneId

SELECT
	cms.PageZoneElement.TenantId,
	cms.PageZoneElement.PageId,
	cms.PageZoneElement.PageZoneId,
	cms.PageZoneElement.PageZoneElementId,
	cms.PageZoneElement.SortOrder,
	cms.PageZoneElement.ElementId,
	cms.PageZoneElement.MasterPageId,
	cms.PageZoneElement.MasterPageZoneId,
	cms.PageZoneElement.MasterPageZoneElementId,
	cms.Element.ElementTypeId
FROM
	cms.PageZoneElement
INNER JOIN
	cms.Element
ON
	cms.PageZoneElement.TenantId  = cms.Element.TenantId AND
	cms.PageZoneElement.ElementId = cms.Element.ElementId
WHERE
	cms.PageZoneElement.TenantId = @TenantId AND
	cms.PageZoneElement.PageId   = @PageId
ORDER BY
	cms.PageZoneElement.PageZoneId,
	cms.PageZoneElement.SortOrder

SELECT
	cms.[Tag].TenantId,
	cms.[Tag].TagId,
	cms.[Tag].Name,
	cms.[Tag].Created,
	cms.[Tag].Updated
FROM
	cms.[Tag]
INNER JOIN
	cms.[TagPage]
ON
	cms.[Tag].TenantId = cms.[TagPage].TenantId AND
	cms.[Tag].TagId    = cms.[TagPage].TagId
WHERE
	cms.[TagPage].TenantId = @TenantId AND
	cms.[TagPage].PageId   = @PageId
ORDER BY
	cms.[Tag].Name
SET NOCOUNT ON

INSERT INTO
	[cms].[Page] (TenantId, ParentPageId, MasterPageId, Name, [Description], Created, Updated, Occurred, ImageTenantId, ThumbnailImageUploadId, PreviewImageUploadId, ImageUploadId)
VALUES
	(@TenantId, @ParentPageId, @MasterPageId, @Name, @Description, @Created, @Updated, @Occurred, @ImageTenantId, @ThumbnailImageUploadId, @PreviewImageUploadId, @ImageUploadId)

SET @PageId = SCOPE_IDENTITY()

INSERT INTO
	cms.PageZone (TenantId, PageId, MasterPageId, MasterPageZoneId)
SELECT
	@TenantId, @PageId, PageZones.MasterPageId, PageZones.MasterPageZoneId
FROM
	@PageZones PageZones

INSERT INTO
	cms.PageZoneElement (TenantId, PageId, PageZoneId, SortOrder, ElementId, MasterPageId, MasterPageZoneId, MasterPageZoneElementId)
SELECT
	@TenantId, @PageId, cms.PageZone.PageZoneId, PageZoneElements.SortOrder, PageZoneElements.ElementId, PageZoneElements.MasterPageId, PageZoneElements.MasterPageZoneId, PageZoneElements.MasterPageZoneElementId
FROM
	@PageZoneElements PageZoneElements
INNER JOIN
	cms.PageZone
ON
	cms.PageZone.TenantId         = @TenantId AND
	cms.PageZone.PageId           = @PageId AND
	cms.PageZone.MasterPageId     = @MasterPageId AND
	cms.PageZone.MasterPageZoneId = PageZoneElements.PageZoneMasterPageZoneId
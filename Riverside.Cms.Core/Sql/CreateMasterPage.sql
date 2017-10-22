SET NOCOUNT ON

INSERT INTO
	cms.MasterPage (
		TenantId, Name, PageName, PageDescription, AncestorPageId, AncestorPageLevel, PageType, HasOccurred, Creatable, Deletable, Taggable, Administration,
		HasImage, ThumbnailImageWidth, ThumbnailImageHeight, ThumbnailImageResizeMode, PreviewImageWidth, PreviewImageHeight, PreviewImageResizeMode,
		ImageMinWidth, ImageMinHeight, BeginRender, EndRender)
VALUES
	(@TenantId, @Name, @PageName, @PageDescription, @AncestorPageId, @AncestorPageLevel, @PageType, @HasOccurred, @Creatable, @Deletable, @Taggable, @Administration,
		@HasImage, @ThumbnailImageWidth, @ThumbnailImageHeight, @ThumbnailImageResizeMode, @PreviewImageWidth, @PreviewImageHeight, @PreviewImageResizeMode,
		@ImageMinWidth, @ImageMinHeight, @BeginRender, @EndRender)

SET @MasterPageId = SCOPE_IDENTITY()

INSERT INTO
	cms.MasterPageZone (TenantId, MasterPageId, Name, SortOrder, AdminType, ContentType, BeginRender, EndRender)
SELECT
	@TenantId, @MasterPageId, MasterPageZones.Name, MasterPageZones.SortOrder, MasterPageZones.AdminType, MasterPageZones.ContentType,
	MasterPageZones.BeginRender, MasterPageZones.EndRender
FROM
	@MasterPageZones MasterPageZones

INSERT INTO
	cms.MasterPageZoneElementType (TenantId, MasterPageId, MasterPageZoneId, ElementTypeId)
SELECT
	@TenantId, @MasterPageId, cms.MasterPageZone.MasterPageZoneId, MasterPageZoneElementTypes.ElementTypeId
FROM
	@MasterPageZoneElementTypes MasterPageZoneElementTypes
INNER JOIN
	cms.MasterPageZone
ON
	cms.MasterPageZone.TenantId		= @TenantId AND
	cms.MasterPageZone.MasterPageId	= @MasterPageId AND
	cms.MasterPageZone.SortOrder	= MasterPageZoneElementTypes.MasterPageZoneSortOrder

INSERT INTO
	cms.MasterPageZoneElement (TenantId, MasterPageId, MasterPageZoneId, SortOrder, ElementId, BeginRender, EndRender)
SELECT
	@TenantId, @MasterPageId, cms.MasterPageZone.MasterPageZoneId, MasterPageZoneElements.SortOrder, MasterPageZoneElements.ElementId, MasterPageZoneElements.BeginRender, MasterPageZoneElements.EndRender
FROM
	@MasterPageZoneElements MasterPageZoneElements
INNER JOIN
	cms.MasterPageZone
ON
	cms.MasterPageZone.TenantId     = @TenantId AND
	cms.MasterPageZone.MasterPageId = @MasterPageId AND
	cms.MasterPageZone.SortOrder    = MasterPageZoneElements.MasterPageZoneSortOrder
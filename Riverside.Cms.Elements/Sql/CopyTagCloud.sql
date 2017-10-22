SET NOCOUNT ON

INSERT INTO
	element.TagCloud (TenantId, ElementId, PageTenantId, PageId, DisplayName, [Recursive], NoTagsMessage)
SELECT
	@DestTenantId,
	@DestElementId,
	element.TagCloud.PageTenantId,
	element.TagCloud.PageId,
	element.TagCloud.DisplayName,
	element.TagCloud.[Recursive],
	element.TagCloud.NoTagsMessage
FROM
	element.TagCloud
WHERE
	element.TagCloud.TenantId  = @SourceTenantId AND
	element.TagCloud.ElementId = @SourceElementId
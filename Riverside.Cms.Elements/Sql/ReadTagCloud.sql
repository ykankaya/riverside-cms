SET NOCOUNT ON

SELECT
	element.TagCloud.TenantId,
	element.TagCloud.ElementId,
	element.TagCloud.PageTenantId,
	element.TagCloud.PageId,
	element.TagCloud.DisplayName,
	element.TagCloud.[Recursive],
	element.TagCloud.NoTagsMessage
FROM
	element.TagCloud
WHERE
	element.TagCloud.TenantId = @TenantId AND
	element.TagCloud.ElementId = @ElementId
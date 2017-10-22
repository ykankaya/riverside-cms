SET NOCOUNT ON

UPDATE
	element.TagCloud
SET
	element.TagCloud.PageTenantId   = @PageTenantId,
	element.TagCloud.PageId         = @PageId,
	element.TagCloud.DisplayName	= @DisplayName,
	element.TagCloud.[Recursive]	= @Recursive,
	element.TagCloud.NoTagsMessage	= @NoTagsMessage
WHERE
	element.TagCloud.TenantId = @TenantId AND
	element.TagCloud.ElementId = @ElementId
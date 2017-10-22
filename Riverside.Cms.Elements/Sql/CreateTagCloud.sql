SET NOCOUNT ON

INSERT INTO
	element.TagCloud (TenantId, ElementId, PageTenantId, PageId, DisplayName, [Recursive], NoTagsMessage)
VALUES
	(@TenantId, @ElementId, @PageTenantId, @PageId, @DisplayName, @Recursive, @NoTagsMessage)
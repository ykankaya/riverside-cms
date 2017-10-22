SET NOCOUNT ON

INSERT INTO
	element.LatestThread (TenantId, ElementId, PageTenantId, PageId, DisplayName, [Recursive], NoThreadsMessage, Preamble, PageSize)
VALUES
	(@TenantId, @ElementId, @PageTenantId, @PageId, @DisplayName, @Recursive, @NoThreadsMessage, @Preamble, @PageSize)
SET NOCOUNT ON

UPDATE
	element.LatestThread
SET
	element.LatestThread.PageTenantId		= @TenantId,
	element.LatestThread.PageId				= @PageId,
	element.LatestThread.DisplayName		= @DisplayName,
	element.LatestThread.[Recursive]		= @Recursive,
	element.LatestThread.NoThreadsMessage	= @NoThreadsMessage,
	element.LatestThread.Preamble			= @Preamble,
	element.LatestThread.PageSize			= @PageSize
WHERE
	element.LatestThread.TenantId  = @TenantId AND
	element.LatestThread.ElementId = @ElementId
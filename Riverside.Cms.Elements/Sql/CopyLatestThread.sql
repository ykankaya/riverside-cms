SET NOCOUNT ON

INSERT INTO
	element.LatestThread (TenantId, ElementId, PageTenantId, PageId, DisplayName, [Recursive], NoThreadsMessage, Preamble, PageSize)
SELECT
	@DestTenantId,
	@DestElementId,
	element.LatestThread.PageTenantId,
	element.LatestThread.PageId,
	element.LatestThread.DisplayName,
	element.LatestThread.[Recursive],
	element.LatestThread.NoThreadsMessage,
	element.LatestThread.Preamble,
	element.LatestThread.PageSize
FROM
	element.LatestThread
WHERE
	element.LatestThread.TenantId  = @SourceTenantId AND
	element.LatestThread.ElementId = @SourceElementId
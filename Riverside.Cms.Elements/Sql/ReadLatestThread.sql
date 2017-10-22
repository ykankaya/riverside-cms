SET NOCOUNT ON

SELECT
	element.LatestThread.TenantId,
	element.LatestThread.ElementId,
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
	element.LatestThread.TenantId = @TenantId AND
	element.LatestThread.ElementId = @ElementId
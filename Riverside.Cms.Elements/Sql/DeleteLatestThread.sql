SET NOCOUNT ON

DELETE
	element.LatestThread
WHERE
	element.LatestThread.TenantId  = @TenantId AND
	element.LatestThread.ElementId = @ElementId
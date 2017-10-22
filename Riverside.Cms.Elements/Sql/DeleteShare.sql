SET NOCOUNT ON

DELETE
	element.Share
WHERE
	element.Share.TenantId  = @TenantId AND
	element.Share.ElementId = @ElementId
SET NOCOUNT ON

DELETE
	element.[Table]
WHERE
	element.[Table].TenantId  = @TenantId AND
	element.[Table].ElementId = @ElementId
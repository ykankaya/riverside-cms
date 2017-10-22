SET NOCOUNT ON

DELETE
	element.Map
WHERE
	element.Map.TenantId  = @TenantId AND
	element.Map.ElementId = @ElementId
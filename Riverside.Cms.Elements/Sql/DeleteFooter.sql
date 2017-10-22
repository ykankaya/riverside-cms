SET NOCOUNT ON

DELETE
	element.Footer
WHERE
	element.Footer.TenantId  = @TenantId AND
	element.Footer.ElementId = @ElementId
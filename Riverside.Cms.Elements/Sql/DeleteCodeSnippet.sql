SET NOCOUNT ON

DELETE
	element.CodeSnippet
WHERE
	element.CodeSnippet.TenantId  = @TenantId AND
	element.CodeSnippet.ElementId = @ElementId
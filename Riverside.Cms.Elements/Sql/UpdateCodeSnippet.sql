SET NOCOUNT ON

UPDATE
	element.CodeSnippet
SET
	element.CodeSnippet.Code		= @Code,
	element.CodeSnippet.[Language]	= @Language
WHERE
	element.CodeSnippet.TenantId  = @TenantId AND
	element.CodeSnippet.ElementId = @ElementId
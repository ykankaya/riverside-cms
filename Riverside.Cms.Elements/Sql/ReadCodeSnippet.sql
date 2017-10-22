SET NOCOUNT ON

SELECT
	element.CodeSnippet.TenantId,
	element.CodeSnippet.ElementId,
	element.CodeSnippet.Code,
	element.CodeSnippet.[Language]
FROM
	element.CodeSnippet
WHERE
	element.CodeSnippet.TenantId  = @TenantId AND
	element.CodeSnippet.ElementId = @ElementId
SET NOCOUNT ON

INSERT INTO
	element.CodeSnippet (TenantId, ElementId, Code, [Language])
SELECT
	@DestTenantId,
	@DestElementId,
	element.CodeSnippet.Code,
	element.CodeSnippet.[Language]
FROM
	element.CodeSnippet
WHERE
	element.CodeSnippet.TenantId  = @SourceTenantId AND
	element.CodeSnippet.ElementId = @SourceElementId
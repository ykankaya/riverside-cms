SET NOCOUNT ON

INSERT INTO
	element.CodeSnippet (TenantId, ElementId, Code, [Language])
VALUES
	(@TenantId, @ElementId, @Code, @Language)
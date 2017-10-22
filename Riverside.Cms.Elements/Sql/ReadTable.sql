SET NOCOUNT ON

SELECT
	element.[Table].TenantId,
	element.[Table].ElementId,
	element.[Table].DisplayName,
	element.[Table].Preamble,
	element.[Table].ShowHeaders,
	element.[Table].[Rows]
FROM
	element.[Table]
WHERE
	element.[Table].TenantId = @TenantId AND
	element.[Table].ElementId = @ElementId
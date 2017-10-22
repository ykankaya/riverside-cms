SET NOCOUNT ON

UPDATE
	element.[Table]
SET
	element.[Table].DisplayName = @DisplayName,
	element.[Table].Preamble = @Preamble,
	element.[Table].ShowHeaders = @ShowHeaders,
	element.[Table].[Rows] = @Rows
WHERE
	element.[Table].TenantId = @TenantId AND
	element.[Table].ElementId = @ElementId
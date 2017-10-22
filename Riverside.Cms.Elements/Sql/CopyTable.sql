SET NOCOUNT ON

INSERT INTO
	element.[Table] (TenantId, ElementId, DisplayName, Preamble, ShowHeaders, [Rows])
SELECT
	@DestTenantId,
	@DestElementId,
	element.[Table].DisplayName,
	element.[Table].Preamble,
	element.[Table].ShowHeaders,
	element.[Table].[Rows]
FROM
	element.[Table]
WHERE
	element.[Table].TenantId  = @SourceTenantId AND
	element.[Table].ElementId = @SourceElementId
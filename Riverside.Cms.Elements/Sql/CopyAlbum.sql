SET NOCOUNT ON

INSERT INTO
	element.Album (TenantId, ElementId, DisplayName)
SELECT
	@DestTenantId,
	@DestElementId,
	element.Album.DisplayName
FROM
	element.Album
WHERE
	element.Album.TenantId  = @SourceTenantId AND
	element.Album.ElementId = @SourceElementId
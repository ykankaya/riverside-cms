SET NOCOUNT ON

UPDATE
	element.Map
SET
	element.Map.DisplayName	= @DisplayName,
	element.Map.Latitude	= @Latitude,
	element.Map.Longitude	= @Longitude
WHERE
	element.Map.TenantId = @TenantId AND
	element.Map.ElementId = @ElementId
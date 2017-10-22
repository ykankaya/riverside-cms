SET NOCOUNT ON

INSERT INTO
	element.Map (TenantId, ElementId, DisplayName, Latitude, Longitude)
SELECT
	@DestTenantId,
	@DestElementId,
	element.Map.DisplayName,
	element.Map.Latitude,
	element.Map.Longitude
FROM
	element.Map
WHERE
	element.Map.TenantId  = @SourceTenantId AND
	element.Map.ElementId = @SourceElementId
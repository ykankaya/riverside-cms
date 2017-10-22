SET NOCOUNT ON

SELECT
	element.Map.TenantId,
	element.Map.ElementId,
	element.Map.DisplayName,
	element.Map.Latitude,
	element.Map.Longitude
FROM
	element.Map
WHERE
	element.Map.TenantId = @TenantId AND
	element.Map.ElementId = @ElementId
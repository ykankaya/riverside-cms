SET NOCOUNT ON

INSERT INTO
	element.Map (TenantId, ElementId, DisplayName, Latitude, Longitude)
VALUES
	(@TenantId, @ElementId, @DisplayName, @Latitude, @Longitude)
SET NOCOUNT ON

INSERT INTO
	element.Album (TenantId, ElementId, DisplayName)
VALUES
	(@TenantId, @ElementId, @DisplayName)
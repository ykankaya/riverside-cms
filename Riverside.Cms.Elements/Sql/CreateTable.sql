SET NOCOUNT ON

INSERT INTO
	element.[Table] (TenantId, ElementId, DisplayName, Preamble, ShowHeaders, [Rows])
VALUES
	(@TenantId, @ElementId, @DisplayName, @Preamble, @ShowHeaders, @Rows)
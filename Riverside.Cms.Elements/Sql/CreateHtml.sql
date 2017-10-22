SET NOCOUNT ON

INSERT INTO
	element.Html (TenantId, ElementId, Html)
VALUES
	(@TenantId, @ElementId, @Html)
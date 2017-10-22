SET NOCOUNT ON

INSERT INTO
	element.Html (TenantId, ElementId, Html)
SELECT
	@DestTenantId,
	@DestElementId,
	element.Html.Html
FROM
	element.Html
WHERE
	element.Html.TenantId  = @SourceTenantId AND
	element.Html.ElementId = @SourceElementId
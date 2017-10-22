SET NOCOUNT ON

DELETE
	element.HtmlUpload
WHERE
	element.HtmlUpload.TenantId  = @TenantId AND
	element.HtmlUpload.ElementId = @ElementId

DELETE
	element.Html
WHERE
	element.Html.TenantId  = @TenantId AND
	element.Html.ElementId = @ElementId
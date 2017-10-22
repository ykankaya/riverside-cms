SET NOCOUNT ON

/*----- Update main HTML details -----*/

UPDATE
	element.Html
SET
	element.Html.Html = @Html
WHERE
	element.Html.TenantId  = @TenantId AND
	element.Html.ElementId = @ElementId

/*----- Delete HTML uploads that are no longer needed -----*/

DELETE
	element.HtmlUpload
FROM
	element.HtmlUpload
LEFT JOIN
	@HtmlUploads HtmlUploads
ON
	element.HtmlUpload.HtmlUploadId = HtmlUploads.HtmlUploadId
WHERE
	element.HtmlUpload.TenantId  = @TenantId AND
	element.HtmlUpload.ElementId = @ElementId AND
	HtmlUploads.HtmlUploadId IS NULL
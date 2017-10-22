SET NOCOUNT ON

SELECT
	element.HtmlUpload.TenantId,
	element.HtmlUpload.ElementId,
	element.HtmlUpload.HtmlUploadId,
	element.HtmlUpload.ImageTenantId,
	element.HtmlUpload.ThumbnailImageUploadId,
	element.HtmlUpload.PreviewImageUploadId,
	element.HtmlUpload.ImageUploadId,
	element.HtmlUpload.UploadTenantId,
	element.HtmlUpload.UploadId
FROM
	element.HtmlUpload
WHERE
	element.HtmlUpload.TenantId = @TenantId AND
	element.HtmlUpload.ElementId = @ElementId AND
	element.HtmlUpload.HtmlUploadId = @HtmlUploadId
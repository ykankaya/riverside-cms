SET NOCOUNT ON

INSERT INTO
	element.HtmlUpload (TenantId, ElementId, ImageTenantId, ThumbnailImageUploadId, PreviewImageUploadId, ImageUploadId, UploadTenantId, UploadId)
VALUES
	(@TenantId, @ElementId, @ImageTenantId, @ThumbnailImageUploadId, @PreviewImageUploadId, @ImageUploadId, @UploadTenantId, @UploadId)

SET @HtmlUploadId = SCOPE_IDENTITY()
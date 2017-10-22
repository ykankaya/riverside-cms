SET NOCOUNT ON

SELECT
	element.AlbumPhoto.TenantId,
	element.AlbumPhoto.ElementId,
	element.AlbumPhoto.AlbumPhotoId,
	element.AlbumPhoto.ImageTenantId,
	element.AlbumPhoto.ThumbnailImageUploadId,
	element.AlbumPhoto.PreviewImageUploadId,
	element.AlbumPhoto.ImageUploadId,
	element.AlbumPhoto.Name,
	element.AlbumPhoto.[Description],
	element.AlbumPhoto.SortOrder
FROM
	element.AlbumPhoto
WHERE
	element.AlbumPhoto.TenantId = @TenantId AND
	element.AlbumPhoto.ElementId = @ElementId AND
	element.AlbumPhoto.AlbumPhotoId = @AlbumPhotoId
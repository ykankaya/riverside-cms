SET NOCOUNT ON

SELECT
	element.Album.TenantId,
	element.Album.ElementId,
	element.Album.DisplayName
FROM
	element.Album
WHERE
	element.Album.TenantId = @TenantId AND
	element.Album.ElementId = @ElementId

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
	element.AlbumPhoto.TenantId  = @TenantId AND
	element.AlbumPhoto.ElementId = @ElementId
ORDER BY
	element.AlbumPhoto.SortOrder
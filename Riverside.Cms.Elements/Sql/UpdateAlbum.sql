SET NOCOUNT ON

UPDATE
	element.Album
SET
	element.Album.DisplayName = @DisplayName
WHERE
	element.Album.TenantId  = @TenantId AND
	element.Album.ElementId = @ElementId

/*----- Update existing photos -----*/

UPDATE
	element.AlbumPhoto
SET
	element.AlbumPhoto.ImageTenantId		  = AlbumPhotos.ImageTenantId,
	element.AlbumPhoto.ThumbnailImageUploadId = AlbumPhotos.ThumbnailImageUploadId,
	element.AlbumPhoto.PreviewImageUploadId   = AlbumPhotos.PreviewImageUploadId,
	element.AlbumPhoto.ImageUploadId		  = AlbumPhotos.ImageUploadId,
	element.AlbumPhoto.Name					  = AlbumPhotos.Name,
	element.AlbumPhoto.[Description]		  = AlbumPhotos.[Description],
	element.AlbumPhoto.SortOrder			  = AlbumPhotos.SortOrder
FROM
	element.AlbumPhoto
INNER JOIN
	@AlbumPhotos AlbumPhotos
ON
	element.AlbumPhoto.TenantId		= @TenantId AND
	element.AlbumPhoto.ElementId	= @ElementId AND
	element.AlbumPhoto.AlbumPhotoId	= AlbumPhotos.AlbumPhotoId

/*----- Delete album photos that are no longer needed -----*/

DELETE
	element.AlbumPhoto
FROM
	element.AlbumPhoto
LEFT JOIN
	@AlbumPhotos AlbumPhotos
ON
	element.AlbumPhoto.AlbumPhotoId = AlbumPhotos.AlbumPhotoId
WHERE
	element.AlbumPhoto.TenantId  = @TenantId AND
	element.AlbumPhoto.ElementId = @ElementId AND
	AlbumPhotos.AlbumPhotoId IS NULL
	
/*----- Create new album photos -----*/

INSERT INTO
	element.AlbumPhoto (TenantId, ElementId, ImageTenantId, ThumbnailImageUploadId, PreviewImageUploadId, ImageUploadId, Name, [Description], SortOrder)
SELECT
	@TenantId, @ElementId, AlbumPhotos.ImageTenantId, AlbumPhotos.ThumbnailImageUploadId, AlbumPhotos.PreviewImageUploadId, AlbumPhotos.ImageUploadId, AlbumPhotos.Name,
	AlbumPhotos.[Description], AlbumPhotos.SortOrder
FROM
	@AlbumPhotos AlbumPhotos
WHERE
	AlbumPhotos.AlbumPhotoId IS NULL
SET NOCOUNT ON

DELETE
	element.AlbumPhoto
WHERE
	element.AlbumPhoto.TenantId  = @TenantId AND
	element.AlbumPhoto.ElementId = @ElementId

DELETE
	element.Album
WHERE
	element.Album.TenantId  = @TenantId AND
	element.Album.ElementId = @ElementId
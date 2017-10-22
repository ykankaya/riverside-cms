CREATE PROCEDURE [cms].[UpdateImage]
	@TenantId	bigint,
	@UploadId	bigint,
	@UploadType int,
	@Name       nvarchar(max),
	@Size       int,
	@Committed	bit,
	@Created	datetime,
	@Updated	datetime,
	@Width		int,
	@Height		int
AS

SET NOCOUNT ON

UPDATE
	cms.Upload
SET
	cms.Upload.UploadType	= @UploadType,
	cms.Upload.Name			= @Name,
	cms.Upload.Size			= @Size,
	cms.Upload.[Committed]	= @Committed,
	cms.Upload.Created		= @Created,
	cms.Upload.Updated		= @Updated
WHERE
	cms.Upload.TenantId = @TenantId AND
	cms.Upload.UploadId = @UploadId

UPDATE
	cms.[Image]
SET
	cms.[Image].Width  = @Width,
	cms.[Image].Height = @Height
WHERE
	cms.[Image].TenantId = @TenantId AND
	cms.[Image].UploadId = @UploadId
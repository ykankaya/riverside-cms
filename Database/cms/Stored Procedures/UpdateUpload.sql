CREATE PROCEDURE [cms].[UpdateUpload]
	@TenantId	bigint,
	@UploadId	bigint,
	@UploadType int,
	@Name       nvarchar(max),
	@Size       int,
	@Committed	bit,
	@Created	datetime,
	@Updated	datetime
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
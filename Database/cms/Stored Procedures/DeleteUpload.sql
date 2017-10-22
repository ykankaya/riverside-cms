CREATE PROCEDURE [cms].[DeleteUpload]
	@TenantId bigint,
	@UploadId bigint
AS

SET NOCOUNT ON

DELETE 
	cms.[Image]
WHERE
	cms.[Image].TenantId = @TenantId AND
	cms.[Image].UploadId = @UploadId

DELETE 
	cms.Upload
WHERE
	cms.Upload.TenantId = @TenantId AND
	cms.Upload.UploadId = @UploadId
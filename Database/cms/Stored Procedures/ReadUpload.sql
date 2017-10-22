CREATE PROCEDURE [cms].[ReadUpload]
	@TenantId bigint,
	@UploadId bigint
AS

SET NOCOUNT ON

SELECT
	cms.Upload.TenantId,
	cms.Upload.UploadId,
	cms.Upload.UploadType,
	cms.Upload.Name,
	cms.Upload.Size,
	cms.Upload.Created,
	cms.[Image].Width,
	cms.[Image].Height
FROM
	cms.Upload
LEFT JOIN
	cms.[Image]
ON
	cms.Upload.TenantId = cms.[Image].TenantId AND
	cms.Upload.UploadId = cms.[Image].UploadId
WHERE
	cms.Upload.TenantId = @TenantId AND
	cms.Upload.UploadId = @UploadId
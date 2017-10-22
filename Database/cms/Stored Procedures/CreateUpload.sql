CREATE PROCEDURE [cms].[CreateUpload]
	@TenantId	bigint,
	@UploadType int,
	@Name       nvarchar(max),
	@Size       int,
	@Committed	bit,
	@Created	datetime,
	@Updated	datetime,
	@UploadId   bigint OUTPUT
AS

SET NOCOUNT ON

INSERT INTO
	cms.Upload (TenantId, UploadType, Name, Size, [Committed], Created, Updated)
VALUES
	(@TenantId, @UploadType, @Name, @Size, @Committed, @Created, @Updated)

SET @UploadId = SCOPE_IDENTITY()
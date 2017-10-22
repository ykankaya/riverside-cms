CREATE PROCEDURE [cms].[CreateImage]
	@TenantId	bigint,
	@UploadType int,
	@Name       nvarchar(max),
	@Size       int,
	@Committed	bit,
	@Created    datetime,
	@Updated	datetime,
	@Width      int,
	@Height     int,
	@UploadId   bigint OUTPUT
AS

SET NOCOUNT ON

INSERT INTO
	cms.Upload (TenantId, UploadType, Name, Size, [Committed], Created, Updated)
VALUES
	(@TenantId, @UploadType, @Name, @Size, @Committed, @Created, @Updated)

SET @UploadId = SCOPE_IDENTITY()

INSERT INTO
	cms.[Image] (TenantId, UploadId, Width, Height)
VALUES
	(@TenantId, @UploadId, @Width, @Height)
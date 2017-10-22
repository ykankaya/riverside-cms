CREATE PROCEDURE [cms].[UpdatePage]
	@TenantId				bigint,
	@PageId					bigint,
	@ParentPageId			bigint,
	@MasterPageId			bigint,
	@Name					nvarchar(256),
	@Description			nvarchar(max),
	@Created				datetime,
	@Updated				datetime,
	@Occurred				datetime,
	@ImageTenantId			bigint,
	@ThumbnailImageUploadId	bigint,
	@PreviewImageUploadId	bigint,
	@ImageUploadId			bigint
AS

SET NOCOUNT ON

UPDATE
	cms.[Page]
SET
	cms.[Page].ParentPageId				= @ParentPageId,
	cms.[Page].MasterPageId				= @MasterPageId,
	cms.[Page].Name						= @Name,
	cms.[Page].[Description]			= @Description,
	cms.[Page].Created					= @Created,
	cms.[Page].Updated					= @Updated,
	cms.[Page].Occurred					= @Occurred,
	cms.[Page].ImageTenantId			= @ImageTenantId,
	cms.[Page].ThumbnailImageUploadId	= @ThumbnailImageUploadId,
	cms.[Page].PreviewImageUploadId		= @PreviewImageUploadId,
	cms.[Page].ImageUploadId			= @ImageUploadId
WHERE
	cms.[Page].TenantId = @TenantId AND
	cms.[Page].PageId	= @PageId
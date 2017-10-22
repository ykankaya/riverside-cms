CREATE TABLE [element].[AlbumPhoto] (
	[TenantId] [bigint] NOT NULL,
	[ElementId] [bigint] NOT NULL,
	[AlbumPhotoId] [bigint] IDENTITY(1,1) NOT NULL,
	[ImageTenantId] [bigint] NOT NULL,
	[ThumbnailImageUploadId] [bigint] NOT NULL,
	[PreviewImageUploadId] [bigint] NOT NULL,
	[ImageUploadId] [bigint] NOT NULL,
	[Name] [nvarchar](256) NULL,
	[Description] [nvarchar](max) NULL,
	[SortOrder] [int] NOT NULL,
 CONSTRAINT [PK_AlbumPhoto] PRIMARY KEY CLUSTERED ([TenantId] ASC, [ElementId] ASC, [AlbumPhotoId] ASC),
 CONSTRAINT [FK_AlbumPhoto_Album] FOREIGN KEY([TenantId], [ElementId]) REFERENCES [element].[Album] ([TenantId], [ElementId]),
 CONSTRAINT [FK_AlbumPhoto_ThumbnailImage] FOREIGN KEY([ImageTenantId], [ThumbnailImageUploadId]) REFERENCES [cms].[Image] ([TenantId], [UploadId]),
 CONSTRAINT [FK_AlbumPhoto_PreviewImage] FOREIGN KEY([ImageTenantId], [PreviewImageUploadId]) REFERENCES [cms].[Image] ([TenantId], [UploadId]),
 CONSTRAINT [FK_AlbumPhoto_Image] FOREIGN KEY([ImageTenantId], [ImageUploadId]) REFERENCES [cms].[Image] ([TenantId], [UploadId])
)
CREATE TABLE [element].[HtmlUpload]
(
	[TenantId] [bigint] NOT NULL,
	[ElementId] [bigint] NOT NULL,
	[HtmlUploadId] [bigint] IDENTITY(1,1) NOT NULL,
	[ImageTenantId] [bigint] NULL,
	[ThumbnailImageUploadId] [bigint] NULL,
	[PreviewImageUploadId] [bigint] NULL,
	[ImageUploadId] [bigint] NULL,
	[UploadTenantId] [bigint] NULL,
	[UploadId] [bigint] NULL,
	CONSTRAINT [PK_HtmlUpload] PRIMARY KEY CLUSTERED ([TenantId] ASC, [ElementId] ASC, [HtmlUploadId] ASC),
	CONSTRAINT [FK_HtmlUpload_Html] FOREIGN KEY([TenantId], [ElementId]) REFERENCES [element].[Html] ([TenantId], [ElementId]),
	CONSTRAINT [FK_HtmlUpload_ThumbnailImage] FOREIGN KEY([ImageTenantId], [ThumbnailImageUploadId]) REFERENCES [cms].[Image] ([TenantId], [UploadId]),
	CONSTRAINT [FK_HtmlUpload_PreviewImage] FOREIGN KEY([ImageTenantId], [PreviewImageUploadId]) REFERENCES [cms].[Image] ([TenantId], [UploadId]),
	CONSTRAINT [FK_HtmlUpload_Image] FOREIGN KEY([ImageTenantId], [ImageUploadId]) REFERENCES [cms].[Image] ([TenantId], [UploadId]),
	CONSTRAINT [FK_HtmlUpload_Upload] FOREIGN KEY([UploadTenantId], [UploadId]) REFERENCES [cms].[Upload] ([TenantId], [UploadId])
)
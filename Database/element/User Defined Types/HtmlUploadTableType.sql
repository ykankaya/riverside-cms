CREATE TYPE [element].[HtmlUploadTableType] AS TABLE
(
	[HtmlUploadId] [bigint] NULL,
	[ImageTenantId] [bigint] NULL,
	[ThumbnailImageUploadId] [bigint] NULL,
	[PreviewImageUploadId] [bigint] NULL,
	[ImageUploadId] [bigint] NULL,
	[UploadTenantId] [bigint] NULL,
	[UploadId] [bigint] NULL
)
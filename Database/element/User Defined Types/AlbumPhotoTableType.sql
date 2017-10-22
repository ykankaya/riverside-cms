CREATE TYPE [element].[AlbumPhotoTableType] AS TABLE
(
	[AlbumPhotoId] [bigint] NULL,
	[ImageTenantId] [bigint] NOT NULL,
	[ThumbnailImageUploadId] [bigint] NOT NULL,
	[PreviewImageUploadId] [bigint] NOT NULL,
	[ImageUploadId] [bigint] NOT NULL,
	[Name] [nvarchar](256) NULL,
	[Description] [nvarchar](max) NULL,
	[SortOrder] [int] NOT NULL
)
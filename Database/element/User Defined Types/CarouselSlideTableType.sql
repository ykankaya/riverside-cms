CREATE TYPE [element].[CarouselSlideTableType] AS TABLE
(
	[CarouselSlideId] [bigint] NULL,
	[ImageTenantId] [bigint] NOT NULL,
	[ThumbnailImageUploadId] [bigint] NOT NULL,
	[PreviewImageUploadId] [bigint] NOT NULL,
	[ImageUploadId] [bigint] NOT NULL,
	[Name] [nvarchar](256) NULL,
	[Description] [nvarchar](max) NULL,
	[PageTenantId] [bigint] NULL,
	[PageId] [bigint] NULL,
	[PageText] [nvarchar](50) NULL,
	[SortOrder] [int] NOT NULL
)
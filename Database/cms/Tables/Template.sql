CREATE TABLE [cms].[Template](
	[TenantId] [bigint] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[CreateUserEnabled] [bit] NOT NULL,
	[UserHasImage] [bit] NOT NULL,
	[UserThumbnailImageWidth] [int] NULL,
	[UserThumbnailImageHeight] [int] NULL,
	[UserThumbnailImageResizeMode] [int] NULL,
	[UserPreviewImageWidth] [int] NULL,
	[UserPreviewImageHeight] [int] NULL,
	[UserPreviewImageResizeMode] [int] NULL,
	[UserImageMinWidth] [int] NULL,
	[UserImageMinHeight] [int] NULL,
 CONSTRAINT [PK_Template] PRIMARY KEY CLUSTERED ([TenantId] ASC),
 CONSTRAINT [FK_Template_Tenant] FOREIGN KEY([TenantId]) REFERENCES [cms].[Tenant] ([TenantId])
)
GO

CREATE NONCLUSTERED INDEX [IX_Template_Name]
	ON [cms].[Template] ([Name] ASC)
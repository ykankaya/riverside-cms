CREATE TABLE [cms].[Web] (
	[TenantId] [bigint] NOT NULL,
    [Name] [nvarchar](256) NOT NULL,
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
	[FontOption] [nvarchar](100) NULL,
	[ColourOption] [nvarchar](100) NULL,
 CONSTRAINT [PK_Web] PRIMARY KEY CLUSTERED ([TenantId] ASC),
 CONSTRAINT [FK_Web_Tenant] FOREIGN KEY([TenantId]) REFERENCES [cms].[Tenant] ([TenantId])
)
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Web_Name]
    ON [cms].[Web]([Name] ASC)
CREATE TABLE [cms].[Page](
	[TenantId] [bigint] NOT NULL,
	[PageId] [bigint] IDENTITY(1,1) NOT NULL,
	[ParentPageId] [bigint] NULL,
	[MasterPageId] [bigint] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Created] [datetime] NOT NULL,
	[Updated] [datetime] NOT NULL,
	[Occurred] [datetime] NULL,
	[ImageTenantId] [bigint] NULL,
	[ThumbnailImageUploadId] [bigint] NULL,
	[PreviewImageUploadId] [bigint] NULL,
	[ImageUploadId] [bigint] NULL,
 CONSTRAINT [PK_Page] PRIMARY KEY CLUSTERED ([TenantId] ASC, [PageId] ASC),
 CONSTRAINT [FK_Page_MasterPage] FOREIGN KEY([TenantId], [MasterPageId]) REFERENCES [cms].[MasterPage] ([TenantId], [MasterPageId]),
 CONSTRAINT [FK_Page_Page] FOREIGN KEY([TenantId], [ParentPageId]) REFERENCES [cms].[Page] ([TenantId], [PageId]),
 CONSTRAINT [FK_Page_Web] FOREIGN KEY([TenantId]) REFERENCES [cms].[Web] ([TenantId]),
 CONSTRAINT [FK_Page_ThumbnailImage] FOREIGN KEY([ImageTenantId], [ThumbnailImageUploadId]) REFERENCES [cms].[Image] ([TenantId], [UploadId]),
 CONSTRAINT [FK_Page_PreviewImage] FOREIGN KEY([ImageTenantId], [PreviewImageUploadId]) REFERENCES [cms].[Image] ([TenantId], [UploadId]),
 CONSTRAINT [FK_Page_Image] FOREIGN KEY([ImageTenantId], [ImageUploadId]) REFERENCES [cms].[Image] ([TenantId], [UploadId])
)
GO

CREATE NONCLUSTERED INDEX [IX_Page_TenantId_MasterPageId] ON [cms].[Page]
(
	[TenantId] ASC,
	[MasterPageId] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_Page_TenantId_ParentPageId]
	ON [cms].[Page]([TenantId] ASC, [ParentPageId] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_Page_TenantId_Created]
	ON [cms].[Page]([TenantId] ASC, [Created] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_Page_TenantId_Updated]
	ON [cms].[Page]([TenantId] ASC, [Updated] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_Page_TenantId_Occurred]
	ON [cms].[Page]([TenantId] ASC, [Occurred] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_Page_TenantId_Name]
	ON [cms].[Page]([TenantId] ASC, [Name] ASC)

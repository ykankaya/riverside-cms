CREATE TABLE [element].[PageHeader] (
	[TenantId] [bigint] NOT NULL,
	[ElementId] [bigint] NOT NULL,
	[PageTenantId] [bigint] NULL,
	[PageId] [bigint] NULL,
	[ShowName] [bit] NOT NULL,
	[ShowDescription] [bit] NOT NULL,
	[ShowImage] [bit] NOT NULL,
	[ShowCreated] [bit] NOT NULL,
	[ShowUpdated] [bit] NOT NULL,
	[ShowOccurred] [bit] NOT NULL,
	[ShowBreadcrumbs] [bit] NOT NULL,
 CONSTRAINT [PK_PageHeader] PRIMARY KEY CLUSTERED ([TenantId] ASC, [ElementId] ASC),
 CONSTRAINT [FK_PageHeader_Element] FOREIGN KEY([TenantId], [ElementId]) REFERENCES [cms].[Element] ([TenantId], [ElementId]),
 CONSTRAINT [FK_PageHeader_Page] FOREIGN KEY([PageTenantId], [PageId]) REFERENCES [cms].[Page] ([TenantId], [PageId])
)
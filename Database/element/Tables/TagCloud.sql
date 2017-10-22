CREATE TABLE [element].[TagCloud] (
	[TenantId] [bigint] NOT NULL,
	[ElementId] [bigint] NOT NULL,
	[PageTenantId] [bigint] NULL,
	[PageId] [bigint] NULL,
	[DisplayName] [nvarchar](256) NULL,
	[Recursive] [bit] NOT NULL,
	[NoTagsMessage] [nvarchar](max) NULL,
 CONSTRAINT [PK_TagCloud] PRIMARY KEY CLUSTERED ([TenantId] ASC, [ElementId] ASC),
 CONSTRAINT [FK_TagCloud_Element] FOREIGN KEY([TenantId], [ElementId]) REFERENCES [cms].[Element] ([TenantId], [ElementId]),
 CONSTRAINT [FK_TagCloud_Page] FOREIGN KEY([PageTenantId], [PageId]) REFERENCES [cms].[Page] ([TenantId], [PageId])
)
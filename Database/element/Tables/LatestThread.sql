CREATE TABLE [element].[LatestThread] (
	[TenantId] [bigint] NOT NULL,
	[ElementId] [bigint] NOT NULL,
	[PageTenantId] [bigint] NULL,
	[PageId] [bigint] NULL,
	[DisplayName] [nvarchar](256) NULL,
	[Recursive] [bit] NOT NULL,
	[NoThreadsMessage] [nvarchar](max) NULL,
	[Preamble] [nvarchar](max) NULL,
	[PageSize] [int] NOT NULL,
 CONSTRAINT [PK_LatestThread] PRIMARY KEY CLUSTERED ([TenantId] ASC, [ElementId] ASC),
 CONSTRAINT [FK_LatestThread_Element] FOREIGN KEY([TenantId], [ElementId]) REFERENCES [cms].[Element] ([TenantId], [ElementId]),
 CONSTRAINT [FK_LatestThread_Page] FOREIGN KEY([PageTenantId], [PageId]) REFERENCES [cms].[Page] ([TenantId], [PageId])
)
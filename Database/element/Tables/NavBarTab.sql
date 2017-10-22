CREATE TABLE [element].[NavBarTab](
	[TenantId] [bigint] NOT NULL,
	[ElementId] [bigint] NOT NULL,
	[NavBarTabId] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[SortOrder] [int] NOT NULL,
	[PageId] [bigint] NOT NULL,
 CONSTRAINT [PK_NavBarTab] PRIMARY KEY CLUSTERED ([TenantId] ASC, [ElementId] ASC, [NavBarTabId] ASC),
 CONSTRAINT [FK_NavBarTab_NavBar] FOREIGN KEY([TenantId], [ElementId]) REFERENCES [element].[NavBar] ([TenantId], [ElementId]),
 CONSTRAINT [FK_NavBarTab_Page] FOREIGN KEY([TenantId], [PageId]) REFERENCES [cms].[Page] ([TenantId], [PageId])
)
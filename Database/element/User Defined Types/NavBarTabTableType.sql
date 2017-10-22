CREATE TYPE [element].[NavBarTabTableType] AS TABLE
(
	[NavBarTabId] [bigint] NULL,
	[Name] [nvarchar](50) NOT NULL,
	[SortOrder] [int] NOT NULL,
	[PageId] [bigint] NOT NULL
)
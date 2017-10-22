CREATE TYPE [cms].[PageZoneTableType] AS TABLE
(
	[PageZoneId] [bigint] NULL,
	[MasterPageId] [bigint] NOT NULL,
	[MasterPageZoneId] [bigint] NOT NULL,
	[PageId] [bigint] NULL
)

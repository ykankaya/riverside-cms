CREATE TYPE [cms].[PageZoneElementTableType] AS TABLE
(
	[PageZoneId] [bigint] NULL,
	[PageZoneElementId] [bigint] NULL,
	[SortOrder] [int] NULL,
	[ElementId] [bigint] NOT NULL,
	[MasterPageId] [bigint] NULL,
	[MasterPageZoneId] [bigint] NULL,
	[MasterPageZoneElementId] [bigint] NULL,
	[PageZoneMasterPageZoneId] [bigint] NULL,
	[PageId] [bigint] NULL
)
CREATE TYPE [cms].[MasterPageZoneElementTableType] AS TABLE
(
	[MasterPageZoneId] [bigint] NULL,
	[MasterPageZoneElementId] [bigint] NULL,
	[MasterPageZoneSortOrder] [int] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[ElementId] [bigint] NOT NULL,
	[BeginRender] [nvarchar](max) NULL,
	[EndRender] [nvarchar](max) NULL
)
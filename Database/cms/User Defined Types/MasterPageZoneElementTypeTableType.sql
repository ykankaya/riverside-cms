CREATE TYPE [cms].[MasterPageZoneElementTypeTableType] AS TABLE
(
	[MasterPageZoneId] [bigint] NULL,
	[MasterPageZoneSortOrder] [int] NOT NULL,
	[ElementTypeId] [uniqueidentifier] NOT NULL
)
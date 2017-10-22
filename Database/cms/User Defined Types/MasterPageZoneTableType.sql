CREATE TYPE [cms].[MasterPageZoneTableType] AS TABLE
(
	[MasterPageZoneId] [bigint] NULL,
	[Name] [nvarchar](50) NOT NULL,
	[SortOrder] [int] NOT NULL,
	[AdminType] [int] NOT NULL,
	[ContentType] [int] NOT NULL,
	[BeginRender] [nvarchar](max) NULL,
	[EndRender] [nvarchar](max) NULL
)
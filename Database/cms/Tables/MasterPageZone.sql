CREATE TABLE [cms].[MasterPageZone](
	[TenantId] [bigint] NOT NULL,
	[MasterPageId] [bigint] NOT NULL,
	[MasterPageZoneId] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[SortOrder] [int] NOT NULL,
	[AdminType] [int] NOT NULL,
	[ContentType] [int] NOT NULL,
	[BeginRender] [nvarchar](max) NULL,
	[EndRender] [nvarchar](max) NULL,
 CONSTRAINT [PK_MasterPageZone] PRIMARY KEY CLUSTERED ([TenantId] ASC, [MasterPageId] ASC, [MasterPageZoneId] ASC),
 CONSTRAINT [FK_MasterPageZone_MasterPage] FOREIGN KEY([TenantId], [MasterPageId]) REFERENCES [cms].[MasterPage] ([TenantId], [MasterPageId])
)
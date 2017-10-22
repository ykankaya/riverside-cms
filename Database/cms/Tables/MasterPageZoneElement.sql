CREATE TABLE [cms].[MasterPageZoneElement](
	[TenantId] [bigint] NOT NULL,
	[MasterPageId] [bigint] NOT NULL,
	[MasterPageZoneId] [bigint] NOT NULL,
	[MasterPageZoneElementId] [bigint] IDENTITY(1,1) NOT NULL,
	[SortOrder] [int] NOT NULL,
	[ElementId] [bigint] NOT NULL,
	[BeginRender] [nvarchar](max) NULL,
	[EndRender] [nvarchar](max) NULL,
 CONSTRAINT [PK_MasterPageZoneElement] PRIMARY KEY CLUSTERED ([TenantId] ASC, [MasterPageId] ASC, [MasterPageZoneId] ASC, [MasterPageZoneElementId] ASC),
 CONSTRAINT [FK_MasterPageZoneElement_Element] FOREIGN KEY([TenantId], [ElementId]) REFERENCES [cms].[Element] ([TenantId], [ElementId]),
 CONSTRAINT [FK_MasterPageZoneElement_MasterPageZone] FOREIGN KEY([TenantId], [MasterPageId], [MasterPageZoneId]) REFERENCES [cms].[MasterPageZone] ([TenantId], [MasterPageId], [MasterPageZoneId])
)
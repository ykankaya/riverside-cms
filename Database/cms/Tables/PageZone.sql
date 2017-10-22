CREATE TABLE [cms].[PageZone](
	[TenantId] [bigint] NOT NULL,
	[PageId] [bigint] NOT NULL,
	[PageZoneId] [bigint] IDENTITY(1,1) NOT NULL,
	[MasterPageId] [bigint] NOT NULL,
	[MasterPageZoneId] [bigint] NOT NULL,
 CONSTRAINT [PK_PageZone] PRIMARY KEY CLUSTERED ([TenantId] ASC, [PageId] ASC, [PageZoneId] ASC),
 CONSTRAINT [FK_PageZone_MasterPageZone] FOREIGN KEY([TenantId], [MasterPageId], [MasterPageZoneId]) REFERENCES [cms].[MasterPageZone] ([TenantId], [MasterPageId], [MasterPageZoneId]),
 CONSTRAINT [FK_PageZone_Page] FOREIGN KEY([TenantId], [PageId]) REFERENCES [cms].[Page] ([TenantId], [PageId])
)
GO

CREATE NONCLUSTERED INDEX [IX_PageZone_TenantId_MasterPageId_MasterPageZoneId] ON [cms].[PageZone]
(
	[TenantId] ASC,
	[MasterPageId] ASC,
	[MasterPageZoneId] ASC
)
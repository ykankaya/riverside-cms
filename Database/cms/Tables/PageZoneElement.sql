CREATE TABLE [cms].[PageZoneElement](
	[TenantId] [bigint] NOT NULL,
	[PageId] [bigint] NOT NULL,
	[PageZoneId] [bigint] NOT NULL,
	[PageZoneElementId] [bigint] IDENTITY(1,1) NOT NULL,
	[MasterPageId] [bigint] NULL,
	[MasterPageZoneId] [bigint] NULL,
	[MasterPageZoneElementId] [bigint] NULL,
	[SortOrder] [int] NULL,
	[ElementId] [bigint] NOT NULL,
 CONSTRAINT [PK_PageZoneElement] PRIMARY KEY CLUSTERED ([TenantId] ASC, [PageId] ASC, [PageZoneId] ASC, [PageZoneElementId] ASC),
 CONSTRAINT [FK_PageZoneElement_Element] FOREIGN KEY([TenantId], [ElementId]) REFERENCES [cms].[Element] ([TenantId], [ElementId]),
 CONSTRAINT [FK_PageZoneElement_PageZone] FOREIGN KEY([TenantId], [PageId], [PageZoneId]) REFERENCES [cms].[PageZone] ([TenantId], [PageId], [PageZoneId]),
 CONSTRAINT [FK_PageZoneElement_MasterPageZoneElement] FOREIGN KEY([TenantId], [MasterPageId], [MasterPageZoneId], [MasterPageZoneElementId]) REFERENCES [cms].[MasterPageZoneElement] ([TenantId], [MasterPageId], [MasterPageZoneId], [MasterPageZoneElementId])
)
GO

CREATE NONCLUSTERED INDEX [IX_PageZoneElement_TenantId_MasterPageId_MasterPageZoneId_MasterPageZoneElementId] ON [cms].[PageZoneElement]
(
	[TenantId] ASC,
	[MasterPageId] ASC,
	[MasterPageZoneId] ASC,
	[MasterPageZoneElementId] ASC
)
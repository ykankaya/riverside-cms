CREATE TABLE [cms].[MasterPageZoneElementType] (
	[TenantId] [bigint] NOT NULL,
	[MasterPageId] [bigint] NOT NULL,
	[MasterPageZoneId] [bigint] NOT NULL,
	[ElementTypeId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_MasterPageZoneElementType] PRIMARY KEY CLUSTERED ([TenantId] ASC, [MasterPageId] ASC, [MasterPageZoneId] ASC, [ElementTypeId] ASC),
 CONSTRAINT [FK_MasterPageZoneElementType_MasterPageZone] FOREIGN KEY([TenantId], [MasterPageId], [MasterPageZoneId]) REFERENCES [cms].[MasterPageZone] ([TenantId], [MasterPageId], [MasterPageZoneId]),
 CONSTRAINT [FK_MasterPageZoneElementType_ElementType] FOREIGN KEY([ElementTypeId]) REFERENCES [cms].[ElementType] ([ElementTypeId])
)
CREATE TABLE [cms].[TemplatePageZoneElementType] (
	[TenantId] [bigint] NOT NULL,
	[TemplatePageId] [bigint] NOT NULL,
	[TemplatePageZoneId] [bigint] NOT NULL,
	[ElementTypeId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_TemplatePageZoneElementType] PRIMARY KEY CLUSTERED ([TenantId] ASC, [TemplatePageId] ASC, [TemplatePageZoneId] ASC, [ElementTypeId] ASC),
 CONSTRAINT [FK_TemplatePageZoneElementType_TemplatePageZone] FOREIGN KEY([TenantId], [TemplatePageId], [TemplatePageZoneId]) REFERENCES [cms].[TemplatePageZone] ([TenantId], [TemplatePageId], [TemplatePageZoneId]),
 CONSTRAINT [FK_TemplatePageZoneElementType_ElementType] FOREIGN KEY([ElementTypeId]) REFERENCES [cms].[ElementType] ([ElementTypeId])
)
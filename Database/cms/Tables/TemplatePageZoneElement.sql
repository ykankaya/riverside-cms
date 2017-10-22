CREATE TABLE [cms].[TemplatePageZoneElement](
	[TenantId] [bigint] NOT NULL,
	[TemplatePageId] [bigint] NOT NULL,
	[TemplatePageZoneId] [bigint] NOT NULL,
	[TemplatePageZoneElementId] [bigint] IDENTITY(1,1) NOT NULL,
	[SortOrder] [int] NOT NULL,
	[ElementId] [bigint] NOT NULL,
	[BeginRender] [nvarchar](max) NULL,
	[EndRender] [nvarchar](max) NULL,
 CONSTRAINT [PK_TemplatePageZoneElement] PRIMARY KEY CLUSTERED ([TenantId] ASC, [TemplatePageId] ASC, [TemplatePageZoneId] ASC, [TemplatePageZoneElementId] ASC),
 CONSTRAINT [FK_TemplatePageZoneElement_Element] FOREIGN KEY([TenantId], [ElementId]) REFERENCES [cms].[Element] ([TenantId], [ElementId]),
 CONSTRAINT [FK_TemplatePageZoneElement_TemplatePageZone] FOREIGN KEY([TenantId], [TemplatePageId], [TemplatePageZoneId]) REFERENCES [cms].[TemplatePageZone] ([TenantId], [TemplatePageId], [TemplatePageZoneId])
)
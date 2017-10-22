CREATE TABLE [cms].[TemplatePageZone](
	[TenantId] [bigint] NOT NULL,
	[TemplatePageId] [bigint] NOT NULL,
	[TemplatePageZoneId] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[SortOrder] [int] NOT NULL,
	[AdminType] [int] NOT NULL,
	[ContentType] [int] NOT NULL,
	[BeginRender] [nvarchar](max) NULL,
	[EndRender] [nvarchar](max) NULL,
 CONSTRAINT [PK_TemplatePageZone] PRIMARY KEY CLUSTERED ([TenantId] ASC, [TemplatePageId] ASC, [TemplatePageZoneId] ASC),
 CONSTRAINT [FK_TemplatePageZone_TemplatePage] FOREIGN KEY([TenantId], [TemplatePageId]) REFERENCES [cms].[TemplatePage] ([TenantId], [TemplatePageId])
)
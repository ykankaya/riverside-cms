CREATE TABLE [element].[Album] (
	[TenantId] [bigint] NOT NULL,
	[ElementId] [bigint] NOT NULL,
	[DisplayName] [nvarchar](256) NULL,
 CONSTRAINT [PK_Album] PRIMARY KEY CLUSTERED ([TenantId] ASC, [ElementId] ASC),
 CONSTRAINT [FK_Album_Element] FOREIGN KEY([TenantId], [ElementId]) REFERENCES [cms].[Element] ([TenantId], [ElementId])
)
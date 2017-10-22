CREATE TABLE [element].[Map] (
	[TenantId] [bigint] NOT NULL,
	[ElementId] [bigint] NOT NULL,
	[DisplayName] [nvarchar](256) NULL,
	[Latitude] [float](53) NOT NULL,
	[Longitude] [float](53) NOT NULL,
 CONSTRAINT [PK_Map] PRIMARY KEY CLUSTERED ([TenantId] ASC, [ElementId] ASC),
 CONSTRAINT [FK_Map_Element] FOREIGN KEY([TenantId], [ElementId]) REFERENCES [cms].[Element] ([TenantId], [ElementId])
)
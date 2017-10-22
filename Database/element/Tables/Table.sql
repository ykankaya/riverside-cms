CREATE TABLE [element].[Table] (
	[TenantId] [bigint] NOT NULL,
	[ElementId] [bigint] NOT NULL,
	[DisplayName] [nvarchar](256) NOT NULL,
	[Preamble] [nvarchar](max) NOT NULL,
	[ShowHeaders][bit] NOT NULL,
	[Rows][nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Table] PRIMARY KEY CLUSTERED ([TenantId] ASC, [ElementId] ASC),
 CONSTRAINT [FK_Table_Element] FOREIGN KEY([TenantId], [ElementId]) REFERENCES [cms].[Element] ([TenantId], [ElementId])
)
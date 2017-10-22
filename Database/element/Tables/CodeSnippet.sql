CREATE TABLE [element].[CodeSnippet] (
	[TenantId] [bigint] NOT NULL,
	[ElementId] [bigint] NOT NULL,
	[Code] [nvarchar](max) NOT NULL,
	[Language] [int] NOT NULL,
 CONSTRAINT [PK_CodeSnippet] PRIMARY KEY CLUSTERED ([TenantId] ASC, [ElementId] ASC),
 CONSTRAINT [FK_CodeSnippet_Element] FOREIGN KEY([TenantId], [ElementId]) REFERENCES [cms].[Element] ([TenantId], [ElementId])
)